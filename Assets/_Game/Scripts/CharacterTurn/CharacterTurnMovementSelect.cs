using UnityEngine;

namespace _Game.Scripts.CharacterTurn
{
    public class CharacterTurnMovementSelect : IState
    {
        private CharacterTurnFSM _characterTurnFsm;

        public CharacterTurnMovementSelect(CharacterTurnFSM characterTurnFsm)
        {
            _characterTurnFsm = characterTurnFsm;
        }
        
        public void Enter()
        {
            Debug.Log("State Entered: Character Turn: Movement Select");
            Debug.Log("<color=#fdbb43>PRESS [1] Confirm Movement; [2] Cancel</color>");
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                _characterTurnFsm.ChangeState(_characterTurnFsm.MovementAction);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                _characterTurnFsm.ChangeState(_characterTurnFsm.ActionSelection);
        }

        public void FixedTick()
        {
        }

        public void Exit()
        {
        }
    }
}