using UnityEngine;

namespace _Game.Scripts.CharacterTurn
{
    public class CharacterTurnMovementSelect : IState
    {
        private CharacterTurnFSM _characterTurnFsm;
        //private Soldier _soldier;

        public CharacterTurnMovementSelect(CharacterTurnFSM characterTurnFsm)
        {
            _characterTurnFsm = characterTurnFsm;
            //_soldier = _characterTurnFsm._soldier;
        }
        
        public void Enter()
        {
            Debug.Log("State Entered: Character Turn: Movement Select");
            Debug.Log("<color=#fdbb43>PRESS [1] Confirm Movement; [2] Cancel</color>");

            Vector3 pos = _characterTurnFsm._soldier.transform.position;
            Debug.Log("Character Pos: " + (int)pos.x + ", " + (int)pos.z);
            MapHandler.current.ShowNodesInDistance((int)pos.z, (int)pos.x, _characterTurnFsm._soldier.Movement);
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
            MapHandler.current.ClearHighlightedNodes();
        }
    }
}