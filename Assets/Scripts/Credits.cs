using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Credits : MonoBehaviour
{

    public bool start = false;
    public bool background = false;
    public bool songPlaying = false;
    public AudioClip audioClip;
    private AudioSource audioSource;

    private float timer = 0f; // go to 6.5f, where the music kicks in
    private Vector3 targetPosition = new Vector3(-.2f, 0.6931884f, -0.01098946f);

    private GameObject player;
    public float bobHeight = 0.5f;
    public float bobSpeed = .2f;

    public GameObject text;
    private TMP_Text txt;
    private Color currentColor;

    public GameObject earth;
    private bool earthMoving = false;
    private bool movePlayerToEarth = false;
    public float earthSpeed = 7f;

    public GameObject fadeObj;
    private SpriteRenderer fadeSprite;
    private Color fadeColor;
    private float transparency = 0f;
    private bool fade = false;
    private bool fadeStart = true;
    private float startOverlayTimer = 0f;

    private int fadeText = -1; // -1 = do nothing, 0 = fade in, 1 = fade out
    private float transparencyText = 0f;

    private bool fadeAudio = false;
    private float fadeAudioTimer = 0f;
   
    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = audioClip;
        audioSource.volume = 0.25f;

        player = GameObject.FindGameObjectWithTag("Main Player");

        fadeSprite = fadeObj.GetComponent<SpriteRenderer>();
        fadeSprite.color = Color.black;
        fadeColor = fadeSprite.color;
        fadeColor.a = 0f;
        fadeSprite.color = fadeColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            if (timer <= 6.5f)
            {
                if (!songPlaying)
                {
                    songPlaying = true;
                    audioSource.Play();
                }
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, 1.5f * Time.deltaTime);
                timer += Time.deltaTime;
            }
            else
            {
                if (!background)
                {
                    background = true;
                    player.transform.position = targetPosition;
                    GetComponent<Background>().moveBackground = true;

                    text = Instantiate(text, new Vector3(13.2f, 5.13f, -0.5f), Quaternion.identity);
                    txt = text.GetComponent<TMP_Text>();

                    txt.text = "DEVELOPER:\nTHATMAJESTICGUY";

                    currentColor = txt.color;
                    currentColor.a = 1f;
                    txt.color = currentColor;
                    StartCoroutine(changeText());
                }

                if (earthMoving)
                {
                    Vector3 newPosition = earth.transform.position + new Vector3(-1 * earthSpeed * Time.deltaTime, 0f, 0f);
                    earth.transform.position = newPosition;
                }

                if (movePlayerToEarth)
                {
                    player.transform.position = Vector3.MoveTowards(player.transform.position, earth.transform.position, 4f * Time.deltaTime);
                    Vector3 newScale = Vector3.Lerp(player.transform.localScale, new Vector3(.1f, .1f, .1f), 2f * Time.deltaTime);
                    player.transform.localScale = newScale;
                }

                if (fade)
                {
                    if (fadeStart)
                    {
                        fadeStart = false;
                        fadeObj = Instantiate(fadeObj, new Vector3(0f, 0f, -5f), Quaternion.identity);
                        fadeSprite = fadeObj.GetComponent<SpriteRenderer>();
                        fadeSprite.color = Color.black;
                        fadeColor = fadeSprite.color;
                        fadeColor.a = 0f;
                        fadeSprite.color = fadeColor;
                    }
                    float elapsedTime = Time.time - startOverlayTimer;
                    float fadePercentage = elapsedTime / 3.5f;

                    // Calculate the new transparency value based on the fade percentage
                    float currentTransparency = Mathf.Lerp(0f, 1f, fadePercentage);
                    if (currentTransparency == 1f)
                        fade = false;
                    // Apply the new transparency value to the object's material
                    fadeColor.a = currentTransparency;
                    fadeSprite.color = fadeColor;
                }
                if (fadeText!= -1)
                {
                    if (fadeText == 0)
                        fadeIn(currentColor, txt);
                    else
                        fadeOut(currentColor, txt);

                }

                if (fadeAudio)
                {
                    if (fadeAudioTimer <= 3.5f)
                    {
                        // Calculate the new volume based on the fade progress
                        float targetVolume = Mathf.Lerp(.25f, 0f, fadeAudioTimer / 3.5f);

                        // Apply the new volume to the audio source
                        audioSource.volume = targetVolume;

                        // Increment the fade timer
                        fadeAudioTimer += Time.deltaTime;
                    }
                    else
                    {
                        // Fade out complete, stop the audio source
                        audioSource.Stop();
                    }
                }
            }
                float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;

                // Update the object's position with the bobbing motion
                player.transform.position = new Vector3(player.transform.position.x, 0.6931884f + yOffset, player.transform.position.z);
            
        }
    }

    private void changeTransparency(float transparent)
    {
        currentColor.a = transparent;
        txt.color = currentColor;
    }


    private void fadeIn(Color c, TMP_Text s)
    {
        float elapsedTime = Time.time - startOverlayTimer;
        float fadePercentage = elapsedTime / 3.5f;

        // Calculate the new transparency value based on the fade percentage
        float currentTransparency = Mathf.Lerp(0f, 1f, fadePercentage);
        // Apply the new transparency value to the object's material
        c.a = currentTransparency;
        s.color = c;
    }

    private void fadeOut(Color c, TMP_Text s)
    {
        float elapsedTime = Time.time - startOverlayTimer;
        float fadePercentage = elapsedTime / 3.5f;

        // Calculate the new transparency value based on the fade percentage
        float currentTransparency = Mathf.Lerp(1f, 0f, fadePercentage);
        // Apply the new transparency value to the object's material
        c.a = currentTransparency;
        s.color = c;
    }
    private System.Collections.IEnumerator changeText()
    {
        yield return new WaitForSeconds(5f);
        changeTransparency(0f);

        yield return new WaitForSeconds(1f);
        txt.text = "TITLE MUSIC:\n8-WAVE - ACORN";
        changeTransparency(5f);
        yield return new WaitForSeconds(5f);
        changeTransparency(0f);

        yield return new WaitForSeconds(1f);
        txt.text = "MAIN MUSIC:\nSUGAR RUSH - ACORN";
        changeTransparency(5f);
        yield return new WaitForSeconds(5f);
        changeTransparency(0f);

        yield return new WaitForSeconds(1f);
        txt.text = "BOSS MUSIC:\nWARFARE - ACORN";
        changeTransparency(5f);
        yield return new WaitForSeconds(5f);
        changeTransparency(0f);

        yield return new WaitForSeconds(1f);
        txt.text = "CREDITS MUSIC:\nFINISH LINE - ACORN";
        changeTransparency(5f);
        yield return new WaitForSeconds(5f);
        changeTransparency(0f);

        yield return new WaitForSeconds(1f);
        txt.text = "ART:\nTHATMAJESTICGUY";
        changeTransparency(5f);
        yield return new WaitForSeconds(5f);
        changeTransparency(0f);

        yield return new WaitForSeconds(1f);
        txt.text = "LASER SPRITES:\nWENREXA";
        changeTransparency(5f);
        yield return new WaitForSeconds(5f);
        changeTransparency(0f);

        yield return new WaitForSeconds(1f);
        txt.text = "SFX:\nFREESOUND";
        changeTransparency(5f);
        yield return new WaitForSeconds(5f);
        changeTransparency(0f);

        yield return new WaitForSeconds(1f);
        txt.text = "PROGRAMS USED:\nUNITY\nADOBE ILLUSTRATOR";
        changeTransparency(5f);
        yield return new WaitForSeconds(3f);
        changeTransparency(0f);

        earth = Instantiate(earth, new Vector3(29.77866f, 1.836301f, 0f), Quaternion.identity);
        earthMoving = true;

        yield return new WaitForSeconds(2f);
        GetComponent<Background>().moveBackground = false;
        earthMoving = false;
        movePlayerToEarth = true;

        yield return new WaitForSeconds(2f);
        startOverlayTimer = Time.time;
        fade = true;
        

        yield return new WaitForSeconds(4f);
        text.transform.position = new Vector3(4.09f, 2.29f, -7f);
        txt.text = "THANK YOU FOR PLAYING!";
        txt.fontSize = 50f;
        startOverlayTimer = Time.time;
        fadeText = 0;

        yield return new WaitForSeconds(4f);
        fadeText = 1;
        startOverlayTimer = Time.time;
        fadeAudio = true;

        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Title");
    }
}
