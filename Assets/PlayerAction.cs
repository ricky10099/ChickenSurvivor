using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] float AccelBias = 2.0f; //iPad�̌X���W��
    [SerializeField] float RotateBias = 3.0f; //��]�W��

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float v;
        float h;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            h = Input.acceleration.x * AccelBias;
            v = Input.acceleration.y * AccelBias;
            
        }
        else
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }

        Vector3 Dir = new Vector3(h, 0, v);
        //���͕���������
        transform.forward = Vector3.Slerp(transform.forward, //���݂̌���
                                            Dir, //�{���A��������������
                                            Time.deltaTime * RotateBias); //���̍�����0.0�`�P.0�Ŏw��

        Vector3 Dir2 = new Vector3(h, -1, v);
        Physics.gravity = 9.81f * Dir2.normalized; //�d�͕�����ψ�
        //transform.position = Vector3.Lerp(Pos, //���݂̃v���C���[�̈ʒu
        //new Vector3(MoveBall.transform.position.x, Pos.y, Pos.z), //�s�������{�[���̈ʒu
        //Time.deltaTime * MoveBias); //�}�C���h�ɓ����W��
    }
}
