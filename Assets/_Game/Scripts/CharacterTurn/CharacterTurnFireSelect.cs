using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.CharacterTurn
{
    public class CharacterTurnFireSelect : IState
    {
        private CharacterTurnFSM _characterTurnFsm;
        private List<Soldier> _enemyTeamCache;
        //private ShotData[] _hitCalculations;
        private Dictionary<Soldier, ShotData> _hitCalculations;
        private RaycastHit prevMouseOver;
        private Camera _camera;
        private bool isBlueTeam;
        private const int BURSTFIRE_SHOTS = 5;

        public CharacterTurnFireSelect(CharacterTurnFSM characterTurnFsm)
        {
            _characterTurnFsm = characterTurnFsm;
            _camera = Camera.main;
            _enemyTeamCache = new List<Soldier>();
            _hitCalculations = new Dictionary<Soldier, ShotData>();
        }
        
        public void Enter()
        {
            Debug.Log("State Entered: Character Turn: Fire Select");
            Debug.Log("<color=#fdbb43>PRESS [Mouse1] Confirm Target; [Mouse2] Cancel</color>");
            
            // Update the action panel
            HUD.qt.SetApHeaderText("SELECT TARGET");
            HUD.qt.ShowControls(true);
            
            isBlueTeam = _characterTurnFsm._soldier.Team == Soldier.team.Blue;

            // get enemy team pos
            List<Vector2> enemyTeamPos = isBlueTeam
                ? MapHandler.current.getRedTeamPos()
                : MapHandler.current.getBlueTeamPos();
            
            // make sure cache is clear
            _enemyTeamCache.Clear();

            // get the nodes for each enemy location and add them to the cache
            foreach (var pos in enemyTeamPos)
                _enemyTeamCache.Add(MapHandler.current.GetNodeAt((int)pos.x, (int)pos.y).Occupied);
        }

        struct ShotData
        {
            public List<RaycastHit> ShotsFired;
            public List<RaycastHit> ShotsConnected;
            public float HitChance => GetHitChance();
            public float GetHitChance()
            {
                return ((float)ShotsConnected.Count / ShotsFired.Count) * 100;
            }
        }

        private ShotData FindShotData(Soldier target)
        {
            int sampleSize = 39;
            float maxDeviation = 5f;
            
            _characterTurnFsm._soldier.gameObject.transform.LookAt(target.transform);

            List<RaycastHit> raycastHits = new List<RaycastHit>();
            List<RaycastHit> connectedShots = new List<RaycastHit>();
            
            Vector3 gunPos = new Vector3(_characterTurnFsm._soldier.transform.position.x, 1.25f,
                _characterTurnFsm._soldier.transform.position.z);
            
            for (int i = 0; i < sampleSize; i++)
            {
                Vector3 forwardVector = Vector3.forward;
                float deviation = Random.Range(0f, maxDeviation);
                float angle = Random.Range(0f, 360f);
                forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
                forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
                forwardVector = _characterTurnFsm._soldier.transform.rotation * forwardVector;
                Debug.DrawRay(gunPos, forwardVector, Color.cyan, 2, true);

                RaycastHit h;
                if (Physics.Raycast(gunPos, forwardVector, out h))
                {
                    // Note: This raycast code is a mess and uses a ton of if-elses because I wrote it in 15 min
                    
                    Soldier s = h.transform.GetComponent<Soldier>();
                    if (s != null)
                    {
                        if (s == _characterTurnFsm._soldier) // I just need people to stop shooting themselves. Please.
                        {
                            // I don't have time to make this better
                            RaycastHit[] hits = Physics.RaycastAll(gunPos, forwardVector, 999);
                            System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance)); // sort the array based on distance
                            foreach (var rch in hits)
                            {
                                s = rch.transform.GetComponent<Soldier>();
                                if (s != null)
                                {
                                    if (s == _characterTurnFsm._soldier) continue;
                                    
                                    connectedShots.Add(rch);
                                    raycastHits.Add(rch);
                                    break;
                                }
                                else
                                    raycastHits.Add(rch);
                            }
                        }
                        else
                        {
                            connectedShots.Add(h);
                            raycastHits.Add(h);
                            // Debug.DrawRay(gunPos, forwardVector * 1000, Color.red, 2, true);
                        }
                    }
                    else
                    {
                        raycastHits.Add(h);
                    }
                }
            }
            Debug.Log("Shots Fired: " + raycastHits.Count);
            Debug.Log("Shots Hit: " + connectedShots.Count);

            ShotData sd = new ShotData();
            sd.ShotsFired = raycastHits;
            sd.ShotsConnected = connectedShots;
            return sd;
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                _characterTurnFsm.ChangeState(_characterTurnFsm.FireAction);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                _characterTurnFsm.ChangeState(_characterTurnFsm.ActionSelection);
            
            // if moused over target
            RaycastHit h;
            Ray r = _camera.ScreenPointToRay(Input.mousePosition);
            bool isEnemy = false;
            int x = 0, z = 0;
            if (Physics.Raycast(r, out h) && h.transform != prevMouseOver.transform)
            {
                isEnemy = h.transform.gameObject.CompareTag(isBlueTeam ? "RedTeam" : "BlueTeam");
                // if (h.transform.gameObject.GetComponentInParent<Soldier>() != null)
                //     isEnemy = h.transform.gameObject.GetComponentInParent<Soldier>().Team != _characterTurnFsm._soldier.Team;
                //
                Soldier s = h.transform.gameObject.GetComponent<Soldier>();
                if (s != null)
                    isEnemy = (s.Team != _characterTurnFsm._soldier.Team);
                
                if (isEnemy)
                {
                    x = (int)h.transform.position.x;
                    z = (int)h.transform.position.z;
                    Debug.Log("enemy targeted: " + h.transform.gameObject.name + " @ [" + x + "," + z + "]");
                    SelectionIndicator.current.OnMoveIndicator.Invoke(x,z);
                    SelectionIndicator.current.Highlight(true);

                    //Soldier s = h.transform.gameObject.GetComponent<Soldier>();
                    if (_hitCalculations.ContainsKey(s))
                    {
                        Debug.Log("(Prev Calculated) Hit Chance: " + _hitCalculations[s].HitChance + "%");
                    }
                    else
                    {
                        ShotData sd = FindShotData(s);
                        _hitCalculations.Add(s, sd);
                        Debug.Log("Hit Chance: " + _hitCalculations[s].HitChance + "%");
                    }
                }
                else
                {
                    SelectionIndicator.current.Highlight(false);
                }
            }
            prevMouseOver = h;
            
            // On Left Mouse Button
            if (Input.GetMouseButtonDown(0) && prevMouseOver.transform.gameObject.CompareTag(isBlueTeam 
                ? "RedTeam" 
                : "BlueTeam"))
            {
                // roll to select which raycasts to use
                List<RaycastHit> shotsFired = _hitCalculations[prevMouseOver.transform.gameObject.GetComponent<Soldier>()].ShotsFired;
                List<RaycastHit> shots = new List<RaycastHit>();
                for (int i = 0; i < BURSTFIRE_SHOTS; i++)
                {
                    int j = Random.Range(0, shotsFired.Count);
                    shots.Add(shotsFired[j]);
                }
                
                // store selected casts in the main fsm
                _characterTurnFsm._queuedShots = shots;
                
                // go to fire action
                _characterTurnFsm.ChangeState(_characterTurnFsm.FireAction);
            } 
            // On Right Mouse Button
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
            // clear caches
            _enemyTeamCache.Clear();
            _hitCalculations.Clear();
            
            // clear the selection indicator
            SelectionIndicator.current.Highlight(false);
            
            // hide controls
            HUD.qt.ShowControls(false);
        }
    }
}