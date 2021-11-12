using UnityEngine;

namespace _Game.Scripts
{
    public class Node
    {
        public bool isWalkable;
        public Vector4 Covering; // North, East, South, West
        public Vector4 Covered;  // North, East, South, West
        public GameObject Occupied = null;
        public GameObject Tile;
        private float _posX, _posY;
        
        public Node(string str)
        {
            Covering = Vector4.zero;
            Covered = Vector4.zero;
            
            switch (str)
            {
                case "H":
                    Covering = new Vector4(1, 1, 1, 1);
                    isWalkable = false;
                    break;
                case "F":
                    Covering = new Vector4(2, 2, 2, 2);
                    isWalkable = false;
                    break;
                default:
                    Covering = Vector4.zero;
                    isWalkable = true;
                    break;
            }

            if (Covering == Vector4.zero)
            {
                foreach (var c in str)
                {
                    switch (c)
                    {
                        case 'N':
                            //wallN = true;
                            Covering[0] = 2;
                            Covered[0] = 2;
                            break;
                        case 'E':
                            //wallE = true;
                            Covering[1] = 2;
                            Covered[1] = 2;
                            break;
                        case 'S':
                            //wallS = true;
                            Covering[2] = 2;
                            Covered[2] = 2;
                            break;
                        case 'W':
                            //wallW = true;
                            Covering[3] = 2;
                            Covered[3] = 2;
                            break;
                    }
                }
            }
        }
    }
}