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

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();

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
                    Vector3 dir = player.transform.position - transform.position;

                    Vector3 newPos = transform.position + dir;
                    agent.speed = 2f;
                    agent.SetDestination(newPos);
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
