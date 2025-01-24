using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpTimer : MonoBehaviour
{
    [SerializeField] Image _fill; // Graphic that shows how much time is left
    float _time = 15;
    float _currTime;

    private void OnDisable()
    {
        Debug.Log("Powerup is disabled");
    }

    void Update()
    {
        if (_currTime >= 0) // As the timer counts down, more of the graphic disappears
        {
            _currTime -= Time.deltaTime;
            _fill.fillAmount = _currTime / _time;
        }
    }

    public void SetTime(int newTime)
    {
        _time = newTime;
        _currTime = _time;
    }
}
