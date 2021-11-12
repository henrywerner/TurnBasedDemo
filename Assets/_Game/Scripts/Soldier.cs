using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class Soldier : MonoBehaviour
    {
        [SerializeField] private Texture[] _poses;
        private Camera _cam;
        [SerializeField] private GameObject parent;
        private Renderer _rend;
        
        public bool Team;
        public int AmmoCount, MaxAmmo;
        public int CurrentHP, MaxHP;
        public bool IsDead => CurrentHP <= MaxHP;
        public short RespawnTimer = 0;
        public int forwardDirection;

        public Soldier(bool team, int maxHp, int maxAmmo)
        {
            Team = team;
            MaxHP = maxHp;
            CurrentHP = maxHp;
            MaxAmmo = maxAmmo;
            AmmoCount = maxAmmo;
            forwardDirection = 3;
        }

        public int GetCover(int direction) // 0 = N; 1 = E, 2 = S, 3 = W
        {
            var position = gameObject.transform.position;
            Node n = MapBuilder.current.GetNodeAt((int)position.x, (int)position.y);

            return (int)n.Covered[direction];
        }

        private void Start()
        {
            _cam = Camera.main;
            _rend = gameObject.GetComponent<Renderer>();
        }

        public void Update()
        {
            transform.rotation = Quaternion.LookRotation(_cam.transform.position) * Quaternion.Euler(0, 180, 0);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

            //var rotDelta = parent.transform.rotation * Quaternion.Inverse(_cam.transform.rotation);
            var rotDelta = parent.transform.rotation.eulerAngles.y - _cam.transform.rotation.eulerAngles.y;
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