using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSpawner : MonoBehaviour
{
    [SerializeField] GameObject wormPrefab;
    [SerializeField] float maxRadius = 20.0f; //生成半径
    [SerializeField] float interval = 2.0f; //初期生成間隔

    float elapsed; //経過時間
    int currEnemy;
    GameObject[] worms;
    GameObject player;
    GameManager manager;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        worms = new GameObject[GameManager.maxWorm];
        for(int i = 0; i < GameManager.maxWorm; i++)
        {
            worms[i] = Instantiate(wormPrefab, Vector3.zero, Quaternion.identity);
            worms[i].SetActive(false);
        }
        Ready(); //準備処理
    }

    //準備処理
    void Ready()
    {
        elapsed = 0.0f;
        //前プレイでのメテオが残っていれば撤去
        //GameObject[] Worms = GameObject.FindGameObjectsWithTag("Worm");
        foreach (GameObject Stored in worms)
        {
            Stored.SendMessage("ResetObject", SendMessageOptions.DontRequireReceiver);
            Stored.SetActive(false);
        }

        currEnemy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.GameMode)
        {
            case GameManager.MODE.TITLE:
                Ready();
                break;
            case GameManager.MODE.PLAY:
                PlayAction();
                break;
        }
    }

    void PlayAction()
    {
        elapsed += Time.deltaTime;
        if (elapsed > interval && currEnemy < GameManager.maxWorm)
        {
            //Debug.Log("SpawnEnemy");
            //生成位置を求める
            Vector3 Pos = player.transform.position;
            float Theta = Random.Range(0, Mathf.PI * 2.0f);
            float Radius = Random.Range(10, maxRadius);
            Pos.x += Mathf.Cos(Theta) * Radius;
            Pos.x = Mathf.Max(-64, Mathf.Min(55, Pos.x));
            Pos.z += Mathf.Sin(Theta) * Radius;
            Pos.z = Mathf.Max(-54, Mathf.Min(59, Pos.z));
            Pos.y = 2.5f;
            //敵を生成
            //Instantiate(enemyPrefab, Pos, Quaternion.identity);
            worms[currEnemy].transform.position = Pos;
            worms[currEnemy].SetActive(true);
            elapsed = 0.0f;
            currEnemy++;
        }
    }
}
