using System.Diagnostics;
using System.Transactions;
using UnityEngine;
using Debug = UnityEngine.Debug;


namespace Assets.Scripts.Player.States
{
    public class PlayerIdleState : PlayerStateBase
    {

        int current_code;

        public PlayerIdleState(PlayerController player, PlayerStateMachine stateMachine)
            : base(player, stateMachine) { }


        public override void Enter()
        {
            Debug.Log("Player entered Idle state");

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

        }

        private void HandleInput()
        {
            float moveInput = player.moveInput;

            // 1️⃣ Transition to Running if horizontal input
            if (Mathf.Abs(moveInput) > 0.1f)
            {
                stateMachine.ChangeState(stateMachine.runState);
                return;
            }

            // 2️⃣ Transition to Jumping if jump pressed
            // if (player.jumpPressed)
            // {
            //     player.jumpPressed = false; // reset
            //     Debug.Log("Jump pressed");
            //     // stateMachine.ChangeState(player.jumpState);
            //     return;
            // }

            // 3️⃣ Optional: other transitions like attack, dash, etc.

            if (player.jumpPressed)
            {
                
                stateMachine.ChangeState(stateMachine.jumpState); 
                return;
            }
        }
    }
}
