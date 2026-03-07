using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerJumpingState : PlayerStateBase
    {
        private bool hasJumped;

        public PlayerJumpingState(PlayerController player, PlayerStateMachine stateMachine) 
            : base(player, stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Player entered Jumping state");

            // Apply jump impulse
            player.rb.linearVelocity = new Vector3(player.rb.linearVelocity.x, player.jumpForce, player.rb.linearVelocity.z);
            // hasJumped = true;
            player.jumpPressed = false; 
        }

        public override void Update()
        {
            // Si ya tocó el suelo, volver al Idle o Running
            if (player.isGrounded)
            {
                if (Mathf.Abs(player.moveInput) > 0.1f)
                    stateMachine.ChangeState(stateMachine.runState);
                else
                    stateMachine.ChangeState(stateMachine.idleState);

                return;
            }
        }

        public override void FixedUpdate()
        {
            HandleAirMovement();
        }

        public override void Exit()
        {
            // Reset de variables si es necesario
        }

        private void HandleAirMovement()
        {
            float moveInput = player.moveInput;
            Rigidbody rb = player.rb;

            // Allow movement in the air
            rb.linearVelocity = new Vector3(moveInput * player.runSpeed, rb.linearVelocity.y, rb.linearVelocity.z);

            if (moveInput != 0)
                player.FlipCharacter(moveInput);
        }
    }
}
