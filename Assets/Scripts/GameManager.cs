using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int[,] map;
    [SerializeField] GameObject GroundPrefab;
    [SerializeField] GameObject WallPrefab;

    // Start is called before the first frame update
    void Start()
    {
        map = new int[8, 8];

        for (int i = 0; i < map.GetLength(0); i++)
        {
            for(int j = 0; j < map.GetLength(1); j++)
            {
                if(i == 0 || j == 0 || i == map.GetLength(0) - 1 || j == map.GetLength(1) - 1)
                {
                    map [i, j] = 0;
                }
                else
                {
                    map[i, j] = 1;
                }
                SpawnMap(i - map.GetLength(0) * 0.5f, j - map.GetLength(1) * 0.5f, map[i, j]);
            }
        }
    }

    void SpawnMap(float x, float z, int ID)
    {
        Vector3 Pos = new Vector3(x, 0, z); 
 
        if ( ID == 1 ) 
        {
            Instantiate(GroundPrefab, Pos, Quaternion.identity); 
        }
        else if(ID == 0)
        {
            Instantiate(WallPrefab, Pos, Quaternion.identity); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
