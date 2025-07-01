using Cinemachine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Bomb : Bullet
{
    [SerializeField] GameObject Explosion; // The colored explosion of the bomb
    CinemachineImpulseSource _impulseSource;
    Bullet _bulletBase; // The inherited variables and methods of its parent Bullet script
    LayerMask _hitable; // What objects cause the bomb to explode

    private void OnEnable() // This Monobehaviour Function overwrites the bullet parent's equivalent. It not only gets the bullet attributes, but sends it flying from the player
    {
        _bulletBase = GetComponent<Bullet>();
        _hitable = _bulletBase.GetHitable();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _bulletBase.SetCurrentAngledVelocity(transform.right);
        StartCoroutine(BulletLife()); // This calls the Bomb's own coroutine rather than the parent bullet's
        GetComponent<Rigidbody2D>().velocity = _bulletBase.GetCurrentAngledVelocity() * _bulletBase.GetSpeed();
    }

    private void OnDisable()
    {
        StopCoroutine(BulletLife());
    }

    new IEnumerator BulletLife() // After a short amount of time, the bomb will explode
    {
        yield return new WaitForSeconds(_bulletBase.GetDuration());
        Explode();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GetComponent<Rigidbody2D>().IsTouchingLayers(_hitable)) // If the bomb touches anything that is hitable, it will explode.
        {
            if (collision.gameObject.GetComponent<PlayerInput>())
            {
                PlayerInput opposingPlayer = collision.gameObject.GetComponent<PlayerInput>();
                if (_bulletBase.bulletOfPlayersWeapon != opposingPlayer.playerNum)
                {
                    Explode();
                }
            }
            else if (collision.gameObject.GetComponent<Bomb>()) // As of now, the bomb has its own case where it collides with another of the same type. A future revamp of the bullet script will address this as well
            {
                /* 
                 * Note: If two of the player's own bombs explode, the resulting explosion will deal twice the damage.
                 * Although such behavior might be considered abnormal, lining up the bombs correctly to hit the opponent requires a lot of skill 
                 */
                Bomb opposingBomb = collision.gameObject.GetComponent<Bomb>();
                opposingBomb.Explode();
                Explode();
            }
            else if (collision.gameObject.GetComponent<Bullet>())
            {
                Bullet opposingBullet = collision.gameObject.GetComponent<Bullet>();
                opposingBullet.DetermineBulletDestruction();
                Explode();
            }
        }
    }

    public void Explode()
    {
        _impulseSource.GenerateImpulse();
        Instantiate(Explosion, transform.position, transform.rotation);
        _bulletBase.DetermineBulletDestruction(); // Ditto for bomb
    }
}
