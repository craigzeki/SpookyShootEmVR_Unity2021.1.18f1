//Author: Craig Zeki
//Student ID: zek21003166

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectAudioManager
{
    private AudioSource audioSource;

    public ObjectAudioManager(AudioSource source)
    {
        audioSource = source;
    }

    public void playClip(AudioClip clip, bool interupt, bool loop)
    {
        if((interupt && audioSource.isPlaying) || (!audioSource.isPlaying))
        {
            audioSource.Stop();
            audioSource.clip = clip;
            if (loop) audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void playClip(AudioClip clip)
    {
        playClip(clip, true, false);
    }

    public void stopClip()
    {
        audioSource.Stop();
    }
}
