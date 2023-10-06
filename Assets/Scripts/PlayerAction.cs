using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    float h; //������
    float v; //������
    Vector3 Dir; //�ړ�����
    Animator myAnim; //���g�̃A�j���[�^�[
    [SerializeField] float foreSpeed = 15f; //�O�i���x
    [SerializeField] float rotSpeed = 90.0f; //���񑬓x
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float v;
        float h;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            h = Input.acceleration.x;
            v = Input.acceleration.y;

        }
        else
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }

        //h = Input.GetAxis("Horizontal"); //���̓f�o�C�X�̐�����
        //v = Input.GetAxis("Vertical"); //���̓f�o�C�X�̐�����
        Debug.Log(h);
        Vector3 MoveDir = new Vector3(h, 0, v);
        MoveDir = MoveDir.normalized;
        MoveDir *= foreSpeed;
        if(MoveDir != Vector3.zero)
            Dir = MoveDir;
        transform.forward = Vector3.Slerp(transform.forward, Dir, Time.deltaTime * rotSpeed);
        transform.localPosition += MoveDir * Time.deltaTime; //�L�����N�^�[���ړ�
    }
}
