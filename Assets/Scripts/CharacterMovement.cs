using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))] // Ensures that a Rigidbody component is attached to the GameObject
public class CharacterMovement : MonoBehaviour
{
    // ============================== Movement Settings ==============================
    [Header("Movement Settings")]
    [SerializeField] private float baseWalkSpeed = 5f;    // Base speed when walking
    [SerializeField] private float baseRunSpeed = 8f;     // Base speed when running
    [SerializeField] private float rotationSpeed = 10f;   // Speed at which the character rotates

    // ============================== Jump Settings =================================
    [Header("Jump Settings")]
    [SerializeField] public float jumpForce = 5f;        // Jump force applied to the character
    [SerializeField] private float groundCheckDistance = 1.1f; // Distance to check for ground contact (Raycast)
    public bool doubleJump;
    private bool hasJumpBoost = false;
    private ParticleSystem doubleJumpParticles;

    // ============================== Health & Respawn ==============================
    [Header("Fall Settings")]
    [SerializeField] private float fallThreshold = -5f;  // Y coordinate for falling off the map
    [SerializeField] private Vector3 respawnPoint = new Vector3(0, 1, 0);  // Respawn position
    private bool isRespawning = false;

    // ============================== Modifiable from other scripts ==================
    public float speedMultiplier = 1.0f; // Additional multiplier for character speed ( WINK WINK )

    // ============================== Private Variables ==============================
    private Rigidbody rb; // Reference to the Rigidbody component
    private Transform cameraTransform; // Reference to the camera's transform

    // Input variables
    private float moveX; // Stores horizontal movement input (A/D or Left/Right Arrow)
    private float moveZ; // Stores vertical movement input (W/S or Up/Down Arrow)
    private bool jumpRequest; // Flag to check if the player requested a jump
    private Vector3 moveDirection; // Stores the calculated movement direction

    // ============================== Animation Variables ==============================
    [Header("Anim values")]
    public float groundSpeed; // Speed value used for animations

    // ============================== Character State Properties ==============================
    /// <summary>
    /// Checks if the character is currently grounded using a Raycast.
    /// If false, the character is in the air.
    /// </summary>
    public bool IsGrounded => 
        Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance);

    /// <summary>
    /// Checks if the player is currently holding the "Run" button.
    /// </summary>
    private bool IsRunning => Input.GetButton("Run");

    // ============================== Unity Built-in Methods ==============================

    /// <summary>
    /// Called when the script is first initialized.
    /// </summary>
    private void Awake()
    {
        InitializeComponents(); // Initialize Rigidbody and Camera reference
    }

    private void Start()
    {
        doubleJumpParticles = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// Called every frame, used to register player input.
    /// </summary>
    private void Update()
    {
        RegisterInput(); // Collect player input
        CheckForFall();
    }

    /// <summary>
    /// Called every physics update (FixedUpdate ensures physics stability).
    /// </summary>
    private void FixedUpdate()
    {
        HandleMovement(); // Process movement and physics-based updates
    }

    // ============================== Initialization ==============================

    /// <summary>
    /// Initializes Rigidbody and camera reference.
    /// Also locks and hides the cursor for better control.
    /// </summary>
    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody>(); // Get the Rigidbody component
        rb.freezeRotation = true; // Prevent Rigidbody from rotating due to physics interactions
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smooth physics interpolation

        // Assign the main camera if available
        if (Camera.main)
            cameraTransform = Camera.main.transform;

        // Lock and hide the cursor for better gameplay control
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    // ============================== Input Handling ==============================

    /// <summary>
    /// Reads player input values and registers movement/jump requests.
    /// </summary>
    private void RegisterInput()
    {
        moveX = Input.GetAxis("Horizontal"); // Get horizontal movement input
        moveZ = Input.GetAxis("Vertical");   // Get vertical movement input

        // Register a jump request if the player presses the Jump button
        if (Input.GetButtonDown("Jump"))
        {
            jumpRequest = true;
        }
    }

    // ============================== Movement Handling ==============================

    /// <summary>
    /// Handles movement-related logic: calculating direction, jumping, rotating, and moving.
    /// </summary>
    private void HandleMovement()
    {
        CalculateMoveDirection(); // Compute the movement direction based on input
        HandleJump(); // Process jump input
        RotateCharacter(); // Rotate the character towards the movement direction
        MoveCharacter(); // Move the character using velocity-based movement
    }

    /// <summary>
    /// Calculates the movement direction based on player input and camera orientation.
    /// </summary>
    private void CalculateMoveDirection()
    {
        // If the camera is not assigned, move based on world space
        if (!cameraTransform)
        {
            moveDirection = new Vector3(moveX, 0, moveZ).normalized;
        }
        else
        {
            // Get forward and right vectors from the camera perspective
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            // Ignore Y-axis movement to prevent unwanted tilting
            forward.y = 0f;
            right.y = 0f;

            // Normalize vectors to maintain consistent movement speed
            forward.Normalize();
            right.Normalize();

            // Calculate movement direction relative to the camera orientation
            moveDirection = (forward * moveZ + right * moveX).normalized;
        }
    }

    /// <summary>
    /// Handles jumping by applying an impulse force if the character is grounded.
    /// </summary>
    private void HandleJump()
    {
        if (IsGrounded)
        {
            doubleJump = true; // Reset double jump when grounded
        }

        if (jumpRequest)
        {
            if (IsGrounded) // Regular jump
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            }
            else if (doubleJump && hasJumpBoost) // Allow double jump only if Jump Boost is active
            {
                doubleJumpParticles.Play();
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                doubleJump = false;
            }

            jumpRequest = false;
            Invoke(nameof(StopDoubleJumpParticles), 0.8f);
        }
    }

    private void StopDoubleJumpParticles()
    {
        doubleJumpParticles.Stop();
    }

    public void SetJumpBoost(bool state)
    {
        hasJumpBoost = state;
    }

    /// <summary>
    /// Rotates the character towards the movement direction.
    /// </summary>
    private void RotateCharacter()
    {
        // Rotate only if the character is moving
        if (moveDirection.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    /// <summary>
    /// Moves the character using Rigidbody's velocity instead of MovePosition.
    /// This ensures smooth movement while avoiding physics conflicts.
    /// </summary>
    private void MoveCharacter()
    {
        // Determine movement speed (walking or running)
        float speed = IsRunning ? baseRunSpeed : baseWalkSpeed;
        
        // Set ground speed value for animation purposes
        groundSpeed = (moveDirection != Vector3.zero) ? speed : 0.0f;

        // Preserve the current Y velocity to maintain gravity effects
        Vector3 newVelocity = new Vector3(
            moveDirection.x * speed * speedMultiplier, 
            rb.linearVelocity.y, // Keep the existing Y velocity for jumping & gravity
            moveDirection.z * speed * speedMultiplier
        );

        // Apply the new velocity directly
        rb.linearVelocity = newVelocity;
    }

    private void CheckForFall()
    {
        if (!isRespawning && transform.position.y < fallThreshold)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        if (isRespawning) return; // Prevent multiple calls

        isRespawning = true; // Set cooldown
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TakeDamage();
        }

        // Determine respawn point based on scene
        string currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName == "Level1")
        {
            respawnPoint = new Vector3(0, 1, 0);
        }
        else if (currentSceneName == "Level2")
        {
            respawnPoint = new Vector3(27, 1, 0);
        }
        else if (currentSceneName == "Level3")
        {
            respawnPoint = new Vector3(0, 1, 0);
        }
        else
        {
            respawnPoint = new Vector3(0, 1, 0);
        }

        // Disable physics temporarily to avoid issues
        rb.isKinematic = true;
        rb.linearVelocity = Vector3.zero; // Clear velocity
        rb.angularVelocity = Vector3.zero; // Clear angular velocity

        // Reset position safely
        rb.MovePosition(respawnPoint);

        // Re-enable physics after a short delay
        Invoke(nameof(ResetPhysics), 0.1f);
    }

    /// <summary>
    /// Re-enables physics after respawning.
    /// </summary>
    private void ResetPhysics()
    {
        rb.isKinematic = false;
        isRespawning = false;
    }
}
