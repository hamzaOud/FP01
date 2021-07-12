using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

//Script for moving the pokemons on different tiles
public class MovePokemon : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    public GameObject tile;

    public Pokemon pokemon;
    private GameController gameController;

    public int layeringtest;
    public Data data;
    public PhotonView photonView;

    private void Start()
    {
        gameController = GameController.Instance;
        data = Data.Instance;

        photonView.ViewID = GameController.Instance.nextViewID;
        GameController.Instance.nextViewID++;
    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    
     void OnMouseDown()
     {
         gameController.selectedPokemon = this.gameObject;
         mZCoord = Camera.main.WorldToScreenPoint(transform.position).z;
         // Store offset = gameobject world pos - mouse world pos

        mOffset = transform.position - GetMouseAsWorldPoint();
        disablePokemonColliders();
     }

     void OnMouseDrag()
     {
         Vector3 yOffset = GetMouseAsWorldPoint() + mOffset;
         yOffset.y = 2.0f;
         transform.position = yOffset;
     }

     private void OnMouseUp() //!!!Clean up this code 
     {
        //If a tile is selected
        if (gameController.selectedTile != null)
        {

            //If selected a tile on the board
            if (gameController.myBoard.myTiles.Contains(gameController.selectedTile))
            {
                //3 options to move a pokemon on the board tiles:
                //1. If trainer's level is > to the number of pokemons on board
                //2. if moving a pokemon from one board tile to another
                //3. If moving a pokemon from the bench to the board (the selected tile's pokemonObject is not null)
                if (data.trainer.level > data.trainer.pokemonsOnBoard.Count || (gameController.myBoard.myTiles.Contains(tile) && gameController.myBoard.myTiles.Contains(gameController.selectedTile)) || (data.trainer.level == data.trainer.pokemonsOnBoard.Count && gameController.selectedTile.GetComponent<Tile>().pokemonObject != null))
                {
                    pokemonMoveLogic();
                }
                else //if trainer is too low level to add pokemon to the board, return pokemon to original position
                {
                    transform.position = tile.transform.position;
                }

            }
            else //If selected a tile on bench
            {
                pokemonMoveLogic();
            }
        }
        else //If no tile is selected, return pokemon to its original tile
        {
            transform.position = tile.transform.position;
        }

        enablePokemonColliders();
        GameController.Instance.updatePokemonsOnBoard(); //Fix this function before un-commenting
        UIController.Instance.UpdateUI();

    }

    void disablePokemonColliders() //So the pokemon's collider does not interfere with the tile's collider
    {
        foreach (KeyValuePair<Pokemon, List<GameObject>> item in data.trainer.pokedex)
        {
            foreach(GameObject go in item.Value)
            {
                go.GetComponent<BoxCollider>().enabled = false;
            }
        }

    }

    void enablePokemonColliders()
    {
        foreach (KeyValuePair<Pokemon, List<GameObject>> item in data.trainer.pokedex)
        {
            foreach (GameObject go in item.Value)
            {
                go.GetComponent<BoxCollider>().enabled = true;
            }
        }

    }

    private void pokemonMoveLogic()
    {
        //If the tile selected has no pokemon on it
        if (gameController.selectedTile.GetComponent<Tile>().pokemonObject == null)
        {
            /*transform.position = gameController.selectedTile.transform.position;
            gameController.selectedTile.gameObject.GetComponent<Tile>().pokemonObject = this.gameObject;
            tile.gameObject.GetComponent<Tile>().pokemonObject = null;
            */

            photonView.RPC("MoveUnit", RpcTarget.All, this.gameObject.GetComponent<PokemonController>().unitID,
             gameController.selectedTile.gameObject.GetComponent<Tile>().tileID, this.tile.GetComponent<Tile>().tileID, 888);
            //MoveUnit(int unitID, int targetTileID, int startTileID, int otherunitID)
        }
        else //If the tile selected has a pokemon already placed on it
        {
            /*
            //Replace the tile attribute of the still pokemon to this tile
            gameController.selectedTile.GetComponent<Tile>().pokemonObject.GetComponent<MovePokemon>().tile = tile;


            //Swap the position of the pokemons
            gameController.selectedTile.GetComponent<Tile>().pokemonObject.transform.position = tile.transform.position;
            transform.position = gameController.selectedTile.transform.position;

            //Swap the pokemonObject attributes over
            tile.gameObject.GetComponent<Tile>().pokemonObject = gameController.selectedTile.GetComponent<Tile>().pokemonObject;
            gameController.selectedTile.GetComponent<Tile>().pokemonObject = this.gameObject;
            */
            photonView.RPC("MoveUnit", RpcTarget.All, this.gameObject.GetComponent<PokemonController>().unitID,
             gameController.selectedTile.gameObject.GetComponent<Tile>().tileID, tile.gameObject.GetComponent<Tile>().tileID, 
                gameController.selectedTile.GetComponent<Tile>().pokemonObject.GetComponent<PokemonController>().unitID);
        }
        //tile = gameController.selectedTile;
    }

    [PunRPC]
    public void MoveUnit(int unitID, int targetTileID, int startTileID, int otherunitID)
    {

        GameObject[] pokemons = GameObject.FindGameObjectsWithTag("Units");
        GameObject unit = null;
        GameObject otherUnit = null;
        GameObject startTile = null;
        GameObject targetTile = null;
        print("start tile ID: " + startTileID + ", target tile ID: " + targetTileID);
        for (int i = 0; i < pokemons.Length; i++)
        {
            if(unitID == pokemons[i].GetComponent<PokemonController>().unitID)
            {
                unit = pokemons[i];
            }
            else if(otherunitID != 888 && otherunitID == pokemons[i].GetComponent<PokemonController>().unitID)
            {
                otherUnit = pokemons[i];
            }
        }
        for(int i = 0; i< GameController.Instance.tiles.Length; i++)
        {
            if(GameController.Instance.tiles[i].GetComponent<Tile>().tileID == startTileID)
            {
                startTile = GameController.Instance.tiles[i];
            } else if (GameController.Instance.tiles[i].GetComponent<Tile>().tileID == targetTileID)
            {
                targetTile = GameController.Instance.tiles[i];
            }
        }
        if(targetTileID == startTileID)
        {
            unit.transform.position = unit.GetComponent<MovePokemon>().tile.transform.position;
            return;
        }
        if (targetTile.gameObject.GetComponent<Tile>().pokemonObject == null)
        {//If target tile has no pokemon on it
            unit.transform.position = targetTile.transform.position;
            targetTile.GetComponent<Tile>().pokemonObject = unit;
            startTile.GetComponent<Tile>().pokemonObject = null;
        }
        else
        {//If target tile is already occupied

            //Replace the tile attribute of the still pokemon to this tile
            targetTile.GetComponent<Tile>().pokemonObject.GetComponent<MovePokemon>().tile = unit.GetComponent<MovePokemon>().tile;


            //Swap the position of the pokemons
            targetTile.GetComponent<Tile>().pokemonObject.transform.position = startTile.transform.position;
            unit.transform.position = targetTile.transform.position;

            //Swap the pokemonObject attributes over
            tile.gameObject.GetComponent<Tile>().pokemonObject = otherUnit;
            targetTile.GetComponent<Tile>().pokemonObject = unit;


        }

        unit.GetComponent<MovePokemon>().tile = targetTile;
    }
}
