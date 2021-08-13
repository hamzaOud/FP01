using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MoveTest : MonoBehaviour
{
    private NavMeshAgent navMesh;
    public GameObject target;
    public List<GameObject> enemies;

    public bool isMoving;

    public Slider healthbar;
    public GameObject sliderPosition;

    [SerializeField]
    private float attackRange = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        PositionHealthbar();

        if(target == null)
        {
           target =  FindClosestTarget();
        }

        if(Vector3.Distance(transform.position, target.transform.position) > attackRange)
        {
            navMesh.destination = target.transform.position;
            navMesh.isStopped = false;
        } else
        {
            navMesh.isStopped = true;
            AttackTarget();
        }

        isMoving = navMesh.isStopped;
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

    void AttackTarget()
    {

    }
    void PositionHealthbar()
    {
        healthbar.transform.position = Camera.main.WorldToScreenPoint(sliderPosition.transform.position);
    }
}
