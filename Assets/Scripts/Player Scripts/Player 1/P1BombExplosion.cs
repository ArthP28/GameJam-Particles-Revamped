using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class P1BombExplosion : ExplosionScript
{
    int damage = 20;
    LayerMask _hitable;
    [SerializeField] GameObject impactEffect;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player2Movement>())
        {
            Instantiate(impactEffect, transform.position, transform.rotation); // Colored Explosion - Player hit
            collision.GetComponent<Player2Movement>().GetComponent<Health>().DealDamage(damage);
        }
    }
}
