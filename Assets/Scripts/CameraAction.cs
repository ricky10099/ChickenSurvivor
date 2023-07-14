using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    [SerializeField] Vector3 OffSet = new Vector3(0f, 1.4f, 0f); //�ޏ��̌����t��
    [SerializeField] Vector3 CamDir = new Vector3(0f, 13f, -13f); //�ޏ����猩���J��������
    GameObject P; //�v���C���[

    // Start is called before the first frame update
    void Start()
    {
        P = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!P)
        {
            return; //�v���C���[�s�݂Ȃ瓮�����Ȃ��B
        }
        //�J�����̈ʒu�B�v���C���[�ʒu����Z�o����B
        transform.position = P.transform.position + CamDir;
        //�J�����̉�]�B�����_�̓v���C���[�̏����������B
        transform.LookAt(P.transform.position + OffSet);
    }
}
