using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    public GameObject[] myBench;
    public GameObject[] myTiles;
    public GameObject[] enemyTiles;
    public GameObject[] tiles;

    public Transform myCameraPosition;
    public Transform enemyCameraPosition;

    public bool isMine;

    public Node[,] nodes;

    private void Awake()
    {
        myTiles = new GameObject[28];
        tiles = new GameObject[56];
        enemyTiles = new GameObject[28];
        nodes = new Node[7, 8];
    }
}
