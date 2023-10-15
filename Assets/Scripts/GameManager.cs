using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{    public enum MODE
    {
        TITLE, //タイトル
        PLAY, //プレイ中
        OVER //ゲームオーバー
    }
    static public MODE GameMode; //ゲームの状態

    [SerializeField] GameObject PlayerPrefab;


    NavMeshSurface myNavMesh; //自身のナビメッシュサーフェース

    // Start is called before the first frame update
    void Start()
    {
        //自身のナビメッシュサーフェースを取得
        myNavMesh = GetComponent<NavMeshSurface>();

        //CreateMap();

        GameObject M = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity);

        GameMode = MODE.PLAY;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
