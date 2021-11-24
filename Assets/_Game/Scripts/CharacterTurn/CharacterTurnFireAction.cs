using System.Collections.Generic;
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
            
            // move the gunfire animator to the soldier's pos. This is mostly for debugging with raycasts.
            GunFireAnimator.current.gameObject.transform.position = new Vector3(
                _characterTurnFsm._soldier.gameObject.transform.position.x,
                1.25f,
                _characterTurnFsm._soldier.gameObject.transform.position.z);
            
            // play gunfire animation
            GunFireAnimator.current.FireShots(_characterTurnFsm._queuedShots, 0.1f, _characterTurnFsm._soldier);

            // force this non-monobehavior script to wait for a coroutine?
            // while (GunFireAnimator.current.animationInProgress)
            // {
            //     // wait?
            // }
            
            // move to next state
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
            // clear queued shots
            _characterTurnFsm._queuedShots.Clear();
        }
    }
}