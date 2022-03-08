using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


public class Gun : MonoBehaviour
{
    public SteamVR_Action_Boolean actionShoot = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Shooter", "Shoot");
    private AudioSource myAudio;

    private Interactable interactable;

    [SerializeField] private float debounceTimeThreshold = 0.5f;
    [SerializeField] private float weaponRange = 100.0f;
    [SerializeField] private Transform barrelEndPosition;
    [SerializeField] private LineRenderer debugLine;
    private float timeLastShot = 0f;
    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<Interactable>();
        myAudio = GetComponent<AudioSource>();
        debugLine.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool shoot = false;

        if(interactable.attachedToHand)
        {
            Debug.Log("Gun attached");
            SteamVR_Input_Sources hand = interactable.attachedToHand.handType;
            shoot = actionShoot.GetState(hand);
            Debug.Log("Shoot state: " + shoot.ToString());
        }

        timeLastShot += Time.deltaTime;

        if (shoot && (timeLastShot >= debounceTimeThreshold))
        {

            timeLastShot = 0f;

            StartCoroutine(Shoot());

            RaycastHit hit;

            debugLine.SetPosition(0, barrelEndPosition.position);
            if (Physics.Raycast(barrelEndPosition.position, barrelEndPosition.forward, out hit, weaponRange))
            {
                debugLine.SetPosition(1, hit.point);
                
            }
            else
            {
                debugLine.SetPosition(1, barrelEndPosition.position + (barrelEndPosition.forward * weaponRange));
            }
        }

    }

    private IEnumerator Shoot()
    {
        Debug.Log("Shoot");
        myAudio.Play();
        debugLine.enabled = true;

        yield return debounceTimeThreshold;
        debugLine.enabled = false;
    }

    void OnShoot()
    {
        Debug.Log("Shoot pressed");
    }
}
