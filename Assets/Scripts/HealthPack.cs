using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : PickUp
{
    [SerializeField] int amount = 50;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Health>())
        {
            Health playerHealth = collision.GetComponent<Health>();
            if(playerHealth.GetCurrentHealth() < playerHealth.GetMaxHealth())
            {
                collision.GetComponent<PlayerNew>().CreateHealthMessage(amount);
                playerHealth.RestoreHealth(amount);
                Destroy(gameObject);
            }
        }
    }
}
