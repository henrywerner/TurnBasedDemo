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
            Marker.current.OnMoveMarker.Invoke(
                (int)_characterTurnFsm._soldier.gameObject.transform.position.x, 
                (int)_characterTurnFsm._soldier.gameObject.transform.position.z);
            Marker.current.SetVisable(true);
            
            // Print to debug console
            Debug.Log("State Entered: Character Turn: Action Selection");
            Debug.Log("<color=#fdbb43>PRESS [1] Fire; [2] Move; [3] Reload</color>");
            
            // Show the action select buttons
            HUD.qt.SetApHeaderText("SELECT ACTION");
            HUD.qt.ShowActionButtons(true);
            
            // Listen for button presses
            HUD.qt.OnAttackButtonPress += Fire;
            HUD.qt.OnMoveButtonPress += Move;
            HUD.qt.OnReloadButtonPress += Reload;
        }

        private void Fire()
        {
            _characterTurnFsm.ChangeState(_characterTurnFsm.FireSelect);
        }

        private void Move()
        {
            _characterTurnFsm.ChangeState(_characterTurnFsm.MovementSelect);
        }

        private void Reload()
        {
            _characterTurnFsm.ChangeState(_characterTurnFsm.Reload);
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                _characterTurnFsm.ChangeState(_characterTurnFsm.MovementSelect);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                _characterTurnFsm.ChangeState(_characterTurnFsm.FireSelect);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                _characterTurnFsm.ChangeState(_characterTurnFsm.Reload);
        }

        public void FixedTick()
        {
        }

        public void Exit()
        {
            // Stop listening for buttons
            HUD.qt.OnAttackButtonPress -= Fire;
            HUD.qt.OnMoveButtonPress -= Move;
            HUD.qt.OnReloadButtonPress -= Reload;
            
            // hide button panel
            HUD.qt.ShowActionButtons(false);
        }
    }
}