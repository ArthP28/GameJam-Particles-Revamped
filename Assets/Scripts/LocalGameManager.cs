using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LocalGameManager : MonoBehaviour, UIControls.IGeneralUIActions
{
    [SerializeField] GameObject LoadingScreen;

    SurvivalBattleScript SurvivalMode;

    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject P1VictoryScreen;
    [SerializeField] GameObject P2VictoryScreen;
    [SerializeField] GameObject TieScreen;
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] int countDownTime = 3;

    [SerializeField] AudioSource _bgmMusic;
    [SerializeField] bool enableCountdown = true;

    PlayerInput[] _allPlayers;

    UIControls _controls;
    PowerUpManager _powerUpManager;
    bool gameStarted = true;
    public event Action Pause;

    void Awake()
    {
        PauseMenu.SetActive(false);
        _powerUpManager = FindObjectOfType<PowerUpManager>();
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
        if (enableCountdown)
        {
            _powerUpManager.enabled = false;
            gameStarted = false;
            ToggleInput(false);
            StartCoroutine(StartCountdown());
        } else
        {
            _powerUpManager.enabled = true;
            gameStarted = true;
            _bgmMusic.Play();
        }

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
        if(!(SurvivalMode.Player1Won() && SurvivalMode.Player2Won()))
        {
            if (Time.timeScale > 0)
            {
                _bgmMusic.Pause();
                PauseMenu.SetActive(true);
                ToggleInput(false);
                Time.timeScale = 0f;
            } else
            {
                if (gameStarted)
                {
                    _bgmMusic.Play();
                }
                PauseMenu.SetActive(false);
                ToggleInput(true);
                Time.timeScale = 1f;
            }
        }
    }

    IEnumerator StartCountdown()
    {
        yield return new WaitForSeconds(1f);
        countDownText.gameObject.SetActive(true);
        while (countDownTime > 0)
        {
            countDownText.text = countDownTime.ToString();
            yield return new WaitForSeconds(1f);
            countDownTime--;
        }
        countDownText.text = "START";
        ToggleInput(true);
        _bgmMusic.Play();
        _powerUpManager.enabled = true;
        gameStarted = true;
        yield return new WaitForSeconds(1f);
        countDownText.gameObject.SetActive(false);
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
