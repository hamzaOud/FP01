using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer
{
    public int balance;
    public int level;
    public int currentExp;
    public Dictionary<Pokemon, List<GameObject>> pokedex; //Dictionary that holds all the pokemons we own with their gameobjects
    public List<GameObject> pokemonsOnBoard; //List if all pokemons on the board
    public int trainerID;
    public GameObject[] spawnPoints;
    public int currentHP;

    public Trainer(int playerID)
    {
        balance = 50; //Change this to 2, so start with 2 gold
        level = 1;
        currentExp = 0;
        pokedex = new Dictionary<Pokemon, List<GameObject>>();
        pokemonsOnBoard = new List<GameObject>();
        spawnPoints = new GameObject[GameController.Instance.myBoard.myBench.Length + GameController.Instance.myBoard.myTiles.Length];
        trainerID = playerID;
        currentHP = 100;
    }

    public void LevelUP()
    {
        level++;
    }

    public void AddXP(int amountToAdd)
    {
        currentExp += amountToAdd; //add exp 

        //if we've gone over the next level's required exp
        if(currentExp >= ShopController.Instance.levelsXP[level])
        {
            GameController.Instance.LvlUP(this.trainerID);
            //remove previous levels required exp to current exp
            currentExp -= ShopController.Instance.levelsXP[level - 1];
        }
    }
}
