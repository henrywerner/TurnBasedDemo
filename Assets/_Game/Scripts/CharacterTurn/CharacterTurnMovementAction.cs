using UnityEngine;

namespace _Game.Scripts.CharacterTurn
{
    public class CharacterTurnMovementAction : IState
    {
        private CharacterTurnFSM _characterTurnFsm;

        public CharacterTurnMovementAction(CharacterTurnFSM characterTurnFsm)
        {
            _characterTurnFsm = characterTurnFsm;
        }
        
        public void Enter()
        {
            Debug.Log("State Entered: Character Turn: Movement Action");
        }

        public void Tick()
        {
            _characterTurnFsm.ChangeState(_characterTurnFsm.Limbo);
        }

        public void FixedTick()
        {
        }

        public void Exit()
        {
        }
    }
}