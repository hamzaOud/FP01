using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public Text pokemonNameText;
    public Text pokemonTypeText;
    public Text pokemonClassText;

    public PlayerUIElement[] playerUIElements;

    public GameObject statsCanvas;

    private void Start()
    {
        Instance = this;


        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerUIElements[i].playerName.text = PhotonNetwork.PlayerList[i].NickName;
        }
    }

    public string[] TypeAndClassNamesStrings(Pokemon pokemon)
    {
        string typeString;
        string classString;

        if(pokemon.type.Length == 1)
        {
            typeString = pokemon.type[0].ToString();
        }
        else
        {
            typeString = pokemon.type[0].ToString();
            for(int i = 1;i < pokemon.type.Length; i++)
            {
                typeString += "/" + pokemon.type[i].ToString();
            }
        }

        if (pokemon.pokeClass.Length == 1)
        {
            classString = pokemon.pokeClass[0].ToString();
        }
        else
        {
            classString = pokemon.pokeClass[0].ToString();
            for (int i = 1; i < pokemon.pokeClass.Length; i++)
            {
                classString += "/" + pokemon.pokeClass[i].ToString();
            }
        }

        string[] returnValue = new string[] { typeString, classString };
        return returnValue;
    }
}
