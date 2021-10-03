using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentTileTest : MonoBehaviour
{

    public GameObject currentTile;

    // Start is called before the first frame update
    void Start()
    {
        GetCurrentTile();
    }

    void Update()
    {
        GetCurrentTile();
    }

    public void GetCurrentTile()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        {
            print("found something beneath");
            if (hit.collider.gameObject.tag == "Tile")
            {
                print("Should assign something to currenttiletest");
                currentTile = hit.collider.gameObject;
            }
        }

    }
}
