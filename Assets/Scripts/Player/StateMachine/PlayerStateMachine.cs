using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private IPlayerState currentState;

    public void Initialize(IPlayerState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(IPlayerState newState)
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
