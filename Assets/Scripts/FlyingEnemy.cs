using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


[RequireComponent(typeof(AudioSource))]
//implements the iAnimatableObject interface to allow animations to be triggered
public class FlyingEnemy : MonoBehaviour, iAnimatableObject
{
    [SerializeField] private AudioClip flyingSound;
    [SerializeField] private AudioClip takeDamageSound;
    [SerializeField] private AudioClip hitGroundSound;
    [SerializeField] private AudioClip dyingSound;

    private ObjectAudioManager audioManager;

    Animator animator;
    Rigidbody myRB;

    //required by the interface
    public void DoAnimations()
    {
        DoAnimations(0); //death
    }

    public void DoAnimations(int health)
    {
        if (health <= 0)
        {
            //play death animation

            //stop animating
            animator.SetTrigger("falling");
            //enable physics to drop to ground - ground will trigger death animation
            myRB.isKinematic = false;
            myRB.useGravity = true;

        }
        else
        {
            //play damage animation
            animator.SetTrigger("damage");
        }
    }

  

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.transform.tag)
        {
            case "Ground":
                PlayHitGroundSound();
                myRB.isKinematic = true;
                animator.SetTrigger("dead");
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //get reference to my animator controller
        animator = GetComponent<Animator>();
        myRB = GetComponent<Rigidbody>();
        audioManager = new ObjectAudioManager(GetComponent<AudioSource>());
        myRB.isKinematic = true;
        myRB.useGravity = false;

    }

    public void PlayFlapSound()
    {
        if (flyingSound != null) audioManager.playClip(flyingSound, true, false);
    }

    public void PlayDamageSound()
    {
        if (takeDamageSound != null) audioManager.playClip(takeDamageSound, true, false);
    }

    public void PlayHitGroundSound()
    {
        if (hitGroundSound != null) audioManager.playClip(hitGroundSound, true, false);
    }

    public void PlayDeathSound()
    {
        if (dyingSound != null) audioManager.playClip(dyingSound, true, false);
    }
}
