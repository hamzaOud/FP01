using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PokemonController : MonoBehaviour, IPunInstantiateMagicCallback
{
    public Pokemon pokemon;
    public UnitStats stats;

    public Tile tilePosition;

    public bool isDead;
    public bool isAttacking;

    public bool isParalyzed;
    public bool isStunned;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //Retrieve the info passed in when object was instantiated (here only the tileID)
        object[] instantiationData = info.photonView.InstantiationData;

        //Go through the spawn game objects
        for (int i = 0; i < Data.Instance.spawnObjects.Length; i++)
        {//If the tileID passed through matches this spawn objects
            if (Data.Instance.spawnObjects[i].GetComponent<Tile>().tileID == (int)instantiationData[0])
            {//Assign game object property of tile and tile property of pokemon controller
                this.gameObject.GetComponent<MovePokemon>().tile = Data.Instance.spawnObjects[i];
                Data.Instance.spawnObjects[i].gameObject.GetComponent<Tile>().pokemonObject = this.gameObject;

                return;
            }
        }
    }


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
