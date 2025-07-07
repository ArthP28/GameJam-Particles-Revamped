using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] GameObject LoadingScreen;
    [SerializeField] string[] LevelPaths;
    [SerializeField] GameObject[] LevelPreviews;
    int currentPathIndex = 0;
    Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void GoToBattle()
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene(LevelPaths[currentPathIndex]);
    }

    public void SelectRight()
    {
        currentPathIndex++;
        if (currentPathIndex >= LevelPaths.Length)
        {
            LevelPreviews[LevelPaths.Length - 1].SetActive(false);
            currentPathIndex = 0;
        } else
        {
            LevelPreviews[currentPathIndex-1].SetActive(false);
        }
        LevelPreviews[currentPathIndex].SetActive(true);
    }

    public void SelectLeft()
    {
        currentPathIndex--;
        if (currentPathIndex < 0)
        {
            LevelPreviews[0].SetActive(false);
            currentPathIndex = LevelPaths.Length - 1;
        }
        else
        {
            LevelPreviews[currentPathIndex+1].SetActive(false);
        }
        LevelPreviews[currentPathIndex].SetActive(true);
    }

    public void GoToTitle()
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("Menu");
    }

    public void GoToCredits()
    {
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Debug.Log("Game Exited");
        Application.Quit();
    }

}
