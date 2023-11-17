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
    //[SerializeField] Image arrow;

    GameObject[] worms;
    Collider[] wormColliders;
    Camera cam;
    Plane[] planes;

    AudioSource audioSrc;
    Vector3 titlePos = new Vector3(0, 0, -22);
    Vector3 titleDir = new Vector3(0, 141, 0);
    Vector3 dir; //移動方向
    Animator anim; //自身のアニメーター
    Rigidbody rb;
    BoxCollider headCollider;
    float runGauge = 100;
    float elapsed = 0;

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
        audioSrc = GetComponent<AudioSource>();
        isAttack = false;
        anim.SetBool("isTitle", true);

        cam = Camera.main;
        worms = GameObject.FindGameObjectsWithTag("Worm");
        wormColliders = new Collider[worms.Length];
        for(int i = 0; i < wormColliders.Length; i++)
        {
            wormColliders[i] = worms[i].GetComponentInChildren<Collider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if(wormColliders.Length == 0)
        {
            worms = GameObject.FindGameObjectsWithTag("Worm");
            wormColliders = new Collider[worms.Length];
            for (int i = 0; i < wormColliders.Length; i++)
            {
                wormColliders[i] = worms[i].GetComponentInChildren<Collider>();
            }
        }

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
        //Vector3 dir;
        //if (worms.Length != 0) { 
        //    ShowIndicator();
        //    dir = worms[0].transform.position - transform.position;
        //    float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        //    arrow.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
        //}

        for (int i = 0; i < wormColliders.Length; i++)
        {
            if (GeometryUtility.TestPlanesAABB(planes, wormColliders[i].bounds))
            {
                //Debug.Log(worms[i].name + " has been detected!");
            }
            else
            {
                //Debug.Log(worms[i].name + " has not been detected");
            }
        }

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
        //Debug.Log(isRun ? runSpeed : walkSpeed);
        //Debug.Log(runSpeed);
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
        }
    }

    public void Walk()
    {
        isRun = false;
        Invoke("RecoverStemina", 0.5f);
    }

    public void Eat()
    {
        audioSrc.Play();
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
    }

    public void Stun(float force)
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
