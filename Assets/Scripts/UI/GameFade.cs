using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameFade : MonoBehaviour
{
    public GameObject fadeObj;
    private SpriteRenderer fadeSprite;
    private Color fadeColor;

    public GameObject waveText;
    private TMP_Text wave;

    public GameObject controls;
    

    public bool fadeOut = true;
    private bool textGone = false;

    private float startOverlayTimer = 0f;
    private float textTimer = 0f;
    void Start()
    {
        fadeObj = gameObject;
        fadeSprite = gameObject.GetComponent<SpriteRenderer>();
        fadeColor = fadeSprite.color;

        waveText = Instantiate(waveText, new Vector3(3.11f, 8.1f, -0.5f), Quaternion.identity);
        wave = waveText.GetComponent<TMP_Text>();
         wave.text = "WAVE    1";

        controls = Instantiate(controls, new Vector3(-11.38f, 11.05f, -.5f), Quaternion.identity);
        startOverlayTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut)
        {
            FadeOut();
            if (fadeColor.a == 0f)
                fadeOut = false;
        }

        if (!textGone)
        {
            textTimer += Time.deltaTime;

            if (textTimer >= 3f)
            {
                Destroy(waveText);
                Destroy(controls);
            }
        }


    }

    private void FadeOut()
    {
        float elapsedTime = Time.time - startOverlayTimer;
        float fadePercentage = elapsedTime / 1.5f;

        // Calculate the new transparency value based on the fade percentage
        float currentTransparency = Mathf.Lerp(1f, 0f, fadePercentage);
        // Apply the new transparency value to the object's material
        fadeColor.a = currentTransparency;
        fadeSprite.color = fadeColor;
    }
}
