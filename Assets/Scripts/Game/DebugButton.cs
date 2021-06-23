using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButton : MonoBehaviour
{

    public Pokemon pikachu;

    public void OnClickDebugButton()
    {
        Data data = GameObject.Find("Scripts").GetComponent<Data>();
        List<GameObject> value = new List<GameObject>();
        data.trainer.pokedex.TryGetValue(pikachu, out value);
        print("number of pikachus: " + value.Count);

        print("no. of pokemons on board: " + data.trainer.pokemonsOnBoard.Count);
    }
}
