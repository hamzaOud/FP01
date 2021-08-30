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
        GamePlayController.Instance.RepositionUnit(GameController.Instance.trainers[1].pokemonsOnBoard[0], 0);

        GameObject unit = GameController.Instance.trainers[0].pokemonsOnBoard[0];

        Node startNode = HexTileMapGenerator.Instance.findNodeFromTile(unit.GetComponent<MovePokemon>().tile);

        print(HexTileMapGenerator.Instance.mapWidth - startNode.gridX);
        print(HexTileMapGenerator.Instance.mapHeight - startNode.gridY);
        Node finalNode = HexTileMapGenerator.Instance.nodes[1, HexTileMapGenerator.Instance.mapWidth - startNode.gridX - 1,HexTileMapGenerator.Instance.mapHeight - startNode.gridY - 1];

        GameObject tileObject = HexTileMapGenerator.Instance.FindTileFromNode(finalNode);
        print(tileObject.name);
    }
}
