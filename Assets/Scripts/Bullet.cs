using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 20f;
    [SerializeField] int damage = 10;
    [SerializeField] float bulletDuration = 1f;
    [Tooltip("Element 0: Default Impact - Used when player shoots a wall\n" +
        "Element 1: Damage Impact - Explosion is colored to the player's laser when hitting something that takes in damage")]
    [SerializeField] GameObject[] impactEffects = new GameObject[2];
    [SerializeField] float MaxAngle = 72.5f;
    [SerializeField] float MinAngle = 25f;
    Rigidbody2D rb;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * speed;
        StartCoroutine(BulletLife());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            float randVelocityIndex = Random.Range(-MaxAngle, MaxAngle);
            if (randVelocityIndex >= -MinAngle && randVelocityIndex <= 0)
            {
                randVelocityIndex -= MinAngle;
            }
            else if (randVelocityIndex <= MinAngle)
            {
                randVelocityIndex += MinAngle;
            }
            rb.velocity = new Vector2(rb.velocity.x, randVelocityIndex);
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    public void DirectHit()
    {
        Instantiate(impactEffects[1], transform.position, transform.rotation); // Colored Explosion - Player hit
    }

    public void Miss()
    {
        Instantiate(impactEffects[0], transform.position, transform.rotation); // Colored Explosion - Player hit
    }

    private IEnumerator BulletLife()
    {
        yield return new WaitForSeconds(bulletDuration);
        Destroy(gameObject);
    }

    public float GetDuration()
    {
        return bulletDuration;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
