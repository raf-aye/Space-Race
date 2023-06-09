using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTwo : MonoBehaviour
{

    public float roboTimer = 0f;
    public float asteroidTimer = 0f;
    public int roboSpawned = 0;
    public GameObject asteroid;
    public GameObject robo;
    public bool waveCurrent = false;


    // First robo bird, checks if down
    public bool initialRoboDown = false;
    public bool threeRobosSpawned = false;

    // Checks into about all three robo birds when that comes
    public int robosDown = 0;

    public GameObject letter;
    private bool letterFound = false;

    private Background bg;
    // Start is called before the first frame update
    void Start()
    {
        bg = GetComponent<Background>();
        Debug.Log(bg.moveBackground);
    }

    // Update is called once per frame
    void Update()
    {
        if (waveCurrent)
        {
            roboTimer += Time.deltaTime;
            asteroidTimer += Time.deltaTime;

            if (roboSpawned == 0)
            {
                Instantiate(robo, new Vector3(28f, 0, 0), Quaternion.identity);
                roboSpawned++;
            }
            if (asteroidTimer >= 5f && !initialRoboDown)
            {
                Instantiate(asteroid, new Vector3(28f, 0, 0), Quaternion.identity);
                asteroidTimer = 0f;
            }

            if (initialRoboDown && !threeRobosSpawned)
            {
                StartCoroutine(Spawn());
                threeRobosSpawned = true;
                Debug.Log(robosDown);
            }

            if (robosDown == 3 && !letterFound)
            {
                GameObject l  = Instantiate(letter, new Vector3(28f, 1.473324f, 0), Quaternion.identity);

                l.GetComponent<Item>().wave = 2;
                bg.moveBackground = false;
                letterFound = true;
                waveCurrent = false;
            }
            /*
            if (slimeTimer >= 2.5f && slimesSpawned < 15)
            {
                
                Instantiate(robo, new Vector3(28f, 0, 0), Quaternion.identity);
                slimeTimer = 0f;
                slimesSpawned++;
            }

            if (slimeTimer >= 3f && slimesSpawned >= 15)
            {
                bg.moveBackground = false;
                waveCurrent = false;
                Instantiate(letter, new Vector3(28f, 1.473324f, 0), Quaternion.identity);
                slimeTimer = 0f;
                
               // Instantiate(asteroid, new Vector3(28f, 2.2f, 0), Quaternion.identity);
                //Instantiate(asteroid, new Vector3(28f, -3.1f, 0), Quaternion.identity);
                //Instantiate(asteroid, new Vector3(28f, 7.4f, 0), Quaternion.identity);
                //slimeTimer = 0f;
                
            }
        */
        }
    }

    private System.Collections.IEnumerator Spawn()
    {

        Instantiate(robo, new Vector3(28f, 5.48f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(robo, new Vector3(28f, -5.48f, 0), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(robo, new Vector3(28f, 0f, 0), Quaternion.identity);
 
    }
}
