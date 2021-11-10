using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionPanel : MonoBehaviour
{

    public Text title;
    public Text[] answerButtons;
    public Button[] btns;

    public Question question;

    public AppController appController;

    public Image[] feedbackImg;
    public Sprite[] feedbackSprites;

    public void FillQuestion(Question question, AppController appController)
    {
        this.appController = appController;
        this.question = question;
        title.text = question.title;
        feedbackImg[0].color = new Color(1f, 1f, 1f, 0f);
        feedbackImg[1].color = new Color(1f, 1f, 1f, 0f);
        feedbackImg[2].color = new Color(1f, 1f, 1f, 0f);
        answerButtons[0].text = question.answers[0];
        answerButtons[1].text = question.answers[1];
        answerButtons[2].text = question.answers[2];
        btns[0].interactable = true;
        btns[1].interactable = true;
        btns[2].interactable = true;


        feedbackImg[0].sprite = feedbackSprites[question.correct == 0 ? 1 : 0];
        feedbackImg[1].sprite = feedbackSprites[question.correct == 1 ? 1 : 0];
        feedbackImg[2].sprite = feedbackSprites[question.correct == 2 ? 1 : 0];
    }

    public void AnswerSelected(int index)
    {
        btns[0].interactable = false;
        btns[1].interactable = false;
        btns[2].interactable = false;
        print("Selected " + index + " Correct " + question.correct);
        
        StartCoroutine(AnimateResults(index));
         
    }

    public IEnumerator AnimateResults(int index)
    {
        feedbackImg[0].color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(0.1f);
        feedbackImg[1].color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(0.1f);
        feedbackImg[2].color = new Color(1f, 1f, 1f, 1f);
        yield return new WaitForSeconds(2f);
        appController.AnswerReceived(index == question.correct);
    }
}

public class Question
{

    public string title;

    public string[] answers = new string[4];

    public int correct = -1;


}