using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 20f;
    [SerializeField] float wallSlidingSpeed = 10f;
    [SerializeField] float jumpSpeed = 10f;

    bool isAlive = true;
    bool isFacingRight = true;
    bool antiGravEnabled = false;
    Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        //_collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Run();
            Jump();
            FlipSprite();
            //Die();
            //IsOnWall();
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
        }
    }

    void FlipSprite()
    {
        if (_rigidbody.gravityScale < 0 && !antiGravEnabled)
        {
            antiGravEnabled = true;
            transform.Rotate(180f, 0f, 0f);
        } else if (_rigidbody.gravityScale > 0 && antiGravEnabled)
        {
            antiGravEnabled = false;
            transform.Rotate(180f, 0f, 0f);
        }
        if (_rigidbody.velocity.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
            //transform.localScale = new Vector2(Mathf.Sign(_rigidbody.velocity.x), 1f);
            transform.Rotate(0f, 180f, 0f);
        }else if (_rigidbody.velocity.x < 0 && isFacingRight)
        {
            isFacingRight = false;
            transform.Rotate(0f, 180f, 0f);
        }
    }
}

