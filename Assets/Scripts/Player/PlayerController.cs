using UnityEngine;
using UnityEngine.InputSystem;
using Assets.Scripts.Player.States;

[RequireComponent(typeof(Animator), typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{
    public float runSpeed = 5f;
    public float jumpForce = 12f;

    [HideInInspector] public Rigidbody rb;
    public Animator animator;   // reference


    [HideInInspector] public PlayerInputActions input;
    [HideInInspector] public float moveInput;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool jumpPressed;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.1f;
    public LayerMask groundLayer;

    public bool IsFalling => !isGrounded && rb.linearVelocity.y < 0;

    public static float movementThreshold = 0.1f;


    private PlayerStateMachine stateMachine;
    // States

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        input = new PlayerInputActions();
        stateMachine = GetComponent<PlayerStateMachine>();
        animator = GetComponent<Animator>();


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
        animator.SetFloat("Speed", Mathf.Abs(moveInput));
        Debug.Log("input" + moveInput);

        isGrounded = Physics.OverlapSphere(groundCheck.position, groundRadius, groundLayer).Length > 0;
        stateMachine.UpdateState();

    }

    void FixedUpdate()
    {
        stateMachine.FixedUpdateState();
    }

    public void FlipCharacter(float direction)
    {
        // For 2.5D, rotate the character model on the Y axis
        if (direction > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (direction < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

}
