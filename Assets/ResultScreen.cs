using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScreen : MonoBehaviour
{
    public Text titleWin;
    public Text titleReward;
    public Text correctasAmount;

    public Color gColor;
    public Color rColor;

    public GameObject winImage;
    public GameObject loseImage;

    public AppController appController;

    public AudioSource audioSource;
    public AudioClip[] clips;

    public void FillScreen(string reward, AppController appController)
    {
        this.appController = appController;

        winImage.SetActive(false);
        loseImage.SetActive(false);

        if (reward == "-2")
        {
            titleWin.text = "No lo lograste, vuelve a intentarlo";
            titleReward.text = "";
            correctasAmount.color = rColor;
            loseImage.SetActive(true);
            audioSource.clip = clips[0];
        }
        else if (reward == "-1")
        {
            titleWin.text = "Lo lograste!";
            //titleReward.text = reward;
            correctasAmount.color = gColor;
            winImage.SetActive(true);
            audioSource.clip = clips[1];
        }
        else
        {
            titleWin.text = "Lo lograste, este es tu premio";
            titleReward.text = reward;
            correctasAmount.color = gColor;
            winImage.SetActive(true);
            audioSource.clip = clips[1];
        }

        audioSource.Play();

        correctasAmount.text = "Respuestas correctas: " + appController.correctAmount
            + "/5";
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
