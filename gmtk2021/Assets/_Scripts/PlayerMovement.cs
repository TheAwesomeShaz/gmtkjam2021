using System;
using System.Collections;
using UnityEngine;

// https://www.youtube.com/watch?v=NEUzB5vPYrE&lc=UgzHCcRV30rQ7Wrej-x4AaABAg.9MXBSlau5fu9MXPNtA2ExI&ab_channel=TKGgames
// initially yeeted from here and combined with brackeys character controller to form a rigidbody character controller

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    // ******************************ALL GAMECONTROLLER STUFF HAS BEEN DISABLED FOR NOW***********************************************
    #region Variables

    [Header("Speeds")] public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float currentSpeed;

    [Header("Rotation Stuff")]
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;

    [Header("Component References")]
    public static PlayerMovement instance;
    public Animator anim;
    public Transform orientation;
    public Transform enemyLocation;
    //Components
    AudioSource audioSource;
    Transform cam;


    [Header("Stealth Stuff")]
    public bool inStealth;
    public bool isDetected;
    public bool isAlive;

    [Header("Jumping Stuff")]
    public LayerMask whatIsGround;
    public bool isGrounded;
    public CapsuleCollider col;
    public float jumpForce = 550f;
    // float xRotation;
    // private bool cancellingGrounded;
    // private bool readyToJump = true;
    // private float jumpCooldown = 0.25f;
    // public int startDoubleJumps = 1;
    // int doubleJumpsLeft;




    //Input and movement related
    private float xAxis;
    private float zAxis;
    private Rigidbody rb;
    Vector3 moveDir;
    private bool leftShiftPressed;
    private bool leftAltPressed;


    private RaycastHit hit;

    bool underKillAnimation;
    #endregion


    [Header("Wallrunning Stuff")]
    public LayerMask whatIsWall;
    public float wallrunForce, maxWallrunTime, maxWallSpeed;
    public float maxWallrunCamTilt, wallrunCamTilt;
    [SerializeField] bool isWallRight, isWallLeft;
    [SerializeField] bool isWallRunning;

    private Vector3 normalVector = Vector2.up;
    private bool alreadyStoppedAtLadder;
    public float maxClimbSpeed;
    public float climbForce;
    public float sensitivity;
    public float sensMultiplier;
    private float maxSlopeAngle;

    #region wallrun stuff
    void WallRunInput()
    {
        if (Input.GetMouseButton(1) && isWallRight) StartWallRun();
        if (Input.GetMouseButton(1) && isWallLeft) StartWallRun();
        else
        {
            anim.SetBool("WallRun", false);
        }
    }
    void StartWallRun()
    {
        if (isWallRight)
        {
            anim.SetBool("WallRunRight", true);
        }

        if (isWallLeft)
        {
            anim.SetBool("WallRunLeft", true);
        }

        rb.useGravity = false;
        isWallRunning = true;
        if (rb.velocity.magnitude < maxWallSpeed)
        {
            rb.AddForce(orientation.forward * wallrunForce * Time.deltaTime);
            Debug.Log("Adding forward force to character's forward direction");
            //make character stick to the wall
            if (isWallRight)
            {
                rb.AddForce(orientation.right * wallrunForce / 5 * Time.deltaTime);
            }
            else
            {
                rb.AddForce(-orientation.right * wallrunForce / 5 * Time.deltaTime);
                Debug.Log("Adding forward force to character's left direction");

            }
        }
    }
    void StopWallRun()
    {
        rb.useGravity = true;
        isWallRunning = false;
        anim.SetBool("WallRunLeft", false);
        anim.SetBool("WallRunRight", false);

    }
    void CheckForWall()// to check if there is a wall next to you
    {
        isWallRight = Physics.Raycast(transform.position, orientation.right, 1f, whatIsWall);
        isWallLeft = Physics.Raycast(transform.position, -orientation.right, 1f, whatIsWall);

        //leave wall run
        if (!isWallLeft && !isWallRight) StopWallRun();
    }

    #endregion
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        #region initializing components
        cam = Camera.main.transform;
        col = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        inStealth = false;
        isAlive = true;
        //        GetComponent<Outline>().OutlineWidth = 0;
        audioSource = Camera.main.GetComponent<AudioSource>();
        #endregion

    }

    private void Update()
    {
        if (isAlive && !underKillAnimation)
        {
            InputStuff();
            SetPlayerDirection(); // for player rotation
            Animate();
            StealthStuff();
            CheckForWall();
            WallRunInput();
            // JumpInput();
            CheckGrounded();
            Jump();

        }

    }

    private void Jump()
    {
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");

            //AddJumpForce();
        }
    }

    public void AddJumpForce()
    {
        if (!isWallRunning)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (isWallRunning && isWallLeft)
        {
            Vector3 jumpDirection = ((orientation.forward + orientation.right) / 2).normalized;
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            //jump off wall 
        }

    }

    void CheckGrounded()
    {
        isGrounded = Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x,
        col.bounds.min.y, col.bounds.center.z), col.radius * .9f, whatIsGround);

    }

    #region the land of wtf is going on

    // private void JumpInput()
    // {
    //     if (Input.GetButtonDown("Jump") && !grounded && doubleJumpsLeft >= 1)
    //     {
    //         Jump();
    //         doubleJumpsLeft--;
    //     }
    //     if (grounded)
    //     {
    //         doubleJumpsLeft = startDoubleJumps;
    //     }
    // }

    // private bool IsFloor(Vector3 v)
    // {
    //     float angle = Vector3.Angle(Vector3.up, v);
    //     return angle < maxSlopeAngle;
    // }

    // private void OnCollisionStay(Collision other)
    // {
    //     //Make sure we are only checking for walkable layers
    //     int layer = other.gameObject.layer;
    //     if (whatIsGround != (whatIsGround | (1 << layer))) return;

    //     //Iterate through every collision in a physics update
    //     for (int i = 0; i < other.contactCount; i++)
    //     {
    //         Vector3 normal = other.contacts[i].normal;
    //         //FLOOR
    //         if (IsFloor(normal))
    //         {
    //             grounded = true;
    //             cancellingGrounded = false;
    //             normalVector = normal;
    //             CancelInvoke(nameof(StopGrounded));
    //         }
    //     }

    //     //Invoke ground/wall cancel, since we can't check normals with CollisionExit
    //     float delay = 3f;
    //     if (!cancellingGrounded)
    //     {
    //         cancellingGrounded = true;
    //         Invoke(nameof(StopGrounded), Time.deltaTime * delay);
    //     }
    // }


    // private void StopGrounded()
    // {
    //     grounded = false;
    // }

    // private void Jump()
    // {
    //     if (grounded)
    //     {
    //         readyToJump = false;
    //         Debug.Log("Jumping now");

    //         //Add jump forces
    //         rb.AddForce(Vector2.up * jumpForce * 1.5f);

    //         //If jumping while falling, reset y velocity.
    //         Vector3 vel = rb.velocity;
    //         if (rb.velocity.y < 0.5f)
    //             rb.velocity = new Vector3(vel.x, 0, vel.z);
    //         else if (rb.velocity.y > 0)
    //             rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

    //         Invoke(nameof(ResetJump), jumpCooldown);
    //     }
    //     if (!grounded)
    //     {
    //         readyToJump = false;

    //         //Add jump forces
    //         rb.AddForce(orientation.forward * jumpForce * 1f);
    //         rb.AddForce(Vector2.up * jumpForce * 1.5f);

    //         //Reset Velocity
    //         rb.velocity = Vector3.zero;



    //         Invoke(nameof(ResetJump), jumpCooldown);
    //     }

    //     //Walljump
    //     if (isWallRunning)
    //     {
    //         readyToJump = false;

    //         //normal jump
    //         if (isWallLeft && !Input.GetKey(KeyCode.D) || isWallRight && !Input.GetKey(KeyCode.A))
    //         {
    //             rb.AddForce(Vector2.up * jumpForce * 1.5f);
    //         }

    //         //sidwards wallhop
    //         if (isWallRight || isWallLeft && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) rb.AddForce(-orientation.up * jumpForce * 1f);
    //         if (isWallRight && Input.GetKey(KeyCode.A)) rb.AddForce(-orientation.right * jumpForce * 3.2f);
    //         if (isWallLeft && Input.GetKey(KeyCode.D)) rb.AddForce(orientation.right * jumpForce * 3.2f);

    //         //Always add forward force
    //         rb.AddForce(orientation.forward * jumpForce * 1f);


    //         Invoke(nameof(ResetJump), jumpCooldown);
    //     }
    // }

    // private void ResetJump()
    // {
    //     readyToJump = true;
    // }

    // private void Climb()
    // {
    //     //Makes possible to climb even when falling down fast
    //     Vector3 vel = rb.velocity;
    //     if (rb.velocity.y < 0.5f && !alreadyStoppedAtLadder)
    //     {
    //         rb.velocity = new Vector3(vel.x, 0, vel.z);
    //         //Make sure char get's at wall
    //         alreadyStoppedAtLadder = true;
    //         rb.AddForce(orientation.forward * 500 * Time.deltaTime);
    //     }

    //     //Push character up
    //     if (rb.velocity.magnitude < maxClimbSpeed)
    //         rb.AddForce(orientation.up * climbForce * Time.deltaTime);

    //     //Doesn't Push into the wall
    //     if (!Input.GetKey(KeyCode.S)) zAxis = 0; // since z axis is the vertical input of the joystick
    // }

    // void camRotate()
    // {

    //     float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
    //     float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;


    //     Vector3 rot = cam.transform.localRotation.eulerAngles;
    //     float desiredX = rot.y + mouseX;

    //     //Rotate, and also make sure we dont over- or under-rotate.
    //     xRotation -= mouseY;
    //     xRotation = Mathf.Clamp(xRotation, -90f, 90f);

    //     //Perform the rotations
    //     cam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, wallrunCamTilt);
    //     orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

    //     //While Wallrunning
    //     //Tilts camera in .5 second
    //     if (Mathf.Abs(wallrunCamTilt) < maxWallrunCamTilt && isWallRunning && isWallRight)
    //         wallrunCamTilt += Time.deltaTime * maxWallrunCamTilt * 2;
    //     if (Mathf.Abs(wallrunCamTilt) < maxWallrunCamTilt && isWallRunning && isWallLeft)
    //         wallrunCamTilt -= Time.deltaTime * maxWallrunCamTilt * 2;

    //     //Tilts camera back again
    //     if (wallrunCamTilt > 0 && !isWallRight && !isWallLeft)
    //         wallrunCamTilt -= Time.deltaTime * maxWallrunCamTilt * 2;
    //     if (wallrunCamTilt < 0 && !isWallRight && !isWallLeft)
    //         wallrunCamTilt += Time.deltaTime * maxWallrunCamTilt * 2;
    // }

    #endregion

    private void Animate()
    {
        anim.SetBool("IsGrounded", isGrounded);

        if (Input.GetMouseButtonUp(1))
        {
            StopWallRun();
        }

        if (leftAltPressed || leftAltPressed && leftShiftPressed)
        {
            anim.SetBool("Crouch", true);
            //GameController.instance.EnableGuardOutlines();
            anim.SetBool("Run", false);
            currentSpeed = crouchSpeed;
        }
        if (!leftAltPressed && !inStealth)
        {
            //GameController.instance.DisableGuardOutlines();
            anim.SetBool("Crouch", false);
        }
        if (!leftAltPressed)
        {
            //GameController.instance.DisableGuardOutlines();
        }

        if (leftShiftPressed && !leftAltPressed && currentSpeed != 0)
        {
            anim.SetBool("Run", true);
            currentSpeed = runSpeed;
        }
        if (!leftShiftPressed)
        {
            anim.SetBool("Run", false);
            if (!inStealth)
            {
                currentSpeed = walkSpeed;
            }

        }
        if (currentSpeed == 0)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Move", false);
        }
    }

    private void StealthStuff()
    {
        if (inStealth)
        {
            currentSpeed = crouchSpeed;
            anim.SetBool("Crouch", true);
            if (leftShiftPressed)
            {
                currentSpeed = runSpeed;
                anim.SetBool("Crouch", false);
                anim.SetBool("Run", true);
                inStealth = false;
            }
            //         GetComponent<Outline>().OutlineWidth = 3;
        }

        if (isDetected)
        {
            //GameController.instance.DisplayLoseScreen();
        }

        if (!inStealth)
        {
            //            GetComponent<Outline>().OutlineWidth = 0;
        }
    }

    private void SetPlayerDirection()
    {
        Vector3 direction = new Vector3(xAxis, 0f, zAxis).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y,
            targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            if (!isWallRunning)
            {
                transform.rotation = Quaternion.Euler(0f, angle, 0f);
            }


            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        }
    }

    private void InputStuff()
    {
        xAxis = UnityEngine.Input.GetAxisRaw("Horizontal");
        zAxis = UnityEngine.Input.GetAxisRaw("Vertical");

        leftShiftPressed = UnityEngine.Input.GetKey(KeyCode.LeftShift);
        leftAltPressed = UnityEngine.Input.GetKey(KeyCode.LeftAlt);
    }


    public void PlayerDie()
    {
        if (isAlive)
        {
            anim.SetTrigger("Die");
            isAlive = false;
            GetComponent<Rigidbody>().isKinematic = true;
            // GameController.instance.DisplayDeathScreen();
        }

    }

    public void StealthKill()
    {
        inStealth = false;
        anim.SetBool("Crouch", false);
        anim.SetTrigger("StealthKill");
        underKillAnimation = true;
    }

    public void NotUnderKillAnimation()
    {
        underKillAnimation = false;
    }

    private void FixedUpdate()
    {
        #region move player

        if (!underKillAnimation)
        {

            if (xAxis != 0 || zAxis != 0)
            {
                anim.SetBool("Move", true);
                if (inStealth)
                {
                    currentSpeed = crouchSpeed;
                    anim.SetBool("Crouch", true);
                }
                if (!isWallRunning)
                {
                    rb.MovePosition(transform.position + Time.deltaTime * currentSpeed *
                        moveDir.normalized);

                }
            }
            else
            {
                currentSpeed = 0f;
                anim.SetBool("Move", false);
                if (inStealth)
                {
                    anim.SetBool("Crouch", true);
                }
            }



        }
        #endregion

    }

    public void PlayFootstepSFX()
    {
        //audioSource.PlayOneShot(GameController.instance.footstepSFX, 0.7f);
        return;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "StealthBush" && !underKillAnimation && isAlive)
        {
            inStealth = true;
            anim.SetBool("Crouch", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "StealthBush")
        {

            inStealth = false;
            anim.SetBool("Crouch", false);
        }

    }

}


