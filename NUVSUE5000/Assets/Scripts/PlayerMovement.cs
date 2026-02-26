using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] Collider2D groundCheck;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform cameraTarget;

    [Header("Juice")]
    [SerializeField] ParticleSystem PlayerWalkingSmoke;
    [SerializeField] ParticleSystem PlayerJumpSmoke;
    [SerializeField] ParticleSystem PlayerDashParticle;

    [Header("Movement")]
    [SerializeField] float playerSpeed = 6f;
    [SerializeField] float jumpPower = 12f;
    [SerializeField] float dashPower = 20f;
    [SerializeField] float coyoteTime = 0.15f;

    InputAction movementAction;
    InputAction jumpAction;
    InputAction fireAction;
    AudioManager audioManager;

    bool canDash;
    bool grounded;
    bool inDash;

    float coyoteTimer;
    float defaultGravity;
    float lastDirection = 1f;
    float fireCooldown;

    void Awake()
    {
        movementAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        fireAction = InputSystem.actions.FindAction("Attack");
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        defaultGravity = playerRB.gravityScale;
        movementAction.Enable();
        jumpAction.Enable();
        fireAction.Enable();
    }


    void Update()
    {
        CheckGrounded();
        HandleMovement();
        HandleGunFire();
        cameraTargetController();

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
            audioManager.playSFX(audioManager.Jump);
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
            playerRB.linearVelocity = new Vector2(moveInput.x * playerSpeed,playerRB.linearVelocity.y);
        }
    }

    IEnumerator Dash()
    {
        inDash = true;
        audioManager.playSFX(audioManager.Dash);
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
        if (fireAction.IsPressed() && fireCooldown <= 0 && bullet != null)
        {
            Quaternion rot;
            if (lastDirection >= 0)
            {
                rot = Quaternion.identity;
            }
            else
            {
                rot = Quaternion.Euler(0, 180, 0);
            }

            Instantiate(bullet, transform.position, rot);
            audioManager.playSFX(audioManager.Shot);
            fireCooldown = 0.2f;
        }
    }

    void CheckGrounded()
    {
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

    void cameraTargetController()
    {
        if (movementAction.ReadValue<Vector2>().x == 0 && !inDash)
        {
            cameraTarget.position = transform.position;
        }
        if (movementAction.ReadValue<Vector2>().x != 0 && !inDash)
        {
            cameraTarget.position = transform.position + new Vector3(movementAction.ReadValue<Vector2>().x * 3, 0, 0);
        }
        if (movementAction.ReadValue<Vector2>().x == 0 && inDash)
        {
            cameraTarget.position = transform.position + new Vector3(lastDirection * 3, 0, 0);
        }
        if (movementAction.ReadValue<Vector2>().x != 0 && inDash)
        {
            cameraTarget.position = transform.position + new Vector3(movementAction.ReadValue<Vector2>().x * 3, 0, 0);
        }
    }
}