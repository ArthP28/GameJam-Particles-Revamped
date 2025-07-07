using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    [SerializeField] GameObject LoadingScreen;
    Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void GoToBattle()
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("Level2");
    }

    public void GoToHowToPlay()
    {
        _anim.Play("TitleToHowToPlay");
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
