using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab; //メテオプレハブ
    [SerializeField] float MaxRadius = 20.0f; //生成半径
    [SerializeField] float InitInterval = 2.0f; //初期生成間隔

    float Interval; //生成間隔
    float Elapsed; //経過時間
    //[SerializeField] int maxEnemy = 30;
    int currEnemy;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Ready(); //準備処理
    }

    //準備処理
    void Ready()
    {
        Elapsed = 0.0f;
        Interval = InitInterval;
        //前プレイでのメテオが残っていれば撤去
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject Stored in Enemies)
        {
            Destroy(Stored);
        }

        player = GameObject.FindGameObjectWithTag("Player");
        currEnemy = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameMode == GameManager.MODE.PLAY)
        {
            Elapsed += Time.deltaTime;
            if (Elapsed > Interval && currEnemy < GameManager.maxWorm)
            {
                //Debug.Log("SpawnEnemy");
                //生成位置を求める
                Vector3 Pos = player.transform.position;
                float Theta = Random.Range(0, Mathf.PI * 2.0f);
                float Radius = Random.Range(10, MaxRadius);
                Pos.x += Mathf.Cos(Theta) * Radius;
                Pos.x = Mathf.Max(-64, Mathf.Min(55, Pos.x));
                Pos.z += Mathf.Sin(Theta) * Radius;
                Pos.z = Mathf.Max(-54, Mathf.Min(59, Pos.z));
                Pos.y = 2.5f;
                //敵を生成
                Instantiate(EnemyPrefab, Pos, Quaternion.identity);
                Elapsed = 0.0f;
                currEnemy++;
            }
        }
    }
}
