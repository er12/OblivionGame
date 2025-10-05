using UnityEngine;
using System.Collections.Generic;

public class PlayerStateMachine : MonoBehaviour
{
    private IPlayerState currentState;

    public void ChangeState(IPlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    private void Update()
    {
        currentState?.Update();
    }

    private void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }
}