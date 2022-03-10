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
    [SerializeField] private GameObject inPlayReSpawnPoint;
    private Transform originParent;
    private Vector3 originPosition;
    private Quaternion originRotation;
    private float timeLastShot = 0f;
    private int targetLayerMask = 1 << 8;
    private bool wasPreviouslyHeld = false;
    [SerializeField] private float respawnDelay = 0.75f;

    

    // Start is called before the first frame update
    void Start()
    {
        //subscribe to the game restart event
        GameSystem.Instance.onGameRestart += ResetGun;

        originParent = this.transform.parent;
        originPosition = this.transform.localPosition;
        originRotation = this.transform.rotation;
        interactable = GetComponent<Interactable>();
        myAudio = GetComponent<AudioSource>();
        debugLine.enabled = false;
    }

    private void ResetGun()
    {
        StopAllCoroutines();
        wasPreviouslyHeld = false;
        this.transform.SetParent(originParent);
        this.transform.localPosition = originPosition;
        this.transform.rotation = originRotation;
    }

    IEnumerator ReturnToSpawnPoint()
    {
        float timeElapsed = 0;

        while(timeElapsed < respawnDelay)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        this.transform.SetParent(inPlayReSpawnPoint.transform);
        this.transform.localPosition = Vector3.zero;
        this.transform.Rotate(new Vector3(90, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        bool shoot = false;

        if(interactable.attachedToHand)
        {
            //stop returning to respawn point if it was
            if (!wasPreviouslyHeld) StopCoroutine(ReturnToSpawnPoint());
            //set flag to detect when not being held anymore
            wasPreviouslyHeld = true;
            Debug.Log("Gun attached");
            SteamVR_Input_Sources hand = interactable.attachedToHand.handType;
            shoot = actionShoot.GetState(hand);
            Debug.Log("Shoot state: " + shoot.ToString());
        }
        else
        {
            //if was previously held, then must have dropped it as not attached to hand anymore
            if(wasPreviouslyHeld)
            {
                //no longer being held start short debounce timer and move to in play spawn  point
                StartCoroutine(ReturnToSpawnPoint());
                wasPreviouslyHeld = false;
            }
        }

        timeLastShot += Time.deltaTime;

        if (shoot && (timeLastShot >= debounceTimeThreshold))
        {

            timeLastShot = 0f;

            StartCoroutine(Shoot());

            RaycastHit hit;

            debugLine.SetPosition(0, barrelEndPosition.position);
            if (Physics.Raycast(barrelEndPosition.position, barrelEndPosition.forward, out hit, weaponRange, targetLayerMask))
            {
                debugLine.SetPosition(1, hit.point);
                hit.transform.gameObject.GetComponentInChildren<IShootable>().DoDamage();
                
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
