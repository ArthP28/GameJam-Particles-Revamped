using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensePickup : PickUp
{
    [SerializeField] Shield[] playerShields;
    [SerializeField] Sprite TimerFillIcon;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerInput>())
        {
            PlayerInput playerPickingUp = collision.GetComponent<PlayerInput>();
            if (playerPickingUp.playerNum == PlayerNumber.Player1) // Player 1
            {
                CreateShield(playerPickingUp, playerShields[0]);
                Debug.Log("Player 1 obtains " + name);
                Destroy(gameObject);
            }
            else if (playerPickingUp.playerNum == PlayerNumber.Player2) // Player 2
            {
                CreateShield(playerPickingUp, playerShields[1]);
                Debug.Log("Player 2 obtains " + name);
                Destroy(gameObject);
            }
            collision.GetComponent<PlayerNew>().CreateEffectTimer(powerUpDuration, TimerFillIcon);
            collision.GetComponent<PlayerNew>().CreatePickUpMessage(PowerUpName);
        }
    }

    void CreateShield(PlayerInput player, Shield shield)
    {
        if (!player.GetComponentInChildren<Shield>())
        {
            Instantiate(shield, player.transform);
        } else
        {
            player.GetComponentInChildren<Shield>().ActivateShield();
        }
        shield.transform.position = new Vector2(0.22f, -0.89f);
    }
}
