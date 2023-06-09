using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    // Movement
    public float moveSpeed = 5f;
    public bool canMove = true;

    // Bullets
    public GameObject bullet;
    public bool canShoot = true;
    public GameObject ship;
    public GameObject astronaut;
    private int bulletsShot = 0;
    private bool spaceHeld = false;
    private float shotCooldown = .2f;
    private float shotTimer = 0f; // Timer determining when a shot can be fired


    // Health
    private float initialScale; // Will be decreasing by percentage
    public float health = 100f;
    public bool dead = false;
    private Dictionary<string, float> enemyHealthMap = new Dictionary<string, float>();
    public GameObject deathScreen;
    private bool screenSpawn = false;

    private float cooldownHit = 0f;
    private bool isHit = false;

    public bool invincible = true; // CHANGE ONCE DONE!

    public GameObject healthText;
    public GameObject healthBar;
    

    // Misc
    private Color originalColor;
    private int state = 1;
    private AudioSource audio;
    private Audio bgAudio;


    // Start is called before the first frame update
    void Start()
    {
        ship = transform.GetChild(0).gameObject;
        originalColor = ship.GetComponent<SpriteRenderer>().material.color;
        initialScale = healthBar.transform.localScale.x;
        // initialPos = healthBar.transform.position.x;

        enemyHealthMap["Default"] = 5f; // Failsafe
        enemyHealthMap["Slime"] = 5f;
        enemyHealthMap["Asteroid"] = 8f;
        enemyHealthMap["RoboBird"] = 10f;
        enemyHealthMap["Snake"] = 3f;
        enemyHealthMap["MonkeyBullet"] = 5f;
        audio = GetComponent<AudioSource>();
        bgAudio = GameObject.FindGameObjectWithTag("Background").GetComponent<Audio>();
    }


    void Update()
    {
        if (isHit)
        {
            cooldownHit += Time.deltaTime;

            if (cooldownHit >= 1.5f)
            {
                cooldownHit = 0f;
                isHit = false;

            }
        }

        if (dead)
        {
            Vector3 newPosition = transform.position + new Vector3(0f, -1 * 20f * Time.deltaTime, 0f);
            if (newPosition.y <= -25f && !screenSpawn)
            {
                Time.timeScale = 0f;
                Instantiate(deathScreen, new Vector3(-4.4f, .2f, -5f), Quaternion.identity);
                bgAudio.quietAudio();
                screenSpawn = true;
            }
            transform.position = newPosition;
            return;
        }
        if (!canMove) return;
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        if ((horizontalInput > 0f && ship.transform.position.x >= -2.98f) || (horizontalInput < 0f && ship.transform.position.x <= -20.14f))
        {
            horizontalInput = 0f;
        }

        if ((verticalInput > 0f && ship.transform.position.y >= 7.15f) || (verticalInput < 0f && transform.position.y <= -5.510981f))
        {
            verticalInput = 0f;
        }
        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);



        if (Input.GetKeyDown(KeyCode.Space) && canShoot)
        {
            spaceHeld = true;
            canShoot = false;
            SpawnBullet();
        }


        if (spaceHeld)
        {
            shotTimer += Time.deltaTime;

            if (shotTimer >= shotCooldown)
            {
                shotTimer = 0f;
                canShoot = true;
                spaceHeld = false;
            }
        }

        


    }

    private void SpawnBullet()
    {
        GameObject obj = Instantiate(bullet, transform.position, Quaternion.identity);
        obj.transform.position = new Vector3(ship.transform.position.x + 1.618695f, ship.transform.position.y - 0.430127f, ship.transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isHit && !invincible && canMove)
        {
            if (LayerMask.LayerToName(collision.gameObject.layer).Equals("Snake"))
            {
                Snake s = collision.gameObject.GetComponent<Snake>();
                if (!s.collidedPlayer) return;
            }

            if (LayerMask.LayerToName(collision.gameObject.layer).Equals("MonkeyBullet"))
            {
                MonkeyBullet s = collision.gameObject.GetComponent<MonkeyBullet>();
                if (!s.collidedPlayer) return;
            }
            health -= enemyHealthMap[LayerMask.LayerToName(collision.gameObject.layer)];

            if (health > 0)
                StartCoroutine(FlashSprite());
            else
            {
                /*
                sprite.color = Color.gray;
                state = 3;
                sprite.sprite = state3;
                */
                ship.transform.Rotate(0f, 0.0f, 180f, Space.World);
                astronaut.transform.Rotate(0f, 0.0f, 180f, Space.World);
                dead = true;
            }
            changeHealth();


        }
    }

    public void changeHealth()
    {

        TextMeshPro txt = healthText.GetComponent<TextMeshPro>();
        if (health > 100)
            health = 100;

        if (health < 0)
            health = 0;

        txt.SetText(health + "");

        float percentage = health / 100;
        //float newX = initialPos - (Math.Abs(zeroPos - initialPos) * (1-percentage));
        healthBar.transform.localScale = new Vector3(initialScale * percentage, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        //healthBar.transform.position = new Vector3(newX, healthBar.transform.position.y, healthBar.transform.position.z);
    }

    private System.Collections.IEnumerator FlashSprite()
    {
        SpriteRenderer sprite = ship.GetComponent<SpriteRenderer>();
        SpriteRenderer sprite2 = astronaut.GetComponent<SpriteRenderer>();
        sprite.color = Color.red;
        sprite2.color = Color.red;
        state = 3;
        audio.Play();
        //sprite.sprite = state3;

        yield return new WaitForSeconds(0.1f);

        sprite.color = originalColor;
        sprite2.color = originalColor;
        state = 1;
        //sprite.sprite = state1;
    }


    /*
    private void shoot()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

        foreach (GameObject obj in bullets)
        {
            Vector3 movement = new Vector3(moveSpeed, 0f, 0f) * Time.deltaTime;
            obj.transform.Translate(movement);

            if (obj.transform.position.x > 13f)
            {
                Destroy(obj);
            }
        }
    }
    */
}
