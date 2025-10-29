using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Player.States;

[RequireComponent(typeof(Animator), typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpForce = 12f;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    [HideInInspector] public PlayerInputActions input;
    [HideInInspector] public float moveInput;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool jumpPressed;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;

    public bool IsFalling => !isGrounded && rb.linearVelocity.y < 0;




    private PlayerStateMachine stateMachine;
    // States

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        input = new PlayerInputActions();
        stateMachine = GetComponent<PlayerStateMachine>();

    }

    void OnEnable()
    {
        input.Player.Enable();
        input.Player.Move.performed += ctx => moveInput = ctx.ReadValue<float>();
        input.Player.Move.canceled += ctx => moveInput = 0f;
        input.Player.Jump.performed += ctx => jumpPressed = true;
        input.Player.Attack.performed += ctx => stateMachine.ChangeState(stateMachine.attackState);

    }

    void OnDisable()
    {
        input.Player.Disable();
    }

    void Start()
    {
           
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
        stateMachine.UpdateState();
     
    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdateState();
    }

    public void FlipCharacter(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Sign(direction) * Mathf.Abs(scale.x);
        transform.localScale = scale;
    }

}
