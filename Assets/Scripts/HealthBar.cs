using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Health _playerHealth;
    [SerializeField] Image fill;
    Slider _barSlider;
    // Start is called before the first frame update
    void Start()
    {
        _barSlider = GetComponent<Slider>();

        _barSlider.maxValue = _playerHealth.GetCurrentHealth();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
    }

    void UpdateHealth()
    {
        _barSlider.value = _playerHealth.GetCurrentHealth();
    }
}
