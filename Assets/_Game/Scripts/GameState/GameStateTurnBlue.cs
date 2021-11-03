using _Game.Scripts;
using UnityEngine;

public class GameStateTurnBlue : IState, ITeamTurn
{
    private GameStateFSM _gameState;
    private short characterTurnsRemaining;
    private const short TEAM_SIZE = 6;
    public Soldier[] soldiers;
    public CharacterTurnFSM[] soldierFSMs;
    private int currentSoldier;
    private GameObject _gameObject;
    
    public GameStateTurnBlue(Soldier[] roster, GameObject obj, GameStateFSM gameState)
    {
        _gameState = gameState;
        characterTurnsRemaining = TEAM_SIZE;
        soldiers = roster;
        _gameObject = obj;

        soldierFSMs = new CharacterTurnFSM[roster.Length];
        for (int i = 0; i < roster.Length; i++)
        {
            soldierFSMs[i] = _gameObject.AddComponent<CharacterTurnFSM>();
            soldierFSMs[i]._teamTurn = this;
            soldierFSMs[i]._soldier = soldiers[i];
        }
    }

    public void AddScore()
    {
        /*
         Called at the end of each turn (or potentially round?).
         Adds team score based on the number of flags held.
         */
        
        if (_gameState._redScore >= _gameState.SCORE_LIMIT)
            _gameState.ChangeState(_gameState.WinRed);
        else if (_gameState._blueScore >= _gameState.SCORE_LIMIT)
            _gameState.ChangeState(_gameState.WinBlue);
    }
    
    public void Enter()
    {
        Debug.Log("State Entered: Turn Blue");
        characterTurnsRemaining = TEAM_SIZE; // reset remaining turns
        currentSoldier = TEAM_SIZE - 1;

        Debug.Log("<color=#46a8e0><b>~~ BLUE TURN ~~</b></color>");
        Debug.Log("Current Move: <color=#46a8e0>Blue Soldier " + currentSoldier + "</color>");
        soldierFSMs[currentSoldier].BeginTurn();
    }

    public void Tick()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            _gameState._blueScore++;
            Debug.Log("Blue Team: " + _gameState._blueScore + " pts  |  Red Team: " + _gameState._redScore + " pts");
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            _gameState._redScore++;
            Debug.Log("Blue Team: " + _gameState._blueScore + " pts  |  Red Team: " + _gameState._redScore + " pts");
        }
    }

    public void FixedTick()
    {
    }

    public void Exit()
    {
    }

    public void NextTurn()
    {
        // called whenever a character FSM enters the idle state
        
        // advance the turn by one
        characterTurnsRemaining--;
        currentSoldier = characterTurnsRemaining - 1;

        // add score
        AddScore();

        if (characterTurnsRemaining == 0)
            _gameState.ChangeState(_gameState.TurnRed); // no turns remain, switch to other team
        else
        {
            Debug.Log("<color=#46a8e0><b>~~ NEXT TURN ~~</b></color>");
            Debug.Log("Current Move: <color=#46a8e0>Blue Soldier " + currentSoldier + "</color>");
            soldierFSMs[currentSoldier].BeginTurn(); // call the next state machine
        }
    }
}
