using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] float walkSpeed = 15f; //前進速度
    [SerializeField] float rotSpeed = 90.0f; //旋回速度
    [SerializeField] float runSpeed = 30f;
    [SerializeField] Button runButton;

    Vector3 Dir; //移動方向
    Animator anim; //自身のアニメーター
    Rigidbody rb;
    BoxCollider headCollider;
    float runGauge = 100;

    bool isAttack;
    bool isStun;
    bool isRun;
    bool isRecoverStemina;

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
        if(GameManager.GameMode != GameManager.MODE.PLAY)
        {
            return;
        }

        runGauge = Math.Max(0, runGauge);
        if(runGauge <= 0)
        {
            Walk();
        }

        if(runGauge >= 100 || isRun)
        {
            isRecoverStemina = false;
        }

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

        if (isRecoverStemina)
        {
            runGauge += 30 * Time.deltaTime;
        }

        runButton.image.fillAmount = runGauge / 100;
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

        Vector3 MoveDir = new Vector3(h, 0, v);
        MoveDir = MoveDir.normalized;

        if (MoveDir.magnitude > 0.1)
        {
            Dir = MoveDir;
        }
        //Debug.Log(isRun ? runSpeed : walkSpeed);
        //Debug.Log(runSpeed);
        MoveDir = MoveDir * (isRun? runSpeed : walkSpeed);
        if(MoveDir.magnitude > 0 && isRun)
        {
            runGauge -= 70 * Time.deltaTime;
        }

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
                Stun((isRun ? (runSpeed * 1.1f): (walkSpeed * 1.2f)) );
            }
        }
    }

    public void Run()
    {
        if (runGauge > 0)
        {
            isRun = true;
            isRecoverStemina = false;
        }
    }

    public void Walk()
    {
        isRun = false;
        Invoke("RecoverStemina", 0.5f);
    }

    public void Eat()
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

    void RecoverStemina()
    {
        isRecoverStemina = true;
    }
}
