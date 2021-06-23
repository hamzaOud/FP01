using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovePokemon : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;

    public GameObject tile;

    public Pokemon pokemon;
    private GameController gameController;

    public int layeringtest;
    public Data data;

    private void Start()
    {
        gameController = GameObject.Find("Scripts").GetComponent<GameController>();

        data = GameObject.Find("Scripts").GetComponent<Data>();
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
        //gameObject.GetComponent<BoxCollider>().enabled = false;
     }

     void OnMouseDrag()

     {

         Vector3 yOffset = GetMouseAsWorldPoint() + mOffset;
         yOffset.y = 2.0f;

         //transform.position = GetMouseAsWorldPoint() + mOffset;
         transform.position = yOffset;
     }

     private void OnMouseUp()
     {
        //If a tile is selected
        if (gameController.selectedTile != null)
        {

            //If selected a tile on the board
            if (gameController.boardTiles.Contains(gameController.selectedTile))
            {
                //3 options to move a pokemon on the board tiles:
                //1. If trainer's level is > to the number of pokemons on board
                //2. if moving a pokemon from one board tile to another
                //3. If moving a pokemon from the bench to the board (the selected tile's pokemonObject is not null)
                if (data.trainer.level > data.trainer.pokemonsOnBoard.Count || (gameController.boardTiles.Contains(tile) && gameController.boardTiles.Contains(gameController.selectedTile)) || (data.trainer.level == data.trainer.pokemonsOnBoard.Count && gameController.selectedTile.GetComponent<Tile>().pokemonObject != null))
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
        GameController.Instance.updatePokemonsOnBoard();
    }

    void disablePokemonColliders()
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
            transform.position = gameController.selectedTile.transform.position;
            gameController.selectedTile.gameObject.GetComponent<Tile>().pokemonObject = this.gameObject;
            tile.gameObject.GetComponent<Tile>().pokemonObject = null;
        }
        else //If the tile selected has a pokemon already placed on it
        {
            //Replace the tile attribute of the still pokemon to this tile
            gameController.selectedTile.GetComponent<Tile>().pokemonObject.GetComponent<MovePokemon>().tile = tile;


            //Swap the position of the pokemons
            gameController.selectedTile.GetComponent<Tile>().pokemonObject.transform.position = tile.transform.position;
            transform.position = gameController.selectedTile.transform.position;

            //Swap the pokemonObject attributes over
            tile.gameObject.GetComponent<Tile>().pokemonObject = gameController.selectedTile.GetComponent<Tile>().pokemonObject;
            gameController.selectedTile.GetComponent<Tile>().pokemonObject = this.gameObject;
        }
        tile = gameController.selectedTile;
    }
    
}
