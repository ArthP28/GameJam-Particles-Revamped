using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2Movement : MonoBehaviour, PlayerControls.IPlayer2Actions
{
    public Vector2 MovementValue { get; private set; }
    PlayerControls _controls;
    public event Action JumpEvent;
    public event Action ShootEvent;

    [SerializeField] float moveSpeed = 20f; // How fast the player moves
    [SerializeField] ParticleSystem antiGravEffect;
    [Tooltip("Index 0: Death by Opposing Player\nIndex 1: Self-Destruct (Accidental Death)")]
    [SerializeField] GameObject[] DeathExplosions = new GameObject[2];
    SpriteRenderer _playerSprite;

    Health _health;
    Rigidbody2D _rigidbody; // Used to check collisions and physics
    Animator _anim;
    Vector2 _spawnPoint;
    Player2Weapon _weapon;
    BoxCollider2D _collider;
    float currentGravityForce;

    bool isAlive = true; // If the player has any health left
    bool isFacingRight = true;
    bool antiGravEnabled = false;

    // Start is called before the first frame update
    void Start()
    {
        _controls = new PlayerControls();
        _controls.Player2.SetCallbacks(this);
        _controls.Player2.Enable();

        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        antiGravEffect.Stop();
        _weapon = GetComponent<Player2Weapon>();
        _spawnPoint = transform.position;
        _anim = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
        _collider = GetComponent<BoxCollider2D>();
        currentGravityForce = _rigidbody.gravityScale;
        JumpEvent += Jump;
        ShootEvent += Shoot;
    }

    void OnDestroy()
    {
        _controls.Disable();
        JumpEvent -= Jump;
        ShootEvent -= Shoot;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        FlipSprite();
        Die();
        IsGrounded();
        // Also check if player is grounded
    }

    void Move()
    {
        //float control = Input.GetAxis("Horizontal");
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
            Debug.Log(antiGravEffect.isPlaying);
        }
        else
        {
            antiGravEffect.Stop(); // Antigravity is disabled
        }
    }
    void Shoot()
    {
        _anim.SetTrigger("Shoot");
        _weapon.Fire();
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
    void Die()
    {
        if (_health.GetCurrentHealth() <= 0 && isAlive)
        {
            Debug.Log("Player 2 Dead");
            _weapon.LoseUpgrade();
            isAlive = false;
            antiGravEffect.Stop();
            _playerSprite.gameObject.SetActive(false);
            Instantiate(DeathExplosions[0], transform.position, Quaternion.identity);
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = Vector2.zero;
            _collider.enabled = false;
            OnDestroy();
            //StartCoroutine(WaitAndRespawn());
        }
    }

    public IEnumerator WaitAndRespawn()
    {
        yield return new WaitForSeconds(3f);
        isAlive = true;
        _playerSprite.gameObject.SetActive(true);
        _rigidbody.gravityScale = currentGravityForce;
        _collider.enabled = true;
        transform.position = _spawnPoint;
        _health.ResetHealth();
        _controls.Player2.Enable();
        JumpEvent += Jump;
        ShootEvent += Shoot;
    }

    bool IsGrounded()
    {
        if (_rigidbody.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            _anim.SetBool("OnGround", true);
            return true;
        }
        else
        {
            _anim.SetBool("OnGround", false);
            return false;
        }
    }

    public void ChangeControlState(bool newState)
    {
        if (!newState)
        {
            OnDestroy();
        }
        else
        {
            _controls.Player2.Enable();
            JumpEvent += Jump;
            ShootEvent += Shoot;
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

    public void OnFire(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        ShootEvent?.Invoke();
    }
    public Animator GetAnimations()
    {
        return _anim;
    }
}
