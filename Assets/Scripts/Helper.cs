using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour, iAnimatableObject
{
    private ParticleSystem particles;

    public void DoAnimations()
    {
        particles.Play();
    }

    public void DoAnimations(int health)
    {
        DoAnimations();
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
