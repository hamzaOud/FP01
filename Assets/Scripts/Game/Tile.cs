using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { Bench, Board}

public class Tile : MonoBehaviour
{
    public GameObject pokemonObject;
    public int tileID;
    public bool isMine;
    public TileType tileType;
}
