using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine 
{
    public PlayerState currentState { get; private set; }
    // Start is called before the first frame update
    public void Initialize(PlayerState state)
    {
        currentState = state;
        state.Enter();
    }

    public void ChangeState(PlayerState state)
    {
        currentState.Exit();
        currentState = state;
        currentState.Enter();
    }
}
