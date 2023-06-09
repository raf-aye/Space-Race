using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveOne : MonoBehaviour
{

    public float slimesToSpawn = 15f;
    public float slimeTimer = 0f;
    public float asteroidTimer = 0f;
    public int slimesSpawned = 0;
    public GameObject asteroid;
    public GameObject slime;
    public bool waveCurrent = true;

    public GameObject journal;

    private Background bg;
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
            if (asteroidTimer >= 5f && slimesSpawned < slimesToSpawn)
            {
                Instantiate(asteroid, new Vector3(28f, 0, 0), Quaternion.identity);
                asteroidTimer = 0f;
            }

            if (slimeTimer >= 2.5f && slimesSpawned < slimesToSpawn)
            {
                Instantiate(slime, new Vector3(28f, 0, -1f), Quaternion.identity);
                slimeTimer = 0f;
                slimesSpawned++;
            }

            if (slimeTimer >= 3f && slimesSpawned >= slimesToSpawn)
            {
                bg.moveBackground = false;
                waveCurrent = false;
                Instantiate(journal, new Vector3(28f, 1.473324f, 0), Quaternion.identity);
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
