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
    //[SerializeField] float timeBeforeNextCollision = 0.65f;
    [SerializeField] LayerMask _hitable; // What this bullet can hit beyond the ground
    public PlayerNumber bulletOfPlayersWeapon; // What player does it belong to?
    Rigidbody2D rb;
    //bool isCollided = false;
    //Coroutine collisionDelay;
    BulletPool _bulletPool; // The object pool where the bullet will inevitably return to
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() // Every time the bullet enters onto the screen, it launches away from the player
    {
        StartCoroutine(BulletLife());
        rb.velocity = transform.right * speed;
        //isCollided = false;
    }

    private void OnDisable() // Disable the bullet's coroutine so that it does not linger even when firing again
    {
        StopCoroutine(BulletLife());
    }

    private void OnCollisionEnter2D(Collision2D collision) // Determines how the bullet will interact with the various objects it will hit
    {
        Bounce();
        if (rb.IsTouchingLayers(_hitable))
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
            float randVelocityIndex = Random.Range(-MaxAngle, MaxAngle);
            if (randVelocityIndex >= -MinAngle && randVelocityIndex <= 0) // If the random negative angle is larger than the negative minimum, subtract more
            {
                randVelocityIndex -= MinAngle;
            }
            else if (randVelocityIndex <= MinAngle) // If the random positive angle is larger than the positive minimum, add more
            {
                randVelocityIndex += MinAngle;
            }
            rb.velocity = new Vector2(rb.velocity.x, randVelocityIndex).normalized * speed; // Add the random "angle" as the bullet's vertical velocity to change its direction
            //rb.AddForce(transform.up, -45f);
            //rb.
            //rb.SetRotation(45f);
            //rb.MoveRotation
            //transform.rotation = new Quaternion(45, 45, 0, 0);
        }
    }
    // Both of these methods generate a bullet explosion. The first if the player hits an opponent and the second for anything else
    void DirectHit()
    {
        Instantiate(impactEffects[1], transform.position, transform.rotation); // Colored Explosion - Player hit
    }

    void Miss()
    {
        Instantiate(impactEffects[0], transform.position, transform.rotation); // Default Explosion - Hit
    }
    private IEnumerator BulletLife() // The bullet lasts for a couple of seconds before it disperses
    {
        yield return new WaitForSeconds(bulletDuration);
        Debug.Log("Bullet released");
        DetermineBulletDestruction();
    }

    public void DetermineBulletDestruction() // Determines how the bullet will be handled
    {
        /*
         The Pool's exception: If a powerup is activated/changed and any leftover bullets are still active, instead of releasing them back into the pool and risking a mixup, all active bullets are destroyed on the spot. 
         The bullet destruction is handled outside the pool and not upon releasing because the pool may get the bullet that it just destroyed and an null error will occur.
        */
        if ((_bulletPool.powerUpEnabled() && !(gameObject.tag == "PowerUp")) || ((!_bulletPool.powerUpEnabled() || _bulletPool.powerUpChanged()) && (gameObject.tag == "PowerUp")))
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
}
