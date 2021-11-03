using UnityEngine;

namespace _Game.Scripts.CharacterTurn
{
    public class CharacterTurnLimbo : IState
    {
        private CharacterTurnFSM _characterTurnFsm;
        private IState nextState;

        public CharacterTurnLimbo(CharacterTurnFSM characterTurnFsm)
        {
            _characterTurnFsm = characterTurnFsm;
        }
        
        public void Enter()
        {
            Debug.Log("State Entered: Character Turn: Limbo");
            _characterTurnFsm.ActionsRemaining--;
            
            if (_characterTurnFsm.ActionsRemaining > 0)
            {
                nextState = _characterTurnFsm.ActionSelection;
            }
            else
                nextState = _characterTurnFsm.Idle;
            
            
            Debug.Log("Character remaining actions: [" + _characterTurnFsm.ActionsRemaining + "/2]");
        }

        public void Tick()
        {
            _characterTurnFsm.ChangeState(nextState);
        }

        public void FixedTick()
        {
        }

        public void Exit()
        {
        }
    }
}