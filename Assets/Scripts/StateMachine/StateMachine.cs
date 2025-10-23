
using UnityEngine;

namespace Assets.Scripts.StateMachine
{
    public abstract class StateMachine<TState> : MonoBehaviour where TState : IState
    {
        protected TState currentState;

        public void Initialize(TState startingState)
        {
            currentState = startingState;
            currentState.Enter();
        }

        public void ChangeState(TState newState)
        {
            currentState?.Exit();
            currentState = newState;
            currentState.Enter();
        }

        public void UpdateState()
        {
            currentState?.Update();
        }

        public void FixedUpdateState()
        {
            currentState?.FixedUpdate();
        }

    }
}