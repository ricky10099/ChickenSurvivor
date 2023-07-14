using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{    public enum MODE
    {
        TITLE, //タイトル
        PLAY, //プレイ中
        OVER //ゲームオーバー
    }
    static public MODE GameMode; //ゲームの状態

    int[,] map;
    [SerializeField] GameObject GroundPrefab;
    [SerializeField] GameObject WallPrefab;
    [SerializeField] GameObject WallColliderPrefab;

    [SerializeField] int MapSize;
    // Start is called before the first frame update
    void Start()
    {
        WallPrefab.transform.localScale = new Vector3( 2.0f, 0.7f, 1.1f);

        map = new int[MapSize, MapSize];

        for (int i = 0; i < MapSize; i++)
        {
            for(int j = 0; j < MapSize; j++)
            {
                if(i == 0 || j == 0 || i == MapSize - 1 || j == MapSize - 1)
                {
                    map [i, j] = 0;
                }
                else
                {
                    map[i, j] = 1;
                }
                SpawnMap(i - MapSize * 0.5f, j - MapSize * 0.5f, map[i, j]);
            }
        }
        float colliderLength = MapSize + 20;
        WallColliderPrefab.transform.localScale = new Vector3(colliderLength, 20, 2);
        Instantiate(WallColliderPrefab, new Vector3(0, 0, (MapSize * 0.5f) - 1), Quaternion.Euler(0, 0, 0));
        Instantiate(WallColliderPrefab, new Vector3((MapSize * 0.5f) - 1, 0, 0), Quaternion.Euler(0, 90, 0));
        Instantiate(WallColliderPrefab, new Vector3(0, 0, (-MapSize * 0.5f)), Quaternion.Euler(0, 0, 0));
        Instantiate(WallColliderPrefab, new Vector3((-MapSize * 0.5f), 0, 0), Quaternion.Euler(0, 90, 0));
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
            GameObject wallObj = Instantiate(WallPrefab, Pos, Quaternion.Euler(0, 180, 0)); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
