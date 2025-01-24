using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PowerUpUI : MonoBehaviour
{
    public PlayerNumber playerOfUI; // What player is getting affected by the PowerUpUI
    [SerializeField] PowerUpTimer _timer;
    [SerializeField] GameObject _message;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateTimer(int powerUpTime)
    {
        _timer.SetTime(powerUpTime);
        _timer.gameObject.SetActive(true);
    }

    public void DeActivateTimer()
    {
        _timer.gameObject.SetActive(false);
    }

    public IEnumerator DisplayMessage(string PowerUpName)
    {
        TextMeshProUGUI _messageText = _message.GetComponentInChildren<TextMeshProUGUI>();
        _messageText.text = PowerUpName + " obtained";
        _message.SetActive(true);
        yield return new WaitForSeconds(3f);
        RemoveMessage();
    }

    public void RemoveMessage()
    {
        _message.SetActive(false);
    }
}
