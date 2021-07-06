using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameStage { Preparation, Combat, Loss };

public class GamePlayController : MonoBehaviour
{
    public static GamePlayController Instance;

    public GameStage currentGameStage;
    public int roundNumber = 0; //Keeps track of the round number, to determine what to put on board
    public const int preparationRoundTime = 100; //Constant that keeps track of length of preparation round
    public const int combatRoundTime = 10; //Constant that keeps track of length of combat round
    private float currentRoundTimer = 0.0f;
    public int timerDisplay;

    public int baseGoldIncome = 5; //Base amount that you gain each round no matter what

    [HideInInspector]
    public List<PokemonController> enemyPokemons;//List of enemy pokemons that are alive on the board
    [HideInInspector]
    public List<PokemonController> myPokemonsAlive; //List of my units pokemons are alive on the board

    public Dictionary<PokeType, int> pokeTypeCounter; //List of unique Pokemon Types with associated count
    public Dictionary<PokeClass, int> pokeClassCounter; //List of unique Pokemon Classes with associated count

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


        if (hasTimerEnded())
        {
            NextRound();
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
        else if (currentGameStage == GameStage.Combat)
        {
            if (currentRoundTimer < combatRoundTime)
            {
                return false;
            }
            else return true;
        }
        else return false;

    }

    private void NextRound()
    {
        currentRoundTimer = 0.0f; //Reset timer
        if (currentGameStage == GameStage.Preparation)
        {//Switch game stage
            currentGameStage = GameStage.Combat;
            roundNumber++; //Only increment round number for combat rounds.
        }
        else if (currentGameStage == GameStage.Combat)
        {
            currentGameStage = GameStage.Preparation;
        }
        PrepareBoard();
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
        int income = baseGoldIncome + Data.Instance.trainer.balance / 10;
        return income;
    }

    private void PrepareBoard()
    {
        ResetBoard();

        if (currentGameStage == GameStage.Combat)
        {
            switch (roundNumber)
            {
                case 0:
                    for (int i = 0; i < 1; i++)
                    {
                        InstantiateEnemy(Data.Instance.pokemons[1].model, GameController.Instance.myBoard.enemyTiles[i]);
                    }
                    break;
                case 1:
                    for (int i = 0; i < 3; i++)
                    {
                        InstantiateEnemy(Data.Instance.pokemons[1].model, GameController.Instance.myBoard.enemyTiles[i]);
                    }
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                default:
                    break;
            }
        } else if(currentGameStage == GameStage.Preparation)
        {

        }
    }
    private void ResetBoard()
    {
        enemyPokemons.Clear();
        foreach(PokemonController enemy in enemyPokemons)
        {
            enemy.Reset();
        }
    }

    public void InstantiateEnemy(GameObject prefab, GameObject tile)
    {

        GameObject enemy = Instantiate(prefab, tile.transform.position, Quaternion.Euler(0,180,0));
        enemyPokemons.Add(enemy.GetComponent<PokemonController>());
        tile.gameObject.GetComponent<Tile>().pokemonObject = enemy;
        enemy.GetComponent<MovePokemon>().tile = tile;
    }

    bool AreAllEnemiesDead()
    {
        bool value = true;
        for(int i = 0; i < enemyPokemons.Count; i++)
        {
            if (enemyPokemons[i].isDead == false)
            {
                value = false;
            }
        }
        return value;
    }
}