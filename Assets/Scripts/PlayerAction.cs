using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    float h; //水平軸
    float v; //垂直軸
    Vector3 Dir; //移動方向
    Animator myAnim; //自身のアニメーター
    [SerializeField] float foreSpeed = 15f; //前進速度
    [SerializeField] float rotSpeed = 90.0f; //旋回速度
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

        //h = Input.GetAxis("Horizontal"); //入力デバイスの水平軸
        //v = Input.GetAxis("Vertical"); //入力デバイスの垂直軸
        Debug.Log(h);
        Vector3 MoveDir = new Vector3(h, 0, v);
        MoveDir = MoveDir.normalized;
        MoveDir *= foreSpeed;
        if(MoveDir != Vector3.zero)
            Dir = MoveDir;
        transform.forward = Vector3.Slerp(transform.forward, Dir, Time.deltaTime * rotSpeed);
        transform.localPosition += MoveDir * Time.deltaTime; //キャラクターを移動
    }
}
