using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerAttackingState : PlayerStateBase
    {
        private bool attackFinished;
        private float attackDuration = 0.5f;
        private float attackTimer;

        public PlayerAttackingState(PlayerController player, PlayerStateMachine stateMachine)
            : base(player, stateMachine) { }

        public override void Enter()
        {
            Debug.Log("Player entered Attacking state");

            attackFinished = false;
            attackTimer = 0f;

            // Stop horizontal movement when attacking
            player.rb.linearVelocity = new Vector3(0, player.rb.linearVelocity.y, 0);

            // Activate animation
            player.animator.SetBool("IsAttacking", true);
        }

        public override void Update()
        {
            attackTimer += Time.deltaTime;

            // If attack duration finishes, transition based on movement
            if (attackTimer >= attackDuration)
            {
                attackFinished = true;
                if (Mathf.Abs(player.moveInput) > PlayerController.movementThreshold)
                    stateMachine.ChangeState(stateMachine.walkingState);
                else
                    stateMachine.ChangeState(stateMachine.idleState);
            }
        }

        public override void FixedUpdate()
        {
            // Keep player still horizontally during attack
            player.rb.linearVelocity = new Vector3(0, player.rb.linearVelocity.y, 0);
        }

        public override void Exit()
        {
            Debug.Log("Player exited Attacking state");
            player.animator.SetBool("IsAttacking", false);
            attackFinished = false;
        }
    }
}
