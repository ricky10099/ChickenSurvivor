using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    [SerializeField] Vector3 OffSet = new Vector3(0f, 0f, 0f); 
    [SerializeField] Vector3 CamDir = new Vector3(0f, 25f, -15f); 
    GameObject P; //プレイヤー

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
            P = GameObject.FindGameObjectWithTag("Player");
            return; //プレイヤー不在なら動かさない。
        }
        //カメラの位置。プレイヤー位置から算出する。
        transform.position = P.transform.position + CamDir;
        //カメラの回転。注視点はプレイヤーの少し上を見る。
        transform.LookAt(P.transform.position + OffSet);
    }
}
