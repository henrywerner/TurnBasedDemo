using _Game.Scripts;
using UnityEngine;

public class GameStateFSM : StateMachine
{
    public GameStateTurnBlue TurnBlue { get; private set; }
    public GameStateTurnRed TurnRed { get; private set; }
    public GameStateMatchStart MatchStart { get; private set; }
    public GameStateWinBlue WinBlue { get; private set; }
    public GameStateWinRed WinRed { get; private set; }

    public int _blueScore, _redScore;
    public int SCORE_LIMIT = 15;
    public int TurnNumber { get; private set; }

    [SerializeField] private GameObject _blueTeamObj, _redTeamObj;
    
    private void Awake()
    {
        MatchStart = new GameStateMatchStart(this);

        Soldier[] blueTeam = new Soldier[6];
        Soldier[] redTeam = new Soldier[6];

        TurnBlue = new GameStateTurnBlue(blueTeam, _blueTeamObj, this);
        TurnRed = new GameStateTurnRed(redTeam, _redTeamObj, this);
        WinBlue = new GameStateWinBlue();
        WinRed = new GameStateWinRed();
    }

    private void Start()
    {
        ChangeState(MatchStart);
    }
}
