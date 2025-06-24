using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int _health = 100;
    [SerializeField] AudioClip healSound;

    AudioSource m_AudioSource; // Required for Sound to work
    int _maxHealth; // Determines maximum amount of health that can be restored

    void Start()
    {
        _maxHealth = _health; // Maximum health is set to the Serialized variable health (Value manually set)
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void DealDamage(int damage) // Subtracts health
    {
        Debug.Log("Health before damage: " + _health + " | Health AFTER damage: " + (_health - damage));
        _health -= damage;
        if (gameObject.GetComponent<PlayerNew>())
        {
            gameObject.GetComponent<PlayerNew>().GetAnimations().SetTrigger("Hurt");
        }
    }

    public void RestoreHealth(int healthRestored) // Adds health
    {
        m_AudioSource.PlayOneShot(healSound);
        int newHealth = _health + healthRestored;
        if (newHealth > _maxHealth)
        {
            newHealth = _maxHealth;
        }
        Debug.Log("Health before restoring: " + _health + " | Health AFTER restoring: " + newHealth);
        _health = newHealth;
    }

    public int GetCurrentHealth()
    {
        return _health;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
    }
}
