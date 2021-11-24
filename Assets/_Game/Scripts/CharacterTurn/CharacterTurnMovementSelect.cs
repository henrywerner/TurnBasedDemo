using UnityEngine;

namespace _Game.Scripts.CharacterTurn
{
    public class CharacterTurnMovementSelect : IState
    {
        private CharacterTurnFSM _characterTurnFsm;
        private RaycastHit prevHit;
        private Camera _camera;
        //private Soldier _soldier;

        public CharacterTurnMovementSelect(CharacterTurnFSM characterTurnFsm)
        {
            _characterTurnFsm = characterTurnFsm;
            _camera = Camera.main;
            //_soldier = _characterTurnFsm._soldier;
        }
        
        public void Enter()
        {
            Debug.Log("State Entered: Character Turn: Movement Select");
            Debug.Log("<color=#fdbb43>PRESS [Mouse1] Confirm Movement; [Mouse2] Cancel</color>");

            Vector3 pos = _characterTurnFsm._soldier.transform.position;
            Debug.Log("Character Pos: " + (int)pos.x + ", " + (int)pos.z);
            MapHandler.current.ShowNodesInDistance((int)pos.z, (int)pos.x, _characterTurnFsm._soldier.Movement);
        }

        public void Tick()
        {
            RaycastHit h;
            Ray r = _camera.ScreenPointToRay(Input.mousePosition);
            bool isTile = false;
            int x = 0, z = 0;
            if (Physics.Raycast(r, out h) && h.transform != prevHit.transform)
            {
                isTile = h.transform.gameObject.CompareTag("Tile");
                if (isTile)
                {
                    x = (int)h.transform.position.x;
                    z = (int)h.transform.position.z;
                    Debug.Log("hit " + h.transform.gameObject.name + " [" + x + "," + z + "]");
                    SelectionIndicator.current.OnMoveIndicator.Invoke(x,z);
                    SelectionIndicator.current.Highlight(true);
                }
                else
                {
                    SelectionIndicator.current.Highlight(false);
                }
                    
            }
            prevHit = h;
            
            if (Input.GetMouseButtonDown(0) && prevHit.transform.gameObject.CompareTag("Tile"))
            {
                _characterTurnFsm._movementTarget = new Vector2(prevHit.transform.position.x, prevHit.transform.position.z);
                _characterTurnFsm.ChangeState(_characterTurnFsm.MovementAction);
            } 
            else if (Input.GetMouseButtonDown(1))
            {
                _characterTurnFsm.ChangeState(_characterTurnFsm.ActionSelection);
            }
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