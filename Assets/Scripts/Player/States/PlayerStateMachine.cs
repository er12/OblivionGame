using Assets.Scripts.StateMachine;

namespace Assets.Scripts.Player.States
{
    public class PlayerStateMachine : StateMachine<IPlayerState>
    {
        private PlayerController playerController; 

        public PlayerIdleState idleState;
        public PlayerRunningState runState;

        void Awake()
        {
            playerController = GetComponent<PlayerController>();

            // Create states here, passing "this" as machine reference
            idleState = new PlayerIdleState(playerController, this);
            runState = new PlayerRunningState(playerController, this);

            Initialize(idleState);  // start with idle
        }

    }

}