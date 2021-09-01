﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.AI;

public class PokemonController : MonoBehaviour
{
    public Pokemon pokemon;
    public UnitStats stats;

    public Tile tilePosition;

    public bool isDead;
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

    [SerializeField]
    private float attackRange = 1.5f;

    public float attackSpeed = 2.0f;
    public float attackTimer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
            if (GamePlayController.Instance.currentGameStage == GameStage.Preparation)
            {
                navMesh.enabled = false;
            }
            else navMesh.enabled = true;
            if (isOnBoard && GamePlayController.Instance.currentGameStage == GameStage.Combat)
            {
                if (ownerID == Data.Instance.trainer.trainerID)
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
                    PhotonView photonView = PhotonView.Get(this);
                    photonView.RPC("AttackTarget", RpcTarget.All);
                    }
                    isMoving = navMesh.isStopped;          
            }
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

    [PunRPC]
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
                target.GetComponent<PokemonController>().TakeDamage(10);
                AddMana(10);
                attackTimer = 0.0f;
            }
        }
    }

    void SpecialAttack()
    {
        stats.CurrentMana = 0;
        target.GetComponent<PokemonController>().TakeDamage(25);
        attackTimer = 0.0f;
    }



    public void ResetUnit()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("Reset", RpcTarget.All, unitID);
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

        unit.SetActive(true);
        unit.GetComponent<PokemonController>().enemies.Clear();
        unit.transform.position = tilePosition.gameObject.transform.position;
        unit.transform.rotation = Quaternion.Euler(0, 0, 0);
        unit.GetComponent<PokemonController>().stats.Reset();
        unit.GetComponent<PokemonController>().isDead = false;
        unit.GetComponent<PokemonController>().isAttacking = false;
        unit.GetComponent<PokemonController>().isParalyzed = false;
        unit.GetComponent<PokemonController>().isStunned = false;
        unit.GetComponent<PokemonController>().isMoving = false;
    }

    public void TakeDamage(int damage)
    {
        GetComponent<UnitStats>().CurrentHP -= damage;
        if(stats.CurrentHP <= 0)
        {
            Die();
        }
    }

    public void AddMana(int amount)
    {
        stats.CurrentMana += amount;
    }

    private void Die()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Units");

        foreach(GameObject u in units)
        {
            if(u.GetComponent<PokemonController>().target == this.gameObject)
            {
                u.GetComponent<PokemonController>().target = null;
            }
        }
        this.gameObject.SetActive(false);
    }
}
