using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{    public enum MODE
    {
        TITLE, //タイトル
        PLAY, //プレイ中
        OVER //ゲームオーバー
    }
    static public MODE GameMode; //ゲームの状態
    static public int WormLevel;
    //[SerializeField] GameObject PlayerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        GameMode = MODE.PLAY;
        WormLevel = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WormSpeedUp()
    {
        Debug.Log("WSP");
        WormLevel++;
    }
}
