using System.Collections;
using UnityEngine;
using TMPro;

// This script handles the turning of the player and snowboard on a visual level, but does not apply physics/force.
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject sphereMotor;
    private float boardLean = 20.0f;
    private float leanSpeed = 100.0f;
    private float rotateBoard = 50.0f;
    private float straightenSpeed = 300f;
    [SerializeField] private GameObject landingGO;
    private TMP_Text landingText;
    private bool grounded;
    // Grounded Property
    public bool Grounded
    {
        get
        {
            return grounded;
        }
        set
        {
            // Check for the moment the player touches the ground after jumping
            if (!grounded && value == true && RaycastIsHittingGroundTag)
            {
                DisplayLandingTextAnimations();
                StartCoroutine(ResetLandingTextAnimations());
            }
            grounded = value;
        }
    }
    private float yawSpeed = 5f;
    private float yawMultiplier = 1f;
    private Vector3 leanLeft;
    private Vector3 leanRight;
    private GameObject leftStick;
    public Joystick leftJoystick; // Accessed from Motor Controller
    private GameObject rightStick;
    public Joystick rightJoystick; // Accessed from Motor Controller
    private float turnHorizontal;
    private float turnVertical;
    private Vector3 yOffsetSphere;
    private Animator anim;
    private RaycastHit hitInfo;
    private bool RaycastIsHittingGroundTag => hitInfo.transform.CompareTag("Ground");
    private SnowboardParticles snowParticlesScript;
    private Vector3 snowTrailCenterofBoard = new Vector3(-0.08f, 0.159f, 0.085f);
    private Vector3 snowtrailOriginalPos = new Vector3(0f, 0.159f, -0.923f);
    private Vector3 currentSlopeNormal;

    private void Start()
    {
        SetLandingTextComponents();
        SetJoyStickComponents();
        yOffsetSphere = new Vector3(0f, -1f, 0f);
        leanLeft = new Vector3(0, 0, 15);
        leanRight = new Vector3(0, 0, -15);
        anim = GetComponent<Animator>();
        snowParticlesScript = GetComponent<SnowboardParticles>();
    }

    private void Update()
    {
        CheckIfPlayerIsGroundedAndPlaySnowboardAnimations();
        NormalGroundCheckToKeepPlayerUp();
    }

    /* I've moved this into FixedUpdate() since it's tracking the SphereMotor which is being moved in FixedUpdate()
     * It used to be in Update(); but I believe this is what was causing the stutters, also moved FollowCam into FixedUpdate()*/
    private void FixedUpdate()
    {
        FollowSphereMotor();
    }

    private void NormalGroundCheckToKeepPlayerUp()
    {
        RaycastHit hit;
        float rayDistance = 20f;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayDistance))
        {
            currentSlopeNormal = hit.normal;

            float angle = Vector3.Angle(-currentSlopeNormal, transform.up);
            if (angle < 100)
            {
                // Reposition player upwards.
                transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
            }
        }
    }

    private void SetLandingTextComponents()
    {
        landingGO.SetActive(true);
        landingText = landingGO.GetComponent<TMP_Text>();
    }

    public void SetJoyStickComponents()
    {
        if (leftJoystick == null)
        {
            leftStick = GameObject.FindWithTag("JoystickL");
            leftJoystick = leftStick.GetComponent<Joystick>();
        }
        if (rightJoystick == null)
        {
            rightStick = GameObject.FindWithTag("JoystickR");
            rightJoystick = rightStick.GetComponent<Joystick>();
        }
    }

    private bool IsGrounded()
    {
        float groundCheckDistance = 0.20f;
        return Physics.Raycast(transform.position, Vector3.down, out hitInfo, groundCheckDistance);
    }

    private void CheckIfPlayerIsGroundedAndPlaySnowboardAnimations()
    {
        if (IsGrounded() && !hitInfo.transform.CompareTag("Rail"))
        {
            Grounded = true;
            anim.SetBool("isJumping", false);
            snowParticlesScript.snowTrailPS.transform.localPosition = snowtrailOriginalPos;
            LeanPlayerandSnowboard();
        }
        else
        {
            Grounded = false;
            anim.SetBool("isJumping", true);
            SpinPlayerandSnowboard();
        }
    }

    private void FollowSphereMotor()
    {
        transform.position = sphereMotor.transform.position + yOffsetSphere;
    }

    private void LeanPlayerandSnowboard()
    {
        turnHorizontal = leftJoystick.Horizontal;
        turnVertical = leftJoystick.Vertical;

        Quaternion normalRot = Quaternion.FromToRotation(transform.up, hitInfo.normal);
        Quaternion yawLeft = Quaternion.Euler(leanLeft);
        Quaternion yawRight = Quaternion.Euler(leanRight);

        if (turnHorizontal >= 0.1f) // Turn Left Thumb to the Right
        {
            transform.rotation = normalRot * transform.rotation * yawRight;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, rotateBoard, -boardLean), leanSpeed * Time.deltaTime);
        }
        else if (turnHorizontal <= -0.1f) // Turn Left Thumb to the Left
        {
            transform.rotation = normalRot * transform.rotation * yawLeft;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0.0f, -rotateBoard, boardLean), leanSpeed * Time.deltaTime);

        }
        else if (turnVertical <= -0.1f) // Stopping --> Left thumb down
        {
            snowParticlesScript.snowTrailPS.transform.localPosition = snowTrailCenterofBoard;
            transform.rotation = Quaternion.Euler(0f, -90, 0f);
        }
        else
        {
            // Keep the snowboard perpendicular to the surface normal
            Quaternion yaw = Quaternion.Euler(0f, 0f, Mathf.Sin(Time.time * yawSpeed) * yawMultiplier);
            transform.rotation = normalRot * transform.rotation * yaw;

            // Resets the player to forward position
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, straightenSpeed * Time.deltaTime);
        }
    }

    private void SpinPlayerandSnowboard()
    {
        float horizontalAirInput = rightJoystick.Horizontal;
        transform.Rotate(275f * horizontalAirInput * Time.deltaTime * Vector3.up);
    }

    private void DisplayLandingTextAnimations()
    {
        // The numbers refer to the angle of the player
        if (transform.eulerAngles.y >= 350f || transform.eulerAngles.y <= 10f)
        {
            landingText.text = "Perfect Landing!";
            landingGO.SetActive(true);
        }
        else if (transform.eulerAngles.y <= 349f && transform.eulerAngles.y >= 330f || transform.eulerAngles.y >= 11f && transform.eulerAngles.y <= 30f)
        {
            landingText.text = "Nice Landing!";
            landingGO.SetActive(true);

        }
        else if (transform.eulerAngles.y <= 329f && transform.eulerAngles.y >= 301f || transform.eulerAngles.y >= 31f && transform.eulerAngles.y <= 50f)
        {
            landingText.text = "Okay Landing!";
            landingGO.SetActive(true);

        }
        else if (transform.eulerAngles.y <= 300f || transform.eulerAngles.y >= 80f)
        {
            landingText.text = "Sketchy Landing!";
            landingGO.SetActive(true);
        }
    }

    private IEnumerator ResetLandingTextAnimations()
    {
        yield return new WaitForSeconds(1.13f);
        landingGO.SetActive(false);
    }
}