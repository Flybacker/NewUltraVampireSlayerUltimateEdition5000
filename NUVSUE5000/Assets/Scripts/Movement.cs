using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpPower;
    [SerializeField] Collider2D groundCheck;


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
        Debug.Log(grounded);
    }

    void checkMovement()
    {
        if ((jumpAction.IsPressed() == true) && grounded)
        {
            playerRB.linearVelocityY = jumpPower;
        }
        else if ((jumpAction.WasPerformedThisFrame() == true) && canDash)
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