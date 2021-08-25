using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;



public class GameController : MonoBehaviour
{

    public LayerMask championLayer; //Layer to see if any pokemons collide with
    public Data data;
    public GameObject selectedPokemon; //Current selected unit
    public GameObject selectedTile; //Current selected tile
    public GameObject spawnPoint0;

    public GameObject[] tiles; //All tiles in game
    public BoardController[] boardControllers; 
    public BoardController myBoard; //My board controller

    public Trainer[] trainers; //Array of players/trainers
    public int playerID; //my player ID

    public static GameController Instance;

    public Transform myCameraPosition;
    public Transform enemyCameraPosition;

    public int nextViewID = 10;

    private void Start()
    {
        Instance = this;

        //Initialization and Configuration
        ConfigurePlayerID();
        ConfigureBoardController();
        ConfigureTrainers();

        MoveCamera(myBoard.myCameraPosition);

    }

    [PunRPC]
    public void Spawn(int trainerID, int pokemonID)
    {
        Pokemon pokemon = new Pokemon();
        for(int i = 0; i < Data.Instance.pokemonsDatabase.Length; i++)
        {
            if(pokemonID == Data.Instance.pokemonsDatabase[i].pokemonID)
            {
                pokemon = Data.Instance.pokemonsDatabase[i];
            }
        }
        List<GameObject> pokemonObjects = new List<GameObject>();
        //Check to see if we have that pokemon already in our pokedex
        bool tester = trainers[trainerID].pokedex.TryGetValue(pokemon, out pokemonObjects);
        if (pokemon.evolution != null && tester && pokemonObjects.Count >= 2)
        { // If we do and we have 2 already existing, evolve it
            if (pokemonObjects.Count >= 2)
            {
                foreach (GameObject gameObject in pokemonObjects)
                {
                    gameObject.GetComponent<MovePokemon>().tile.GetComponent<Tile>().pokemonObject = null;
                    Destroy(gameObject);
                }
                trainers[trainerID].pokedex.Remove(pokemon);
                StartCoroutine(Evolve(pokemon.evolution, trainerID));
            }
        }
        else //If its not ready for evolution
        {
            List<GameObject> value = new List<GameObject>();
            GameObject spawnPoint = FindSpawnPoint(trainers[trainerID].spawnPoints);

            GameObject newPokemon = Instantiate(pokemon.model, spawnPoint.transform.position, Quaternion.identity);
            spawnPoint.GetComponent<Tile>().pokemonObject = newPokemon;
            newPokemon.GetComponent<MovePokemon>().tile = spawnPoint;
            newPokemon.GetComponent<PokemonController>().unitID = GameObject.FindGameObjectsWithTag("Units").Length;
            newPokemon.GetComponent<PokemonController>().ownerID = playerID;

            if (trainers[trainerID].pokedex.TryGetValue(pokemon, out value))
            {//If we already have one of these pokemons, add new one to list
                value.Add(newPokemon);
            }
            else
            {//If we dont own any of that pokemon, create new list
                List<GameObject> thisPokemonsList = new List<GameObject>();
                thisPokemonsList.Add(newPokemon);
                trainers[trainerID].pokedex.Add(pokemon, thisPokemonsList);
            }
        }

        UIController.Instance.UpdateUI();
    }

    IEnumerator Evolve(Pokemon pokemon, int playerID)
    {
        yield return new WaitForSeconds(0.1f);
        print("should spawn " + pokemon.name);
        //spawnChampion(pokemon);
        Spawn(playerID, pokemon.pokemonID);
    }


    public void spawnChampion(Pokemon pokemon)
    {
        PhotonView.Get(this).RPC("Spawn", RpcTarget.All, playerID, pokemon.pokemonID);
    }


    public void updatePokemonsOnBoard()
    {
        for(int i = 0; i< PhotonNetwork.PlayerList.Length; i++)
        {
            trainers[i].pokemonsOnBoard.Clear();
            foreach(GameObject tile in GameController.Instance.boardControllers[i].myTiles)
            {
                if(tile.GetComponent<Tile>().pokemonObject != null)
                {
                    print(tile.GetComponent<Tile>().tileID + " occupied");
                    GameController.Instance.trainers[i].pokemonsOnBoard.Add(tile.GetComponent<Tile>().pokemonObject);
                }
            }
        }
    }

    public GameObject FindSpawnPoint(GameObject[] spawnObjects)
    {
        //Iterate through all spawnpoints
        for (int i = 0; i < spawnObjects.Length; i++)
        {//If the spawn point is empty
            if (!Physics.CheckSphere(spawnObjects[i].transform.position, 0.5f, championLayer))
            {
                return spawnObjects[i];
            }
        }
        return spawnPoint0; //else return spawn point 0
    }




    private int findNextIndex(int trainerID)
    {
        int value = 0;

        for (int i = 0; i < trainers[trainerID].spawnPoints.Length; i++)
        {
            if (trainers[trainerID].spawnPoints == null)
            {
                value = i;
                break;
            }
        }
        return value;
    }

    private bool CheckIfImOwner(string NickName)
    {
        if(PlayerPrefs.GetString("Name") == NickName)
        {
            return true;
        }
        else return false;
    }

    public GameObject FindTileByID(int tileID) //Searches through all tiles, find the matching gameobject with tileID and returns it
    {
        for (int i = 0; i < tiles.Length; i++)
        {//If the gameobject has a Tile component
            if (tiles[i].GetComponent<Tile>()) 
            {//if the gameobject has same tileID
                if (tiles[i].GetComponent<Tile>().tileID == tileID)
                {
                return tiles[i].gameObject;
                }
            }
            else //If gameobject has no Tile component
            {//Find the Tile component by going on the 1st child object (in editor)
                if(tiles[i].transform.GetChild(0).gameObject.GetComponent<Tile>().tileID == tileID)
                {
                    return tiles[i].transform.GetChild(0).gameObject;
                }
            }
        }
        return null;
    }

    public void MoveCamera(Transform cameraPosition)
    {
        Camera.main.gameObject.transform.position = cameraPosition.position;
        Camera.main.gameObject.transform.rotation = cameraPosition.rotation;
    }

    public bool CheckIfMine(int targetID) //Checks if target's playerID matches mine 
    {
        if (targetID == playerID)
        {
            return true;
        }
        else
            return false;
    }

    public void LvlUP(int trainerID)
    {
        PhotonView.Get(this).RPC("LevelTrainer", RpcTarget.All, trainerID);
    }

    [PunRPC]
    public void LevelTrainer(int trainerID)
    {
        for(int i = 0; i< trainers.Length; i++)
        {
            if(trainers[i].trainerID == trainerID)
            {
                trainers[i].LevelUP();
            }
        }
    }

    private void ConfigureBoardController()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (CheckIfMine(i))
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
        //Initialize new array of trainers
        trainers = new Trainer[PhotonNetwork.PlayerList.Length];

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Trainer trainer = new Trainer(i);
            //trainer.spawnPoints = new GameObject[boardControllers[i].myBench.Length + boardControllers[i].myTiles.Length];
            int caca = 0;
            for (int j = 0; j < boardControllers[i].myBench.Length; j++)
            {
                trainer.spawnPoints[j] = boardControllers[i].myBench[j];
                caca++;
            }
            for (int k = 0; k < boardControllers[i].myTiles.Length; k++)
            {
                trainer.spawnPoints[caca] = boardControllers[i].myTiles[k];
                caca++;
            }
            trainers[i] = trainer;
        }
    }

}
