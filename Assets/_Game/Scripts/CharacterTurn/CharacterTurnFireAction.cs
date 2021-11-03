using UnityEngine;

namespace _Game.Scripts.CharacterTurn
{
    public class CharacterTurnFireAction : IState
    {
        private CharacterTurnFSM _characterTurnFsm;

        public CharacterTurnFireAction(CharacterTurnFSM characterTurnFsm)
        {
            _characterTurnFsm = characterTurnFsm;
        }
        
        public void Enter()
        {
            Debug.Log("State Entered: Character Turn: Fire Action");
        }

        public void Tick()
        {
        }

        public void FixedTick()
        {
            _characterTurnFsm.ChangeState(_characterTurnFsm.Limbo);
        }

        public void Exit()
        {
        }
    }
}