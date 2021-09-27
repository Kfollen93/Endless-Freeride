using UnityEngine;

// This script adds the physics force behind all movement.
public class SphereMotorController : MonoBehaviour
{
    private float speed = 10f;
    private float jumpSpeed = 3.5f;
    private float maxSpeed = 50f;
    public Rigidbody rb; // Accessed by Respawn script
    private Player playerScript;
    private float moveHorizontal, moveVertical, moveVerticalJump;
    private Vector3 movement;
    private Vector3 jumpHeight = new Vector3(0f, 1f, 0f);
    private bool holdDownRightJoystick, flickUpRightJoystick;
    [SerializeField] private GameObject player;
    private bool carveLeft, carveRight;
    [HideInInspector] public bool playerHasJumped; // Accessed from Respawn script
    private float groundTimer = 0.2f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerScript = player.GetComponent<Player>();
        playerScript.SetJoyStickComponents();
    }

    private void FixedUpdate()
    {
        // Add force to create movement
        rb.AddForce(movement * speed);
        // Clamp to the maxSpeed to prevent the player from flying away
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        Jump(); // This is receiving Axis input, which is not single-frame input, and therefore can be called from FixedUpdate().
        ApplyCarvingPhysicsForces();
    }

    private void Update()
    {
        MovementSetUp();
        SetCarvingPhysicsInput();
        HandleAirControlSpeed();
        PreventBackwardsMovement();
    }

    #region New Carving Movement
    private void SetCarvingPhysicsInput()
    {
        if (playerScript.Grounded && moveHorizontal >= 0.1f) // Left Thumbstick - Move Right
        {
            carveRight = true;
        }
        else if (playerScript.Grounded && moveHorizontal <= -0.1f) // Left Thumbstick - Move Left
        {
            carveLeft = true;
        }
    }

    private void ApplyCarvingPhysicsForces()
    {
        if (carveRight)
        {
            rb.AddForce(new Vector3(0.2f, 0.0f, 0.0f), ForceMode.Impulse);
            carveRight = false;
        }
        else if (carveLeft)
        {
            rb.AddForce(new Vector3(-0.2f, 0.0f, 0.0f), ForceMode.Impulse);
            carveLeft = false;
        }
    }
    #endregion

    private void PreventBackwardsMovement()
    {
        if (playerScript.Grounded && moveVertical <= -0.5f && rb.velocity.magnitude <= 10f)
        {
            speed = 0f;
        }
    }

    private void MovementSetUp()
    {
        // Called in Update to constantly check the position of joystick to continually add force (but actual force applied in FixedUpdate)
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        moveHorizontal = playerScript.leftJoystick.Horizontal;
        moveVertical = playerScript.leftJoystick.Vertical;
        moveVerticalJump = playerScript.rightJoystick.Vertical;
    }

    private void HandleAirControlSpeed()
    {
        // Time buffer to allow a moment to check if player jumped and is not grounded
        if (groundTimer > 0)
        {
            groundTimer -= Time.deltaTime;
        }
        else if (groundTimer <= 0)
        {
            groundTimer = 0;
        }

        if (playerHasJumped && !playerScript.Grounded)
        {
            if (moveHorizontal <= -0.1f || moveHorizontal >= 0.1f)
            {
                speed = Mathf.Clamp(speed, 0f, 3f); // Allow slight horizontal air control
            }
        }
        else if (!playerHasJumped && !playerScript.Grounded)
        {
            speed = -3f;
        }
        else if (groundTimer == 0) // Time buffer to allow a moment to check if player jumped and is not grounded
        {
            speed = Mathf.Clamp(speed, 7f, 10f);
            playerHasJumped = false;
        }
    }

    private void Jump()
    {
        if (moveVerticalJump <= -0.9f) // hold down on right stick
        {
            holdDownRightJoystick = true;
        }

        if (moveVerticalJump >= 0.9f) // push up on right stick
        {
            flickUpRightJoystick = true;
        }
        else 
        {
            flickUpRightJoystick = false; 
        }

        if (playerScript.Grounded && holdDownRightJoystick && flickUpRightJoystick)
        {
            rb.AddForce(jumpHeight * jumpSpeed, ForceMode.Impulse);
            ResetJoystickJumpBools();
            playerHasJumped = true;
            groundTimer = 0.2f;
        }
    }

    private void ResetJoystickJumpBools()
    {
        holdDownRightJoystick = false;
        flickUpRightJoystick = false;
    }
}
