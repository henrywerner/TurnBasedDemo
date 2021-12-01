using UnityEngine;

namespace _Game.Scripts.CharacterTurn
{
    public class CharacterTurnReload : IState
    {
        private CharacterTurnFSM _characterTurnFsm;

        public CharacterTurnReload(CharacterTurnFSM characterTurnFsm)
        {
            _characterTurnFsm = characterTurnFsm;
        }
        
        public void Enter()
        {
            Debug.Log("State Entered: Character Turn: Reload");
            _characterTurnFsm._soldier.Reload();
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