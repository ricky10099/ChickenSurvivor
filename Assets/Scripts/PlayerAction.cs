using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    float h; //水平軸
    float v; //垂直軸
    Vector3 Dir; //移動方向
    Animator myAnim; //自身のアニメーター

    [SerializeField] float baseSpeed = 15f; //前進速度
    [SerializeField] float rotSpeed = 90.0f; //旋回速度
    [SerializeField] float runSpeed = 1f;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
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

        #if UNITY_EDITOR
            h = Mathf.Min(h, 0.5f);
            h = Mathf.Max(h, -0.5f);
        Debug.Log(h);
        #endif

        Vector3 MoveDir = new Vector3(h, 0, v);
        MoveDir = MoveDir.normalized;

        if (MoveDir.magnitude > 0.1) { 
            Dir = MoveDir; 
        }

        transform.forward = Vector3.Slerp(transform.forward, Dir, Time.deltaTime * rotSpeed);

        MoveDir.y = -rb.mass;
        MoveDir = MoveDir * baseSpeed * runSpeed;

        rb.velocity = MoveDir;
    }
}
