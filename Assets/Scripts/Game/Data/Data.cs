using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public static Data Instance;

    public GameObject[] spawnObjects;
    public Pokemon[] pokemons;
    public Trainer trainer;
    //public GameObject[] trainers;


    private void Awake()
    {
        Instance = this;
        trainer = new Trainer();
    }

}
