using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assets.Scripts.Player.States
{
    public abstract class PlayerStateBase : IPlayerState
    {
        protected readonly PlayerController player;
        protected readonly PlayerStateMachine stateMachine;

        protected PlayerStateBase(PlayerController player, PlayerStateMachine stateMachine)
        {
            this.player = player;
            this.stateMachine = stateMachine;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void FixedUpdate();
        public abstract void Exit();
    }

}

