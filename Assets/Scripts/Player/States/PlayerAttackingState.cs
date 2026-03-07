using UnityEngine;

namespace Assets.Scripts.Player.States
{
    public class PlayerAttackingState : PlayerStateBase
    {
        private Animator animator;
        private bool attackFinished;
        private float attackDuration = 0.5f; // duración del ataque en segundos
        private float attackTimer;

        public PlayerAttackingState(PlayerController player, PlayerStateMachine stateMachine)
            : base(player, stateMachine)
        {
            animator = player.GetComponent<Animator>();
        }

        public override void Enter()
        {
            Debug.Log("Player entered Attacking state");

            attackFinished = false;
            attackTimer = 0f;

            // Stop horizontal movement when attacking
            player.rb.linearVelocity = new Vector3(0, player.rb.linearVelocity.y, 0);

            // Activate animation
            animator.Play("Attack"); 
        }

        public override void Update()
        {
            attackTimer += Time.deltaTime;

            // Si la animacion o duración termina, volver a Idle o Run
            if (attackTimer >= attackDuration)
            {
                attackFinished = true;
                if (Mathf.Abs(player.moveInput) > 0.1f)
                    stateMachine.ChangeState(stateMachine.runState);
                else
                    stateMachine.ChangeState(stateMachine.idleState);
            }
        }

        public override void FixedUpdate()
        {
            // Keep player still horizontally
            player.rb.linearVelocity = new Vector3(0, player.rb.linearVelocity.y, 0);
        }

        public override void Exit()
        {
            Debug.Log("Player exited Attacking state");
            attackFinished = false;
        }
    }
}
