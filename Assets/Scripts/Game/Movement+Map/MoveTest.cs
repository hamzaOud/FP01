using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Photon.Pun;

public class MoveTest : MonoBehaviour
{
    private NavMeshAgent navMesh;
    public GameObject target;
    public List<GameObject> enemies;

    public bool isMoving;
    public bool isAlive = true;

    public int unitID;
    public int ownerPlayerID;
    private PhotonView photonView;

    [SerializeField]
    private float attackRange = 1.5f;

    public float attackSpeed = 2.0f;
    public float attackTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        photonView = PhotonView.Get(this);
        unitID = photonView.ViewID;
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient && !AreAllEnemiesDead())
        {
            if (target == null)
            {
                target = FindClosestTarget();
            }

            if (Vector3.Distance(transform.position, target.transform.position) > attackRange)
            {
                navMesh.destination = target.transform.position;
                navMesh.isStopped = false;
            }
            else
            {
                navMesh.isStopped = true;
                AttackTarget();
            }

            isMoving = navMesh.isStopped;
        }
    }

    GameObject FindClosestTarget()
    {
            float distance = Mathf.Infinity;
            GameObject enemyTarget = null;
        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeSelf)
            {

                float enemyDistance = Vector3.Distance(this.gameObject.transform.position, enemy.transform.position);
                if (enemyDistance < distance)
                {
                    distance = enemyDistance;
                    enemyTarget = enemy;
                }
            }
        }
            return enemyTarget;
     }

    void AttackTarget()
    {
        attackTimer += Time.deltaTime;

        
        if(attackTimer >= attackSpeed)
        {
            if (GetComponent<UnitStats>().CurrentMana >= GetComponent<UnitStats>().maxMana)
            {
                SpecialAttack();
            }
            else
            {    
                photonView.RPC("DealDamage", RpcTarget.All, target.GetComponent<MoveTest>().unitID, 10);
                photonView.RPC("AddMana", RpcTarget.All, unitID, 10);

                attackTimer = 0.0f;
            }
        }
    }

    void SpecialAttack()
    {
        photonView.RPC("ResetMana", RpcTarget.All, unitID);
        photonView.RPC("DealDamage", RpcTarget.All, target.GetComponent<MoveTest>().unitID, 25);
        attackTimer = 0.0f;
    }

    [PunRPC]
    public void DealDamage(int unitID, int amount)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Units");
        GameObject unit = null;
        for(int i = 0; i < units.Length; i++)
        {
            if(unitID == units[i].GetComponent<MoveTest>().unitID)
            {
                unit = units[i];
                break;
            }
        }
        unit.GetComponent<MoveTest>().TakeDamage(amount);
    }

    [PunRPC]
    public void AddMana(int uniID, int amount)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Units");
        GameObject unit = null;
        for (int i = 0; i < units.Length; i++)
        {
            if (unitID == units[i].GetComponent<MoveTest>().unitID)
            {
                unit = units[i];
                break;
            }
        }

        unit.GetComponent<UnitStats>().CurrentMana += amount;
    }

    [PunRPC]
    public void ResetMana(int unitID)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Units");
        GameObject unit = null;
        for (int i = 0; i < units.Length; i++)
        {
            if (unitID == units[i].GetComponent<MoveTest>().unitID)
            {
                unit = units[i];
                break;
            }
        }
        unit.GetComponent<UnitStats>().CurrentMana = 0;

    }

    public void TakeDamage(int damage)
    {
        GetComponent<UnitStats>().CurrentHP -= damage;
        if (GetComponent<UnitStats>().CurrentHP <= 0)
        {
            Die();
        }
    }


    void Die()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Units");
        isAlive = false;

        foreach(GameObject u in units)
         {
                    if(u.GetComponent<MoveTest>().target == this.gameObject)
                    {
                        if (u.GetComponent<MoveTest>().AreAllEnemiesDead())
                        {
                            print("you won");
                        }
                        break;
                    }
         }
        foreach (GameObject u in units)
        {
            if (u.GetComponent<MoveTest>().target == this.gameObject)
            {
                u.GetComponent<MoveTest>().target = null;
            }
        }
        this.gameObject.SetActive(false);
    }

    public bool AreAllEnemiesDead()
    {
        bool value = true;
        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].GetComponent<MoveTest>().isAlive)
            {
                value = false;
            }
        }
        return value;
    }
}
