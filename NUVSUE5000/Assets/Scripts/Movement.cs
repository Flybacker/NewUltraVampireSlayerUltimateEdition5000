using Mono.Cecil;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] Collider2D groundCheck;

    [SerializeField] float playerSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] float dashPower;

    [SerializeField] GameObject bullet;

    InputAction movementAction;
    InputAction jumpAction;
    InputAction fireAction;

    bool canDash;
    bool grounded;
    bool inDash;

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
        fireCooldown -= Time.deltaTime;
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
        playerRB.linearVelocityX = movementAction.ReadValue<Vector2>().x * dashPower;
        if (Mathf.Abs(playerRB.linearVelocityX) > 1)
        {
            yield return new WaitForSeconds(0.05f);
        }
        playerRB.gravityScale = defaultGravity;
        inDash = false;
    }


    void checkGrounded()
    {
        grounded = groundCheck.IsTouchingLayers(1 << 3);

        if (grounded)
        {
            canDash = true;
        }
    }
}