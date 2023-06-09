using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public bool fadeOut = false;
    private bool started = false;
    private bool playerCanMove = false;

    private float startOverlayTimer = 0f;

    private SpriteRenderer overlaySprite;
    private Color overlayColor;
    private GameObject player;
    void Start()
    {
        overlaySprite = GetComponent<SpriteRenderer>();
        overlayColor = overlaySprite.color;
        player = GameObject.FindGameObjectWithTag("Main Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeOut && !started)
        {
            started = true;
            startOverlayTimer = Time.time;
        }
        else if (fadeOut)
        {
            float elapsedTime = Time.time - startOverlayTimer;
            float fadePercentage = elapsedTime / 0.5f;

            // Calculate the new transparency value based on the fade percentage
            float currentTransparency = Mathf.Lerp(1f, 0f, fadePercentage);

            // Apply the new transparency value to the object's material
            overlayColor.a = currentTransparency;
            overlaySprite.color = overlayColor; 
            
            if (overlayColor.a == 0f && !playerCanMove)
            {
                playerCanMove = true;
                player.GetComponent<Player>().canMove = true;
            } 
                
        }
    }
}
