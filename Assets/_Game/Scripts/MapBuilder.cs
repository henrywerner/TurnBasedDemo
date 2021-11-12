using System;
using UnityEngine;

namespace _Game.Scripts
{
    public class MapBuilder : MonoBehaviour
    {
        public static MapBuilder current;
        
        [SerializeField] private GameObject _tile;
        [SerializeField] private GameObject _halfCover, _fullCover;
        private const int North = 0, East = 1, South = 2, West = 3;

        private Node[][] currentMap;

        private void Start()
        {
            current = this;

            MapGenerator generator = new MapGenerator();
            currentMap = generator.Generate("Assets/_Game/Map/map1.txt");
            BuildMap(currentMap);
        }

        public void BuildMap(Node[][] map)
        {
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (!map[i][j].isWalkable) continue;

                    Vector3 pos = new Vector3(j + 0.5f, 0.002f, i + 0.5f); // define with 0.5u offset
                    
                    GameObject mapTile = Instantiate(_tile, pos, Quaternion.Euler(90, 0, 0));
                    pos.y = 0.5f;

                    float offset = 0.498f;
                    
                    var cover = map[i][j].Covered[North];
                    if (cover > 0)
                        Instantiate(cover == 2 ? _fullCover : _halfCover, 
                            new Vector3(pos.x, pos.y, pos.z + offset), 
                            Quaternion.Euler(0, 0, 0),
                            mapTile.transform);
                    
                    cover = map[i][j].Covered[East];
                    if (cover > 0)
                        Instantiate(cover == 2 ? _fullCover : _halfCover, 
                            new Vector3(pos.x + offset, pos.y, pos.z), 
                            Quaternion.Euler(0, 90, 0),
                            mapTile.transform);

                    cover = map[i][j].Covered[South];
                    if (cover > 0)
                        Instantiate(cover == 2 ? _fullCover : _halfCover, 
                            new Vector3(pos.x, pos.y, pos.z - offset), 
                            Quaternion.Euler(0, 180, 0),
                            mapTile.transform);
                    
                    cover = map[i][j].Covered[West];
                    if (cover > 0)
                        Instantiate(cover == 2 ? _fullCover : _halfCover, 
                            new Vector3(pos.x - offset, pos.y, pos.z), 
                            Quaternion.Euler(0, 270, 0),
                            mapTile.transform);
                }
            }
        }
    }
}