using System.Collections;
using UnityEngine;

namespace _Game.Scripts.CharacterTurn
{
    public class CharacterTurnMovementAction : IState
    {
        private CharacterTurnFSM _characterTurnFsm;
        private float _duration, _time;
        private float _startPosX, _startPosZ, _endPosX, _endPosZ;
        private GameObject sully;

        public CharacterTurnMovementAction(CharacterTurnFSM characterTurnFsm)
        {
            _characterTurnFsm = characterTurnFsm;
        }
        
        public void Enter()
        {
            Debug.Log("State Entered: Character Turn: Movement Action");
            Debug.Log("Moving to [" + _characterTurnFsm._movementTarget.x +", " + _characterTurnFsm._movementTarget.y + "]");

            // hide the current player marker because it doesn't lerp with the character
            Marker.current.SetVisable(false);
            
            // print to event feed?
            _characterTurnFsm._soldier.evtFeed.AddEvent("Moving");
            
            // we have to reset the time vars everytime we enter
            _duration = 0.4f;
            _time = 0;
            
            Transform solTransform = _characterTurnFsm._soldier.gameObject.transform;
            _startPosX = solTransform.position.x;
            _startPosZ = solTransform.position.z;
            _endPosX = _characterTurnFsm._movementTarget.x;
            _endPosZ = _characterTurnFsm._movementTarget.y;
            sully = _characterTurnFsm._soldier.gameObject;
            
            // set the current tile as unoccupied
            MapHandler.current.setNodeOccupied((int)_startPosX, (int)_startPosZ, null);
        }

        public void Tick()
        {
            //_characterTurnFsm.ChangeState(_characterTurnFsm.Limbo);

            if (_time < _duration)
            {
                _time += Time.deltaTime;
                float x = Mathf.Lerp(_startPosX, _endPosX, _time / _duration);
                float z = Mathf.Lerp(_startPosZ, _endPosZ, _time / _duration);
                sully.transform.position = new Vector3(x, sully.transform.position.y, z);
            }
            else
            {
                sully.transform.position  = new Vector3(_endPosX, sully.transform.position.y, _endPosZ);
                _characterTurnFsm.ChangeState(_characterTurnFsm.Limbo); // exit state
            }
        }

        public void FixedTick()
        {
        }

        public void Exit()
        {
            // clear the current movement target
            _characterTurnFsm._movementTarget = new Vector2();
            
            // clear the selection indicator
            SelectionIndicator.current.Highlight(false);
            
            // update the current tile as occupied
            MapHandler.current.setNodeOccupied((int)_endPosX, (int)_endPosZ, _characterTurnFsm._soldier);
        }
    }
}