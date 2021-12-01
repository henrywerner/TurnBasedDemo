using System.Collections.Generic;
using _Game.Scripts;
using UnityEngine;

public class PathfindingHandler
{
    private List<Node> openList, closedList;
    
    public void FindPossibleMovement(int startX, int startY, int maxDistance, Node[][] map)
    {
        // make sure source node exists
        if (map[startX][startY] == null)
        {
            Debug.Log("Player position is not on map");
            return;
        }
        
        // Limit the x y coords to the array bounds
        int minX = startX - maxDistance;
        if (minX < 0) minX = 0;

        int maxX = startX + maxDistance;
        if (maxX >= map.Length) maxX = map.Length - 1;

        int minY = startY - maxDistance;
        if (minY < 0) minY = 0;

        int maxY = startY + maxDistance;
        if (maxY >= map[0].Length) maxY = map[0].Length - 1;
        
        // for every x y coord that's inbounds
        for (int x = startX - maxDistance; x < startX + maxDistance; x++)
        {
            for (int y = startY - maxDistance; y < startY + maxDistance; y++)
            {
                // check if tile is obstructed
                if (!map[x][y].isWalkable)
                    continue;
                
                // check if tile is reachable
            }
        }
    }
    
    private struct pathNode
    {
        public Node parent;
        public int f, g, h;
    }

    private List<Node> AStar(int startX, int startY, int endX, int endY)
    {
        // If the destination is also the starting point
        if (startX == endX && startY == endY)
            return null;

        Node origin = MapHandler.current.GetNodeAt(startX, startY);
        openList = new List<Node> { origin };
        closedList = new List<Node>();
        
        // TODO: delete this. this is just so unity won't yell at me.
        return null;
    }
}