using UnityEngine;

namespace _Game.Scripts.CharacterTurn
{
    public class CharacterTurnIdle : IState
    {
        private CharacterTurnFSM _characterTurnFsm;

        public CharacterTurnIdle(CharacterTurnFSM characterTurnFsm)
        {
            _characterTurnFsm = characterTurnFsm;
        }
        
        public void Enter()
        {
            Debug.Log("State Entered: Character Turn: Idle");
            _characterTurnFsm._teamTurn.NextTurn(); // tell the parent state to advance the turn by one
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
}