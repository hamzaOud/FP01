using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    public PlayerUIElement[] playerUIElements;
    public PlayersUnitsElement[] playersUnitsElements;
    public Transform bonusPanelTransform;

    public GameObject statsCanvas;
    public Text pokemonNameText;
    public Text pokemonTypeText;
    public Text pokemonClassText;
    public GameObject altPanel;
    public EndScreen endCanvas;

    public GameObject bonusPrefab; //Prefab used to show class and types on board

    public Text timerText;

    private void Start()
    {
        Instance = this;

        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        { //Show players names and levels on players canvas
            if (i == GameController.Instance.playerID)
            {
                playerUIElements[i].playerName.text = "(YOU)" + PhotonNetwork.PlayerList[i].NickName
            + " Level " + GameController.Instance.trainers[i].level.ToString();
                playerUIElements[i].fill.color = Color.yellow;
            }
            else
                playerUIElements[i].playerName.text = PhotonNetwork.PlayerList[i].NickName
                + " Level " + GameController.Instance.trainers[i].level.ToString();

            playerUIElements[i].playerHP.value = (float)GameController.Instance.trainers[i].currentHP / (float)100;
        }
        if (PhotonNetwork.PlayerList.Length < 8)
        {
            for (int i = PhotonNetwork.PlayerList.Length; i < 8; i++)
            {
                playerUIElements[i].gameObject.SetActive(false);
             }
        }
        altPanel.SetActive(false);
        endCanvas.gameObject.SetActive(false);
        StartCoroutine(Tester());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            for(int i = 0;i < PhotonNetwork.PlayerList.Length; i++)
            {
                for(int j = 0; j < playersUnitsElements[i].pokemonImages.Length; j++)
                {
                    playersUnitsElements[i].pokemonImages[j].sprite = null;
                }
            }

            GameController.Instance.updatePokemonsOnBoard();
            altPanel.SetActive(true);
            for(int i =0; i < PhotonNetwork.PlayerList.Length; i++)
             {
                playersUnitsElements[i].playerName.text = PhotonNetwork.PlayerList[i].NickName;
                for(int j = 0; j < GameController.Instance.trainers[i].pokemonsOnBoard.Count; j++)
                {
                    playersUnitsElements[i].pokemonImages[j].sprite = GameController.Instance.trainers[i].
                    pokemonsOnBoard[j].GetComponent<MovePokemon>().pokemon.image;
                }

            }
        } else if (Input.GetKeyUp(KeyCode.Tab))
        {
            altPanel.SetActive(false);
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

    IEnumerator Tester()
    {
        yield return new WaitForSeconds(0.3f);
        UpdateUI();
    }
    public void UpdateTimer()
    {
        timerText.text = GamePlayController.Instance.timerDisplay.ToString();
    }

    public void UpdateBonusesUI() //Updates the class/type UI elements on the left-hand side
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
            GameObject uielement = Instantiate(bonusPrefab, bonusPanelTransform);
            uielement.GetComponent<BonusUIElement>().bonusName.text = keyValuePair.Key.ToString();
            uielement.GetComponent<BonusUIElement>().bonusCount.text = keyValuePair.Value.ToString();
            
        }
        foreach(KeyValuePair<PokeClass,int> keyValue in GamePlayController.Instance.pokeClassCounter)
        {
            GameObject classBonus = Instantiate(bonusPrefab, bonusPanelTransform);
            classBonus.GetComponent<BonusUIElement>().bonusName.text = keyValue.Key.ToString();
            classBonus.GetComponent<BonusUIElement>().bonusCount.text = keyValue.Value.ToString();
        }
    }

    public void UpdateUI()
    {
        GamePlayController.Instance.CountBonuses();
        UpdateBonusesUI();

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(i == GameController.Instance.playerID)
            {
                playerUIElements[i].playerName.text = "(YOU)" + PhotonNetwork.PlayerList[i].NickName
            + " Level " + GameController.Instance.trainers[i].level.ToString();
                playerUIElements[i].fill.color = Color.yellow;
            }
            else 
            playerUIElements[i].playerName.text = PhotonNetwork.PlayerList[i].NickName
            + " Level " + GameController.Instance.trainers[i].level.ToString();

            playerUIElements[i].playerHP.value = (float)GameController.Instance.trainers[i].currentHP / (float)100;
        }
    }

    IEnumerator ReturnToLobby()
    {
        yield return new WaitForSeconds(5f);
        //Spawn(playerID, pokemon.pokemonID);
        SceneManager.LoadScene(0);
    }
    public void LoadLogin()
    {
        StartCoroutine(ReturnToLobby());
    }
}
