using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] Collider2D groundCheck;

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] float playerSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float dashPower;
    [SerializeField] float coyoteTime;

    [SerializeField] GameObject bullet;

    [SerializeField] Transform cameraTarget;

    InputAction movementAction;
    InputAction jumpAction;
    InputAction fireAction;

    bool canDash;
    bool grounded;
    bool inDash;

    float coyoteTimer;

    float defaultGravity;

    float lastDirection;

    float fireCooldown;
    void Start()
    {
        movementAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        fireAction = InputSystem.actions.FindAction("Attack");

        defaultGravity = playerRB.gravityScale;
    }

    void Update()
    {
        checkGrounded();
        checkMovement();
        gunFire();
        cameraTargetController();
        fireCooldown -= Time.deltaTime;
        coyoteTimer -= Time.deltaTime;
    }


    void gunFire()
    {
        if (fireAction.IsPressed() && fireCooldown < 0 && lastDirection >= 0)
        {
            Instantiate(bullet, transform.position, Quaternion.Euler(0,0,0));
            fireCooldown = 0.2f;
        }
        else if (fireAction.IsPressed() && fireCooldown < 0 && lastDirection < 0)
        {
            Instantiate(bullet, transform.position, Quaternion.Euler(0,180,0));
            fireCooldown = 0.2f;
        }
    }

    void checkMovement()
    {
        if (jumpAction.IsPressed() && grounded)
        {
            playerRB.linearVelocityY = jumpPower;
            coyoteTimer = 0;
        }
        else if (jumpAction.WasPerformedThisFrame() && canDash)
        {
            canDash = false;
            StartCoroutine(Dash());
        }
        if (!inDash)
        {
            playerRB.linearVelocityX = movementAction.ReadValue<Vector2>().x * playerSpeed;
        }
        if (movementAction.ReadValue<Vector2>().x != 0)
        {
            lastDirection = movementAction.ReadValue<Vector2>().x;
        }
    }

    IEnumerator Dash()
    {
        inDash = true;
        playerRB.gravityScale = 0;
        playerRB.linearVelocityY = 0;
        playerRB.linearVelocityX = 0;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.hotPink;
        if (movementAction.ReadValue<Vector2>().x != 0)
        {
            playerRB.linearVelocityX = movementAction.ReadValue<Vector2>().x * dashPower;
        }
        else if (movementAction.ReadValue<Vector2>().x == 0)
        {
            playerRB.linearVelocityX = lastDirection * dashPower;
        }
        Time.timeScale = 0.30f;
        if (Mathf.Abs(playerRB.linearVelocityX) > 1)
        {
            yield return new WaitForSeconds(0.05f);
        }
        Time.timeScale = 1f;
        playerRB.gravityScale = defaultGravity;
        spriteRenderer.color = Color.white;
        inDash = false;
    }


    void checkGrounded()
    {
        bool currentlyGrounded = groundCheck.IsTouchingLayers(1 << 3);

        if (currentlyGrounded)
        {
            canDash = true;
            coyoteTimer = coyoteTime;
            grounded = true;
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