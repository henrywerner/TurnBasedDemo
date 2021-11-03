using UnityEngine;

public class GameStateWinRed : IState
{
    public void Enter()
    {
        Debug.Log("State Entered: Win Red");
    }

    public void Tick()
    {
    }

    public void FixedTick()
    {
    }

    public void Exit()
    {
    }
}