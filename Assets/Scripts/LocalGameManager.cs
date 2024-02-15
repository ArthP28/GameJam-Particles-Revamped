using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LocalGameManager : MonoBehaviour, UIControls.IGeneralUIActions
{
    [SerializeField] GameObject LoadingScreen;

    SurvivalBattleScript SurvivalMode;

    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject P1VictoryScreen;
    [SerializeField] GameObject P2VictoryScreen;
    [SerializeField] GameObject TieScreen;

    [SerializeField] AudioSource _bgmMusic;

    Player1Movement p1Controls;
    Player2Movement p2Controls;

    UIControls _controls;
    public event Action Pause;

    void Awake()
    {
        PauseMenu.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        SurvivalMode = GetComponentInChildren<SurvivalBattleScript>();

        p1Controls = FindObjectOfType<Player1Movement>();
        p2Controls = FindObjectOfType<Player2Movement>();

        _controls = new UIControls();
        _controls.GeneralUI.SetCallbacks(this);
        _controls.GeneralUI.Enable();
        Pause += PauseGame;
    }

    void OnDestroy()
    {
        _controls.Disable();
        Pause -= PauseGame;
    }

    // Update is called once per frame
    void Update()
    {
        if(SurvivalMode.Player1Won() && SurvivalMode.Player2Won())
        {
            TieScreen.SetActive(true);
            p1Controls.ChangeControlState(false);
            p2Controls.ChangeControlState(false);
        } else if (SurvivalMode.Player1Won())
        {
            P1VictoryScreen.SetActive(true);
            p1Controls.ChangeControlState(false);
            p2Controls.ChangeControlState(false);
        } else if (SurvivalMode.Player2Won())
        {
            p1Controls.ChangeControlState(false);
            p2Controls.ChangeControlState(false);
            P2VictoryScreen.SetActive(true);
        }
    }

    void PauseGame()
    {
        if (Time.timeScale > 0)
        {
            _bgmMusic.mute = true;
            PauseMenu.SetActive(true);
            p1Controls.ChangeControlState(false);
            p2Controls.ChangeControlState(false);
            Time.timeScale = 0f;
        } else
        {
            _bgmMusic.mute = false;
            PauseMenu.SetActive(false);
            p1Controls.ChangeControlState(true);
            p2Controls.ChangeControlState(true);
            Time.timeScale = 1f;
        }
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("Level1");
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        LoadingScreen.SetActive(true);
        SceneManager.LoadScene("Menu");
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        Pause?.Invoke();
    }
}
