using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] Collider2D groundCheck;

    [SerializeField] float playerSpeed;
    [SerializeField] float jumpPower;

    InputAction movementAction;
    InputAction jumpAction;
    InputAction fireAction;

    bool canDash;
    bool grounded;
    void Start()
    {
        movementAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        fireAction = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        checkGrounded();
        checkMovement();
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
            dash();
        }
        playerRB.linearVelocityX = movementAction.ReadValue<Vector2>().x * playerSpeed;
    }

    void dash()
    {

    }


    void checkGrounded()
    {
        grounded = groundCheck.IsTouchingLayers(1 << 3);
    }
}