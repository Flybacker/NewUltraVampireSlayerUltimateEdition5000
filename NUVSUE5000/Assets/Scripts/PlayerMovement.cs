using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] Collider2D groundCheck;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject bullet;

    [Header("Juice")]
    [SerializeField] ParticleSystem PlayerWalkingSmoke;
    [SerializeField] ParticleSystem PlayerJumpSmoke;
    [SerializeField] ParticleSystem PlayerDashParticle;

    [Header("Movement")]
    [SerializeField] float playerSpeed = 6f;
    [SerializeField] float jumpPower = 12f;
    [SerializeField] float dashPower = 20f;
    [SerializeField] float coyoteTime = 0.15f;

    PlayerInput playerInput;
    InputAction movementAction;
    InputAction jumpAction;
    InputAction fireAction;

    bool canDash;
    bool grounded;
    bool inDash;

    float coyoteTimer;
    float defaultGravity;
    float lastDirection = 1f;
    float fireCooldown;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        movementAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        fireAction = playerInput.actions["Attack"];
    }

    void OnEnable()
    {
        movementAction.Enable();
        jumpAction.Enable();
        fireAction.Enable();
    }

    void OnDisable()
    {
        movementAction.Disable();
        jumpAction.Disable();
        fireAction.Disable();
    }

    void Start()
    {
        if (playerRB == null)
            playerRB = GetComponent<Rigidbody2D>();

        defaultGravity = playerRB.gravityScale;
    }

    void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleGunFire();

        fireCooldown -= Time.deltaTime;
        coyoteTimer -= Time.deltaTime;
    }

    void HandleMovement()
    {
        Vector2 moveInput = movementAction.ReadValue<Vector2>();

        if (moveInput.x != 0)
            lastDirection = Mathf.Sign(moveInput.x);

        // Jump
        if (jumpAction.WasPressedThisFrame() && grounded)
        {
            PlayerJumpSmoke.Play();
            playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, jumpPower);
            coyoteTimer = 0;
        }
        // Dash
        else if (jumpAction.WasPressedThisFrame() && canDash && !grounded)
        {
            canDash = false;
            StartCoroutine(Dash());
        }

        if (!inDash)
        {
            playerRB.linearVelocity = new Vector2(
                moveInput.x * playerSpeed,
                playerRB.linearVelocity.y
            );
        }
    }

    IEnumerator Dash()
    {
        inDash = true;
        playerRB.gravityScale = 0;
        playerRB.linearVelocity = Vector2.zero;

        spriteRenderer.color = Color.red;

        PlayerDashParticle.Play();

        yield return new WaitForSeconds(0.15f);

        playerRB.linearVelocity = new Vector2(lastDirection * dashPower, 0);

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;
        playerRB.gravityScale = defaultGravity;
        inDash = false;
    }

    void HandleGunFire()
    {
        if (!fireAction.IsPressed() || fireCooldown > 0 || bullet == null)
            return;

        Quaternion rot = lastDirection >= 0
            ? Quaternion.identity
            : Quaternion.Euler(0, 180, 0);

        Instantiate(bullet, transform.position, rot);
        fireCooldown = 0.2f;
    }

    void CheckGrounded()
    {
        if (groundCheck == null)
            return;

        bool currentlyGrounded = groundCheck.IsTouchingLayers(1 << 3);

        if (currentlyGrounded)
        {
            grounded = true;
            canDash = true;
            coyoteTimer = coyoteTime;
        }
        else if (coyoteTimer > 0)
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}