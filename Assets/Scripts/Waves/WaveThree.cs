using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveThree : MonoBehaviour
{

    public float slimeTimer = 0f;
    public float asteroidTimer = 0f;
    public int slimesSpawned = 0;
    public GameObject asteroid;
    public GameObject snake;
    public bool waveCurrent = false;

    public GameObject drawing;

    private Background bg;
    private System.Random random = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        bg = GetComponent<Background>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waveCurrent)
        {
            slimeTimer += Time.deltaTime;
            asteroidTimer += Time.deltaTime;
            if (asteroidTimer >= 5f && slimesSpawned < 10)
            {
                Instantiate(asteroid, new Vector3(28f, 0, 0), Quaternion.identity);
                asteroidTimer = 0f;
            }

            if (slimeTimer >= 2.5f && slimesSpawned < 10)
            {
                float randomY = (float) random.NextDouble() * (3.78f + 1.54f) - 1.54f;
                Instantiate(snake, new Vector3(28f, randomY, -1f), Quaternion.identity);
                slimeTimer = 0f;
                slimesSpawned++;
            }

            if (slimeTimer >= 3f && slimesSpawned >= 10)
            {
                bg.moveBackground = false;
                waveCurrent = false;
                GameObject l = Instantiate(drawing, new Vector3(28f, 1.473324f, 0), Quaternion.identity);
                l.GetComponent<Item>().wave = 3;
                slimeTimer = 0f;
                /*
                Instantiate(asteroid, new Vector3(28f, 2.2f, 0), Quaternion.identity);
                Instantiate(asteroid, new Vector3(28f, -3.1f, 0), Quaternion.identity);
                Instantiate(asteroid, new Vector3(28f, 7.4f, 0), Quaternion.identity);
                slimeTimer = 0f;
                */
            }
        }
    }
}
