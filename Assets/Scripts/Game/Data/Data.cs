using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data Instance;
    public GameObject[] spawnObjects;
    public Pokemon[] pokemons; //For shop
    public Pokemon[] pokemonsDatabase;
    public Trainer trainer;//me


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        trainer = GameController.Instance.trainers[GameController.Instance.playerID];

        /*spawnObjects = new GameObject[GameController.Instance.myBoard.myBench.Length + GameController.Instance.myBoard.myTiles.Length];

        for (int i = 0;i < GameController.Instance.myBoard.myBench.Length; i++)
        {
            spawnObjects[i] = GameController.Instance.myBoard.myBench[i];
        }
        for(int j =0; j < GameController.Instance.myBoard.myTiles.Length; j++)
        {
            spawnObjects[findNextIndex()] = GameController.Instance.myBoard.myTiles[j];
        }*/
    }

    public int findNextIndex()
    {
        int value = 0;

        for (int i = 0; i < spawnObjects.Length; i++)
        {
            if (spawnObjects[i] == null)
            {
                value = i;
                break;
            }
        }
        return value;
    }
}
