using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;
using UnityEngine;

enum Panels { Main = 0, Question = 1, Result = 2, Inventory = 3 }

public class AppController : MonoBehaviour
{
    public Question[] questions;
    public CanvasGroup[] panels;
    public CanvasGroup fade;

    public Inventory inventory;
    public QuestionPanel questionScreen;
    public ResultScreen resultScreen;

    public int correctAmount = 0;
    private int currentQuestion = 0;

    private int[] randomOrder = new int[11];


    public AudioSource feedbackAudio;
    public AudioSource bgmAudio;

    public AudioClip[] clips;
    

    public void Start()
    {
      
    }

    public void StartGame()
    {
        bgmAudio.Play();
        correctAmount = 0;
        currentQuestion = 0;
        FillQuestions();
        randomizeQuestions();
        ShowQuestion(true);
    }

    public void randomizeQuestions()
    {
        bool[] numbersTaken = {false,false,false,false,false,
            false,false,false,false,false,false};

        int currentIndex = 0;

        while (currentIndex < 10)
        {
            int randomSelected = Random.Range(0, 10);

            while (numbersTaken[randomSelected] == true)
            {
                randomSelected = Random.Range(0, 10);
            }

            numbersTaken[randomSelected] = true;

            randomOrder[currentIndex] = randomSelected;

            currentIndex++;
        }
    }

    public void OpenInventory()
    {
        StartCoroutine(OpenInventoryWorker());
    }

    public IEnumerator OpenInventoryWorker()
    {
        yield return null;
        yield return StartCoroutine(FadeIn());
        inventory.FillRewards();
        inventory.InstantiateRewards();
        panels[((int)Panels.Inventory)].alpha = 1;
        panels[((int)Panels.Inventory)].interactable = true;
        panels[((int)Panels.Inventory)].blocksRaycasts = true;

        StartCoroutine(FadeOut());
    }

    public IEnumerator CloseInventoryWorker()
    {
        yield return StartCoroutine(FadeIn());
        panels[((int)Panels.Inventory)].alpha = 0;
        panels[((int)Panels.Inventory)].interactable = false;
        panels[((int)Panels.Inventory)].blocksRaycasts = false;
        yield return StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn()
    {
        fade.alpha = 0;
        while (fade.alpha < 1f)
        {
            fade.alpha += 5f * Time.deltaTime;
            yield return null;
        }
        fade.alpha = 1f;
    }

    public IEnumerator FadeOut()
    {
        fade.alpha = 1f;
        while (fade.alpha > 0f)
        {
            fade.alpha -= 5f * Time.deltaTime;
            yield return null;
        }
        fade.alpha = 0f;
    }

    public void ClearInventory()
    {
        PlayerPrefs.SetString("DB", "");
    }

    public void GetReward()
    {
        print(inventory.GetReward());
    }

    public void AnswerReceived(bool right)
    {
       
        if (right)
        {
            correctAmount++;
            feedbackAudio.clip = clips[1];
        }
        else
        {
            feedbackAudio.clip = clips[0];
        }

        feedbackAudio.Play();

        if (currentQuestion < 4)
        {
            currentQuestion++;
            ShowQuestion(true);
        }
        else
        {
            StartCoroutine(ShowResultWorker());
        }

        print("Correct? " + right + " Amount " + correctAmount);

    }

    public IEnumerator ShowResultWorker()
    {
        yield return StartCoroutine(FadeIn());
        questionScreen.GetComponent<CanvasGroup>().alpha = 0;
        questionScreen.GetComponent<CanvasGroup>().interactable = false;
        questionScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;

        resultScreen.GetComponent<CanvasGroup>().alpha = 1;
        resultScreen.GetComponent<CanvasGroup>().interactable = true;
        resultScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;

        string result = "-2";
        if (correctAmount > 4)
        {
            result = inventory.GetReward();
        }
        yield return new WaitForSeconds(1f);
        print("Correct amount " + correctAmount);
        resultScreen.FillScreen(result, this);
        yield return null;
        yield return StartCoroutine(FadeOut());
    }

    public void ShowQuestion(bool doFade)
    {
        StartCoroutine(ShowQuestionWorker(doFade));
    }

    public IEnumerator ShowQuestionWorker(bool doFade)
    {
        yield return StartCoroutine(FadeIn());
        yield return new WaitForSeconds(1f);
        questionScreen.FillQuestion(questions[randomOrder[currentQuestion]], this);
        questionScreen.GetComponent<CanvasGroup>().alpha = 1;
        questionScreen.GetComponent<CanvasGroup>().interactable = true;
        questionScreen.GetComponent<CanvasGroup>().blocksRaycasts = true;
        yield return StartCoroutine(FadeOut());
        fade.alpha = 0f;
        yield return null;
    }

    public void RestartGame()
    {
        StartCoroutine(RestartGameWorker());
    }

    public IEnumerator RestartGameWorker()
    {
        bgmAudio.Stop();
        yield return StartCoroutine(FadeIn());
        resultScreen.GetComponent<CanvasGroup>().alpha = 0;
        resultScreen.GetComponent<CanvasGroup>().interactable = false;
        resultScreen.GetComponent<CanvasGroup>().blocksRaycasts = false;
        yield return StartCoroutine(FadeOut());

    }

    public void FillQuestions()
    {
        questions = new Question[10];


        questions[0] = new Question();
        questions[0].title = "Qué porcentaje de tus ingresos deberías ahorrar cada mes?";
        questions[0].answers[0] = "5 %";
        questions[0].answers[1] = "10 a 15 %";
        questions[0].answers[2] = "No menos del 20 %";
        questions[0].correct = 1;

        questions[1] = new Question();
        questions[1].title = "Cuanto dinero deberías apartar para emergencias?";
        questions[1].answers[0] = "El equivalente a 2 meses de tus gastos diarios";
        questions[1].answers[1] = "El equivalente a 4 meses de alimentos y gastos médicos";
        questions[1].answers[2] = "El equivalente a 6 meses de salario neto";

        questions[1].correct = 1;

        questions[2] = new Question();
        questions[2].title = "Cual  es el porcentaje máximo de tus ingresos que deberías tener en deuda?";
        questions[2].answers[0] = "No más del 15%";
        questions[2].answers[1] = "Cerca del 20%";
        questions[2].answers[2] = "No mas del 25%";
        questions[2].correct = 2;

        questions[3] = new Question();
        questions[3].title = "Es mejor pagar deudas de tarjetas de crédito o ahorrar dinero?";
        questions[3].answers[0] = "Pagar las facturas de las tarjetas de crédito a medida que ahorras cancelando primero la deuda de las tarjetas de mayor interés";
        questions[3].answers[1] = "Pagar la deuda de todas las tarjetas de crédito antes de ahorrar";
        questions[3].answers[2] = "Ahorrar todo lo posible y no pagar mas del mínimo mensual en cada tarjeta";
        questions[3].correct = 0;

        questions[4] = new Question();
        questions[4].title = "Qué porcentaje de tu ingreso deberías ahorrar para gastos en viajes,compras y entretenimiento?";
        questions[4].answers[0] = "Un 20%";
        questions[4].answers[1] = "Un 25%";
        questions[4].answers[2] = "Un 35%";
        questions[4].correct = 0;

        questions[5] = new Question();
        questions[5].title = " Si tienes dinero disponible ¿Es mas seguro realizar una inversión, o múltiples inversiones?";
        questions[5].answers[0] = "Una inversión";
        questions[5].answers[1] = "Múltiples inversiones";
        questions[5].answers[2] = "Ambas respuestas son correctas";
        questions[5].correct = 1;

        questions[6] = new Question();
        questions[6].title = "Que tipo de cuenta de ahorro es donde el titular deposita una suma de dinero por un periodo de tiempo definido a una tasa de interés que varía de acuerdo al monto y plazo?";
        questions[6].answers[0] = "A la vista";
        questions[6].answers[1] = "A plazo fijo";
        questions[6].answers[2] = "Planificado";
        questions[6].correct = 1;

        questions[7] = new Question();
        questions[7].title = "Qué tipo de cuenta de ahorro sirve para planificar sus ahorros a mediano o largo plazo a través de depósitos mensuales cuyo monto y plazo lo determina el socio?";
        questions[7].answers[0] = "Ahorro a la vista";
        questions[7].answers[1] = "Ahorro infanto-juvenil";
        questions[7].answers[2] = "Ahorro planificado";
        questions[7].correct = 2;

        questions[8] = new Question();
        questions[8].title = "Cuantas sucursales tiene COOPEDUC Ltda. en todo el país?";
        questions[8].answers[0] = "10 sucursales";
        questions[8].answers[1] = "13 sucursales";
        questions[8].answers[2] = "12 sucursales";
        questions[8].correct = 1;

        questions[9] = new Question();
        questions[9].title = "Donde queda la Casa Matriz de la Cooperativa?";
        questions[9].answers[0] = "Villarrica";
        questions[9].answers[1] = "Mbocayaty";
        questions[9].answers[2] = "Coronel Oviedo";
        questions[9].correct = 0;

    }

    
}






