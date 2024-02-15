using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Bomb : Bullet
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
        Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    bool hasCollided = false;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasCollided)
        {
            if (collision.GetComponent<Player1Movement>())
            {
                Instantiate(Explosion, transform.position, transform.rotation);
                Destroy(gameObject);
                hasCollided = true;
            }
            else if (collision.GetComponent<Player1Bullet>())
            {
                Instantiate(Explosion, transform.position, transform.rotation);
                Destroy(gameObject);
                //GetComponent<Bullet>().Miss();
                hasCollided = true;
            }
        }
    }
}
