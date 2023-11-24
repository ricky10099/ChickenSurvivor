using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WolfAction : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    GameObject gameManager;
    float baseSpeed;
    const float attackCoolDown = 5;
    float attackTimer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        attackTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.GameMode)
        {
            case GameManager.MODE.PLAY:
                PlayAction();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            PlayerAction player = other.gameObject.GetComponent<PlayerAction>();
            Debug.Log(player.IsStun);
            if (!player.IsStun)
            {
                Debug.Log("attack");
                Vector3 dir = other.transform.position - transform.position;
                dir.y = 0;
                player.Stun(dir, 3);
                attackTimer = attackCoolDown;
            }
        }
    }

    void PlayAction()
    {
        attackTimer -= Time.deltaTime;
        attackTimer = Mathf.Max(0, attackTimer);

        float distance = Vector3.Distance(transform.position, player.transform.position);
        Debug.Log(attackTimer);
        if(attackTimer <= 0) { 
            if (distance < 15)
            {
                Vector3 dir = player.transform.position - transform.position;

                Vector3 newPos = transform.position + dir;
                agent.speed = 10f;
                agent.SetDestination(newPos);
            }
        }
    }
}
