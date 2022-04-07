using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGAudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;
    // Start is called before the first frame update

    private static BGAudioManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 0; //set to 2D (all around as this is BG music)
    }

    //Setup as singleton
    public static BGAudioManager Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<BGAudioManager>();
            return instance;
        }
    }

    private void PlayClip(AudioClip clip, bool loop, bool interupt)
    {
        if (audioSource == null) return;

        if (audioSource.isPlaying && interupt == false) return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.Play();
    }

    public void PlayMenuMusic()
    {
        PlayClip(menuMusic, true, true);
    }

    public void PlayGameMusic()
    {
        PlayClip(gameMusic, true, true);
    }
}
