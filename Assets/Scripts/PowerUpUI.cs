using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpUI : MonoBehaviour
{
    public PlayerNumber playerOfUI; // What player is getting affected by the PowerUpUI
    [SerializeField] PowerUpTimer _powerUpTimer;
    [SerializeField] PowerUpTimer _effectTimer;
    [SerializeField] GameObject _message;

    bool powerUpActive = false;
    bool effectActive = false;
    float offset = 105f;

    public void ActivatePowerUpTimer(int powerUpTime, Sprite fillSprite)
    {
        _powerUpTimer.SetTime(powerUpTime);
        _powerUpTimer.gameObject.SetActive(true);
        _powerUpTimer.ChangeFill(fillSprite);
        if (effectActive && !powerUpActive)
        {
            if(playerOfUI == PlayerNumber.Player1)
            {
                _powerUpTimer.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset, 0);
            } else if (playerOfUI == PlayerNumber.Player2)
            {
                _powerUpTimer.GetComponent<RectTransform>().anchoredPosition = new Vector2(-offset - 3, 0);
            }
        }
        else if (!powerUpActive)
        {
            _powerUpTimer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        powerUpActive = true;
    }

    public void DeActivatePowerUpTimer()
    {
        _powerUpTimer.gameObject.SetActive(false);
        powerUpActive = false;
        if (effectActive)
        {
            _effectTimer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    public void ActivateEffectTimer(int powerUpTime, Sprite fillSprite)
    {
        _effectTimer.SetTime(powerUpTime);
        _effectTimer.gameObject.SetActive(true);
        _effectTimer.ChangeFill(fillSprite);
        if (powerUpActive && !effectActive)
        {
            if (playerOfUI == PlayerNumber.Player1)
            {
                _effectTimer.GetComponent<RectTransform>().anchoredPosition = new Vector2(offset, 0);
            }
            else if (playerOfUI == PlayerNumber.Player2)
            {
                _effectTimer.GetComponent<RectTransform>().anchoredPosition = new Vector2(-offset - 3, 0);
            }
        }
        else if (!effectActive)
        {
            _effectTimer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        effectActive = true;
    }

    public void DeActivateEffectTimer()
    {
        _effectTimer.gameObject.SetActive(false);
        effectActive = false;
        if (powerUpActive)
        {
            _powerUpTimer.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    public IEnumerator DisplayMessage(string PowerUpName)
    {
        TextMeshProUGUI _messageText = _message.GetComponentInChildren<TextMeshProUGUI>();
        _messageText.text = PowerUpName + " obtained";
        _message.SetActive(true);
        yield return new WaitForSeconds(3f);
        RemoveMessage();
    }

    public IEnumerator HealthMessage(int healthRecovered)
    {
        TextMeshProUGUI _messageText = _message.GetComponentInChildren<TextMeshProUGUI>();
        _messageText.text = healthRecovered + " HP Gained";
        _message.SetActive(true);
        yield return new WaitForSeconds(3f);
        RemoveMessage();
    }

    public void RemoveMessage()
    {
        _message.SetActive(false);
    }
}
