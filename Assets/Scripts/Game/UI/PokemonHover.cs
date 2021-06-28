using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PokemonHover : MonoBehaviourPunCallbacks
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
    
}
