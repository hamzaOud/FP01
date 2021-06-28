using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;



public class GameController : MonoBehaviour
{

    public LayerMask championLayer;
    public Data data;
    public GameObject selectedPokemon;
    public GameObject selectedTile;
    public GameObject spawnPoint0;

    public GameObject[] tiles;
    public BoardController[] boardControllers;
    public BoardController myBoard;

    public Trainer[] trainers;


    public int playerID;

    public static GameController Instance;

    private void Start()
    {
        Instance = this;

        ConfigurePlayerID();
        //Initialization and Configuration
        tiles = GameObject.FindGameObjectsWithTag("Tile");
        ConfigureBoardController();
        ConfigureTrainers();


        if(playerID != 0)
        {
            MoveCamera(boardControllers[0].enemyCameraPosition);
        }
    }


    public void spawnChampion(Pokemon pokemon)
    {
       
    List<GameObject> pokemonObjects = new List<GameObject>();
    //Check to see if we have that pokemon already in our pokedex
    bool tester = data.trainer.pokedex.TryGetValue(pokemon, out pokemonObjects);
    if (pokemon.evolution != null && tester && pokemonObjects.Count >=2)
    { // If we do and we have 2 already existing, evolve it
           if (pokemonObjects.Count >= 2)
                {
                    foreach (GameObject gameObject in pokemonObjects)
                    {
                        Destroy(gameObject);
                    }
                data.trainer.pokedex.Remove(pokemon);
                StartCoroutine(Evolve(pokemon.evolution));
                }
    }
    else //If its not ready for evolution
        {
            List<GameObject> value = new List<GameObject>();
            GameObject spawnPoint = FindSpawnPoint(data.spawnObjects);

            object[] dataToTransfer = new object[1];
            dataToTransfer[0] = FindSpawnPoint(data.spawnObjects).GetComponent<Tile>().tileID;
            GameObject newPokemon = PhotonNetwork.Instantiate(pokemon.name + "GO", FindSpawnPoint(data.spawnObjects).transform.position, Quaternion.identity,0,dataToTransfer);
            //newPokemon.gameObject.GetComponent<MovePokemon>().tile = spawnPoint;
            //spawnPoint.gameObject.GetComponent<Tile>().pokemonObject = newPokemon;

            //GameObject pokemonObject = Instantiate(pokemon.model, FindSpawnPoint(data.spawnObjects).transform.position, Quaternion.identity);
            //pokemonObject.gameObject.GetComponent<MovePokemon>().tile = spawnPoint;
            //spawnPoint.gameObject.GetComponent<Tile>().pokemonObject = pokemonObject;
            if (data.trainer.pokedex.TryGetValue(pokemon, out value))
            {//If we already have one of these pokemons, add new one to list
                //value.Add(pokemonObject);
                value.Add(newPokemon);
            }
            else
            {//If we dont own any of that pokemon, create new list
                List<GameObject> thisPokemonsList = new List<GameObject>();
                //thisPokemonsList.Add(pokemonObject);
                thisPokemonsList.Add(newPokemon);
                data.trainer.pokedex.Add(pokemon, thisPokemonsList);
            }
        }

        UIController.Instance.UpdateUI();              
    }   


    public void updatePokemonsOnBoard() //Called in MovePokemon, bugging out because cant file Tile component
    {
        data.trainer.pokemonsOnBoard.Clear();

        foreach (GameObject tile in myBoard.myTiles)
        {
            if (tile.GetComponent<Tile>().pokemonObject != null)
            {
                data.trainer.pokemonsOnBoard.Add(tile.GetComponent<Tile>().pokemonObject);
            }
        }
    }

    public GameObject FindSpawnPoint(GameObject[] spawnObjects)
    {
        for (int i = 0; i < spawnObjects.Length; i++)
        {
            if (!Physics.CheckSphere(spawnObjects[i].transform.position, 0.5f, championLayer))
            {
                return spawnObjects[i];
            }
        }
        return spawnPoint0;
    }

    IEnumerator Evolve(Pokemon pokemon)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("wait is over");
        spawnChampion(pokemon);
    }


    private void ConfigureBoardController()
    {
        for(int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (CheckIfImOwner(PhotonNetwork.PlayerList[i].NickName))
            {
                boardControllers[i].isMine = true;
                myBoard = boardControllers[i];
                return;
            }
        }
    }

    private void ConfigurePlayerID()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PlayerPrefs.GetString("Name") == PhotonNetwork.PlayerList[i].NickName)
            {
                playerID = i;
            }
        }
    }

    public void ConfigureTrainers()
    {
        trainers = new Trainer[PhotonNetwork.PlayerList.Length];

        for (int i =0; i< PhotonNetwork.PlayerList.Length; i++)
        {
            Trainer trainer = new Trainer();
            trainers[i] = trainer;
        }
    }

    private bool CheckIfImOwner(string NickName)
    {
        if(PlayerPrefs.GetString("Name") == NickName)
        {
            return true;
        }
        else return false;
    }

    public GameObject FindTileByID(int tileID)
    {
        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i].GetComponent<Tile>())
            {
                if (tiles[i].GetComponent<Tile>().tileID == tileID)
                {
                return tiles[i].gameObject;
                }
            }
            else
            {
                if(tiles[i].transform.GetChild(0).gameObject.GetComponent<Tile>().tileID == tileID)
                {
                    return tiles[i].transform.GetChild(0).gameObject;
                }
            }
        }
        return null;
    }

    private void MoveCamera(GameObject cameraPosition)
    {
        Camera.main.gameObject.transform.position = cameraPosition.transform.position;
        Camera.main.gameObject.transform.rotation = cameraPosition.transform.rotation;
    }

    public bool CheckIfMine(int targetID)
    {
        if (targetID == playerID)
        {
            return true;
        }
        else
            return false;
    }
}
