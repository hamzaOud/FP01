using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public ShopItemImageButton[] shopImageButtons;
    public Data data;
    public GameController gameController;

    //UI Elements
    public Slider xpSlider;
    public Text xpText;
    public Text balanceText;
    public Text levelText;
    public Button buyXPButton;
    private Dictionary<int, int> levelsXP;
    private int currentXp;
    public Trainer trainer;

    // Start is called before the first frame update
    void Start()
    {
        // initialization
        levelsXP = new Dictionary<int, int>();
        PopulateLevels(levelsXP);

        trainer = data.trainer;

        updateUI();
        RefreshShop();

    }

    void PopulateLevels(Dictionary<int, int> levelsInfo)
    {
        levelsXP.Add(0, 0);
        levelsXP.Add(1, 4);
        levelsXP.Add(2, 10);
        levelsXP.Add(3, 20);
        levelsXP.Add(4, 32);
        levelsXP.Add(5, 0);
        //levelsXP.Add(5, 54);
    }

    public void updateUI()
    {
        xpText.text = trainer.currentExp.ToString() + "/" + levelsXP[trainer.level].ToString();
        double value = (double)trainer.currentExp / (double)levelsXP[trainer.level];
        xpSlider.value = (float)value;
        balanceText.text = "C :" + data.trainer.balance;
        levelText.text = data.trainer.level.ToString();
        if(trainer.balance < 2)
        {
            buyXPButton.interactable = false;
        }
        else
        {
            buyXPButton.interactable = true;
        }
    }

    public void buyXP(int xpToAdd)
    {
        trainer.currentExp += xpToAdd;
        if (trainer.currentExp >= levelsXP[trainer.level])
            {
                trainer.LevelUP();
                trainer.currentExp -= levelsXP[trainer.level - 1];
            }
        trainer.balance -=2;
        updateUI();
        checkIfMaxLevel();
    }

    public void RefreshShop()
    {
        for (int i = 0; i < shopImageButtons.Length; i++)
        {
            shopImageButtons[i].gameObject.SetActive(true);
            shopImageButtons[i].GetComponent<ShopItemImageButton>().color = Color.white;

            int random = Random.Range(0, data.pokemons.Length);
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
        if (trainer.balance >= 2)
        {
            RefreshShop();
            trainer.balance -= 2;
            updateUI();
        }
    }

    public void checkIfMaxLevel()
    {
        if (trainer.level == levelsXP.Count)
        {
            print("max level reached");
            buyXPButton.interactable = false;
        }

    }
}