using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Barrel : MonoBehaviour, iAnimatableObject
{
    Animator animator;
    [SerializeField] BalloonSpawner balloonSpawner;
    //bool isOpen = false;
    void iAnimatableObject.DoAnimations()
    {
        animator.SetBool("isOpen", !animator.GetBool("isOpen"));
        if(animator.GetBool("isOpen"))
        {
            balloonSpawner.SpawnBalloon(Balloon.BalloonColor.Red);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
