using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PokemonHover : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    private void OnMouseEnter()
    {
        UIController.Instance.statsCanvas.SetActive(true);
        UIController.Instance.pokemonNameText.text = this.gameObject.GetComponent<MovePokemon>().pokemon.name;
        string[] stringValues = UIController.Instance.TypeAndClassNamesStrings(this.gameObject.GetComponent<MovePokemon>().pokemon);
        UIController.Instance.pokemonTypeText.text = stringValues[0];
        UIController.Instance.pokemonClassText.text = stringValues[1];
    }

    private void OnMouseExit()
    {
        UIController.Instance.statsCanvas.SetActive(false);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info) //move this to a more relevant class
    {
        object[] instantiationData = info.photonView.InstantiationData;
        //print("tileID :" + instantiationData[0]);

        for(int i = 0; i < Data.Instance.spawnObjects.Length; i++)
        {
            if(Data.Instance.spawnObjects[i].GetComponent<Tile>().tileID == (int)instantiationData[0])
            {
                this.gameObject.GetComponent<MovePokemon>().tile = Data.Instance.spawnObjects[i];
                Data.Instance.spawnObjects[i].gameObject.GetComponent<Tile>().pokemonObject = this.gameObject;

                return;
            }
        }

    }
    
}
