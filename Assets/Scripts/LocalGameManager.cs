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

    PlayerInput[] _allPlayers;

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
        _allPlayers = FindObjectsOfType<PlayerInput>();

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
            ToggleInput(false);
        } else if (SurvivalMode.Player1Won())
        {
            P1VictoryScreen.SetActive(true);
            ToggleInput(false);
        } else if (SurvivalMode.Player2Won())
        {
            P2VictoryScreen.SetActive(true);
            ToggleInput(false);
        }
    }

    void PauseGame()
    {
        if (Time.timeScale > 0)
        {
            _bgmMusic.Pause();
            PauseMenu.SetActive(true);
            ToggleInput(false);
            Time.timeScale = 0f;
        } else
        {
            _bgmMusic.Play();
            PauseMenu.SetActive(false);
            ToggleInput(true);
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

    void ToggleInput(bool toggle) // Enables/Disables the input for all players
    {
        foreach (PlayerInput _player in _allPlayers)
        {
            _player.enabled = toggle;
        }
    }
}
