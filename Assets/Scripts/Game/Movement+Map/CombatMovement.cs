using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMovement : MonoBehaviour
{
    public GameObject target;
    public bool isMoving;
    public bool isAttacking;

    public List<GameObject> enemies;

    public float moveSpeed = 4.0f;
    public Vector3 heading;
    
    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            target = FindClosestTarget();
        }

        if(Vector3.Distance(transform.position, target.transform.position) > 1f)
        {
            Move();
        }
    }

    void Move()
    {
        isMoving = true;
        Vector3 velocity = new Vector3();

        heading = (target.transform.position) - transform.position;
        heading.Normalize();

        velocity = heading * moveSpeed;
        transform.forward = heading;
        if (velocity != Vector3.zero)
        {
            transform.position += velocity * Time.deltaTime;
        }
    }


    GameObject FindClosestTarget()
    {
        float distance = Mathf.Infinity;
        GameObject enemyTarget = null;
        foreach (GameObject enemy in enemies)
        {
            float enemyDistance = Vector3.Distance(this.gameObject.transform.position, enemy.transform.position);
            if (enemyDistance < distance)
            {
                distance = enemyDistance;
                enemyTarget = enemy;
            }
        }
        return enemyTarget;
    }
}
