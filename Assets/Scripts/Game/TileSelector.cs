﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileSelector : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.Find("Scripts").GetComponent<GameController>();
    }

    private void OnMouseEnter()
    {
        if (GamePlayController.Instance.currentGameStage == GameStage.Preparation)
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
            gameController.selectedTile = this.gameObject;
        }
    }

    private void OnMouseExit()
    {
        if (GamePlayController.Instance.currentGameStage != GameStage.Preparation)
            return;
        GetComponent<MeshRenderer>().material.color = Color.white;
        gameController.selectedTile = null;
    }

    /*public void OnPointerEnter(PointerEventData eventData)
    {
        print("caca");
        GetComponent<MeshRenderer>().material.color = Color.red;
        gameController.selectedTile = this.gameObject;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<MeshRenderer>().material.color = Color.white;
        gameController.selectedTile = null;
    }*/
}
