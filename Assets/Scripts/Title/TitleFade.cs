using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TitleFade : MonoBehaviour
{
    public GameObject fadeObj;
    private SpriteRenderer fadeSprite;
    private Color fadeColor;

    public bool fadeOut = true;
    public bool fadeIn = false;

    private float startOverlayTimer = 0f;
    void Start()
    {
        fadeObj = gameObject;
        fadeSprite = gameObject.GetComponent<SpriteRenderer>();
        fadeColor = fadeSprite.color;
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
        

        if (fadeIn)
        {
            FadeIn();
            if (fadeColor.a == 1f)
            {
                fadeIn = false;
                SceneManager.LoadScene("SampleScene");
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

    private void FadeIn()
    {
        float elapsedTime = Time.time - startOverlayTimer;
        float fadePercentage = elapsedTime / 1f;

        // Calculate the new transparency value based on the fade percentage
        float currentTransparency = Mathf.Lerp(0f, 1f, fadePercentage);
        // Apply the new transparency value to the object's material
        fadeColor.a = currentTransparency;
        fadeSprite.color = fadeColor;
    }

    public void button_event()
    {
        startOverlayTimer = Time.time;
        fadeIn = true;
        fadeOut = false;

        fadeColor.a = 0f;
        fadeSprite.color = fadeColor;
    }

}
