using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab; //メテオプレハブ
    [SerializeField] float Radian = 100.0f; //生成半径
    [SerializeField] float IntervalBias = 0.99f; //生成間隔係数
    [SerializeField] float SpeedBias = 0.1f; //速度係数
    [SerializeField] float InitInterval = 4.5f; //初期生成間隔
    [SerializeField] float InitSpeed = 5.0f; //初期速度
    float Speed; //速度
    float Interval; //生成間隔
    float Elapsed; //経過時間

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
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameMode == GameManager.MODE.PLAY)
        {
            Elapsed += Time.deltaTime;
            if (Elapsed > Interval)
            {
                //生成位置を求める
                Vector3 Pos = Vector3.zero;
                float Theta = Random.Range(0, Mathf.PI * 2.0f);
                Pos.x = Mathf.Cos(Theta) * Radian;
                Pos.z = Mathf.Sin(Theta) * Radian;
                //メテオを生成
                GameObject M = Instantiate(EnemyPrefab, Pos, Random.rotation);
                //生成位置の逆ベクトルを進行方向として与える
                M.GetComponent<Rigidbody>().velocity = -M.transform.position.normalized * Speed;
                Interval *= IntervalBias; //次の生成間隔を短くする
                Speed += SpeedBias;//次の速度を上げる
                Elapsed = 0.0f;
            }
        }
    }
}
