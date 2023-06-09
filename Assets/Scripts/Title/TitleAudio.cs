using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleAudio : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip a1;
    public AudioClip a2;

    private AudioSource start;
    private AudioSource loop;

    public bool fadeOut = false;
    private float fadeTimer = 0f;
    private bool loopStop = true;

    void Start()
    {
        start = gameObject.AddComponent<AudioSource>();
        start.clip = a1;
        loop = gameObject.AddComponent<AudioSource>();
        loop.clip = a2;

        loop.loop = true;
        start.volume = 0.5f;
        loop.volume = 0.5f;

        start.Play();
        loop.PlayDelayed(start.clip.length);
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {
            if (start.isPlaying && loopStop)
            {
                loopStop = false;
                loop.Stop();
            }
            if (fadeTimer <= 1f)
            {
                // Calculate the new volume based on the fade progress
                float targetVolume = Mathf.Lerp(0.5f, 0f, fadeTimer / 1f);

                // Apply the new volume to the audio source

                if (start.isPlaying)
                    start.volume = targetVolume;
                else if (loop.isPlaying)
                    loop.volume = targetVolume;

                // Increment the fade timer
                fadeTimer += Time.deltaTime;
            }
            else
            {
                // Fade out complete, stop the audio source
                if (start.isPlaying)
                    start.Stop();
                else if (loop.isPlaying)
                    loop.Stop();
            }
        }
    }

    public void changeFade()
    {
        fadeOut = true;
    }
}
