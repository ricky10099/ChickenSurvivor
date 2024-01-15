using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] float walkSpeed = 15f; //前進速度
    [SerializeField] float rotSpeed = 90.0f; //旋回速度
    [SerializeField] float runSpeed = 30f;
    [SerializeField] Button runButton;
    [SerializeField] ParticleSystem runEffect;
    [SerializeField] Image arrow;
    [SerializeField] GameObject head;

    public bool IsStun{ get{ return isStun;} }

    Vector3 titlePos = new Vector3(0, 0, -22);
    Vector3 titleDir = new Vector3(0, 141, 0);
    Vector3 dir; //移動方向
    Animator anim; //自身のアニメーター
    Rigidbody rb;
    float runGauge = 100;
    float elapsed = 0;
    bool isAttack;
    bool isRun;
    bool isRecoverStemina;
    bool isStun;
    AudioSource audioSrc;
    BoxCollider headCollider;

    // Start is called before the first frame update
    void Start()
    {
        rb =  GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>(); //自身のアニメーターを取得
        headCollider = head.GetComponent<BoxCollider>();
        audioSrc = GetComponent<AudioSource>();
        isAttack = false;
        anim.SetBool("isTitle", true);
        runEffect.Stop(true);
        isStun = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.GameMode)
        {
            
            case GameManager.MODE.TITLE:
                SetTitle();
                break;
            case GameManager.MODE.OVER:
                FinishGame();
                break;
            case GameManager.MODE.TITLE_TO_PLAY:
                ReadyAction();
                break;
            case GameManager.MODE.PLAY:
                PlayModeAction();
                break;
        }
    }

    void FinishGame()
    {
        anim.SetBool("isTitle", true);
        elapsed = 0;
    }

    public void SetTitle()
    {
        transform.position = titlePos;
        transform.rotation = Quaternion.Euler(titleDir);
    }

    void ReadyAction()
    {
        elapsed += Time.deltaTime;
        anim.SetBool("isTitle", false);
        transform.forward = Vector3.Slerp(transform.forward, new Vector3(0, 0, 1), elapsed / 1);
    }

    void PlayModeAction()
    {
        runGauge = Math.Max(0, runGauge);
        if (runGauge <= 0)
        {
            Walk();
        }

        if (runGauge >= 100 || isRun)
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

        Vector3 moveDir = new Vector3(h, 0, v);
        moveDir = moveDir.normalized;

        if (moveDir.magnitude > 0.1)
        {
            dir = moveDir;
        }

        moveDir = moveDir * (isRun? runSpeed : walkSpeed);
        if(moveDir.magnitude > 0 && isRun)
        {
            runGauge -= 50 * Time.deltaTime;
        }

        anim.SetFloat("Speed", moveDir.magnitude);
        if (dir != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, dir, Time.deltaTime * rotSpeed);
        }
        moveDir.y = -rb.mass;
        rb.velocity = moveDir;
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
            runEffect.Play();
        }
    }

    public void Walk()
    {
        isRun = false;
        Invoke("RecoverStemina", 0.5f);
        if (!runEffect.isStopped) { 
            runEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    public void Eat()
    {
        if(isAttack || isStun)
        {
            return;
        }
        audioSrc.Play();
        anim.SetTrigger("Attack");
        anim.SetFloat("Speed", 0);
        isAttack = true;
        rb.velocity = Vector3.zero;
        if (GameManager.difficulty == GameManager.DIFFICULTY.EASY)
        {
            head.transform.localScale = new Vector3(3f, 3f, 3f);
        }
        else
        {
            head.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        }
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
        head.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void Stun(float force)
    {
        Stun(-transform.forward, force);
    }

    public void Stun(Vector3 Direction, float force)
    {
        rb.AddForce(Direction * force, ForceMode.VelocityChange);
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
