using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public PlayerUIElement[] playerUIElements;

    public GameObject statsCanvas;
    public Text pokemonNameText;
    public Text pokemonTypeText;
    public Text pokemonClassText;

    public GameObject bonusPrefab; //Prefab used to show class and types on board

    public Text timerText;

    private void Start()
    {
        Instance = this;


        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        { //Show players names and levels on players canvas
            playerUIElements[i].playerName.text = PhotonNetwork.PlayerList[i].NickName
            + " Level " + GameController.Instance.trainers[i].level.ToString();
        }
    }

    //Function that returns 2 strings, one for the specified pokemon's types
    //the other for the specified pokemon's classes
    //This function is used to generate the shop buttons
    public string[] TypeAndClassNamesStrings(Pokemon pokemon)
    {
        string typeString;
        string classString;

        if(pokemon.type.Length == 1)
        {
            typeString = pokemon.type[0].ToString();
        }
        else
        {//If the pokemon has more than one type, produce the string
        //types are divided with the '/' symbol
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
        {//If the pokemon has more than one class, produce the string
         //classes are divided with the '/' symbol
            classString = pokemon.pokeClass[0].ToString();
            for (int i = 1; i < pokemon.pokeClass.Length; i++)
            {
                classString += "/" + pokemon.pokeClass[i].ToString();
            }
        }

        string[] returnValue = new string[] { typeString, classString };
        return returnValue;
    }

    public void UpdateTimer()
    {
        timerText.text = GamePlayController.Instance.timerDisplay.ToString();
    }

    public void UpdateBonusesUI()
    {
        //Find the panel to place UI elements in
        GameObject panel = GameObject.Find("BonusPanel");

        //Remove all existing type/class UI elements
        for (int i = 0; i < panel.transform.childCount; i++)
        {
            BonusUIElement element = panel.transform.GetChild(i).GetComponent<BonusUIElement>();
            element.DeleteElement();
        }
        //Create a new UI element for each unique type
        foreach (KeyValuePair<PokeType, int> keyValuePair in GamePlayController.Instance.pokeTypeCounter)
        {
            GameObject uielement = Instantiate(bonusPrefab, panel.transform);
            uielement.GetComponent<BonusUIElement>().bonusName.text = keyValuePair.Key.ToString();
            uielement.GetComponent<BonusUIElement>().bonusCount.text = keyValuePair.Value.ToString();

        }

    }

    public void UpdateUI()
    {
        GamePlayController.Instance.CountBonuses();
        UpdateBonusesUI();

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerUIElements[i].playerName.text = PhotonNetwork.PlayerList[i].NickName
            + " Level " + GameController.Instance.trainers[i].level.ToString();
        }
    }
}
