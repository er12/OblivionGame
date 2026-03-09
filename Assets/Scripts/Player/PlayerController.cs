using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Player.States;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float runSpeed = 5f;
    public float jumpForce = 12f;
    public static float movementThreshold = 0.1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;

    [Header("References")]
    public Animator animator; // Assign in inspector or finds automatically

    // Component references
    [HideInInspector] public Rigidbody rb;
    private PlayerInput playerInput;
    private PlayerStateMachine stateMachine;

    // Input state
    [HideInInspector] public float moveInput;
    [HideInInspector] public bool jumpPressed;
    [HideInInspector] public bool isGrounded;

    // Property for falling state
    public bool IsFalling => !isGrounded && rb && rb.linearVelocity.y < 0;

    void Awake()
    {
        // Get components
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        stateMachine = GetComponent<PlayerStateMachine>();

        if (playerInput == null)
        {
            Debug.LogError("PlayerInput NOT FOUND! Add PlayerInput component to this GameObject!");
        }

        if (stateMachine == null)
        {
            Debug.LogError("PlayerStateMachine NOT FOUND! Add PlayerStateMachine component to this GameObject!");
        }

        // Try to find animator on this object first, then check children
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
            Debug.LogWarning("Animator not found on Player or children - animations will not work!");
        }
        else
        {
            Debug.Log("Animator found: " + animator.gameObject.name);
        }

        Debug.Log("PlayerController initialized successfully!");
        
        // Check Ground Check setup
        if (groundCheck == null)
        {
            Debug.LogWarning("GroundCheck transform is not assigned!");
        }
        else
        {
            Debug.Log($"GroundCheck position: {groundCheck.position}, GroundCheck layer: {LayerMask.LayerToName(groundCheck.gameObject.layer)}");
        }
        
        if (groundLayer == 0)
        {
            Debug.LogWarning("Ground Layer is not set! Make sure to assign the Ground layer.");
        }
        else
        {
            Debug.Log($"Ground layer mask set to: {groundLayer.value}");
        }
    }

    void Update()
    {
        // Safety checks
        if (playerInput == null || stateMachine == null)
        {
            return;
        }

        // Get input directly from PlayerInput
        GetInput();
        
        // Update animator - Speed blends Walk/Run in the Blend Tree
        if (animator != null)
        {
            // Map moveInput (-1 to 1) to Speed parameter (scale up for bigger range)
            animator.SetFloat("Speed", Mathf.Abs(moveInput) * 2f);
        }

        // Check if grounded using Raycast (more reliable than OverlapSphere)
        if (groundCheck != null)
        {
            // Cast a ray downward from GroundCheck position
            RaycastHit hit;
            bool hitGround = Physics.Raycast(
                groundCheck.position,          // Start position
                Vector3.down,                  // Direction (downward)
                out hit,                       // Hit info
                groundRadius,                  // Max distance
                groundLayer,                   // Layer mask
                QueryTriggerInteraction.Ignore // Ignore triggers
            );
            
            bool wasGrounded = isGrounded;
            isGrounded = hitGround;
            
            // Debug visualization - ray color based on detection
            Color debugColor = isGrounded ? Color.green : Color.red;
            Debug.DrawRay(groundCheck.position, Vector3.down * groundRadius, debugColor);
        }
        
        // Update state machine
        stateMachine.UpdateState();
    }

    void FixedUpdate()
    {
        if (stateMachine != null)
        {
            stateMachine.FixedUpdateState();
        }
    }

    private void GetInput()
    {
        // Get movement input (1D axis for platformer - just horizontal)
        var moveAction = playerInput.actions["Move"];
        if (moveAction != null)
        {
            // Read as float since Move is a 1D Axis, not 2D
            float rawInput = moveAction.ReadValue<float>();
            
            // Apply deadzone to prevent tiny movements
            moveInput = Mathf.Abs(rawInput) < 0.1f ? 0f : rawInput;
        }
        else
        {
            moveInput = 0f;
            Debug.LogWarning("Move action not found in PlayerInputActions!");
        }

        // Get jump input
        var jumpAction = playerInput.actions["Jump"];
        if (jumpAction != null)
        {
            jumpPressed = jumpAction.triggered;
        }
        else
        {
            Debug.LogWarning("Jump action not found in PlayerInputActions!");
        }
    }

    public void FlipCharacter(float direction)
    {
        // For 2.5D, rotate the parent (not the model child)
        if (direction > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }
}
