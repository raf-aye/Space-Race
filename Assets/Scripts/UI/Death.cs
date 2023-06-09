using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    private bool yes = false;
    private bool no = false;

    private float startOverlayTimer = 0f;

    private GameObject fadeObj;
    private SpriteRenderer fadeSprite;
    private Color fadeColor;

    void Start()
    {
        fadeObj = GameObject.FindGameObjectWithTag("Overlay");
        fadeSprite = fadeObj.GetComponent<SpriteRenderer>();
        fadeColor = fadeSprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (yes)
        {
            
            FadeIn();
            Debug.Log(fadeColor.a);
            if (fadeColor.a == 1f)
            {
                yes = false;
                StartCoroutine(ChangeScene("SampleScene"));
            }
        }

        if (no)
        {
            FadeIn();
            if (fadeColor.a == 1f)
            {
                no = false;
                StartCoroutine(ChangeScene("Title"));
            }
        }
    }
    private void FadeIn()
    {
        float elapsedTime = Time.time - startOverlayTimer;
        float fadePercentage = elapsedTime / 0.5f;

        // Calculate the new transparency value based on the fade percentage
        float currentTransparency = Mathf.Lerp(0f, 1f, fadePercentage);
        // Apply the new transparency value to the object's material
        fadeColor.a = currentTransparency;
        fadeSprite.color = fadeColor;
    }

    public void pickedYes()
    {
        if (no) return;
        Time.timeScale = 1f;
        startOverlayTimer = Time.time;
        yes = true;
    }

    public void pickedNo()
    {
        if (yes) return;
        Time.timeScale = 1f;
        startOverlayTimer = Time.time;
        no = true;
    }

    private System.Collections.IEnumerator ChangeScene(string sceneName)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Scene Change");
        SceneManager.LoadScene(sceneName);
    }

}
