using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public PlayerInputActions input;
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool jumpPressed;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;

    private PlayerStateMachine stateMachine;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new PlayerInputActions();
        stateMachine = GetComponent<PlayerStateMachine>();

    }

    void OnEnable()
    {
        input.Player.Enable();
        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        input.Player.Jump.performed += ctx => jumpPressed = true;
    }

    void OnDisable()
    {
        input.Player.Disable();
    }

    void Start()
    {
        // stateMachine.Initialize(new PlayerIdleState(this, stateMachine));
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        // stateMachine.UpdateState();
    }

    void FixedUpdate()
    {

        //stateMachine.FixedUpdateState();

        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
        }
    }
}
