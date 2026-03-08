using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerJumpingState : PlayerStateBase
    {
        public PlayerJumpingState(PlayerController player, PlayerStateMachine stateMachine) 
            : base(player, stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Player entered Jumping state");

            // Apply jump impulse
            player.rb.linearVelocity = new Vector3(
                player.rb.linearVelocity.x, 
                player.jumpForce, 
                player.rb.linearVelocity.z
            );
            
            player.jumpPressed = false;
            player.animator.SetBool("IsJumping", true);
        }

        public override void Update()
        {
            // Check if grounded to transition out of jump
            // Added small delay to prevent immediate landing
            if (player.isGrounded && player.rb.linearVelocity.y <= 0)
            {
                Debug.Log("Player landed! Transitioning to: " + (Mathf.Abs(player.moveInput) > PlayerController.movementThreshold ? "Walking" : "Idle"));
                
                // Land in walking or idle based on input
                if (Mathf.Abs(player.moveInput) > PlayerController.movementThreshold)
                    stateMachine.ChangeState(stateMachine.walkingState);
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
            player.animator.SetBool("IsJumping", false);
        }

        private void HandleAirMovement()
        {
            float moveInput = player.moveInput;
            Rigidbody rb = player.rb;

            // Allow smooth movement in the air
            float targetVelocity = moveInput * player.runSpeed;
            float currentVelocity = rb.linearVelocity.x;
            float newVelocity = Mathf.Lerp(currentVelocity, targetVelocity, Time.fixedDeltaTime * 3f);
            
            rb.linearVelocity = new Vector3(newVelocity, rb.linearVelocity.y, rb.linearVelocity.z);

            if (moveInput != 0)
                player.FlipCharacter(moveInput);
        }
    }
}
