using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    public Sprite[] planets = new Sprite[7];
    public float moveSpeed = 5f;

    void Start()
    {
        System.Random random = new System.Random();
        int randomPlanet = random.Next(0, 7);
        float randomScale = (float) random.NextDouble() * (15 - 5) + 5;
        float randomY = (float) random.NextDouble() * (5.78f + 2.54f) - 2.54f;
        float randomRot = (float) random.NextDouble() * (360 - 0);

        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        spriteRenderer.sprite = planets[randomPlanet];

        transform.position = new Vector3(28f, randomY, .25f);
        transform.Rotate(0f, 0.0f, randomRot, Space.World);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);

        Color spriteColor = spriteRenderer.color;
        spriteColor.a = 0.6f;
        spriteRenderer.color = spriteColor;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position + new Vector3(-1 * moveSpeed * Time.deltaTime, 0f, 0f);
        transform.position = newPosition;

        if (transform.position.x <= -28f)
        {
            Destroy(gameObject);
        }
    }
}
