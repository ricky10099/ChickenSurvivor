using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMask : MonoBehaviour
{
    Vector3 raycastOriginOffset;
    Vector3 raycastTargetOffset;

    float maxMaskSize = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        raycastOriginOffset =  new Vector3(0, 30, -30);
        raycastTargetOffset =  new Vector3(0, 0.2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        //Debug.DrawRay(Camera.main.transform.position + raycastOriginOffset,
        //    transform.parent.gameObject.transform.position + raycastTargetOffset - Camera.main.transform.position + raycastOriginOffset, Color.red);
        if(GameManager.GameMode == GameManager.MODE.PLAY && 
            Physics.Raycast(Camera.main.transform.position + raycastOriginOffset,
            (transform.parent.gameObject.transform.position + raycastTargetOffset - Camera.main.transform.position), 
            out hit, Mathf.Infinity))
        {
            //Debug.Log(hit.collider.gameObject.name);
            if(hit.collider.tag == "Mask" 
                || hit.collider.tag == "Player"
                || hit .collider.tag == "Head")
            {
                if(transform.localScale.x > 0)
                transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            }
            else
            {
                if(transform.localScale.x < maxMaskSize)
                transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);

            }
        }
        else
        {
            transform.localScale = Vector3.zero;
        }
    }
}
