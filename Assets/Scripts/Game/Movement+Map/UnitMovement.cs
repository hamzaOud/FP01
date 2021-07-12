using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public GameObject target;
    public GameObject[] enemies;

    public GameObject currentTile;
    public List<GameObject> adjacencyList = new List<GameObject>();

    public int attackRange = 1;

    // Update is called once per frame
    void Update()
    {
        GetCurrentTile();
        if(adjacencyList.Count == 0)
        {
            CalculateAdjacencyList();
        }
        if(target == null)
        {
            target = FindClosestEnemyUnit();
        }

        while (DistanceToTarget() > attackRange)
        {
            CalculatePath();
            Move();
        }


    }

    private void Start()
    {
        //GetCurrentTile();
        //CalculateAdjacencyList();
    }
    void CalculatePath()
    {

    }

    void CalculateAdjacencyList()
    {
        CheckTile(new Vector3(1.8f, 0, 0));
        CheckTile(new Vector3(-1.8f, 0, 0));
    }

    void CheckTile(Vector3 direction)
    {
        Vector3 halfExtents = new Vector3(0.25f, 0.25f, 0.25f);
        Collider[] colliders = Physics.OverlapBox(currentTile.transform.position + direction, halfExtents);

        print("calling CheckTile");
        foreach(Collider collider in colliders)
        {
            if (collider.gameObject.tag == "Tile")
            {
                adjacencyList.Add(collider.gameObject);
                print(collider.gameObject.name);
            }
        }
    }
    void Move()
    {

    }
    int DistanceToTarget()
    {
        return 0;
    }

    void GetCurrentTile()
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
    
}
