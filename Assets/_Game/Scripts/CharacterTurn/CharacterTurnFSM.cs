using System;
using System.Collections.Generic;
using _Game.Scripts;
using _Game.Scripts.CharacterTurn;
using UnityEngine;

public class CharacterTurnFSM : StateMachine
 {
     internal Soldier _soldier;
     public short ActionsRemaining;
     private const short CHARACTER_ACTIONS = 2;
     internal ITeamTurn _teamTurn;
     internal Vector2 _movementTarget;

     internal List<RaycastHit> _queuedShots;

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
         
         /*
         // if respawn timer is 0, then respawn soldier at start of turn
         if (_soldier.IsDead && _soldier.RespawnTimer <= 0)
         {
             // respawn
             _soldier.CurrentHP = _soldier.MaxHP;
             HUD.qt.UpdateSoldierDead(_soldier);
             HUD.qt.SetSoldierTurn(_soldier);
             HUD.qt.SetActionsRemaining(_soldier, CHARACTER_ACTIONS);
             ChangeState(ActionSelection);
         }
         */
         
         if (_soldier.IsDead) // this should automatically use the player's turn up
         {
             _soldier.RespawnTimer--;
             ActionsRemaining = 0;
             HUD.qt.UpdateSoldierRespawnCounter(_soldier);
             ChangeState(Limbo);
         }
         else
         {
             HUD.qt.SetSoldierTurn(_soldier);
             HUD.qt.SetActionsRemaining(_soldier, CHARACTER_ACTIONS);
         
             ChangeState(ActionSelection);
         }
     }
 }