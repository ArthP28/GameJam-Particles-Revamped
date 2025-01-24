using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Weapon))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerNew : MonoBehaviour
{
    // The main player script. All of the player's main attributes are stored here
    public static PlayerNew Instance { get; private set; }

    // Actions
    public event Action JumpEvent;
    public event Action ShootEvent;

    // Serializeables
    [SerializeField] float moveSpeed = 20f; // How fast the player moves
    [SerializeField] ParticleSystem antiGravEffect; // The effect that plays when the player is upside down
    [Tooltip("Index 0: Death by Opposing Player\nIndex 1: Self-Destruct (Accidental Death)")]
    [SerializeField] GameObject[] DeathExplosions = new GameObject[2]; // The player explodes upon dying

    // Main Variables
    SpriteRenderer _playerSprite;
    Rigidbody2D _rigidbody; // Used to check collisions and physics
    Animator _anim;
    Vector2 _spawnPoint; // The player will always spawn in their starting position
    Weapon _weapon; // Weapon attributes are handled in a separate script
    BoxCollider2D _collider;
    float currentGravityForce; // Determines the player's gravity-defying ability

    // Booleans
    bool isAlive = true; // If the player has any health left
    bool isFacingRight = true;
    bool antiGravEnabled = false;

    // Custom Variables
    Health _health;
    PlayerInput _playerInput;
    FrameInput _frameInput;
    private void Awake()
    {
        _weapon = GetComponent<Weapon>();
    }

    // Set all variables upon start
    void Start()
    {
        if (Instance == null) { Instance = this; }
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        
        _anim = GetComponentInChildren<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _health = GetComponent<Health>();
        _collider = GetComponent<BoxCollider2D>();
        _playerInput = GetComponent<PlayerInput>();
        _spawnPoint = transform.position;
        antiGravEffect.Stop();
        currentGravityForce = _rigidbody.gravityScale;
    }

    // Allow player to jump or shoot
    private void OnEnable()
    {
        JumpEvent += InvertGravity;
        ShootEvent += Shoot;
    }

    private void OnDestroy()
    {
        JumpEvent -= InvertGravity;
        ShootEvent -= Shoot;
    }

    void Update() // Controls are calculated in this method
    {
        if (isAlive)
        {
            GatherInput();
            Move();
            Jump();
            Fire();
            FlipSprite();
            Die();
            IsGrounded();
        }
    }

    void GatherInput() // Receive the current state of the player controls every frame
    {
        _frameInput = _playerInput.FrameInput;
    }

    void Move() // Calculates movement
    {
        Vector2 playerVelocity = new Vector2(_frameInput.Move.x * moveSpeed, _rigidbody.velocity.y);
        _rigidbody.velocity = playerVelocity;
        if (Mathf.Abs(playerVelocity.x) >= Mathf.Epsilon && _anim.GetBool("OnGround"))
        {
            _anim.SetFloat("PlayerSpeed", 1);
        }
        else
        {
            _anim.SetFloat("PlayerSpeed", 0);
        }
    }

    void Jump() // Handles "Jumping"
    {
        if (!_frameInput.Jump) return;
        JumpEvent?.Invoke();
    }

    void InvertGravity() // JumpEvent's function that changes the player's gravity
    {
        currentGravityForce = _rigidbody.gravityScale * -1;
        _rigidbody.gravityScale = currentGravityForce;

        // Check if player is in antigravity mode, then activate the effect for it
        if (currentGravityForce < 0)
        {
            antiGravEffect.Play(); // Antigravity is enabled
        }
        else
        {
            antiGravEffect.Stop(); // Antigravity is disabled
        }
    }

    void Fire() // Handles Shooting
    {
        if (!_frameInput.Fire) return;
        ShootEvent?.Invoke();
    }
    void Shoot() // ShootEvent's function that fires a bullet from the player's gun
    {
        _anim.SetTrigger("Shoot");
        _weapon.Fire();
    }

    void FlipSprite() // Sprite changes directions based on player input
    {
        // Vartical Flipping
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
        // Horizontal Flipping
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

    void Die() // When the player dies, he will lose a life, lose his powerup, and respawn back at the start
    {
        if (_health.GetCurrentHealth() <= 0 && isAlive)
        {
            // Player is not alive anymore
            Debug.Log("Player 1 Dead");
            isAlive = false;

            // Player explodes and disappears
            Instantiate(DeathExplosions[0], transform.position, Quaternion.identity);
            _playerSprite.gameObject.SetActive(false);
            antiGravEffect.Stop();
            _weapon.LoseUpgrade(); // Player loses his upgraded weapon upon dying

            // Turn off player gravity and velocity
            _rigidbody.gravityScale = 0;
            _rigidbody.velocity = Vector2.zero;

            // Disable collider and player's controls
            _collider.enabled = false;
            _playerInput.enabled = false;
            OnDestroy();
        }
    }

    public IEnumerator WaitAndRespawn() // Player spawns back at the start if he still has any lives left; This method is actually called in the Player's health script
    {
        yield return new WaitForSeconds(3f);
        isAlive = true;
        _playerInput.enabled = true;
        _playerSprite.gameObject.SetActive(true);
        _rigidbody.gravityScale = currentGravityForce;
        _collider.enabled = true;
        transform.position = _spawnPoint;
        _health.ResetHealth();
        OnEnable();
    }

    bool IsGrounded() // This function mainly governs the player's falling animation
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

    // Getters/Setters

    public Animator GetAnimations()
    {
        return _anim;
    }
}
