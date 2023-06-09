using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject explosionOverlay;

    private bool isSecond = false; // Second explosion will spawn overlay

    private SpriteRenderer overlaySprite;
    private Color overlayColor;
    private SpriteRenderer explodeSprite;
    private Color explodeColor;

    private GameObject player;
    private GameObject bg;
    public GameObject teddy;

    // Timers
    private float startOverlayTimer = 0f;
    void Start()
    {
        if (transform.position.z == -2f)
        {
            isSecond = true;
            explosionOverlay = Instantiate(explosionOverlay, new Vector3(0f, 0f, -5f), Quaternion.identity);
            overlaySprite = explosionOverlay.GetComponent<SpriteRenderer>();

            overlayColor = overlaySprite.color;
            overlayColor.a = 0f;
            overlaySprite.color = overlayColor;
            bg = GameObject.FindGameObjectWithTag("Background");
            player = GameObject.FindGameObjectWithTag("Main Player");
            
        }
        GetComponent<AudioSource>().Play();
        explodeSprite = GetComponent<SpriteRenderer>();
        explodeColor = explodeSprite.color;
        explodeColor.a = 0f;
        explodeSprite.color = explodeColor;
        startOverlayTimer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSecond)
        {
            float elapsedTime = Time.time - startOverlayTimer;
            float fadePercentage = elapsedTime / 3.5f;

            // Calculate the new transparency value based on the fade percentage
            float currentTransparency = Mathf.Lerp(0f, 1f, fadePercentage);

            // Apply the new transparency value to the object's material
            overlayColor.a = currentTransparency;
            overlaySprite.color = overlayColor;
            if (overlayColor.a == 1f)
            {
                StartCoroutine(FadeOut());
            }
        }
        float elapsedTime2 = Time.time - startOverlayTimer;
        float fadePercentage2 = elapsedTime2 / .5f;

        float currentTransparency2 = Mathf.Lerp(0f, 1f, fadePercentage2);

        explodeColor.a = currentTransparency2;
        explodeSprite.color = explodeColor;
    }

    private System.Collections.IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        bg.GetComponent<Background>().moveBackground = false;
        player.transform.position = new Vector3(-0.2013767f, 0.6931884f, -0.01098946f);
        Instantiate(teddy, new Vector3(-0.02f, 1.94f, 0f), Quaternion.identity);
        GameObject[] explosions = GameObject.FindGameObjectsWithTag("Explosion");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] planets = GameObject.FindGameObjectsWithTag("Planet");
       
        foreach (GameObject o in explosions)
        {
            if (!o.GetComponent<Explosion>().isSecond)
                Destroy(o);
        }
        foreach (GameObject o in enemies) { Destroy(o); }

        foreach (GameObject o in planets) { Destroy(o); }

        explosionOverlay.GetComponent<FadeOut>().fadeOut = true;
        Destroy(gameObject);
    }
}
