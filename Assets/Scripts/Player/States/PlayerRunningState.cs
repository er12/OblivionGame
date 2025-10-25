using UnityEngine;

namespace Assets.Scripts.Player.States
{

    public class PlayerRunningState : PlayerStateBase
    {
        public PlayerRunningState(PlayerController player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Player entered Running state");
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

        }

         private void HandleInput()
        {
            float moveInput = player.moveInput;

            // Transition to Idle if no horizontal input
            if (Mathf.Abs(moveInput) < 0.1f)
            {
                stateMachine.ChangeState(stateMachine.idleState);
                return;
            }

            // Transition to Jumping if jump pressed
            if (player.jumpPressed)
                {
                    
                    stateMachine.ChangeState(stateMachine.jumpState);
                    return;
                }

            // Optional: transitions to other states like attack, dash, etc.
        }

      
        private void HandleMovement()
        {
            float moveInput = player.moveInput;
            Rigidbody2D rb = player.rb;

            rb.linearVelocity = new Vector2(moveInput * player.runSpeed, rb.linearVelocity.y);

            if (moveInput != 0)
                player.FlipCharacter(moveInput);
        }


    }
}
