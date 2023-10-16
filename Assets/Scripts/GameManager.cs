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

    [SerializeField] static public int maxWorm = 5;
    [SerializeField] Text txtTitle;
    [SerializeField] Text txtWorm;
    [SerializeField] Text txtTime;
    [SerializeField] Button btnStart;
    [SerializeField] Button btnEat;
    [SerializeField] Button btnRun;

    int WormCount = 0;
    float Elapsed = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        SetTitle();
    }

    void SetTitle()
    {
        GameMode = MODE.TITLE;
        txtTitle.enabled = true;
        btnStart.gameObject.SetActive(true);

        txtWorm.enabled = false;
        txtTime.enabled = false;
        btnEat.gameObject.SetActive(false);
        btnRun.gameObject.SetActive(false);
    }

    public void GameStart()
    {
        GameMode = MODE.PLAY;
        WormLevel = 1;
        WormCount = 0;
        txtTitle.enabled = false;
        btnStart.gameObject.SetActive(false);

        txtWorm.enabled = true;
        txtWorm.text = "Worm : 0 / " + maxWorm;
        txtTime.enabled = true;
        txtTime.text = "Time : 0.00s";
        btnEat.gameObject.SetActive(true);
        btnRun.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameMode == MODE.PLAY) { 
            Elapsed += Time.deltaTime;
            txtTime.text = "Time : " + Elapsed.ToString("f2") + "s";
        }
    }

    public void WormSpeedUp()
    {
        txtWorm.text = "Worm : " + ++WormCount + " / " + maxWorm;
        WormLevel++;
    }
}
