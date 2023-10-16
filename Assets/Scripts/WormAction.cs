using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WormAction : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;
    GameObject gameManager;
    float baseSpeed;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        baseSpeed = 3.5f;
        gameManager = GameObject.FindGameObjectWithTag("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        
        if(distance < 10)
        {
            Vector3 dir = transform.position - player.transform.position;

            Vector3 newPos = transform.position + dir;
            agent.speed = baseSpeed * GameManager.WormLevel;
            agent.SetDestination(newPos);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Head")
        {
            Destroy(gameObject);
            gameManager.SendMessage("WormSpeedUp", SendMessageOptions.DontRequireReceiver);
        }

        if (other.name == "Tent")
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

}
