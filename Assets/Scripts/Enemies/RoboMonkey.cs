using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoboMonkey : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float maxYPosition = 7.06f;
    public float minYPosition = -2.59f;
    private int direction = 1; // 1 for moving up, -1 for moving down

    private GameObject booster1;
    private GameObject booster2;
    private GameObject raygun;
    private GameObject bg;
    private GameObject player;

    // Booster Sprites
    private float boosterTimer = 0f; // Change Timer
    public int boosterState = 1;
    private SpriteRenderer boosterSprite1;
    private SpriteRenderer boosterSprite2;

    public Sprite b1;
    public Sprite b2;

    // MONKE Sprites
    private SpriteRenderer monkeySprite;
    public int monkeyState = 1;

    public Sprite m1;
    public Sprite m2;
    public Sprite m3;
    public bool canShoot = false;
    private Vector3 initialPos;

    // Timers
    private float cooldown = 0f;
    private int timesShot = 0;

    // SFX
    private AudioSource laserSound;
    public GameObject laser;

    // Health
    public int health = 200;
    public bool dead = false;
    public Score score;
    private Color originalColor;

    public GameObject BossHealth;
    private GameObject healthBar;
    private float initialScale;
    public GameObject explosion;
    public GameObject explosionOverlay;

    // Initial Animation
    public float bobHeight = 0.5f;
    public float bobSpeed = 2f;
    public float targetX = -5f;

    private float startY;
    private bool doneAnimation = false;

    // Death Animation Fade Out Audio
    private AudioSource aud;
    private float initialVolume;
    private float fadeTimer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        booster1 = transform.GetChild(0).gameObject;
        booster2 = transform.GetChild(1).gameObject;
        raygun = transform.GetChild(2).gameObject;

        boosterSprite1 = booster1.GetComponent<SpriteRenderer>();
        boosterSprite2 = booster2.GetComponent<SpriteRenderer>();
        monkeySprite = GetComponent<SpriteRenderer>();
        laserSound = GetComponent<AudioSource>();
        score = GameObject.FindGameObjectWithTag("Score").GetComponent<Score>();
        originalColor = monkeySprite.color;
        startY = transform.position.y;
        bg = GameObject.FindGameObjectWithTag("Background");
        player = GameObject.FindGameObjectWithTag("Main Player"); // Main Player has the script, PLayer is the hitbox
        AudioSource[] audioSources = bg.GetComponents<AudioSource>();
        aud = audioSources[audioSources.Length - 2];
        initialVolume = aud.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x >= 17.93f)
        {
            transform.Translate(Vector3.left * 5f * Time.deltaTime);

            // Calculate the vertical bobbing motion
            float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;

            // Update the object's position with the bobbing motion
            transform.position = new Vector3(transform.position.x, startY + yOffset, transform.position.z);
        }
        else if (!doneAnimation)
        {
            float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;

            transform.position = new Vector3(transform.position.x, startY + yOffset, transform.position.z);
            cooldown += Time.deltaTime;
            if (cooldown >= 2.5f)
            {
                doneAnimation = true;
                cooldown = 0f;
                bg.GetComponent<Background>().moveBackground = true;
                player.GetComponent<Player>().canMove = true;
                BossHealth = Instantiate(BossHealth, new Vector3(15.07f, 10.44f, -0.09320367f), Quaternion.identity);
                healthBar = BossHealth.transform.GetChild(0).GetChild(1).gameObject;
                initialScale = healthBar.transform.localScale.x;
            }
        } else if (!dead) {
            Vector3 newPosition = transform.position + new Vector3(0f, direction * moveSpeed * Time.deltaTime, 0f);

            if (newPosition.y >= maxYPosition && !canShoot)
            {
                moveSpeed = 5f;
                newPosition.y = maxYPosition;
                direction = -1;
            }
            else if (newPosition.y <= minYPosition && !canShoot)
            {
                newPosition.y = minYPosition;
                direction = 1;

            }

            

            cooldown += Time.deltaTime;
            if ((cooldown >= 0.5f && timesShot == 0) || (cooldown >= 5f && timesShot > 0))
            {
                if (canShoot)
                {
                    cooldown = 0f;
                }
                else
                {
                    monkeyState = 3;
                    initialPos = transform.position;
                    canShoot = true;
                    monkeySprite.sprite = m3;
                    laserSound.Play();
                    cooldown = 0f;
                    StartCoroutine(ShootLaser());
                }

            }
            if (canShoot && Time.timeScale != 0f)
            {
                Vector3 randomOffset = new Vector3(Random.Range(-.075f, .075f), Random.Range(-.075f, .075f), 0f);


                transform.position = initialPos + randomOffset;

            }
            else
            {
                transform.position = newPosition;
            }
        } else if (dead)
        {
            float yOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;

            transform.position = new Vector3(transform.position.x, startY + yOffset, transform.position.z);
            if (fadeTimer <= 1.5f)
            {
                float targetVolume = Mathf.Lerp(initialVolume, 0f, fadeTimer / 1.5f);
                // Apply the new volume to the audio source
                aud.volume = targetVolume;

                // Increment the fade timer
                fadeTimer += Time.deltaTime;
            } else
            {
                aud.Stop();
            }
        } 
        boosterTimer += Time.deltaTime;

        if (boosterTimer >= .1f && !canShoot)
        {
            changeSprite();
        }
    }

    private void changeSprite()
    {
        boosterTimer = 0;
        if (boosterState == 1)
        {
            boosterSprite1.sprite = b2; boosterState = 2; monkeyState = 2;
            boosterSprite2.sprite = b2;
            if (doneAnimation && !dead)
                monkeySprite.sprite = m2;
        }
        else
        {
            boosterSprite1.sprite = b1; boosterState = 1; monkeyState = 1;
            boosterSprite2.sprite = b1;
            if (doneAnimation && !dead)
                monkeySprite.sprite = m1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            

            if (health > 1)
            {
                health--;
                changeHealth();
                StartCoroutine(FlashSprite());
                score.targetScore += 50;
            }
            else if (!dead)
            {
                dead = true;
                score.targetScore += 150;
                bg.GetComponent<WaveFour>().waveCurrent = false;
                monkeySprite.sprite = m3;
                StartCoroutine(spawnExplosions());
            }


        }

    }

    public void changeHealth()
    {
   
        float percentage = health / 150f;
        //float newX = initialPos - (Math.Abs(zeroPos - initialPos) * (1-percentage));
        healthBar.transform.localScale = new Vector3(initialScale * percentage, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        //healthBar.transform.position = new Vector3(newX, healthBar.transform.position.y, healthBar.transform.position.z);
    }

    private System.Collections.IEnumerator FlashSprite()
    {
        monkeySprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        monkeySprite.color = originalColor;
    }

    private System.Collections.IEnumerator ShootLaser()
    {
        yield return new WaitForSeconds(2.29f);
        if (!dead && Time.timeScale != 0f)
        {
            Instantiate(laser, new Vector3(transform.position.x - 6.18f, transform.position.y + .3f, 0), Quaternion.identity);
            canShoot = false;
            monkeyState = 1;
            monkeySprite.sprite = m1;

            transform.position = initialPos;

            Debug.Log("Back to initial position");
            timesShot++;
        }
    }

    private System.Collections.IEnumerator spawnExplosions()
    {
        player.GetComponent<Player>().canMove = false;
        Instantiate(explosion, new Vector3(transform.position.x - 3.84636f, transform.position.y - 4.092376f, -1f), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(explosion, new Vector3(transform.position.x - 4.66636f, transform.position.y + 1.057624f, -2f), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(explosion, new Vector3(transform.position.x + 2.08364f, transform.position.y - 1.332376f, -1f), Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(explosion, new Vector3(transform.position.x + 1.30364f, transform.position.y + 3.287624f, -1f), Quaternion.identity);
        yield return new WaitForSeconds(1f);
    }
}
