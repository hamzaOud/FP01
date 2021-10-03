using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HextileMap2 : MonoBehaviour
{
        public int mapWidth = 7;
        public int mapHeight = 8;

        public GameObject hexTilePrefab;

        public float tileXOffset = 1.8f; //distance in width (x) of 2 hex tiles
        public float tileZOffset = 1.565f; // distance in length(z) of 2 hex tiles

        public Node[,] nodes; //array of all existingn nodes (here 56 nodes)
        public GameObject[] tiles; //array of all the hextile gameobjects

        public float offset = 3.5f;

        public static HextileMap2 Instance;

        public List<Node> occupiedNodes;

        // Start is called before the first frame update
        void Start()
        {
            Instance = this;
            nodes = new Node[mapWidth, mapHeight]; //CHANGE 8 TO GAMECONTROLLER.INSTANCE.BOARDONCTROLLERS.LENGTH
            tiles = new GameObject[mapWidth * mapHeight];
            CreateHexTileMap();
            occupiedNodes = new List<Node>();
        }
        // Update is called once per frame
        void CreateHexTileMap()
        {

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    GameObject TempGO = Instantiate(hexTilePrefab);

                    if (y % 2 == 0)
                    {//If we are on an even row
                        TempGO.transform.position = new Vector3(x * tileXOffset - offset, 0, y * tileZOffset - offset);
                    }
                    else
                    {//If we are on an odd row, shift the tile right by half a tile's width
                        TempGO.transform.position = new Vector3(x * tileXOffset + (tileXOffset / 2) - offset, 0, y * tileZOffset - offset);
                    }
                    Node node = new Node(true, x, y, TempGO.transform.position);
                    nodes[x, y] = node; //Store this tile's node in the nodes array

                    tiles[findNextIndex(tiles)] = TempGO;
                    TempGO.GetComponent<TileController>().coordinates = new Vector2(x, y);
                    //TempGO.GetComponent<Tile>().tileID = FindNextTileID();
                }
            }
        }


        int findNextIndex(GameObject[] tiles)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i] == null)
                {
                    return i;
                }
            }

            return 0;
        }

        int FindNextTileID()
        {
            int tilesCount = GameObject.FindGameObjectsWithTag("Tile").Length;
            return tilesCount;
        }

        //Returns the corresponding node of a tile gameObject.
        public Node findNodeFromTile(GameObject tile)
        {
            Node n = null;

            foreach (Node node in nodes)
            {
                if (node.worldPos == tile.transform.position)
                {
                    n = node;
                }
            }

            return n;
        }

        //Returns the corresponding gameObject of a node.
        public GameObject FindTileFromNode(Node node)
        {
            GameObject tile = null;
            foreach (GameObject t in tiles)
            {
                if (t.transform.position == node.worldPos)
                {
                    tile = t;
                }
            }
            return tile;

        }

        //Converts offset (x,y) coordinates of a given node into cube coordinates (x,y,z)
        //where x+y+z = 0
        public Vector3 GetCubeCoord(Node node)
        {
            int x = node.gridX - (node.gridY - (node.gridY & 1)) / 2;
            int z = node.gridY;
            int y = -x - z;
            return new Vector3(x, y, z);
        }

        //Looks through all adjacent tiles of a given node, the neighbouring coordinates are different for odd and even rows.
        //Tiles have a maximum of 6 neighbours.
        public List<Node> GetNeighbours(Node node)
        {
            List<Node> adjacencyListr = new List<Node>();
            if (node.gridY % 2 == 0)
            {
                CheckTile(node.gridX - 1, node.gridY - 1, adjacencyListr);
                CheckTile(node.gridX - 1, node.gridY, adjacencyListr);
                CheckTile(node.gridX + 1, node.gridY, adjacencyListr);
                CheckTile(node.gridX, node.gridY - 1, adjacencyListr);
                CheckTile(node.gridX, node.gridY + 1, adjacencyListr);
                CheckTile(node.gridX - 1, node.gridY + 1, adjacencyListr);
            }
            else
            {
                CheckTile(node.gridX, node.gridY - 1, adjacencyListr);
                CheckTile(node.gridX - 1, node.gridY, adjacencyListr);
                CheckTile(node.gridX + 1, node.gridY, adjacencyListr);
                CheckTile(node.gridX + 1, node.gridY - 1, adjacencyListr);
                CheckTile(node.gridX + 1, node.gridY + 1, adjacencyListr);
                CheckTile(node.gridX, node.gridY + 1, adjacencyListr);
            }

            return adjacencyListr;
        }

        //Checks to see if a node with these coordinate exist, if so, add that node to the list passed in the parameters.
        void CheckTile(int x, int y, List<Node> adjacencyR)
        {
            foreach (Node node in HexTileMapGenerator.Instance.nodes)
            {
                if (node.gridX == x && node.gridY == y)
                {
                    adjacencyR.Add(node);
                    return;
                }
            }
        }

        //Finds the distance (in tiles) of 2 nodes using the manhattan distance algorithm. 
        //This algorithm requires cube coordinates, do the first thing to do is convert the offset coordinates of the tiles into cube ones.
        //Computes the absolute value of the coordinate of A minus B for each cube coordinate, then returns the largest one.
        public int Distance(Node nodeA, Node nodeB)
        {
            Vector3 cubeNodeA = GetCubeCoord(nodeA);
            Vector3 cubeNodeB = GetCubeCoord(nodeB);
            return (int)Mathf.Max(Mathf.Abs(cubeNodeA.x - cubeNodeB.x), Mathf.Abs(cubeNodeA.y - cubeNodeB.y), Mathf.Abs(cubeNodeA.z - cubeNodeB.z));

            //We could also use the following line of code to compute the distance.
            //Manhattan distance is the absolute value of (Ax-Bx) + (Ay-By), but in the cube grid tiles are distance 2 apart,
            //so we divide the total by 2
            //return (int)(Mathf.Abs(cubeNodeA.x - cubeNodeB.x) + Mathf.Abs(cubeNodeA.y - cubeNodeB.y) + Mathf.Abs(cubeNodeA.z - cubeNodeB.z)) / 2;

        }
 }
