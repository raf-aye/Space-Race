using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float scrollSpeed = 0.2f;
    public float timer = 0f;
    public bool moveBackground = true;

    // Game Objects to Spawn
    public GameObject emptyParent;
    public GameObject[] items = new GameObject[4];

    [SerializeField]
    private Renderer backgroundRenderer;
    public bool title = false;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (moveBackground)
        {
            backgroundRenderer.material.mainTextureOffset += new Vector2(scrollSpeed * Time.deltaTime, 0f);

            if (!title)
            {
                timer += Time.deltaTime;

                if (timer >= 5f)
                {
                    Instantiate(emptyParent, new Vector3(28f, 0, 0), Quaternion.identity);
                    timer = 0f;
                }
            }

        }
    }

   
}