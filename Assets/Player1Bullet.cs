using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Bullet : Bullet
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player2Movement>())
        {
            GetComponent<Bullet>().DirectHit();
            Destroy(gameObject);
            collision.GetComponent<Player2Movement>().GetComponent<Health>().DealDamage(GetComponent<Bullet>().GetDamage());
        }
        else if (collision.GetComponent<Player2Bullet>())
        {
            GetComponent<Bullet>().Miss();
            Destroy(gameObject);
        }
        else if (collision.GetComponent<Player1Movement>() || collision.GetComponent<Bullet>())
        {
            Destroy(gameObject);
        }
    }
}
