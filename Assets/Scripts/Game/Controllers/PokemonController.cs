using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PokemonController : MonoBehaviour
{
    public Pokemon pokemon;
    public UnitStats stats;

    public Tile tilePosition;

    public bool isAlive = true;
    public bool isAttacking;
    public bool isMoving;
    public bool isParalyzed;
    public bool isStunned;
    public bool isOnBoard = false;

    public int unitID;
    public int ownerID;

    private NavMeshAgent navMesh;
    public GameObject target;
    public List<GameObject> enemies;
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
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (GamePlayController.Instance.currentGameStage == GameStage.Preparation)
            {
                navMesh.enabled = false;
            }
            else navMesh.enabled = true;
            if (isOnBoard && GamePlayController.Instance.currentGameStage == GameStage.Combat)
            {
                //if (ownerID == Data.Instance.trainer.trainerID)
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
        }
        else if(!PhotonNetwork.IsMasterClient && GamePlayController.Instance.currentGameStage == GameStage.Preparation)
            {
                navMesh.enabled = false;
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


        if (attackTimer >= attackSpeed)
        {
            if (GetComponent<UnitStats>().CurrentMana >= GetComponent<UnitStats>().maxMana)
            {
                SpecialAttack();
            }
            else
            {
                photonView.RPC("DealDamage", RpcTarget.All, target.GetComponent<PokemonController>().unitID, 10);
                photonView.RPC("AddMana", RpcTarget.All, unitID, 10);
                attackTimer = 0.0f;
            }
        }
    }

    void SpecialAttack()
    {
        photonView.RPC("ResetMana", RpcTarget.All, unitID);
        photonView.RPC("DealDamage", RpcTarget.All, target.GetComponent<PokemonController>().unitID, 25);
        attackTimer = 0.0f;
    }

    [PunRPC]
    public void DealDamage(int unitID, int amount)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Units");
        GameObject unit = null;
        for (int i = 0; i < units.Length; i++)
        {
            if (unitID == units[i].GetComponent<PokemonController>().unitID)
            {
                unit = units[i];
                break;
            }
        }
        unit.GetComponent<PokemonController>().TakeDamage(amount);
    }

    public void TakeDamage(int amount)
    {
        GetComponent<UnitStats>().CurrentHP -= amount;

        if(GetComponent<UnitStats>().CurrentHP <= 0)
        {
            Die();
        }
    }

    [PunRPC]
    public void AddMana(int uniID, int amount)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Units");
        GameObject unit = null;
        for (int i = 0; i < units.Length; i++)
        {
            if (unitID == units[i].GetComponent<PokemonController>().unitID)
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
            if (unitID == units[i].GetComponent<PokemonController>().unitID)
            {
                unit = units[i];
                break;
            }
        }
        unit.GetComponent<UnitStats>().CurrentMana = 0;

    }

    public void ResetUnit()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("Reset", RpcTarget.All, unitID);
    }

    public void ResetTest()
    {
        GetComponent<PokemonController>().target = null;
        this.gameObject.SetActive(true);
        //this.gameObject.active = true;
        GetComponent<PokemonController>().enemies.Clear();
        transform.position = tilePosition.gameObject.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        GetComponent<PokemonController>().stats.Reset();
        GetComponent<PokemonController>().isAlive = true;
        GetComponent<PokemonController>().isAttacking = false;
        GetComponent<PokemonController>().isParalyzed = false;
        GetComponent<PokemonController>().isStunned = false;
        GetComponent<PokemonController>().isMoving = false;
    }
    [PunRPC]
    public void Reset(int unidID)
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Units");
        GameObject unit = null;
        for(int i = 0; i < units.Length; i++)
        {
            if(units[i].GetComponent<PokemonController>().unitID == unitID)
            {
                unit = units[i];
                break;
            }
        }
        unit.GetComponent<PokemonController>().target = null;
        unit.SetActive(true);
        unit.GetComponent<PokemonController>().enemies.Clear();
        unit.transform.position = tilePosition.gameObject.transform.position;
        unit.transform.rotation = Quaternion.Euler(0, 0, 0);
        unit.GetComponent<PokemonController>().stats.Reset();
        unit.GetComponent<PokemonController>().isAlive = true;
        unit.GetComponent<PokemonController>().isAttacking = false;
        unit.GetComponent<PokemonController>().isParalyzed = false;
        unit.GetComponent<PokemonController>().isStunned = false;
        unit.GetComponent<PokemonController>().isMoving = false;
    }

    private void Die()
    {
        isAlive = false;


        GameObject[] units = GameObject.FindGameObjectsWithTag("Units");
        foreach(GameObject u in units)
        {
            if(u.GetComponent<PokemonController>().target == this.gameObject)
            {
                u.GetComponent<PokemonController>().target = null;
            }
        }
        
        this.gameObject.SetActive(false);

        if (ownerID != 888)
        {
            if (GamePlayController.Instance.AreAllMyUnitsDead())
            {
                print("You should be losing HP");
                if (PhotonNetwork.IsMasterClient)
                    GameController.Instance.ReduceTrainerHP(ownerID, 10);
            }
        }
    }
}
