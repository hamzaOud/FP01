using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButton : MonoBehaviour
{

    public Pokemon pikachu;

    public void OnClickDebugButton()
    {

        //GamePlayController.Instance.CountBonuses();
        //UIController.Instance.UpdateBonusesUI();

        print(GamePlayController.Instance.enemyPokemonsAlive.Count);
    }
}
