using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfSpawner : MonoBehaviour
{
    [SerializeField] GameObject wolfPrefab;
    [SerializeField] GameObject wolfSpawnPoint;

    GameObject player;
    GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
            wolfPrefab = Instantiate(wolfPrefab, wolfSpawnPoint.transform.position, wolfSpawnPoint.transform.rotation);
            wolfPrefab.SetActive(false);
    }

    void Ready()
    {
        if(GameManager.difficulty == GameManager.DIFFICULTY.HARD) {
            if (!wolfPrefab.activeInHierarchy)
                wolfPrefab.SetActive(true);
        }
        else { 
            if (wolfPrefab.activeInHierarchy) { 
                wolfPrefab.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (GameManager.GameMode)
        {
            case GameManager.MODE.TITLE:
                Ready();
                break;
            case GameManager.MODE.PLAY:
                if(GameManager.difficulty == GameManager.DIFFICULTY.HARD)
                {
                    PlayAction();
                }
                break;
        }
    }

    void PlayAction()
    {
        if (!wolfPrefab.activeInHierarchy) { 
            wolfPrefab.SetActive(true);
        }
    }
}
