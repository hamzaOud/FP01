using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{

    public Transform startPosition;

    public const int mapSizeX = 7;
    public const int mapsizeZ = 8;

    public Vector3[,] mapPositions;


    // Start is called before the first frame update
    void Start()
    {
        ConfigureGridPositions();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ConfigureGridPositions()
    {
        mapPositions = new Vector3[mapSizeX, mapsizeZ / 2];

        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapsizeZ / 2; j++)
            {
                Vector3 position = startPosition.position;



                position.x =startPosition.position.x + (i * 3.75f);
                position.z = startPosition.position.z + (j * 3.3f);
                if (j % 2 == 1)
                {
                    position.x += 1.875f;
                }
                mapPositions[i,j] = position;
            }
        }
    }
}
