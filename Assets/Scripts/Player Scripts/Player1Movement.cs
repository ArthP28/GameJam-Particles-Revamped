using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1Movement : MonoBehaviour, PlayerControls.IPlayer1Actions
{
    public Vector2 MovementValue { get; private set; }
    PlayerControls _controls;
    public event Action JumpEvent;

    [SerializeField] float moveSpeed = 20f; // How fast the player moves
    [SerializeField] ParticleSystem antiGravEffect;

    Health _health;
    Rigidbody2D _rigidbody; // Used to check collisions and physics
    Animator _anim;

    bool isAlive = true; // If the player has any health left
    bool isFacingRight = true;
    bool isGrounded = true;
    bool antiGravEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        _controls = new PlayerControls();
        _controls.Player1.SetCallbacks(this);
        _controls.Player1.Enable();

        _anim = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
        antiGravEffect = GetComponentInChildren<ParticleSystem>();
        JumpEvent += Jump;
    }

    void OnDestroy()
    {
        _controls.Disable();
        JumpEvent -= Jump;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        FlipSprite();
    }

    void Move()
    {
        //float control = Input.GetAxis("Horizontal");
        Debug.Log(MovementValue);
        Vector2 playerVelocity = new Vector2(MovementValue.x * moveSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity = playerVelocity;
        if (Mathf.Abs(playerVelocity.x) > 0 && _anim.GetBool("OnGround"))
        {
            _anim.SetFloat("PlayerSpeed", 1);
        }
        else
        {
            _anim.SetFloat("PlayerSpeed", 0);
        }
    }

    void Jump()
    {
        _rigidbody.gravityScale = _rigidbody.gravityScale * -1;

        // Check if player is off the ground, then activate the animation for it

        if (_rigidbody.gravityScale < 0)
        {
                antiGravEffect.Play(); // Antigravity is enabled
        }
        else
        {
            antiGravEffect.Stop(); // Antigravity is disabled
        }
    }

    void FlipSprite()
    {
        if (_rigidbody.gravityScale < 0 && !antiGravEnabled) // Player is rightside up -> will turn UPSIDE DOWN
        {
            antiGravEnabled = true;
            transform.Rotate(180f, 0f, 0f);
        }
        else if (_rigidbody.gravityScale > 0 && antiGravEnabled) // Player is UPSIDE DOWN -> will turn rightside up
        {
            antiGravEnabled = false;
            transform.Rotate(180f, 0f, 0f);
        }
        if (_rigidbody.velocity.x > 0 && !isFacingRight) // Player is facing LEFT -> will turn right
        {
            isFacingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (_rigidbody.velocity.x < 0 && isFacingRight) // Player is facing right -> will turn LEFT
        {
            isFacingRight = false;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        JumpEvent?.Invoke();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }
}
