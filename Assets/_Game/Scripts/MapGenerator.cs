using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Game.Scripts
{
    public class MapGenerator
    {
        public Node[][] Generate(string filepath)
        {
            string[] lines = System.IO.File.ReadAllLines(filepath);
            string[][] nodeStr = new string[lines.Length][];
            Node[][] nodes = new Node[lines.Length][];
            //List<List<string>> nodess = new List<List<string>>();

            //for (int i = lines.Length - 1; i >= 0; i--)
            for (int i = 0; i < lines.Length; i++)
            {
                nodeStr[i] = lines[i].Split(']').Select(s => s.Trim('[').Trim()).ToArray();
                
                nodeStr[i] = nodeStr[i].Take(nodeStr[i].Length - 1).ToArray(); // remove the final element of the array
                
                /*
                // debug print
                string s = "";
                foreach (var n in nodeStr[i])
                {
                    if (n == string.Empty)
                        s += '_' + ";";
                    else
                        s += n + ";";
                }
                Debug.Log(s);
                */
            }
            
            Array.Reverse(nodeStr);
                


            for (int i = 0; i < nodeStr.Length; i++)
            {
                nodes[i] = new Node[lines.Length];
                for (int j = 0; j < nodeStr[i].Length; j++)
                {
                    // generate node
                    nodes[i][j] = new Node(nodeStr[i][j]);
                }
            }

            return ComputeCover(nodes); // compute the cover, then return
        }

        private Node[][] ComputeCover(Node[][] map)
        {
            const int north = 0, east = 1, south = 2, west = 3;
            
            // for every tile
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    Node tile = map[i][j];

                    if (tile.Covering[north] > 0 && i + 1 < map.Length) // if covering north
                    {
                        // if target tile's covered value is less, then replace
                        if (map[i + 1][j].Covered[south] < tile.Covering[north])
                            map[i + 1][j].Covered[south] = tile.Covering[north];
                    }
                    
                    if (tile.Covering[east] > 0 && j + 1 < map.Length) // if covering east
                    {
                        // if target tile's covered value is less, then replace
                        if (map[i][j + 1].Covered[west] < tile.Covering[east])
                            map[i][j + 1].Covered[west] = tile.Covering[east];
                    }
                    
                    if (tile.Covering[south] > 0 && i - 1 >= 0) // if covering south
                    {
                        // if target tile's covered value is less, then replace
                        if (map[i - 1][j].Covered[north] < tile.Covering[south])
                            map[i - 1][j].Covered[north] = tile.Covering[south];
                    }
                    
                    if (tile.Covering[west] > 0 && j - 1 >= 0) // if covering west
                    {
                        // if target tile's covered value is less, then replace
                        if (map[i][j - 1].Covered[east] < tile.Covering[west])
                            map[i][j - 1].Covered[east] = tile.Covering[west];
                    }
                }
            }

            return map;
        }
    }
}