using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable; //Boolean to check if the tile is walkable or not (will switch to false when a unit is located on this tile)
    public Vector3 worldPos; //x,y,z positions in scene of this node

    public int gridX; //x coordinate on the grid
    public int gridY; //y coordinate on the grid
    

    public int gCost; //Distance from starting node
    public int hCost; //(heuristic) Distance from end node
    public Node parent; //Used for pathfinding - keeps track of the  
    public int fCost
    {
        get
        {
            return hCost + gCost;
        }
    }

    public int gridZ
    {
        get
        {
            return - gridX - gridY;
        }
    }

    public Node(bool _walkable, int _gridX, int _gridY, Vector3 _worldPos)
    {
        walkable = _walkable;
        gridX = _gridX;
        gridY = _gridY;
        worldPos = _worldPos;
    }
}
