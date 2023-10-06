using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab; //���e�I�v���n�u
    [SerializeField] float MaxRadius = 10.0f; //�������a
    [SerializeField] float IntervalBias = 0.99f; //�����Ԋu�W��
    [SerializeField] float SpeedBias = 0.1f; //���x�W��
    [SerializeField] float InitInterval = 3.0f; //���������Ԋu
    [SerializeField] float InitSpeed = 5.0f; //�������x
    float Speed; //���x
    float Interval; //�����Ԋu
    float Elapsed; //�o�ߎ���

    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        Ready(); //��������
    }

    //��������
    void Ready()
    {
        Elapsed = 0.0f;
        Interval = InitInterval;
        Speed = InitSpeed;
        //�O�v���C�ł̃��e�I���c���Ă���ΓP��
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
                //�����ʒu�����߂�
                Vector3 Pos = player.transform.position;
                float Theta = Random.Range(0, Mathf.PI * 2.0f);
                float Radius = Random.Range(3, MaxRadius);
                Pos.x += Mathf.Cos(Theta) * Radius;
                Pos.z += Mathf.Sin(Theta) * Radius;
                Pos.y = 2.5f;
                //�G�𐶐�
                GameObject M = Instantiate(EnemyPrefab, Pos, Quaternion.identity);
                //�����ʒu�̋t�x�N�g����i�s�����Ƃ��ė^����
                //M.GetComponent<Rigidbody>().velocity = -M.transform.position.normalized * Speed;
                Interval *= IntervalBias; //���̐����Ԋu��Z������
                Speed += SpeedBias;//���̑��x���グ��
                Elapsed = 0.0f;
            }
        }
    }
}
