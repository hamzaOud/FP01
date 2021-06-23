using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trainer
{
    public int balance;
    public int level;
    public int currentExp;
    public Dictionary<Pokemon, List<GameObject>> pokedex;
    public List<GameObject> pokemonsOnBoard;

    public Trainer()
    {
        balance = 50;
        level = 1;
        currentExp = 0;
        pokedex = new Dictionary<Pokemon, List<GameObject>>();
        pokemonsOnBoard = new List<GameObject>();
    }

    public void LevelUP()
    {
        level++;
    }
}
