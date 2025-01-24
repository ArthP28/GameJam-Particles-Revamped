using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Bomb : Bullet
{
    [SerializeField] GameObject Explosion;
    [SerializeField] Collider2D mainCollider;

    void Start()
    {
        GetComponent<Bullet>().GetComponent<Rigidbody2D>().velocity = transform.right * GetComponent<Bullet>().GetSpeed();
        StartCoroutine(TimeBeforeExplosion());
    }

    IEnumerator TimeBeforeExplosion()
    {
        yield return new WaitForSeconds(GetComponent<Bullet>().GetDuration());
        Instantiate(Explosion, transform);
        Destroy(gameObject);
    }

    bool hasCollided = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasCollided)
        {
            if (collision.GetComponent<Player2Movement>())
            {
                Instantiate(Explosion, transform);
                Destroy(gameObject);
                hasCollided = true;
            }
            else if (collision.GetComponent<Player2Bullet>())
            {
                Instantiate(Explosion, transform);
                Destroy(gameObject);
                //GetComponent<Bullet>().Miss();
                hasCollided = true;
            }
        }
    }
}
