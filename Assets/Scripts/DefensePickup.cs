using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefensePickup : PickUp
{
    [SerializeField] Shield[] playerShields;
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
        }
    }

    void CreateShield(PlayerInput player, Shield shield)
    {
        Instantiate(shield, player.transform);
        shield.transform.position = new Vector2(0.22f, -0.89f);
    }
}
