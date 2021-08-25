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
        print(GameController.Instance.boardControllers[0].myTiles.Length);
        print("Pair 0: " + GamePlayController.Instance.PairEnemies()[0]);
    }
}
