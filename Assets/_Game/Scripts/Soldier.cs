using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class Soldier : MonoBehaviour
    {
        [SerializeField] private Texture[] _poses;
        public Sprite charPortrait = null;
        private Camera _cam;
        //[SerializeField] private GameObject parent;
        [SerializeField] private GameObject child;
        private Renderer _rend;
        [SerializeField] internal EventFeed evtFeed;
        [SerializeField] private GameObject teamIndicator;
        [SerializeField] private Material blueColor, redColor;
        
        public team Team;
        public int AmmoCount, MaxAmmo;
        public int CurrentHP, MaxHP;
        public bool IsDead;
        public short RespawnTimer = 0;

        public int Movement = 5;
        //public int forwardDirection;
        
        private Vector2 _movementTarget;
        public Vector2 movementTarget
        {
            get => _movementTarget;
            set => _movementTarget = value;
        }

        public enum team
        {
            Blue = 0,
            Red = 1
        }

        public Soldier(team playerTeam, int maxHp, int maxAmmo)
        {
            Team = playerTeam;
            MaxHP = maxHp;
            CurrentHP = maxHp;
            MaxAmmo = maxAmmo;
            AmmoCount = maxAmmo;
        }

        public int GetCover(int direction) // 0 = N; 1 = E, 2 = S, 3 = W
        {
            var position = gameObject.transform.position;
            Node n = MapHandler.current.GetNodeAt((int)position.x, (int)position.y);

            return (int)n.Covered[direction];
        }

        public void Damage()
        {
            CurrentHP--;
            HUD.qt.UpdateSoldierHealth(this);
            
            // Check if soldier is dead
            IsDead = CurrentHP <= 0;

            if (IsDead)
            {
                SetDead(IsDead);
            }
                
        }

        public void SetDead(bool dead)
        {
            IsDead = dead;
            gameObject.GetComponent<BoxCollider>().enabled = !dead;
            child.GetComponent<MeshRenderer>().enabled = !dead;
            HUD.qt.UpdateSoldierDead(this);
        }

        public void DepleteAmmo()
        {
            AmmoCount--;
            HUD.qt.UpdateSoldierAmmo(this);
        }

        public void Reload()
        {
            AmmoCount = MaxAmmo;
            HUD.qt.UpdateSoldierAmmo(this);
            EffectsManager.current.PlayReloadSound();
        }

        private void Start()
        {
            _cam = Camera.main;
            _rend = child.GetComponent<Renderer>();

            teamIndicator.GetComponent<Renderer>().material = Team == team.Blue ? blueColor : redColor;
        }

        public void Update()
        {
            child.transform.rotation = Quaternion.LookRotation(_cam.transform.position) * Quaternion.Euler(0, 180, 0);
            child.transform.localEulerAngles = new Vector3(0, child.transform.localEulerAngles.y, 0);

            //var rotDelta = parent.transform.rotation * Quaternion.Inverse(_cam.transform.rotation);
            var rotDelta = transform.rotation.eulerAngles.y - _cam.transform.rotation.eulerAngles.y;
            rotDelta = Math.Abs(rotDelta);
            rotDelta = 360 - rotDelta;
            //Debug.Log("Delta: " + rotDelta);
            
            // NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE NIGHTMARE  

            if (rotDelta <= 22.5 || rotDelta >= 337.5)
            {
                // 6:00
                _rend.material.SetTexture("_MainTex", _poses[4]);
            } 
            else if (rotDelta > 22.5 && rotDelta <= 67.5)
            {
                // 4:30
                _rend.material.SetTexture("_MainTex", _poses[3]);
            }
            else if (rotDelta > 67.5 && rotDelta <= 112.5)
            {
                // 3:00
                _rend.material.SetTexture("_MainTex", _poses[2]);
            }
            else if (rotDelta > 112.5 && rotDelta <= 157.5)
            {
                // 1:30
                _rend.material.SetTexture("_MainTex", _poses[1]);
            }
            else if (rotDelta > 157.5 && rotDelta <= 202.5)
            {
                // 12:00
                _rend.material.SetTexture("_MainTex", _poses[0]);
            }
            else if (rotDelta > 202.5 && rotDelta <= 247.5)
            {
                // 10:30
                _rend.material.SetTexture("_MainTex", _poses[7]);
            }
            else if (rotDelta > 247.5 && rotDelta <= 292.5 )
            {
                // 9:00
                _rend.material.SetTexture("_MainTex", _poses[6]);
            }
            else
            {
                // 8:30
                _rend.material.SetTexture("_MainTex", _poses[5]);
            }
        }
    }
}