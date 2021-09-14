using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ShopController : MonoBehaviour
{
    public static ShopController Instance;

    public ShopItemImageButton[] shopImageButtons; //Shop buttons, which are pokemon icons
    public Data data;
    public GameController gameController;

    //UI Elements
    public Slider xpSlider; //Slider representation of current XP / XP required for next level
    public Text xpText; // CurrentXP / Levelsxp[trainer.level]
    public Text balanceText; //My amount of gold text element
    public Text levelText; //My current level text element
    public Button buyXPButton;
    public Button refreshBtn;
    public Dictionary<int, int> levelsXP; //Dictionary that holds levels and xp required
    public Trainer trainer; //me

    public int refreshShopCost = 2; //Cost to refresh shop
    public int buyXPCost = 4; //Cost to buy XP

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        trainer = data.trainer;

        updateUI();
        RefreshShop();

    }

    private void Awake()
    {
        // initialization
        levelsXP = new Dictionary<int, int>();
        PopulateLevels(levelsXP);
    }

    void PopulateLevels(Dictionary<int, int> levelsInfo)
    {
        levelsXP.Add(1, 4);
        levelsXP.Add(2, 10);
        levelsXP.Add(3, 20);
        levelsXP.Add(4, 32);
        levelsXP.Add(5, 0);
        //levelsXP.Add(5, 54);
    }

    public void updateUI()
    {
        xpText.text = "XP: " + trainer.currentExp.ToString() + "/" + levelsXP[trainer.level].ToString();
        double value = (double)trainer.currentExp / (double)levelsXP[trainer.level];
        xpSlider.value = (float)value;
        balanceText.text = "Gold :" + data.trainer.balance;
        levelText.text = "Level: " + data.trainer.level.ToString();
        if(trainer.balance < buyXPCost)
        { 
            buyXPButton.interactable = false;
        }
        else
        {
            buyXPButton.interactable = true;
        }
        if(trainer.balance < refreshShopCost)
        {
            refreshBtn.interactable = false;
        }
        else
        {
            refreshBtn.interactable = true;
        }
    }

    public void buyXP(int xpToAdd)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("buyXPRPC", RpcTarget.All, xpToAdd, GameController.Instance.playerID);
    }

    [PunRPC]
    public void buyXPRPC(int xptoAdd, int trainerID)
    {
        GameController.Instance.trainers[trainerID].AddXP(xptoAdd); //Add xp to trainer
        GameController.Instance.trainers[trainerID].balance -= buyXPCost; // Remove cost from trainers balance
        updateUI();
        if (GameController.Instance.CheckIfMine(trainerID))
        {
            checkIfMaxLevel(); //If max lvl, disable 'buy xp' button
        }

        UIController.Instance.UpdateUI(); //Update Players panel on the right hand side
    }

    public void RefreshShop()
    {
        for (int i = 0; i < shopImageButtons.Length; i++)
        {
            shopImageButtons[i].gameObject.SetActive(true); //Re-enable all shop buttons
            shopImageButtons[i].GetComponent<ShopItemImageButton>().color = Color.white; //Reset their color to white

            int random = Random.Range(0, data.pokemons.Length); //Generate a random number between 0 and the number of pokemons in data
            //Configurate shop button
            shopImageButtons[i].sprite = data.pokemons[random].image; 
            shopImageButtons[i].pokemon = data.pokemons[random];
            shopImageButtons[i].pokemonNameText.text = shopImageButtons[i].pokemon.name;
            shopImageButtons[i].costText.text = shopImageButtons[i].pokemon.price.ToString();

            string[] stringValues = UIController.Instance.TypeAndClassNamesStrings(data.pokemons[random]);
            shopImageButtons[i].typeText.text = stringValues[0];
            shopImageButtons[i].classText.text = stringValues[1];

        }
    }

    public void refreshButton()
    {
        trainer.balance -= refreshShopCost; //Remove cost of refreshing shop to trainer's balance
        updateUI(); //Update Shop UI (in this case, new balance)
        RefreshShop();
    }

    public void checkIfMaxLevel()
    {
        if (trainer.level == levelsXP.Count)
        {
            buyXPButton.interactable = false;
        }

    }
}