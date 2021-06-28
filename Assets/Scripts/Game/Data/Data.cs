using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data Instance;

    public GameObject[] spawnObjects;
    public Pokemon[] pokemons; //For shop
    public Trainer trainer;//me


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        trainer = GameController.Instance.trainers[GameController.Instance.playerID];
    }
}
