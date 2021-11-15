using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts
{
    public class MapHandler : MonoBehaviour
    {
        public static MapHandler current;
        
        [SerializeField] private GameObject _tile;
        [SerializeField] private GameObject _halfCover, _fullCover;
        private const int North = 0, East = 1, South = 2, West = 3;
        private List<Vector2Int> highlightedNodes;

        private Node[][] currentMap;

        private void Awake()
        {
            highlightedNodes = new List<Vector2Int>();
        }

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
                    mapTile.SetActive(false);
                    map[i][j].Tile = mapTile; // store tile into the relevant node

                    pos.y = 0.5f;
                    float offset = 0.498f;
                    
                    // build cover indicators
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

        public Node GetNodeAt(int x, int y)
        {
            return currentMap[x][y];
        }

        public void ShowNodesInDistance(int startX, int startY, int radius)
        {
            /*
            List<Vector2Int> available = BFS(new Vector2Int(startX, startY), radius);

            string debugPrint = "Highlighted Tiles: ";
            
            foreach (var v2 in available)
            {
                Node n = GetNodeAt(v2.x, v2.y);
                debugPrint += "[" + v2.x + "," + v2.y + "] ";
                n.Tile.SetActive(true);
            }
            Debug.Log(debugPrint);
            highlightedNodes = available;
            */

            Gaming g = FindNodesInDistance(new Vector2Int(startX, startY),
                new int[currentMap.Length, currentMap[0].Length], new List<Vector3Int>(), radius);

            List<Vector2Int> available = new List<Vector2Int>();
            string debugPrint = "Highlighted Tiles: ";
            
            for (int i = 0; i < currentMap.Length; i++)
            {
                for (int j = 0; j < currentMap[i].Length; j++)
                {
                    if (g.visited[i, j] > 0)
                    {
                        Node n = GetNodeAt(i, j);
                        debugPrint += "([" + i + "," + j + "]" + g.visited[i,j] + ") ";
                        n.Tile.SetActive(true);
                        available.Add(new Vector2Int(i,j));
                    }
                }
            }
            Debug.Log(debugPrint);
            highlightedNodes = available;
        }

        public void ClearHighlightedNodes()
        {
            foreach (var v2 in highlightedNodes)
            {
                Node n = GetNodeAt(v2.x, v2.y);
                n.Tile.SetActive(false);
            }
            highlightedNodes.Clear();
        }
        
        private struct Gaming
        {
            public List<Vector3Int> path;
            public int[,] visited;
        }

        private Gaming FindNodesInDistance(Vector2Int currentNode, int[,] visited, List<Vector3Int> path, int distance)
        {
            Gaming g = new Gaming
            {
                path = path,
                visited = visited
            };

            if (distance < 0)
                return g;
            
            Node n = GetNodeAt(currentNode.x, currentNode.y);
            visited[currentNode.x, currentNode.y] = distance;
            path.Add(new Vector3Int(currentNode.x, currentNode.y, visited[currentNode.x, currentNode.y]));

            // add all adjacent vertices to list
            List<Vector2Int> adjacent = new List<Vector2Int>();
            Vector2Int coords = new Vector2Int();
            
            /*
            if (currentNode.x - 1 >= 0 && n.Covered[West] == 0)
            {
                coords = new Vector2Int(currentNode.x - 1, currentNode.y);
                if (visited[coords.x, coords.y] != 0 && visited[coords.x, coords.y] < distance || visited[coords.x, coords.y] == 0)
                    if (GetNodeAt(coords.x, coords.y).isWalkable)
                        adjacent.Add(coords);
            }
            */
            
            if (currentNode.x - 1 >= 0 && n.Covered[West] == 0)
            {
                coords = new Vector2Int(currentNode.x - 1, currentNode.y);
                
                // if we haven't visited this before
                if (visited[coords.x, coords.y] == 0)
                {
                    if (GetNodeAt(coords.x, coords.y).isWalkable)
                        adjacent.Add(coords);
                }
                else if (visited[coords.x, coords.y] < distance)
                {
                    adjacent.Add(coords);
                }
            }

            if (currentNode.y - 1 >= 0 && n.Covered[South] == 0)
            {
                coords = new Vector2Int(currentNode.x, currentNode.y - 1);
                if (visited[coords.x, coords.y] != 0 && visited[coords.x, coords.y] < distance || visited[coords.x, coords.y] == 0)
                    if (GetNodeAt(coords.x, coords.y).isWalkable)
                        adjacent.Add(coords);
            }

            if (currentNode.x + 1 < currentMap[0].Length && n.Covered[East] == 0)
            {
                coords = new Vector2Int(currentNode.x + 1, currentNode.y);
                if (visited[coords.x, coords.y] != 0 && visited[coords.x, coords.y] < distance || visited[coords.x, coords.y] == 0)
                    if (GetNodeAt(coords.x, coords.y).isWalkable)
                        adjacent.Add(coords);
            }

            if (currentNode.y + 1 < currentMap.Length && n.Covered[North] == 0)
            {
                coords = new Vector2Int(currentNode.x, currentNode.y + 1);
                if (visited[coords.x, coords.y] != 0 && visited[coords.x, coords.y] < distance || visited[coords.x, coords.y] == 0)
                    if (GetNodeAt(coords.x, coords.y).isWalkable)
                        adjacent.Add(coords);
            }

            foreach (var adj in adjacent)
            {
                Gaming temp = FindNodesInDistance(adj, visited, path, distance - 1);
                foreach (var v in temp.path)
                {
                    if (visited[v.x, v.y] != 0 && visited[v.x, v.y] < v.z)
                        visited[v.x, v.y] = v.z;
                }
            }

            // update the struct again????
            g.path = path;
            g.visited = visited;
            return g;
        }

        private List<Vector2Int> BFS(Vector2Int root, int radius)
        {
            Queue<Vector2Int> queue = new Queue<Vector2Int>();
            bool[,] visted = new bool[currentMap.Length, currentMap[0].Length];
            Dictionary<Vector2Int, int> dist = new Dictionary<Vector2Int, int>();
            List<Vector2Int> validNodes = new List<Vector2Int>();

            visted[root.x, root.y] = true;
            dist.Add(root, 0);
            queue.Enqueue(root);

            while (queue.Count != 0)
            {
                Vector2Int v = queue.Dequeue();
                int vDist = dist[v]; // what if it's null????
                Node vNode = GetNodeAt(v.x, v.y);
                
                // add all adjacent vertices to list
                List<Vector2Int> adjacent = new List<Vector2Int>();
                if (v.x - 1 >= 0 && vNode.Covered[West] == 0)
                    adjacent.Add(new Vector2Int(v.x - 1, v.y));
                if (v.y - 1 >= 0 && vNode.Covered[South] == 0)
                    adjacent.Add(new Vector2Int(v.x, v.y - 1));
                if (v.x + 1 < currentMap[0].Length && vNode.Covered[East] == 0)
                    adjacent.Add(new Vector2Int(v.x + 1, v.y));
                if (v.y + 1 < currentMap.Length && vNode.Covered[North] == 0)
                    adjacent.Add(new Vector2Int(v.x, v.y +1));

                foreach (var n in adjacent)
                {
                    // if not visited and the node is walkable
                    if (!visted[n.x, n.y] && GetNodeAt(n.x, n.y).isWalkable)
                    {
                        visted[n.x, n.y] = true;
                        
                        if (vDist + 1 < dist[n])
                            dist.Remove(n);
                        
                        dist.Add(n, vDist + 1);
                        validNodes.Add(n);
                        
                        // if the total x+y distance is within the total move distance 
                        // if (Math.Abs(root.x - n.x) + Math.Abs(root.y - n.y) <= radius)
                        //     queue.Enqueue(n);
                        
                        if (dist[n] <= radius)
                            queue.Enqueue(n);
                    }
                }
            }

            return validNodes;
        }
    }
}