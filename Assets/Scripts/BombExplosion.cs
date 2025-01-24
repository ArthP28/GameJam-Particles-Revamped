using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : ExplosionScript
{
    [SerializeField] LayerMask _hitable; // What is affected by the blast radius
    [SerializeField] int damage; // How much damage it will deal
    [SerializeField] GameObject impactEffect; // The overall effect
    public PlayerNumber ExplosionofPlayersBomb; // What player detonated the bomb
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D collision) // If anyting that is hitable enters its blast radius, it takes damage
    {
        if (rb.IsTouchingLayers(_hitable))
        {
            if (collision.gameObject.GetComponent<PlayerInput>())
            {
                PlayerInput opposingPlayer = collision.gameObject.GetComponent<PlayerInput>();
                if (ExplosionofPlayersBomb != opposingPlayer.playerNum)
                {
                    Debug.Log(opposingPlayer.playerNum + " Damaged");
                    Instantiate(impactEffect, transform.position, transform.rotation); // Colored Explosion - Player hit
                    opposingPlayer.GetComponent<Health>().DealDamage(damage);
                }
            }
        }
    }
}
