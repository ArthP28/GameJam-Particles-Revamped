using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    [SerializeField] GameObject LoadingScreen;
    public void GoToBattle()
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("Level1");
    }

    public void GoToHowToPlay()
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("HowToPlay");
    }

    public void GoToCredits()
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("Credits");
    }

    public void GoToTitle()
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Game Exited");
        Application.Quit();
    }

}
