using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{

    // Animation of Collection
    public Vector3 targetPoint = new Vector3(13.45f, 1.68f, -.5f); // The target point where the object will move
    public float speed = 8f; // The speed at which the object moves
    public float scaleSpeed = 1f; // The speed at which the object scales up
    public float maxScale = 5f; // The maximum scale the object will reach

    private Vector3 initialScale;
    private bool isAnimating = false;
    private bool collected = false;

    // Text
    public GameObject itemText;
    public GameObject itemText2 = null; // Backup
    public TMP_Text txt;
    Color currentColor;
    private bool textVisible = false;


    

    // ANIMATION 2
    public bool isAnimating2 = false;
    private float timer = 0f;

    private Vector3 targetPoint2 = new Vector3(-3.12f - .54f, -9.456f, -1f);
    private Vector3 targetPoint3 = new Vector3(-0.63f - .54f, -9.48f, -1f);
    private Vector3 targetPoint4 = new Vector3(1.35f, -9.45f, -1f);
    private Vector3 targetPoint5 = new Vector3(3.78f, -9.42f, -1f);

    // Wave
    public int wave = 1;
    public bool isAnimating3 = false;
    public Dictionary<int, String> items = new Dictionary<int, String>();
    public GameObject bg;
    public GameObject player;
    private bool fullyCollected = false;

    private float checkTimer = 0f; // PRevents item being stuck

    void Start()
    {
        initialScale = transform.localScale;
        bg = GameObject.FindGameObjectWithTag("Background");
        player = GameObject.FindGameObjectWithTag("Main Player");
        items.Add(1, "A JOURNAL");
        items.Add(2, "A LETTER");
        items.Add(3, "A DRAWING");
        items.Add(4, "YOUR DAUGHTERS TEDDY BEAR");
    }

    // Update is called once per frame
    void Update()
    {
        if (fullyCollected) return;
        if (transform.position.x >= -2 && !collected)
        {
            Vector3 newPosition = transform.position + new Vector3(-1 * 8f * Time.deltaTime, 0f, 0f);
            transform.position = newPosition;
        }


        // FIRST ANIMATION (showcasing the item)
        if (isAnimating)
        {
            // Move towards the target point
            Vector3 direction = targetPoint - transform.position;
            float distance = direction.magnitude;
            float decelerationFactor = Mathf.Clamp01(distance / 2f); 
            float step = speed * Time.deltaTime * decelerationFactor;
            transform.position += direction.normalized * step;

           
            Vector3 newScale = Vector3.Lerp(transform.localScale, new Vector3(maxScale, maxScale, maxScale), scaleSpeed * Time.deltaTime);
            transform.localScale = newScale;

            if (Math.Abs(transform.position.x - targetPoint.x) <= 1 && checkTimer <= 5f)
            {
                if (!textVisible)
                {
                    itemText = Instantiate(itemText, new Vector3(3.11f, 8.1f, -0.5f), Quaternion.identity);
                    txt = itemText.GetComponent<TMP_Text>();
                    txt.text = "YOU FOUND " + items[wave];
                    currentColor = txt.color;
                }

                textVisible = true;

                if (currentColor.a == 1f)
                    currentColor.a = 0.5f;
                else
                    currentColor.a = 1f;

                txt.color = currentColor;
                checkTimer += Time.deltaTime; ;
            }

            // Check if the target point is reached
            if (transform.position == targetPoint || checkTimer >= 5f)
            {
                currentColor.a = 1f;
                checkTimer = 0f;
                txt.color = currentColor;
                isAnimating = false;
                isAnimating2 = true;
            }
        }

        if (isAnimating2) {
            timer += Time.deltaTime;
            if (timer >= 1.5f)
            {
                checkTimer += Time.deltaTime;
                if (checkTimer <= 3f)
                {
                    changeToUI();
                    if (currentColor.a == 1f)
                        currentColor.a = 0.5f;
                    else
                        currentColor.a = 1f;

                    txt.color = currentColor;
                }

               

                if ((wave == 1 && transform.position == targetPoint2) || (wave == 2 && transform.position == targetPoint3) || (wave == 3 && transform.position == targetPoint4) || (wave != 4 && timer >= 3.5f))
                {
                    Destroy(itemText);
                    textVisible = false;
                    timer = 0f;

                    isAnimating2 = false;
                    isAnimating3 = true;

                    if (wave == 3)
                    {
                        bg.GetComponent<Audio>().fadeOutMain = true;
                    }
                }

                if ((wave == 4 && transform.position == targetPoint5) || (wave == 4 && checkTimer >= 3.5f))
                {
                    Destroy(itemText);
                    textVisible = false;
                    isAnimating2 = false;

                    timer = 0f;
                    startWave();
                }
            }
        }

        if (isAnimating3)
        {
            timer += Time.deltaTime;

            if (timer >= 1f)
            {
                if (!textVisible)
                {
                    itemText2 = Instantiate(itemText2, new Vector3(3.11f, 8.1f, -0.5f), Quaternion.identity);
                    txt = itemText2.GetComponent<TMP_Text>();
                    if (wave < 3)
                        txt.text = "WAVE    " + (wave + 1).ToString();
                    else
                        txt.text = "BOSS   BATTLE";
                    currentColor = txt.color;
                    textVisible = true;
                }

                if (timer <= 2f)
                {
                    if (currentColor.a == 1f)
                        currentColor.a = 0.5f;
                    else
                        currentColor.a = 1f;
                }

            }

            if (timer >= 3f)
            {
                Destroy(itemText2);
                startWave();
                isAnimating3 = false;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !fullyCollected)
        {
            isAnimating = true;
            collected = true;
            player.GetComponent<Player>().health += 25;
            player.GetComponent<Player>().changeHealth();
        }

    }

    private void changeToUI()
    {
        if (wave == 1)
        {
            Vector3 direction = targetPoint2 - transform.position;
            float distance = direction.magnitude;
            float decelerationFactor = Mathf.Clamp01(distance / 2f);
            float step = speed * Time.deltaTime * decelerationFactor;
            transform.position += direction.normalized * step;

            Vector3 newScale = Vector3.Lerp(transform.localScale, new Vector3(1.399609f, 1.399609f, 1.399609f), scaleSpeed * Time.deltaTime);
            transform.localScale = newScale;
        } else if (wave == 2)
        {
            Vector3 direction = targetPoint3 - transform.position;
            float distance = direction.magnitude;
            float decelerationFactor = Mathf.Clamp01(distance / 2f);
            float step = speed * Time.deltaTime * decelerationFactor;
            transform.position += direction.normalized * step;

            Vector3 newScale = Vector3.Lerp(transform.localScale, new Vector3(1.072371f, 1.072371f, 1.072371f), scaleSpeed * Time.deltaTime);
            transform.localScale = newScale;
        } else if (wave == 3)
        {
            Vector3 direction = targetPoint4 - transform.position;
            float distance = direction.magnitude;
            float decelerationFactor = Mathf.Clamp01(distance / 2f);
            float step = speed * Time.deltaTime * decelerationFactor;
            transform.position += direction.normalized * step;

            Vector3 newScale = Vector3.Lerp(transform.localScale, new Vector3(0.1290522f, .1735532f, 0.1290522f), scaleSpeed * 3 * Time.deltaTime);
            transform.localScale = newScale;
        } else if (wave == 4)
        {
            Vector3 direction = targetPoint5 - transform.position;
            float distance = direction.magnitude;
            float decelerationFactor = Mathf.Clamp01(distance / 2f);
            float step = speed * Time.deltaTime * decelerationFactor;
            transform.position += direction.normalized * step;

            Vector3 newScale = Vector3.Lerp(transform.localScale, new Vector3(1.068784f, 1.068784f, 1.068784f), scaleSpeed * 3 * Time.deltaTime);
            transform.localScale = newScale;
        }
    }

    private void startWave()
    {
        fullyCollected = true;
        if (wave == 1)
        {
            bg.GetComponent<Background>().moveBackground = true;
            bg.GetComponent<WaveTwo>().waveCurrent = true;
        } else if (wave == 2)
        {
            bg.GetComponent<Background>().moveBackground = true;
            bg.GetComponent<WaveThree>().waveCurrent = true;
        } else if (wave == 3)
        {
            bg.GetComponent<WaveFour>().waveCurrent = true;
            player.GetComponent<Player>().canMove = false;
        } else if (wave == 4)
        {
            player.GetComponent<Player>().canMove = false;
            bg.GetComponent<Credits>().start = true;
        }
    }
}
