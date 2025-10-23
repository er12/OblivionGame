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
        }

        public override void FixedUpdate() { }

        public override void Exit()
        {

        }

    }
}
