using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;

    float indicatorDistance = 5;
    float appearanceDistance = 20f;

    private Dictionary<GameObject, GameObject> wormToIndicator = new Dictionary<GameObject, GameObject>();

    GameObject[] worms;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (wormToIndicator.Count < GameManager.maxWorm)
        {
            worms = GameObject.FindGameObjectsWithTag("Worm");

            for (int i = 0; i < worms.Length; i++)
            {
                if (!wormToIndicator.ContainsKey(worms[i]))
                {
                    GameObject newIndicator = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
                    wormToIndicator.Add(worms[i], newIndicator);
                    newIndicator.SetActive(false);
                }
            }
        }

        if(GameManager.GameMode == GameManager.MODE.PLAY)
        {
            foreach (GameObject worm in worms)
            {
                float distance = Vector3.Distance(worm.transform.position, transform.position);

                if (distance < appearanceDistance || !worm.GetComponent<WormAction>().IsModelOn)
                {
                    wormToIndicator[worm].SetActive(false);
                }
                else
                {
                    wormToIndicator[worm].SetActive(true);
                    Vector3 directionToWorm = (worm.transform.position - transform.position).normalized;
                    wormToIndicator[worm].transform.position = transform.position + directionToWorm * indicatorDistance;
                    wormToIndicator[worm].transform.rotation = Quaternion.LookRotation(directionToWorm);
                }
            }
        }
        else
        {
            foreach (GameObject worm in worms)
            {
                wormToIndicator[worm].SetActive(false);
            }
        }
    }
}
