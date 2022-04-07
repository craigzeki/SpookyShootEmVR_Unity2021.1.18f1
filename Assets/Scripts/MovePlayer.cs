using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] GameObject playerRoot;
    [SerializeField] GameObject UserWarningMessage;
    // Start is called before the first frame update
    void Start()
    {
        UserWarningMessage.SetActive(false);
        
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
            other.GetComponentInParent<MoveCart>().StartMoving();
            UserWarningMessage.SetActive(false);
            SteamVR_Fade.Start(Color.clear, 1);
            BGAudioManager.Instance.PlayGameMusic();
        }

        if(other.gameObject.GetComponentInChildren<iTeleportPointUnlocker>() != null)
        {
            other.gameObject.GetComponentInChildren<iTeleportPointUnlocker>().UnlockTeleportPoint();
        }

    }

    private void OnTriggerExit(Collider other)
    {
       // Debug.Log("Collision Exit");
        if (other.tag == "MovingPlatform")
        {
            // Debug.Log("Collision Exited with MovingPlatform");
            other.GetComponentInParent<MoveCart>().StopMoving();
            playerRoot.transform.SetParent(null);
            UserWarningMessage.SetActive(true);
            SteamVR_Fade.Start(Color.black, 1);

        }
    }

}
