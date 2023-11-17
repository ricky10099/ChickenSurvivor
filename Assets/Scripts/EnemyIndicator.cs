using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicator : MonoBehaviour
{
    [SerializeField] GameObject arrowPrefab;

    float indicatorDistance = 5;
    float appearanceDistance = 50f;

    private Dictionary<GameObject, GameObject> projectileToIndicator = new Dictionary<GameObject, GameObject>();

    GameObject[] worms;
    //Collider[] wormColliders;
    Camera cam;
    Plane[] planes;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        worms = GameObject.FindGameObjectsWithTag("Worm");
        //wormColliders = new Collider[worms.Length];
        for (int i = 0; i < worms.Length; i++)
        {
            //wormColliders[i] = worms[i].GetComponentInChildren<Collider>();
            if (!projectileToIndicator.ContainsKey(worms[i]))
            {
                GameObject newIndicator = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
                projectileToIndicator.Add(worms[i], newIndicator);
                newIndicator.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        if (projectileToIndicator.Count == 0)
        {
            worms = GameObject.FindGameObjectsWithTag("Worm");
            //wormColliders = new Collider[worms.Length];
            Debug.Log(worms.Length + " Update");
            for (int i = 0; i < worms.Length; i++)
            {
                //wormColliders[i] = worms[i].GetComponentInChildren<Collider>();
                if (!projectileToIndicator.ContainsKey(worms[i]))
                {
                    GameObject newIndicator = Instantiate(arrowPrefab, Vector3.zero, Quaternion.identity);
                    projectileToIndicator.Add(worms[i], newIndicator);
                    newIndicator.SetActive(false);
                }
            }
        }

        if(GameManager.GameMode == GameManager.MODE.PLAY)
        {
            foreach (GameObject worm in worms)
            {
                if (GeometryUtility.TestPlanesAABB(planes, worm.GetComponentInChildren<Collider>().bounds))
                {
                    projectileToIndicator[worm].SetActive(false);
                }
                else
                {
                    Debug.Log(worms.Length);
                    projectileToIndicator[worm].SetActive(true);
                    Vector3 directionToProjectile = (worm.transform.position - transform.position).normalized;
                    projectileToIndicator[worm].transform.position = transform.position + directionToProjectile * indicatorDistance;
                    projectileToIndicator[worm].transform.rotation = Quaternion.LookRotation(directionToProjectile);
                }
            }
        }
        else
        {
            foreach (GameObject worm in worms)
            {
                projectileToIndicator[worm].SetActive(false);
            }
        }
    }
}
