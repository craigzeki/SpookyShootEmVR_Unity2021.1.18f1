using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour, iAnimatableObject
{
    private ParticleSystem particles;

    void iAnimatableObject.DoAnimations()
    {
        particles.Play();
    }

    // Start is called before the first frame update
    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
