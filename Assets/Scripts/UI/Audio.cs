using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{

    // Sugar High (Main Theme)
    public AudioClip audioClip1; // Starting Clip
    public AudioClip audioClip2; // Looping Clip

    private AudioSource audioSource1;
    private AudioSource audioSource2;

    private float initialVolume;
    public bool fadeOutMain = false;
    private float fadeTimer = 0f;

    // Warfare (Boss Theme)
    public AudioClip audioClip3;
    public AudioClip audioClip4;

    private AudioSource audioSource3;
    private AudioSource audioSource4;

    public bool fadeInBoss = false;


    void Start()
    {
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource1.clip = audioClip1;

        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource2.clip = audioClip2;
        audioSource2.loop = true;

        audioSource3 = gameObject.AddComponent<AudioSource>();
        audioSource3.clip = audioClip3;

        audioSource4 = gameObject.AddComponent<AudioSource>();
        audioSource4.clip = audioClip4;
            
        audioSource4.loop = true;

       audioSource1.volume = 0.25f;
       audioSource2.volume = 0.25f;
       audioSource3.volume = 0.25f;
       audioSource4.volume = 0.25f;

        audioSource1.Play();
        audioSource2.PlayDelayed(audioSource1.clip.length);
        initialVolume = audioSource2.volume;
    }

    void Update()
    {
        if (fadeOutMain)
        {
            if(fadeTimer <= 1.5f)
            {
                // Calculate the new volume based on the fade progress
                float targetVolume = Mathf.Lerp(initialVolume, 0f, fadeTimer / 1.5f);

                // Apply the new volume to the audio source
                audioSource2.volume = targetVolume;

                // Increment the fade timer
                fadeTimer += Time.deltaTime;
            }
            else
            {
                // Fade out complete, stop the audio source
                audioSource2.Stop();
                fadeOutMain = false;
                fadeInBoss = true;
                fadeTimer = 0f;

                audioSource3.Play();
                audioSource4.PlayDelayed(audioSource3.clip.length);
            }
        }

        if (fadeInBoss)
        {
            if (fadeTimer <= 3f)
            {
                float targetVolume = Mathf.Lerp(0f, initialVolume, fadeTimer / 3f);

                audioSource3.volume = targetVolume;

                fadeTimer += Time.deltaTime;
            }
        }
    }

    public void quietAudio()
    {
        float changeTo = 0.01f;

        audioSource1.volume = changeTo;
        audioSource2.volume = changeTo;
        audioSource3.volume = changeTo;
        audioSource4.volume = changeTo;
    }
    
    
}
