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

        void HandleInput()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                Debug.Log("right arrow");

                //stateMachine.ChangeState(player.runState);
            }
        }
    }
}
