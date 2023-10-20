using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAction : MonoBehaviour
{
    [SerializeField] Vector3 OffSet = new Vector3(0f, 1.4f, 0f); 
    [SerializeField] Vector3 CamDir = new Vector3(0f, 20f, -20f); 
    
    GameObject P; //プレイヤー
    Vector3 titleOffset = new Vector3(0, 4.5f, 0);
    Vector3 titleDir = new Vector3(15, 6, -20);

    float elapsed = 0;
    bool titleCamStandBy;
    // Start is called before the first frame update
    void Start()
    {
        P = GameObject.FindGameObjectWithTag("Player");
        titleCamStandBy = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!P)
        {
            P = GameObject.FindGameObjectWithTag("Player");
            return; //プレイヤー不在なら動かさない。
        }

        switch (GameManager.GameMode)
        {
            case GameManager.MODE.TITLE:
                elapsed = 0;
                if (!titleCamStandBy) { 
                    //カメラの位置。プレイヤー位置から算出する。
                    transform.position = P.transform.position + titleDir;
                    //カメラの回転。注視点はプレイヤーの少し上を見る。
                    transform.LookAt(P.transform.position + titleOffset);
                    titleCamStandBy = true;
                }
                break;
            case GameManager.MODE.TITLE_TO_PLAY:
                titleCamStandBy = false;
                elapsed += Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, P.transform.position + CamDir, elapsed / 1);
                transform.LookAt(Vector3.Lerp(transform.position, P.transform.position + OffSet, elapsed / 1));
                break;
            case GameManager.MODE.PLAY:
                //カメラの位置。プレイヤー位置から算出する。
                transform.position = P.transform.position + CamDir;
                //カメラの回転。注視点はプレイヤーの少し上を見る。
                transform.LookAt(P.transform.position + OffSet);
                break;
            case GameManager.MODE.OVER:
                RotateCamera();
                break;
        }
    }

    void RotateCamera()
    {
        elapsed += Time.deltaTime;
        float posX = P.transform.position.x + 30 * Mathf.Sin(elapsed / 3);
        float posZ = P.transform.position.z + 30 * Mathf.Cos(elapsed / 3);
        transform.position = new Vector3(posX, 6, posZ);
        transform.LookAt(P.transform.position);
    }
}
