using System;
using _Game.Scripts;
using _Game.Scripts.CharacterTurn;

public class CharacterTurnFSM : StateMachine
 {
     internal Soldier _soldier;
     public short ActionsRemaining;
     private const short CHARACTER_ACTIONS = 2;
     internal ITeamTurn _teamTurn;

     public CharacterTurnActionSelection ActionSelection { get; private set; }
     public CharacterTurnFireAction FireAction { get; private set; }
     public CharacterTurnFireSelect FireSelect { get; private set; }
     public CharacterTurnMovementAction MovementAction { get; private set; }
     public CharacterTurnMovementSelect MovementSelect { get; private set; }
     public CharacterTurnReload Reload { get; private set; }
     public CharacterTurnIdle Idle { get; private set; }
     
     public CharacterTurnLimbo Limbo { get; private set; }

     public CharacterTurnFSM(Soldier soldier, ITeamTurn turn)
     {
         _soldier = soldier;
         _teamTurn = turn;
     }
     
     private void Awake()
     {
         ActionSelection = new CharacterTurnActionSelection(this);
         FireAction = new CharacterTurnFireAction(this);
         FireSelect = new CharacterTurnFireSelect(this);
         MovementAction = new CharacterTurnMovementAction(this);
         MovementSelect = new CharacterTurnMovementSelect(this);
         Reload = new CharacterTurnReload(this);
         Idle = new CharacterTurnIdle(this);
         Limbo = new CharacterTurnLimbo(this);
     }
     
     public void BeginTurn()
     {
         ActionsRemaining = CHARACTER_ACTIONS;
         
         // // if respawn timer is 0, then respawn soldier at start of turn
         // if (_soldier.IsDead && _soldier.RespawnTimer <= 0)
         // {
         //     // respawn
         // }
         // else if (_soldier.IsDead)
         // {
         //     _soldier.RespawnTimer--;
         //     // this should automatically use the player's turn up
         //     return;
         // }
         
         ChangeState(ActionSelection);
     }
 }