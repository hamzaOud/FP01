using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTileMapGenerator : MonoBehaviour
{
    public int mapWidth = 7;
    public int mapHeight = 8;

    public GameObject hexTilePrefab;

    public float tileXOffset = 1f;
    public float tileZOffset = 1.5f;

    public int offset;
    public static HexTileMapGenerator Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        CreateHexTileMap();
    }

    // Update is called once per frame
    void CreateHexTileMap()
    {
        for(int z = 0; z < mapHeight; z++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                GameObject TempGO = Instantiate(hexTilePrefab);

                if(z % 2 == 0)
                {
                    TempGO.transform.position = new Vector3(x * tileXOffset - offset,0,z*tileZOffset - offset);
                } else
                {
                    TempGO.transform.position = new Vector3(x * tileXOffset + tileXOffset / 2 - offset, 0, z * tileZOffset - offset);
                } 

                BoardController board1 = GameObject.Find("Board1").GetComponent<BoardController>();
                TempGO.name = findNextIndex(board1.tiles).ToString();
                board1.tiles[findNextIndex(board1.tiles)] = TempGO;
                TempGO.tag = "Tile";
                
                TempGO.transform.parent = board1.gameObject.transform;

                if (z < 4)
                {      
                    board1.myTiles[findNextIndex(board1.myTiles)] = TempGO;
                }
                else
                {
                    board1.enemyTiles[findNextIndex(board1.enemyTiles)] = TempGO;
                }
            }
        }
    }

    int findNextIndex(GameObject[] tiles)
    {
        for(int i = 0;i < tiles.Length; i++)
        {
            if(tiles[i] == null)
            {
                return i;
            }
        }

        return 0;
    }
}
