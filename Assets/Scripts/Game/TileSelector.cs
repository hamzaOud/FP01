using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

//Script that lets you hover and select a tile
public class TileSelector : MonoBehaviour
{
    private GameController gameController;
    public Material nonselectedMaterial;

    private void Start()
    {
        gameController = GameObject.Find("Scripts").GetComponent<GameController>();
        nonselectedMaterial = (Material)Resources.Load("TileUnselectedMat");
    }

    private void OnMouseEnter()
    {//When mouse enters the tile during preparation round
        if (GamePlayController.Instance.currentGameStage == GameStage.Preparation)
        {//Give it a red color
            if (GameController.Instance.myBoard.myBench.Contains(this.gameObject) || gameController.myBoard.myTiles.Contains(this.gameObject))
            {
              GetComponent<MeshRenderer>().material.color = Color.red;
              //Assign this value to the selected tile property
              gameController.selectedTile = this.gameObject;
            }
            
        }
    }

    private void OnMouseExit()
    {//When mouse exits this tile
        if (GamePlayController.Instance.currentGameStage != GameStage.Preparation)
            return;
        //Return its color to white
        if (GameController.Instance.myBoard.myBench.Contains(this.gameObject))
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
        else { 
        GetComponent<MeshRenderer>().material = nonselectedMaterial;
        }
        //Assign null to the selected tile property
        gameController.selectedTile = null;
    }

}
