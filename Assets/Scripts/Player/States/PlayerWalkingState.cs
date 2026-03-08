using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerWalkingState : PlayerStateBase
    {
        public PlayerWalkingState(PlayerController player, PlayerStateMachine stateMachine) 
            : base(player, stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Player entered Walking state");
            player.animator.SetBool("IsRunning", false);
        }

        public override void Update()
        {
            HandleInput();
        }

        public override void FixedUpdate()
        {
            HandleMovement();
        }

        public override void Exit()
        {
            player.animator.SetBool("IsRunning", false);
        }

        private void HandleInput()
        {
            float moveInput = player.moveInput;

            // Transition to Idle if no horizontal input
            if (Mathf.Abs(moveInput) < PlayerController.movementThreshold)
            {
                stateMachine.ChangeState(stateMachine.idleState);
                return;
            }

            // Transition to Running if input is strong enough (no Shift key needed)
            if (Mathf.Abs(moveInput) > 0.7f)
            {
                stateMachine.ChangeState(stateMachine.runState);
                return;
            }

            // Transition to Jumping if jump pressed
            if (player.jumpPressed)
            {
                stateMachine.ChangeState(stateMachine.jumpState);
                return;
            }
        }

        private void HandleMovement()
        {
            float moveInput = player.moveInput;
            Rigidbody rb = player.rb;

            // Walk at half the run speed - smooth velocity change
            float walkSpeed = player.runSpeed * 0.5f;
            float targetVelocity = moveInput * walkSpeed;
            
            // Smoothly transition velocity to avoid jumpiness
            float currentVelocity = rb.linearVelocity.x;
            float newVelocity = Mathf.Lerp(currentVelocity, targetVelocity, Time.fixedDeltaTime * 5f);
            
            rb.linearVelocity = new Vector3(newVelocity, rb.linearVelocity.y, rb.linearVelocity.z);

            if (moveInput != 0)
                player.FlipCharacter(moveInput);
        }
    }
}
