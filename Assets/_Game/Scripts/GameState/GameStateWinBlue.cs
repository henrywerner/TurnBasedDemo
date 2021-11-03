using UnityEngine;

public class GameStateWinBlue: IState
{
    public void Enter()
    {
        Debug.Log("State Entered: Win Blue");
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