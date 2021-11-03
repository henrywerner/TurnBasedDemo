using UnityEngine;

/// <summary>
/// provide time for everything to be setup;
/// play intro cutscene / map overview;
/// allow for team select(?);
/// play countdown animation;
/// </summary>

public class GameStateMatchStart : IState
{
    private GameStateFSM _gameState;
    
    public GameStateMatchStart(GameStateFSM gameStateFsm)
    {
        _gameState = gameStateFsm;
    }
    
    public void Enter()
    {
        Debug.Log("State Entered: Match Start");
        Debug.Log("PRESS [SPACE] to continue");
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _gameState.ChangeState(_gameState.TurnBlue);
        }
    }

    public void FixedTick()
    {
        
    }

    public void Exit()
    {
        
    }
}
