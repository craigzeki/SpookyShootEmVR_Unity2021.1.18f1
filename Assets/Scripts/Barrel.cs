//Author: Craig Zeki
//Student ID: zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

//implements the iAnimatableObject interface to allow animations to be triggered
public class Barrel : MonoBehaviour, iAnimatableObject
{
    //reference to BalloonSpawner script on the BalloonSpawner object
    [SerializeField] BalloonSpawner balloonSpawner;
    
    Animator animator;

    public void DoAnimations(int health)
    {
        //barrel doesn't need to handle health
        DoAnimations();
    }

    //required by the interface
    public void DoAnimations()
    {
        //set the animator controller to toggle open / close state
        animator.SetBool("isOpen", !animator.GetBool("isOpen"));

        //if the animator is now open
        if(animator.GetBool("isOpen"))
        {
            //spawn a balloon
            balloonSpawner.SpawnBalloon(Balloon.BalloonColor.Red);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //get reference to my animator controller
        animator = GetComponent<Animator>();
    }

}
