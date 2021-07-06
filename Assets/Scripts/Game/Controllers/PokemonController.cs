using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PokemonController : MonoBehaviour
{
    public Pokemon pokemon;
    public UnitStats stats;

    public Tile tilePosition;

    public bool isDead;
    public bool isAttacking;

    public bool isParalyzed;
    public bool isStunned;

    public int unitID;

    public void Reset()
    {
        this.gameObject.transform.position = tilePosition.gameObject.transform.position;
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        stats.Reset();
        isDead = false;
        isAttacking = false;
        isParalyzed = false;
        isStunned = false;
    }
}
