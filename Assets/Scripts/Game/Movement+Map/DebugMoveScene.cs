using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMoveScene : MonoBehaviour
{

    public UnitMovement unit1;
    public GameObject unit2;

    public void OnClickDebugBtn()
    {
        unit1.GetCurrentTile();
        //print("distance :" + HexTileMapGenerator.Instance.Distance(HexTileMapGenerator.Instance.findNodeFromTile(unit1.currentTile),
          //  HexTileMapGenerator.Instance.findNodeFromTile(unit2.GetComponent<CurrentTileTest>().currentTile)));
        print(HexTileMapGenerator.Instance.GetCubeCoord(HexTileMapGenerator.Instance.findNodeFromTile(unit1.currentTile)));
    }
}
