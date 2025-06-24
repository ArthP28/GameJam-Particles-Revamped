using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Seeker : Bullet
{
    [SerializeField] float _steeringForce; // The value that determines how high/low the bullet should fly when seeking
    PlayerNew _target;
    Bullet _bulletBase; // The inherited variables and methods of its parent Bullet script
    LayerMask _hitable; // What this bullet can hit and cause damage

    private void OnEnable() // This Monobehaviour Function overwrites the bullet parent's equivalent. It not only gets the bullet attributes, but sends it flying from the player
    {
        _target = findOpponent();
        _bulletBase = GetComponent<Bullet>();
        _hitable = _bulletBase.GetHitable();
        _bulletBase.SetCurrentAngledVelocity(transform.right);
        StartCoroutine(_bulletBase.BulletLife()); // This calls the Bomb's own coroutine rather than the parent bullet's
        GetComponent<Rigidbody2D>().velocity = _bulletBase.GetCurrentAngledVelocity().normalized * _bulletBase.GetSpeed();
    }

    PlayerNew findOpponent()
    {
        PlayerNew opponent = null;
        PlayerNew[] allPlayers = FindObjectsOfType<PlayerNew>();
        foreach (PlayerNew player in allPlayers)
        {
            if (bulletOfPlayersWeapon != player.GetComponent<PlayerInput>().playerNum)
            {
                opponent = player;
            }
        }
        return opponent;
    }

    // Update is called once per frame
    void Update()
    {
        //Vertical Steering
        Steer();
    }

    void Steer()
    {
        Vector2 distanceFromTarget = _target.transform.position - transform.position;
        Debug.Log(_target.transform.position);
        Vector2 normalizedDistance = Vector2.zero;
        if(Mathf.Abs(distanceFromTarget.y) > 0.0f)
        {
            normalizedDistance = distanceFromTarget.normalized;
        }
        float desiredVertical = normalizedDistance.y;
        desiredVertical *= _bulletBase.GetSpeed();
        float steeringSpeed = (desiredVertical - GetComponent<Rigidbody2D>().velocity.y) * _steeringForce;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, steeringSpeed));
    }

    private void OnCollisionEnter2D(Collision2D collision) // Determines how the bullet will interact with the various objects it will hit
    {
        if (GetComponent<Rigidbody2D>().IsTouchingLayers(_hitable)) // If it collides with anything that can be hit
        {
            if (collision.gameObject.GetComponent<PlayerInput>()) // If it hits an opponent, deal damage to them
            {
                PlayerInput opposingPlayer = collision.gameObject.GetComponent<PlayerInput>();
                if (bulletOfPlayersWeapon != opposingPlayer.playerNum)
                {
                    Debug.Log(opposingPlayer.playerNum + " Damaged");
                    _bulletBase.DirectHit();
                    DetermineBulletDestruction();
                    opposingPlayer.GetComponent<Health>().DealDamage(_bulletBase.GetDamage());
                }
            }
            else if (collision.gameObject.GetComponent<Bullet>()) // If it hits another projectile, "destroy" both bullets
            {
                Bullet opposingBullet = collision.gameObject.GetComponent<Bullet>();
                opposingBullet.DetermineBulletDestruction();
                DetermineBulletDestruction();
                _bulletBase.Miss();
            }
        }
        else // The bullet must have collided with a wall/the ground
        {
            // Destroy bullet
            DetermineBulletDestruction();
            _bulletBase.Miss();
        }
    }
}
