using _Game.Scripts;
using UnityEngine;

public class GameStateTurnRed : IState, ITeamTurn
{
    private GameStateFSM _gameState;
    private short characterTurnsRemaining;
    private const short TEAM_SIZE = 6;
    public Soldier[] soldiers;
    public CharacterTurnFSM[] soldierFSMs;
    private int currentSoldier;
    private GameObject _gameObject;
    
    public GameStateTurnRed(Soldier[] roster, GameObject obj, GameStateFSM gameState)
    {
        _gameState = gameState;
        characterTurnsRemaining = TEAM_SIZE;
        soldiers = roster;
        _gameObject = obj;

        soldierFSMs = new CharacterTurnFSM[roster.Length];
        for (int i = 0; i < roster.Length; i++)
        {
            //soldierFSMs[i] = new CharacterTurnFSM(soldiers[i], this);
            soldierFSMs[i] = _gameObject.AddComponent<CharacterTurnFSM>();
            soldierFSMs[i]._teamTurn = this;
            soldierFSMs[i]._soldier = soldiers[i];
        }
    }

    public bool AddScore()
    {
        /*
         Called at the end of each turn (or potentially round?).
         Adds team score based on the number of flags held.
         */

        if (_gameState._redScore >= _gameState.SCORE_LIMIT)
        {
            _gameState.ChangeState(_gameState.WinRed);
            return true;
        }
        if (_gameState._blueScore >= _gameState.SCORE_LIMIT)
        {
            _gameState.ChangeState(_gameState.WinBlue);
            return true;
        }
        // return false;
        
        // TODO: Remove this if this project is continued after deadline
        // Win game by killing the enemy team
        foreach (var s in soldiers)
        {
            // return false if even one member is alive
            if (!s.IsDead) return false;
        }
        _gameState.ChangeState(_gameState.WinBlue);
        return true; // else return true;
        
    }
    
    public void Enter()
    {
        // reset actions remaining on the top bar
        foreach (var s in soldiers)
        {
            HUD.qt.SetActionsRemaining(s, s.IsDead ? 0 : 2); // set to zero if dead
        }
        
        Debug.Log("State Entered: Turn Red");
        characterTurnsRemaining = TEAM_SIZE; // reset remaining turns
        currentSoldier = TEAM_SIZE - 1;
        
        Debug.Log("<color=#ec4b4c><b>~~ RED TURN ~~</b></color>");
        Debug.Log("Current Move: <color=#ec4b4c>Red Soldier " + currentSoldier + "</color>");
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
        if (AddScore())
            return;
        
        if (characterTurnsRemaining == 0)
            _gameState.ChangeState(_gameState.TurnBlue); // no turns remain, switch to other team
        else
        {
            Debug.Log("<color=#ec4b4c><b>~~ NEXT TURN ~~</b></color>");
            Debug.Log("Current Move: <color=#ec4b4c>Red Soldier " + currentSoldier + "</color>");
            soldierFSMs[currentSoldier].BeginTurn(); // call the next state machine
        }
    }
}
