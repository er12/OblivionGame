using Assets.Scripts.StateMachine;
using Assets.Scripts.Player.States.Combat;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateMachine : StateMachine<IPlayerState>
    {
        private PlayerController playerController;

        public PlayerIdleState idleState;
        public PlayerWalkingState walkingState;
        public PlayerRunningState runState;
        public PlayerJumpingState jumpState;
        public LightComboAttackState lightAttackState;
        public HeavyComboAttackState heavyAttackState;

        void Awake()
        {
            playerController = GetComponent<PlayerController>();

            // Create states, passing "this" as machine reference
            idleState = new PlayerIdleState(playerController, this);
            walkingState = new PlayerWalkingState(playerController, this);
            runState = new PlayerRunningState(playerController, this);
            jumpState = new PlayerJumpingState(playerController, this);
            lightAttackState = new LightComboAttackState(playerController, this);
            heavyAttackState = new HeavyComboAttackState(playerController, this);

            Initialize(idleState);  // start with idle
        }
    }
}