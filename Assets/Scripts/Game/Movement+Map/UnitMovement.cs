using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public GameObject target;
    public GameObject[] enemies;

    public GameObject currentTile;
    public List<Node> adjacencyList = new List<Node>();
    public List<Node> path = new List<Node>();

    public int attackRange = 1;

    public HexTileMapGenerator hexMap;

    public Vector3 heading;
    public float moveSpeed = 4.0f;

    // Update is called once per frame
    void Update()
    {
        GetCurrentTile();
        if(target == null)
        {
            target = FindClosestEnemyUnit();
            print(target.gameObject.name);
        }
        FindPath(hexMap.findNodeFromTile(currentTile), hexMap.findNodeFromTile(target.GetComponent<CurrentTileTest>().currentTile));
        if(path.Count > 1)
        {
            Move();
        }
        //Move();
    }

    private void Start()
    {
        hexMap = HexTileMapGenerator.Instance;
    }
   

    void Move()
    {
        Vector3 velocity = new Vector3();
        
        heading = (path.First().worldPos + new Vector3(0,0.5f,0)) - transform.position;
        heading.Normalize();

        velocity = heading * moveSpeed;
        transform.forward = heading;
        transform.position += velocity * Time.deltaTime;
    }

    //Finds the tile we are currently on by raycasting a ray downwards, and assigns the currentTile attribute
    //to the gameObject we found
    public void GetCurrentTile()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            if (hit.collider.gameObject.tag == "Tile")
            {
                currentTile = hit.collider.gameObject;
            }
        }

    }

    //Returns the closest enemy of this unit, will need to switch to use hex distance instead of euclidian distance
    GameObject FindClosestEnemyUnit()
    {
        float distance = Mathf.Infinity;
        GameObject enemyTarget = null;
        foreach(GameObject enemy in enemies)
        {
            float enemyDistance = Vector3.Distance(this.gameObject.transform.position, enemy.transform.position);
            if(enemyDistance < distance)
            {
                distance = enemyDistance;
                enemyTarget = enemy;
            }
        }
        return enemyTarget;
    }
    

    //A* pathfinding algorithm
    void FindPath(Node startNode, Node targetNode)
    {
        GetCurrentTile();

        //Create an open and closed list
        //The open list contains all the nodes that we have already calculated the f cost
        //The closed list contains all the nodes that have already been evaluated
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        //Add the starting node to the open list
        openSet.Add(startNode);

        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {//Finds the node in the open list with the lowest f cost, or if f cost are equal
                  //then finds the node with the shortest distance to the end node (h cost)
                    currentNode = openSet[i];
                }
            }
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if(currentNode == targetNode) //path has been found
                {
                    RetracePath(startNode, targetNode);
                    return;
                }

                foreach(Node neighbour in HexTileMapGenerator.Instance.GetNeighbours(currentNode))
                {//Loops through every neighbour of the current node
                    if(!neighbour.walkable || closedSet.Contains(neighbour))
                    {//If this node has already been evaluated or is unwalkable, skip this neighbour
                        continue;
                    }

                    //Calculate the distance between start node and this neighbour
                    int newMovementCostToNeighbour = currentNode.gCost + hexMap.Distance(currentNode, neighbour);
                    
                    //If the g cost is larger than the new one we've calculated
                    //or we haven't calculated this node's costs
                    if(newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                    {
                    //Assign new h and g cost and the parent property
                        neighbour.gCost = newMovementCostToNeighbour;
                        neighbour.hCost = hexMap.Distance(neighbour, targetNode);
                        neighbour.parent = currentNode;

                    //Finaly if the node wasn't already gone through, add it to the open list.
                        if (!openSet.Contains(neighbour))
                             openSet.Add(neighbour);
                    }
                }
            }
        }
    

    //Start from the end node, and find the parent of that node, add it to the list and do the same until we are at the start node
    //Then reverse the list because we started on the end node. 
    //Now our path goes from the start node all the way to the end node.
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> p = new List<Node>();
        Node currentNode = endNode;

        while(currentNode != startNode)
        {
            p.Add(currentNode);
            currentNode = currentNode.parent;
        }

        p.Reverse();
        this.path = p;
    }

    private void OnDrawGizmos()
    {
        foreach (Node n in hexMap.nodes)
        {
            if (path != null)
            {
                if (path.Contains(n))
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawCube(n.worldPos, Vector3.one * 0.8f);
                }
                
            }
            
        }
    }
}
