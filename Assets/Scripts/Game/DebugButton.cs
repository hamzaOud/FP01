using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DebugButton : MonoBehaviour
{

    public Pokemon pikachu;

    public void OnClickDebugButton()
    {
        GameController.Instance.updatePokemonsOnBoard();
            
        //print("player 2s number of units on board :" + GameController.Instance.trainers[1].pokemonsOnBoard.Count);
        print(Data.Instance.trainer.pokemonsOnBoard[0].GetComponent<PokemonController>().tilePosition.transform.position);
    }
}
