using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int _health = 50;
    int _maxHealth; // Determines maximum amount of health that can be restored

    void Start()
    {
        _maxHealth = _health; // Maximum health is set to the Serialized variable health (Value manually set)
    }

    public void DealDamage(int damage) // Subtracts health
    {
        Debug.Log("Health before damage: " + _health + " | Health AFTER damage: " + (_health - damage));
        _health -= damage;
        if (gameObject.GetComponent<Player1Movement>())
        {
            gameObject.GetComponent<Player1Movement>().GetAnimations().SetTrigger("Hurt");
        }
        if (gameObject.GetComponent<Player2Movement>())
        {
            gameObject.GetComponent<Player2Movement>().GetAnimations().SetTrigger("Hurt");
        }
    }

    public void RestoreHealth(int healthRestored) // Adds health
    {
        if (_health < _maxHealth)
        {
            Debug.Log("Health before restoring: " + _health + " | Health AFTER restoring: " + (_health + healthRestored));
            _health += healthRestored;
        }
    }

    public int GetCurrentHealth()
    {
        return _health;
    }

    public void ResetHealth()
    {
        _health = _maxHealth;
    }
}
