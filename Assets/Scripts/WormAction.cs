using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WormAction : MonoBehaviour
{
    [SerializeField]GameObject starEffect;
    [SerializeField]GameObject model;

    NavMeshAgent agent;
    GameObject player;
    GameObject gameManager;
    Animator anim;
    float baseSpeed;

    public bool IsModelOn{get{return model.activeSelf;}}

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        baseSpeed = 3.5f;
        gameManager = GameObject.FindGameObjectWithTag("GameController");
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.GameMode)
        {
            case GameManager.MODE.PLAY:
                float distance = Vector3.Distance(transform.position, player.transform.position);

                if (distance < 10)
                {
                    Vector3 dir = transform.position - player.transform.position;

                    Vector3 newPos = transform.position + dir;
                    agent.speed = baseSpeed * GameManager.WormLevel;
                    agent.SetDestination(newPos);
                    anim.SetBool("Walk", true);
                    anim.speed = baseSpeed * GameManager.WormLevel;
                }
                else
                {
                    anim.SetBool("Walk", false);
                    anim.speed = 1;
                }

                if(transform.position.y < -0.47)
                {
                    Debug.Log("-0.47");
                    Reposition();
                }
                break;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Head")
        {
            starEffect.SetActive(true);
            model.SetActive(false);
            gameManager.SendMessage("WormEaten", SendMessageOptions.DontRequireReceiver);
        }

        if (other.name == "Tent")
        {
            Reposition();
        }
    }

    public void ResetObject()
    {
        starEffect.SetActive(false);
        model.SetActive(true);
    }

    void Reposition()
    {
        Vector3 Pos = player.transform.position;
        float Theta = Random.Range(0, Mathf.PI * 2.0f);
        float Radius = Random.Range(10, 20);
        Pos.x += Mathf.Cos(Theta) * Radius;
        Pos.x = Mathf.Max(-64, Mathf.Min(55, Pos.x));
        Pos.z += Mathf.Sin(Theta) * Radius;
        Pos.z = Mathf.Max(-54, Mathf.Min(59, Pos.z));
        Pos.y = 2.5f;

        agent.enabled = false;
        transform.position = Pos;
        agent.enabled = true;
    }
}
