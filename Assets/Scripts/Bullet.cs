using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    // Serializeables
    [SerializeField] float speed = 20f; // How fast the bullet moves
    [SerializeField] int damage = 10; // How much damage it deals
    [SerializeField] float bulletDuration = 1f; // How long the bullet lasts before disappearing
    [Tooltip("Element 0: Default Impact - Used when player shoots a wall\n" +
        "Element 1: Damage Impact - Explosion is colored to the player's laser when hitting something that takes in damage")]
    [SerializeField] GameObject[] impactEffects = new GameObject[2]; // Bullet explodes upon impact
    // The maximum and minimum "angles" of direction when the bullet bounces off a wall
    [SerializeField] float MaxAngle = 72.5f;
    [SerializeField] float MinAngle = 25f;
    Vector2 currAngledVelocity = Vector2.zero;
    //[SerializeField] float timeBeforeNextCollision = 0.65f;
    [SerializeField] LayerMask _hitable; // What this bullet can hit beyond the ground
    [Range(0.0f, 1.0f)]
    float perpendicularThreshold = 0.7f;
    public PlayerNumber bulletOfPlayersWeapon; // What player does it belong to?
    Rigidbody2D rb;
    bool isCollided;
    //Coroutine collisionDelay;
    BulletPool _bulletPool; // The object pool where the bullet will inevitably return to
    string _bulletTag;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        var main = GetComponent<ParticleSystem>().main;
        main.startLifetime = bulletDuration;
         
    }

    private void Start()
    {
        _bulletTag = _bulletPool.getTagOfCurrentBullet();
    }

    private void OnEnable() // Every time the bullet enters onto the screen, it launches away from the player
    {
        StartCoroutine(BulletLife());
        currAngledVelocity = transform.right;
        rb.velocity = currAngledVelocity.normalized * speed;
        //isCollided = false;
    }

    private void OnDisable() // Disable the bullet's coroutine so that it does not linger even when firing again
    {
        StopCoroutine(BulletLife());
    }

    private void OnCollisionEnter2D(Collision2D collision) // Determines how the bullet will interact with the various objects it will hit
    {
        if (rb.IsTouchingLayers(_hitable)) // If it collides with anything that can be hit
        {
            if (collision.gameObject.GetComponent<PlayerInput>()) // If it hits an opponent, deal damage to them
            {
                PlayerInput opposingPlayer = collision.gameObject.GetComponent<PlayerInput>();
                if (bulletOfPlayersWeapon != opposingPlayer.playerNum)
                {
                    Debug.Log(opposingPlayer.playerNum + " Damaged");
                    DirectHit();
                    DetermineBulletDestruction();
                    opposingPlayer.GetComponent<Health>().DealDamage(damage);
                }
            }
            else if (collision.gameObject.GetComponent<Bullet>()) // If it hits another projectile, "destroy" both bullets
            {
                Bullet opposingBullet = collision.gameObject.GetComponent<Bullet>();
                opposingBullet.DetermineBulletDestruction();
                DetermineBulletDestruction();
                Miss();
            }
        } else // The bullet must have collided with a wall/the ground
        {
            ContactPoint2D collisionPoint = collision.contacts[0]; // Get the contact point of the ground with the bullet
            float directionAngle = Vector2.Angle(transform.right, collisionPoint.normal); // Calculate the angle from the bullet's firing direction to the normal of the collision point
            Debug.Log("Angle: " + directionAngle);
            if (isPerpendicular(directionAngle)) // Perpendicular: If the direction angle is equal to 0 or 180, both plus/minus threshold value
            {
                Bounce(); // Bullet is given a random opposite angle to travel
            }
            // Any non-perpendicular collisions are handled through the bullet's physics material (bouncy)
        }
    }
    /*IEnumerator WaitBeforeNextCollision()
    {
        isCollided = true;
        yield return new WaitForSeconds(timeBeforeNextCollision);
        isCollided = false;
    }*/

    /*public void Bounce()
    {
        if (!isCollided)
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
                rb.velocity = new Vector2(rb.velocity.x, randVelocityIndex).normalized * speed;
                //rb.AddForce(transform.up, -45f);
                //rb.
                //rb.SetRotation(45f);
                //rb.MoveRotation
                //transform.rotation = new Quaternion(transform.rotation.x, 45, 0, 0);
                isCollided = true;
                if (collisionDelay != null)
                {
                    StopCoroutine(collisionDelay);
                }
                collisionDelay = StartCoroutine(WaitBeforeNextCollision());
            }
            

        }
    }*/

    public void Bounce() // This method determines the supposed angle the bullet will travel in upon hitting the ground or at a wall; It is public so that all children of the bullet class will use it.
    {
        if (rb.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            // FUTURE: Find a method that actually changes the direction of the bullet rather than simple adding a velocity
            float randVelocityIndex = Random.Range(MinAngle, MaxAngle);
            float isNegative = Random.Range(0, 2);
            if (isNegative == 1)
            {
                randVelocityIndex *= -1;
            }
                /*
                if (randVelocityIndex >= -MinAngle && randVelocityIndex <= 0) // If the random negative angle is larger than the negative minimum, subtract more
                {
                    Debug.Log("Negative Minimum Exceeded");
                    randVelocityIndex -= MinAngle;
                }
                else if (randVelocityIndex <= MinAngle) // If the random positive angle is larger than the positive minimum, add more
                {
                    Debug.Log("Positive Minimum Exceeded");
                    randVelocityIndex += MinAngle;
                }
                */
            randVelocityIndex *= (3.14f / 180.0f);
            currAngledVelocity = new Vector2(Mathf.Cos(randVelocityIndex), Mathf.Sin(randVelocityIndex));
            Debug.Log(randVelocityIndex + " => " + currAngledVelocity);
            rb.velocity = currAngledVelocity * speed;
        }
    }
    // Both of these methods generate a bullet explosion. The first if the player hits an opponent and the second for anything else
    public void DirectHit()
    {
        Instantiate(impactEffects[1], transform.position, transform.rotation); // Colored Explosion - Player hit
    }

    public void Miss()
    {
        Instantiate(impactEffects[0], transform.position, transform.rotation); // Default Explosion - Hit
    }
    public IEnumerator BulletLife() // The bullet lasts for a couple of seconds before it disperses
    {
        yield return new WaitForSeconds(bulletDuration);
        Debug.Log("Bullet released");
        DetermineBulletDestruction();
    }

    public void DetermineBulletDestruction() // Determines how the bullet will be handled
    {
        /*
         The Pool's exception: If a powerup is activated/changed and any leftover bullets are still active, instead of releasing them back into the pool and risking a mixup, all active bullets are destroyed on the spot. 
         The bullet destruction is handled outside the pool script and not upon releasing because the pool may get the bullet that was just destroyed and an null error will occur.
        */
        // Condition 1: Default Bullets spawned just before the powerup was collected
        // Condition 2: Powerup Bullets spawned just before another powerup was collected
        // Condition 3: Powerup Bullets spawned just before the current powerup's effect was depleted
        if ((_bulletPool.powerUpEnabled() && !(gameObject.tag == "PowerUp")) || (_bulletPool.powerUpEnabled() && _bulletTag != _bulletPool.getTagOfCurrentBullet()) || (!_bulletPool.powerUpEnabled() && (gameObject.tag == "PowerUp")))
        {
            Destroy(gameObject);
        }
        else
        {
            _bulletPool.GetPool().Release(this);
        }
    }

    // Getters/Setters
    public int GetDamage()
    {
        return damage;
    }

    public float GetDuration()
    {
        return bulletDuration;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public LayerMask GetHitable()
    {
        return _hitable;
    }

    // This method is important in setting which pool the bullet will be obtained from and released to.
    public void SetPool(BulletPool pool)
    {
        _bulletPool = pool;
    }

    // This method is only for debugging, it accesses the pool that the bullet references to
    public BulletPool GetPoolOfBullet()
    {
        return _bulletPool;
    }

    public Rigidbody2D GetRb()
    {
        return rb;
    }

    public Vector2 GetCurrentAngledVelocity()
    {
        return currAngledVelocity;
    }

    public void SetCurrentAngledVelocity(Vector2 newVel)
    {
        currAngledVelocity = newVel;
    }

    bool isPerpendicular(float angle)
    {
        return (
            (angle <= perpendicularThreshold && angle >= -perpendicularThreshold) ||
            (angle <= 180.0f + perpendicularThreshold && angle >= 180.0f - perpendicularThreshold)
            );
    }
}
