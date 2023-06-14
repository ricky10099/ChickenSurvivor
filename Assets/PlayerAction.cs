using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] float AccelBias = 2.0f;
    [SerializeField] float RotateBias = 3.0f; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float v;
        float h;
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            h = Input.acceleration.x * AccelBias;
            v = Input.acceleration.y * AccelBias;
            
        }
        else
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }

        Vector3 Dir = new Vector3(h, 0, v);
        transform.forward = Vector3.Slerp(transform.forward, Dir, Time.deltaTime * RotateBias); 

        // if(v > 0)
        // transform.position += transform.forward * AccelBias;
        Vector3 Dir2 = new Vector3(h, -1, v);
        Physics.gravity = 9.81f * Dir2.normalized;
    }
}
