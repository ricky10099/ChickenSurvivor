using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{

    [SerializeField] float baseSpeed = 15f; //前進速度
    [SerializeField] float rotSpeed = 90.0f; //旋回速度
    [SerializeField] float runSpeed = 1f;

    Vector3 Dir; //移動方向
    Animator anim; //自身のアニメーター
    Rigidbody rb;
    BoxCollider headCollider;

    bool isAttack;
    bool isStun;

    // Start is called before the first frame update
    void Start()
    {
        rb =  GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>(); //自身のアニメーターを取得
        headCollider = GetComponentInChildren<BoxCollider>(true);
        isAttack = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStun)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Stun(10);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Eat();
        }

        Movement();
    }

    void Movement()
    {
        if (isAttack)
        {
            return;
        }

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
#endif

        if (Input.GetKey(KeyCode.LeftShift))
        {
            runSpeed = 2;
        }
        else
        {
            runSpeed = 1;
        }

        Vector3 MoveDir = new Vector3(h, 0, v);
        MoveDir = MoveDir.normalized;

        if (MoveDir.magnitude > 0.1)
        {
            Dir = MoveDir;
        }

        MoveDir = MoveDir * baseSpeed * runSpeed;
        anim.SetFloat("Speed", MoveDir.magnitude);
        if (Dir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, Dir, Time.deltaTime * rotSpeed);
        }
        MoveDir.y = -rb.mass;
        rb.velocity = MoveDir;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Fire")
        {
            if (!isStun)
            {
                Stun(20);
            }
        }
    }

    void Eat()
    {
        anim.SetTrigger("Attack");
        anim.SetFloat("Speed", 0);
        isAttack = true;
        rb.velocity = Vector3.zero;
        Invoke("Eating", 0.13f);
    }

    void Eating()
    {
        headCollider.enabled = true;
    }

    void FinishEat()
    {
        isAttack = false;
        headCollider.enabled = false;
        //headCollider.
    }

    void Stun(float force)
    {
        rb.AddForce(-transform.forward * force, ForceMode.VelocityChange);
        isStun = true;
        anim.SetFloat("Speed", 0);
        anim.SetTrigger("Stun");
    }

    void Recover()
    {
        isStun = false;
    }
}
