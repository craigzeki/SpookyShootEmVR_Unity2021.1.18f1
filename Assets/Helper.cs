using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour, iAnimatableObject
{
    //reference to paricle system
    private ParticleSystem particles;

    //implement the iAnimatableObject
    void iAnimatableObject.DoAnimations()
    {
        particles.Play();
    }

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

}
