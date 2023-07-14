using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab; //���e�I�v���n�u
    [SerializeField] float Radian = 100.0f; //�������a
    [SerializeField] float IntervalBias = 0.99f; //�����Ԋu�W��
    [SerializeField] float SpeedBias = 0.1f; //���x�W��
    [SerializeField] float InitInterval = 4.5f; //���������Ԋu
    [SerializeField] float InitSpeed = 5.0f; //�������x
    float Speed; //���x
    float Interval; //�����Ԋu
    float Elapsed; //�o�ߎ���

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
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.GameMode == GameManager.MODE.PLAY)
        {
            Elapsed += Time.deltaTime;
            if (Elapsed > Interval)
            {
                //�����ʒu�����߂�
                Vector3 Pos = Vector3.zero;
                float Theta = Random.Range(0, Mathf.PI * 2.0f);
                Pos.x = Mathf.Cos(Theta) * Radian;
                Pos.z = Mathf.Sin(Theta) * Radian;
                //���e�I�𐶐�
                GameObject M = Instantiate(EnemyPrefab, Pos, Random.rotation);
                //�����ʒu�̋t�x�N�g����i�s�����Ƃ��ė^����
                M.GetComponent<Rigidbody>().velocity = -M.transform.position.normalized * Speed;
                Interval *= IntervalBias; //���̐����Ԋu��Z������
                Speed += SpeedBias;//���̑��x���グ��
                Elapsed = 0.0f;
            }
        }
    }
}
