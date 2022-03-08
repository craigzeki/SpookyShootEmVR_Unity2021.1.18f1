using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] GameObject playerRoot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Collision Enter");
        if(other.tag == "MovingPlatform")
        {
            //Debug.Log("Collision Enter with MovingPlatform");
            playerRoot.transform.SetParent(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
       // Debug.Log("Collision Exit");
        if (other.tag == "MovingPlatform")
        {
           // Debug.Log("Collision Exited with MovingPlatform");
            playerRoot.transform.SetParent(null);
        }
    }
}
