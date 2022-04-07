//Author: Craig Zeki
//Student ID: zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour
{
    //Gun settings
    [SerializeField] private float debounceTimeThreshold = 0.5f;
    [SerializeField] private float weaponRange = 100.0f;
    [SerializeField] private int damageDone = 10;
    [SerializeField] private Transform barrelEndPosition;
    [SerializeField] private LineRenderer shotLine;
    [SerializeField] private GameObject inPlayReSpawnPoint;
    [SerializeField] private float respawnDelay = 0.75f;
    [SerializeField] private int fullAmmo = 6;
    [SerializeField] private TextMeshPro ammoText;

    [SerializeField] private AudioClip gunFire;
    [SerializeField] private AudioClip gunEmpty;
    [SerializeField] private AudioClip gunReload;


    //SteamVR input
    [SerializeField] private SteamVR_Action_Boolean actionShoot = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Shooter", "Shoot");

    //Audio source
    private AudioSource myAudio;

    //Interaction and save of location
    private Interactable interactable;
    private Transform originParent;
    private Vector3 originPosition;
    private Quaternion originRotation;
    private bool wasPreviouslyHeld = false;

    //shot delay
    private float timeLastShot = 0f;

    //ammo
    private int ammo;

    // Start is called before the first frame update
    void Start()
    {
        //subscribe to the game restart event
        //No longer used, was intended to be used when Game Reset was called - but now scene is just reloaded
        //GameSystem.Instance.onGameRestart += ResetGun;

        //setup the position save info
        originParent = this.transform.parent;
        originPosition = this.transform.localPosition;
        originRotation = this.transform.rotation;
        interactable = GetComponent<Interactable>();

        //setup audio
        myAudio = GetComponent<AudioSource>();

        //setup the 'bullet' line
        shotLine.enabled = false;

        ammo = fullAmmo;
        //refresh the ammo text
        updateAmmoText(ammo);
    }


    private void updateAmmoText(int ammo)
    {
        ammoText.text = ammo.ToString();
    }

    //No longer used, was intended to be used when Game Reset was called - but now scene is just reloaded
    //private void ResetGun()
    //{
    //    StopAllCoroutines();
    //    wasPreviouslyHeld = false;
    //    this.transform.SetParent(originParent);
    //    this.transform.localPosition = originPosition;
    //    this.transform.rotation = originRotation;
    //}

    //Return the gun to a set position - used so that the gun can be returned to somewhere close to the player if dropped
    //called as soon as gun leaves the hand
    IEnumerator ReturnToSpawnPoint()
    {
        float timeElapsed = 0;

        while (timeElapsed < respawnDelay)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        this.transform.SetParent(inPlayReSpawnPoint.transform);
        this.transform.localPosition = Vector3.zero;
        this.transform.Rotate(new Vector3(90, 0, 0));
    }

    void Update()
    {
        bool shoot = false;

        if (interactable.attachedToHand)
        {
            //stop returning to respawn point if it was
            //allows for ability to switch hands
            if (!wasPreviouslyHeld) StopCoroutine(ReturnToSpawnPoint());
            //set flag to detect when not being held anymore
            wasPreviouslyHeld = true;
            //Debug.Log("Gun attached");

            //get the state of the trigger button on the controller holding the gun
            SteamVR_Input_Sources hand = interactable.attachedToHand.handType;
            shoot = actionShoot.GetState(hand);

            //Debug.Log("Shoot state: " + shoot.ToString());
        }
        else
        {
            //if was previously held, then must have dropped it as not attached to hand anymore
            if (wasPreviouslyHeld)
            {
                //no longer being held start short debounce timer and move to in play spawn  point
                StartCoroutine(ReturnToSpawnPoint());
                wasPreviouslyHeld = false;
            }
        }

        //timer to prevent continuous firing
        timeLastShot += Time.deltaTime;

        //if trigger pulled on controller and allowed to fire (time delay OK)
        // could improve by removing timer and testing if the coroutine is not running
        if (shoot && (timeLastShot >= debounceTimeThreshold))
        {
            timeLastShot = 0f;
            if (ammo > 0)
            {
                ammo--;
                //refresh the ammo text
                updateAmmoText(ammo);

                //start the shooting 'actions' (audio and bullet line draw)
                StartCoroutine(Shoot());

                //Move everything below here to Shoot function?

                //set the bullet line start position
                shotLine.SetPosition(0, barrelEndPosition.position);


                //Test if something was hit
                RaycastHit hit;
                if (Physics.Raycast(barrelEndPosition.position, barrelEndPosition.forward, out hit, weaponRange))
                {
                    //set the bullet line end position to whatever was hit
                    shotLine.SetPosition(1, hit.point);

                    //Test if the object hit implements the iShootable interface, if so, lets call its Damage routine
                    if (hit.transform.gameObject.GetComponent<IShootable>() != null)
                    {
                        hit.transform.gameObject.GetComponent<IShootable>().DoDamage(damageDone);
                    }

                }
                else
                {
                    //Have not hit anything within weaponRange, set the line end position to the range of the weapon
                    shotLine.SetPosition(1, barrelEndPosition.position + (barrelEndPosition.forward * weaponRange));
                }
            }
            else
            {

                //play empty gun sound
                PlayAudio(gunEmpty);
            }

        }


        if(ammo < fullAmmo)
        {
            //Test if we try to reload
            RaycastHit[] reloadHits;
            reloadHits = Physics.RaycastAll(barrelEndPosition.position, barrelEndPosition.forward, 3.0f);
            foreach (RaycastHit reloadHit in reloadHits)
            {
                //Test if we hit the reload pad
                if (reloadHit.transform.gameObject.tag == "ReloadPad")
                {
                    //reload the gun
                    ammo = fullAmmo;
                    updateAmmoText(ammo);
                    PlayAudio(gunReload);
                }
            }
        }
        




    }

    private void PlayAudio(AudioClip clip)
    {
        myAudio.Stop();
        myAudio.clip = clip;
        myAudio.loop = false;
        myAudio.Play();
    }

    //Shoot the gun
    private IEnumerator Shoot()
    {
        //Debug.Log("Shoot");
        // play the 'bang' sound
        PlayAudio(gunFire);

        //allow the bullet line to be drawn
        shotLine.enabled = true;

        //wait the timeout to prevent multiple / continuous shots
        yield return debounceTimeThreshold;

        //disable the bullet line
        shotLine.enabled = false;
    }

    void OnShoot()
    {
        Debug.Log("Shoot pressed");
    }
}
