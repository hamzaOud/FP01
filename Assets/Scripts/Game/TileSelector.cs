using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//Script that lets you hover and select a tile
public class TileSelector : MonoBehaviour
{
    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("Scripts").GetComponent<GameController>();
    }

    private void OnMouseEnter()
    {//When mouse enters the tile during preparation round
        if (GamePlayController.Instance.currentGameStage == GameStage.Preparation)
        {//Give it a red color
            GetComponent<MeshRenderer>().material.color = Color.red;
            //Assign this value to the selected tile property
            gameController.selectedTile = this.gameObject;
        }
    }

    private void OnMouseExit()
    {//When mouse exits this tile
        if (GamePlayController.Instance.currentGameStage != GameStage.Preparation)
            return;
        //Return its color to white
        GetComponent<MeshRenderer>().material.color = Color.white;
        //Assign null to the selected tile property
        gameController.selectedTile = null;
    }

}
