using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 12f;

    private Rigidbody2D rb;
    private PlayerInputActions input;
    private Vector2 moveInput;
    private bool isGrounded;
    private bool jumpPressed;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new PlayerInputActions();
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

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        if (jumpPressed && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpPressed = false;
        }
    }
}
