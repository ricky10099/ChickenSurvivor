using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] float AccelBias = 2.0f; //iPadの傾き係数
    [SerializeField] float RotateBias = 3.0f; //回転係数

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
        //入力方向を向く
        transform.forward = Vector3.Slerp(transform.forward, //現在の向き
                                            Dir, //本来、到着したい向き
                                            Time.deltaTime * RotateBias); //その差分を0.0～１.0で指定

        Vector3 Dir2 = new Vector3(h, -1, v);
        Physics.gravity = 9.81f * Dir2.normalized; //重力方向を変位
        //transform.position = Vector3.Lerp(Pos, //現在のプレイヤーの位置
        //new Vector3(MoveBall.transform.position.x, Pos.y, Pos.z), //行きたいボールの位置
        //Time.deltaTime * MoveBias); //マイルドに動く係数
    }
}
