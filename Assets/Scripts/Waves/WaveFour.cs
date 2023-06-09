using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveFour : MonoBehaviour
{
    public float asteroidTimer = 0f;
    private bool initialWait = false;
    public int asteroidsSpawned = 0;
    public GameObject asteroid;
    public GameObject robo;
    public bool waveCurrent = false;
    private bool monkeySpawned = false;

    public GameObject picture;

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
            if (!monkeySpawned)
            {
                Instantiate(robo, new Vector3(34.62f, 2.81f, 0), Quaternion.identity);
                monkeySpawned = true;
            }

            asteroidTimer += Time.deltaTime;
            if (asteroidTimer >= 10f) {
                System.Random random = new System.Random();
                float randomX = (float)random.NextDouble() * 7f;
                Instantiate(asteroid, new Vector3(randomX, 16.71f, 0), Quaternion.identity);
                Instantiate(asteroid, new Vector3(randomX, 22f, 0), Quaternion.identity);
                Instantiate(asteroid, new Vector3(randomX, 27.19998f, 0), Quaternion.identity);
                asteroidTimer = 0f;
            }
        }
    }
}
