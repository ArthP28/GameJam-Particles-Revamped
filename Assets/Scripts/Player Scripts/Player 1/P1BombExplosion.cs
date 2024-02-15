using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1BombExplosion : ExplosionScript
{
    [SerializeField] int damage = 20;
    [SerializeField] GameObject impactEffect;
    bool hasCollided = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasCollided)
        {
            if (collision.GetComponent<Player2Movement>())
            {
                Instantiate(impactEffect, transform.position, transform.rotation); // Colored Explosion - Player hit
                collision.GetComponent<Player2Movement>().GetComponent<Health>().DealDamage(damage);
                hasCollided = true;
            }
        }
    }   
}
