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

    void Update()
    {
        if (rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            float randVelocityIndex = Random.Range(-72.5f, 72.5f);
            if(randVelocityIndex >= -25f && randVelocityIndex <= 0)
            {
                randVelocityIndex -= 25f;
            } else if (randVelocityIndex <= 25f)
            {
                randVelocityIndex += 25f;
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
}
