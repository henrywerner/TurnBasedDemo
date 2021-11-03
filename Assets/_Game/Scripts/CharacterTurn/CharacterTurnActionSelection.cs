using UnityEngine;

namespace _Game.Scripts.CharacterTurn
{
    public class CharacterTurnActionSelection : IState
    {
        private CharacterTurnFSM _characterTurnFsm;

        public CharacterTurnActionSelection(CharacterTurnFSM characterTurnFsm)
        {
            _characterTurnFsm = characterTurnFsm;
        }
        
        public void Enter()
        {
            Debug.Log("State Entered: Character Turn: Action Selection");
            Debug.Log("<color=#fdbb43>PRESS [1] Fire; [2] Move; [3] Reload</color>");
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                _characterTurnFsm.ChangeState(_characterTurnFsm.FireSelect);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                _characterTurnFsm.ChangeState(_characterTurnFsm.MovementSelect);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                _characterTurnFsm.ChangeState(_characterTurnFsm.Reload);
        }

        public void FixedTick()
        {
        }

        public void Exit()
        {
        }
    }
}