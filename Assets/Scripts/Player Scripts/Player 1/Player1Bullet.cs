using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Bullet : Bullet
{
    [SerializeField] Collider2D mainCollider;

    bool hasCollided = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasCollided)
        {
            if (collision.GetComponent<Player2Movement>())
            {
                GetComponent<Bullet>().DirectHit();
                Destroy(gameObject);
                collision.GetComponent<Player2Movement>().GetComponent<Health>().DealDamage(GetComponent<Bullet>().GetDamage());
                hasCollided = true;
            }
            else if (collision.GetComponent<Player2Bullet>())
            {
                Destroy(gameObject);
                GetComponent<Bullet>().Miss();
                hasCollided = true;
            }
            else if (collision.GetComponent<Player1Movement>() || collision.GetComponent<Bullet>())
            {
                StartCoroutine(PassThrough());
            }
        }
    }

    IEnumerator PassThrough()
    {
        mainCollider.enabled = false;
        yield return new WaitForSeconds(0.05f);
        mainCollider.enabled = true;
    }
}
