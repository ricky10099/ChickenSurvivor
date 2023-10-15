using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab; //メテオプレハブ
    [SerializeField] float MaxRadius = 10.0f; //生成半径
    [SerializeField] float IntervalBias = 0.99f; //生成間隔係数
    [SerializeField] float SpeedBias = 0.1f; //速度係数
    [SerializeField] float InitInterval = 3.0f; //初期生成間隔
    [SerializeField] float InitSpeed = 5.0f; //初期速度
    float Speed; //速度
    float Interval; //生成間隔
    float Elapsed; //経過時間

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
        Speed = InitSpeed;
        //前プレイでのメテオが残っていれば撤去
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject Stored in Enemies)
        {
            Destroy(Stored);
        }

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameMode == GameManager.MODE.PLAY)
        {
            Elapsed += Time.deltaTime;
            if (Elapsed > Interval)
            {
                Debug.Log("SpawnEnemy");
                //生成位置を求める
                Vector3 Pos = player.transform.position;
                float Theta = Random.Range(0, Mathf.PI * 2.0f);
                float Radius = Random.Range(3, MaxRadius);
                Pos.x += Mathf.Cos(Theta) * Radius;
                Pos.z += Mathf.Sin(Theta) * Radius;
                Pos.y = 2.5f;
                //敵を生成
                GameObject M = Instantiate(EnemyPrefab, Pos, Quaternion.identity);
                //生成位置の逆ベクトルを進行方向として与える
                //M.GetComponent<Rigidbody>().velocity = -M.transform.position.normalized * Speed;
                Interval *= IntervalBias; //次の生成間隔を短くする
                Speed += SpeedBias;//次の速度を上げる
                Elapsed = 0.0f;
            }
        }
    }
}
