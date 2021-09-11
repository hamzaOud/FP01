using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public enum GameStage { Preparation, Combat, Loss };

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance;

    public GameStage currentGameStage;
    public int roundNumber = 0; //Keeps track of the round number, to determine what to put on board
    public const int preparationRoundTime = 25; //Constant that keeps track of length of preparation round
    public const int combatRoundTime = 20; //Constant that keeps track of length of combat round
    private float currentRoundTimer = 0.0f;
    public int timerDisplay;
    public int enemyID;

    public int[] baseIncome = { 2, 3, 4, 5, 5, 5, 5, 5, 5 };//Base amount that you gain depending on level

    [HideInInspector]
    public List<PokemonController> enemyPokemons = new List<PokemonController>();//List of enemy pokemons that are alive on the board
    [HideInInspector]
    public List<PokemonController> myPokemonsAlive = new List<PokemonController>(); //List of my units pokemons are alive on the board
    [HideInInspector]
    public List<PokemonController> myUnitsOnBoard = new List<PokemonController>(); //List of my pokemons on the board

    public Dictionary<PokeType, int> pokeTypeCounter; //List of unique Pokemon Types with associated count
    public Dictionary<PokeClass, int> pokeClassCounter; //List of unique Pokemon Classes with associated count

    public PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        currentGameStage = GameStage.Preparation; //Start with Preparation round
        PrepareBoard();
    }

    // Update is called once per frame
    void Update()
    {
        currentRoundTimer += Time.deltaTime; //Increment timer

        if (currentGameStage == GameStage.Preparation)
        {//Calculate timer to show on UI based on current stage
            timerDisplay = preparationRoundTime - (int)currentRoundTimer;
        }
        else if (currentGameStage == GameStage.Combat)
        {//Calculate timer to show on UI based on current stage
            timerDisplay = combatRoundTime - (int)currentRoundTimer;
        }
        UIController.Instance.UpdateTimer(); //update timer on UI


        if (hasTimerEnded() && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("NextRound", RpcTarget.All);
        }
    }

    private bool hasTimerEnded()
    {
        if (currentGameStage == GameStage.Preparation)
        {
            if (currentRoundTimer < preparationRoundTime)
            {
                return false;
            }
            else
                return true;
        }
        else
        {
            if (currentRoundTimer < combatRoundTime)
            {
                return false;
            }
            else return true;
        }
    }

    
    [PunRPC]
    public void NextRound()
    {
        currentRoundTimer = 0.0f; //Reset timer
        if (currentGameStage == GameStage.Preparation)
        {//Switch game stage
            currentGameStage = GameStage.Combat;
        }
        else if (currentGameStage == GameStage.Combat)
        {
            currentGameStage = GameStage.Preparation;
            roundNumber++; //Only increment round number after each combat round
        }
        PrepareBoard();
        ShopController.Instance.RefreshShop();
    }

    public void CountBonuses()
    {
        pokeTypeCounter = new Dictionary<PokeType, int>();
        pokeClassCounter = new Dictionary<PokeClass, int>();

        List<Pokemon> uniquePokemons = new List<Pokemon>();
        foreach(GameObject gameObject in Data.Instance.trainer.pokemonsOnBoard)
        {//Add all unique pokemons on board to a new list
            Pokemon pokemon = gameObject.GetComponent<MovePokemon>().pokemon;
            if (!uniquePokemons.Contains(pokemon))
            {
                uniquePokemons.Add(pokemon);
            }
        }


        for(int i = uniquePokemons.Count - 1; i >= 0; i--)
        {//Remove pokemon evolutions from unique pokemons
            if (uniquePokemons.Contains(uniquePokemons[i].evolution))
            {
                uniquePokemons.Remove(uniquePokemons[i].evolution);
            }
        }

        for (int i = 0; i < uniquePokemons.Count; i++)
        {

            PokeType[] pokemonsType = uniquePokemons[i].type;
            PokeClass[] pokemonsClasses = uniquePokemons[i].pokeClass;

            for (int j = 0; j < pokemonsType.Length; j++)
            {//Extract all unique types with associated count
                if (pokeTypeCounter.ContainsKey(pokemonsType[j]))
                {//If we already have that type in list, increment counter
                    int counter = 0;
                    pokeTypeCounter.TryGetValue(pokemonsType[j], out counter);
                    counter++;
                    pokeTypeCounter[pokemonsType[j]] = counter;
                }
                else
                {//Add new type to list, with a counter of 1
                    pokeTypeCounter.Add(pokemonsType[j], 1);

                }

            }
            for (int j = 0; j < pokemonsClasses.Length; j++)
            {//Extract all unique classes with associated count
                if (pokeClassCounter.ContainsKey(pokemonsClasses[j]))
                {//If we already have that class in list, increment counter
                    int counter = 0;
                    pokeClassCounter.TryGetValue(pokemonsClasses[j], out counter);
                    counter++;
                    pokeClassCounter[pokemonsClasses[j]] = counter;
                }
                else
                {//Add new pokeClass to list, with a counter of 1
                    pokeClassCounter.Add(pokemonsClasses[j], 1);

                }

            }
        }
    }

    public int CalculateIncome()
    {
        int income = baseIncome[Data.Instance.trainer.level + 1] + Data.Instance.trainer.balance / 10;
        return income;
    }

    private void PrepareBoard()
    {
        //ResetBoard();
        if(currentGameStage == GameStage.Preparation)
        {
            ResetBoard();
            GameController.Instance.MoveCamera(GameController.Instance.myBoard.myCameraPosition);
        }
        if (currentGameStage == GameStage.Combat)
        {
            switch (roundNumber)
            {
                /*case 0:
                    for (int i = 0; i < 1; i++)
                    {
                        InstantiateEnemy(Data.Instance.pokemons[1].model, GameController.Instance.myBoard.enemyTiles[i + 3]);
                    }
                    break;
                case 1:
                    for (int i = 0; i < 2; i++)
                    {
                        InstantiateEnemy(Data.Instance.pokemons[1].model, GameController.Instance.myBoard.enemyTiles[i + 3]);
                    }
                    break;
                case 2:
                    for (int i = 0; i < 2; i++)
                    {
                        InstantiateEnemy(Data.Instance.pokemons[1].model, GameController.Instance.myBoard.enemyTiles[i + 2]);
                    }
                    break;*/
                default:
                    if (PhotonNetwork.IsMasterClient)
                    {                    
                        Vector2[] pairs = PairEnemies();
                        foreach (Vector2 pair in pairs)
                        {
                            photonView.RPC("FightEnemy", RpcTarget.All, (int)pair.x, (int)pair.y);
                        }
                    }
                    break;
            }
        } 
    }
    private void ResetBoard()
    {
        
        for(int i = 0; i < GameController.Instance.trainers.Length; i++)
        {
            foreach(GameObject unit in GameController.Instance.trainers[i].pokemonsOnBoard)
            {
                unit.SetActive(true);
                //unit.GetComponent<PokemonController>().ResetTest();
            }
        }
        GameObject[] units = GameObject.FindGameObjectsWithTag("Units");
        print("number of units: " + units.Length);
        for(int i = 0;i < units.Length;i++)
        {
            if (units[i].GetComponent<PokemonController>().ownerID == 888)
                Destroy(units[i]);
            else {
                print("Resetting "+units[i].GetComponent<PokemonController>().unitID + " that is alive: " + units[i].GetComponent<PokemonController>().isAlive);
                units[i].GetComponent<PokemonController>().ResetTest();     
            }
        }
    }

    public void InstantiateEnemy(GameObject prefab, GameObject tile)
    {

        GameObject enemy = Instantiate(prefab, tile.transform.position, Quaternion.Euler(0,180,0));
        tile.gameObject.GetComponent<Tile>().pokemonObject = enemy;
        enemy.GetComponent<MovePokemon>().tile = tile;
        enemy.GetComponent<PokemonController>().ownerID = 888;
        enemy.GetComponent<PokemonController>().isOnBoard = true;
        enemy.GetComponent<PokemonController>().unitID = GameController.Instance.npcUnitID;
        GameController.Instance.incrementNPCUnitID();

        foreach(PokemonController pokemonController in myUnitsOnBoard)
        {
            pokemonController.enemies.Add(enemy);
            enemy.GetComponent<PokemonController>().enemies.Add(pokemonController.gameObject);
        }
    }

    public bool AreAllMyUnitsDead()
    {
        bool value = true;
        for(int i = 0; i < myUnitsOnBoard.Count; i++)
        {
            if (myUnitsOnBoard[i].isAlive)
            {
                value = false;
            }
            //print(myUnitsOnBoard[i].pokemon.name + " is alive:" + myUnitsOnBoard[i].isAlive);
        }
        return value;
    }

    public void RepositionUnit(GameObject unit, int enemyID)
    {//Reposition unit on enemy's board

        //1.find which node its on
        //2.Find matching node on enemy's board
        //3.Find matching tile from node
        //4.position on tile and rotate

        Node startNode = HexTileMapGenerator.Instance.findNodeFromTile(unit.GetComponent<MovePokemon>().tile);
        Node finalNode = HexTileMapGenerator.Instance.nodes[enemyID, HexTileMapGenerator.Instance.mapWidth - startNode.gridX - 1,
            HexTileMapGenerator.Instance.mapHeight - startNode.gridY - 1];

        GameObject tileObject = HexTileMapGenerator.Instance.FindTileFromNode(finalNode);

        unit.transform.position = tileObject.transform.position;
        unit.transform.rotation = Quaternion.Euler(0, 180, 0);

        GameController.Instance.updatePokemonsOnBoard();
        unit.GetComponent<PokemonController>().enemies = GameController.Instance.trainers[enemyID].pokemonsOnBoard;
        foreach(GameObject u in GameController.Instance.trainers[enemyID].pokemonsOnBoard)
        {
            u.GetComponent<PokemonController>().enemies.Add(unit);
        }

    }

    public Vector2[] PairEnemies()
    {
        //1.check if number of players is even
        //2.if not, add one NPC ID to end of playerlist
        //3.randomise array
        //4.Create n pairs of players

        List<int> playerIDs = new List<int>();
        for(int i = 0; i < GameController.Instance.trainers.Length; i++)
        {
            playerIDs.Add(i);
        }
        if(playerIDs.Count % 2 != 0)
        {//If theres an odd number of players
            playerIDs.Add(888); //Add a NPC ID
        }


        List<int> randomList = new List<int>();

        while (playerIDs.Count > 0)
        {
            int randomIndex = Random.Range(0, playerIDs.Count);
            randomList.Add(playerIDs[randomIndex]);
            playerIDs.RemoveAt(randomIndex);
        }

        Vector2[] pairs = new Vector2[randomList.Count/2];

        for(int i = 0; i < pairs.Length; i++)
        {
            int j = i * 2;
            pairs[i] = new Vector2(randomList[j], randomList[j + 1]);
        }
        return pairs;
    }

    void PlaceUnitsOnEnemysBoard(int player1ID, int player2ID)
    {//Moving Player 2's units onto player 1's board
        
        GameController.Instance.updatePokemonsOnBoard();

        for(int i = 0; i < GameController.Instance.trainers[player2ID].pokemonsOnBoard.Count; i++)
        {
            RepositionUnit(GameController.Instance.trainers[player2ID].pokemonsOnBoard[i], player1ID);
        }
    }

    [PunRPC]
    public void FightEnemy(int player1ID, int player2ID)
    {

        if(player1ID == GameController.Instance.playerID)
        {
            GamePlayController.Instance.enemyID = player2ID;
        } else if(player2ID == GameController.Instance.playerID)
        {
            GamePlayController.Instance.enemyID = player1ID;
        }

        if (player1ID != 888 && player2ID != 888)
        {
            if (player2ID == GameController.Instance.playerID)
            {
                GameController.Instance.MoveCamera(GameController.Instance.boardControllers[player1ID].enemyCameraPosition);
            }
            PlaceUnitsOnEnemysBoard(player1ID, player2ID);
        }
        else if (player1ID == 888 || player2ID == 888)
        {
            if (player1ID == Data.Instance.trainer.trainerID || player2ID == Data.Instance.trainer.trainerID)
            {
                for (int i = 0; i < 2; i++)
                {
                    InstantiateEnemy(Data.Instance.pokemons[0].model, GameController.Instance.myBoard.enemyTiles[i + 2]);
                }
            }
        }
    }
}