using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20f; // How fast the player moves
    [SerializeField] ParticleSystem antiGravEffect;

    Health _health;
    Rigidbody2D _rigidbody; // Used to check collisions and physics

    bool isAlive = true; // If the player has any health left
    bool isFacingRight = true;
    bool antiGravEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Run(); // Movement
            Jump(); // Flips gravity
            FlipSprite(); // Change sprite to face in the direction of input movement
            //Die();
        }
    }

    void Run()
    {
        float control = Input.GetAxis("Horizontal");
        Vector2 playerVelocity = new Vector2(control * moveSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity = playerVelocity;
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _rigidbody.gravityScale = _rigidbody.gravityScale * -1;
            if(_rigidbody.gravityScale < 0)
            {
                antiGravEffect.Play(); // Antigravity is enabled
            } else
            {
                antiGravEffect.Stop(); // Antigravity is disabled
            }
        }
    }

    void FlipSprite()
    {
        if (_rigidbody.gravityScale < 0 && !antiGravEnabled) // Player is rightside up -> will turn UPSIDE DOWN
        {
            antiGravEnabled = true;
            transform.Rotate(180f, 0f, 0f);
        } else if (_rigidbody.gravityScale > 0 && antiGravEnabled) // Player is UPSIDE DOWN -> will turn rightside up
        {
            antiGravEnabled = false;
            transform.Rotate(180f, 0f, 0f);
        }
        if (_rigidbody.velocity.x > 0 && !isFacingRight) // Player is facing LEFT -> will turn right
        {
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }else if (_rigidbody.velocity.x < 0 && isFacingRight) // Player is facing right -> will turn LEFT
        {
            isFacingRight = false;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}

