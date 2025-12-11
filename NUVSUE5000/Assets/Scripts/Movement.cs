using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    [SerializeField] Rigidbody2D playerRB;
    [SerializeField] float playerSpeed;
    [SerializeField] float jumpPower;
    [Header("InputLinks")]
    [SerializeField] InputAction movementAction;
    [SerializeField] InputAction jumpAction;
    [SerializeField] InputAction fireAction;
    bool grounded;
    void Start()
    {
        
    }

    void Update()
    {
        checkMovement();
    }

    void checkMovement()
    {
        if (jumpAction.WasPressedThisFrame() == true && grounded)
        {

        }
    }
}
