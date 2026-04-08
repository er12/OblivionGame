using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerIdleState : PlayerStateBase
    {
        public PlayerIdleState(PlayerController player, PlayerStateMachine stateMachine)
            : base(player, stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Player entered Idle state");
            player.animator.SetBool("Idle", true);
            player.animator.SetBool("IsRunning", false);
        }

        public override void Update()
        {
            HandleInput();
        }

        public override void FixedUpdate()
        {
        }

        public override void Exit()
        {
            player.animator.SetBool("Idle", false);
        }

        private void HandleInput()
        {
            float moveInput = player.moveInput;

            // Transition to Light Attack if light attack button pressed
            if (player.lightAttackPressed)
            {
                stateMachine.ChangeState(stateMachine.lightAttackState);
                return;
            }

            // Transition to Heavy Attack if heavy attack button pressed
            if (player.heavyAttackPressed)
            {
                stateMachine.ChangeState(stateMachine.heavyAttackState);
                return;
            }

            // Transition to Walking if horizontal input detected
            if (Mathf.Abs(moveInput) > PlayerController.movementThreshold)
            {
                stateMachine.ChangeState(stateMachine.walkingState);
                return;
            }

            // Transition to Jumping if jump pressed
            if (player.jumpPressed)
            {
                stateMachine.ChangeState(stateMachine.jumpState);
                return;
            }
        }
    }
}
