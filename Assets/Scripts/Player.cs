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
        //float control = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            /*if (_rigidbody.gravityScale > 0)
            {
                _rigidbody.gravityScale = -1;
            }
            else
            {
                _rigidbody.gravityScale = 1;
            }*/
            _rigidbody.gravityScale = _rigidbody.gravityScale * -1;
        }
    }

    void FlipSprite()
    {
        bool hasHorizontalSpeed = (Mathf.Abs(_rigidbody.velocity.x) > Mathf.Epsilon);
        //bool hasVerticalSpeed = (Mathf.Abs(_rigidbody.velocity.y) > Mathf.Epsilon);

        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(_rigidbody.velocity.x), 1f);
        }
    }
}

