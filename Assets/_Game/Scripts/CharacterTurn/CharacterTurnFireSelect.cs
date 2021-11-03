using UnityEngine;

namespace _Game.Scripts.CharacterTurn
{
    public class CharacterTurnFireSelect : IState
    {
        private CharacterTurnFSM _characterTurnFsm;

        public CharacterTurnFireSelect(CharacterTurnFSM characterTurnFsm)
        {
            _characterTurnFsm = characterTurnFsm;
        }
        
        public void Enter()
        {
            Debug.Log("State Entered: Character Turn: Fire Select");
            Debug.Log("<color=#fdbb43>PRESS [1] Confirm Target; [2] Cancel</color>");
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                _characterTurnFsm.ChangeState(_characterTurnFsm.FireAction);
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