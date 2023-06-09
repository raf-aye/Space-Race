using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboBird : MonoBehaviour
{

    // Health
    public int health = 25;

    // Status
    public bool dead = false;
    public Sprite state1; // Green Wing Up
    public Sprite state2; // Green Wing Down
    public Sprite state3; // Red Wing Up
    public Sprite state4; // Red Wing Down
    public int state = 1;
    public bool canShoot = false;

    // Misc
    public float speed = 2f;
    private Color originalColor;
    private SpriteRenderer sprite;
    private Vector3 finalPoint;
    public GameObject bg;

    // Timers
    private float spriteTimer = 0f; // Timer to change sprite
    private float cooldown = 0f;
    private int timesShot = 0;

    // Score
    public GameObject scoreObject;
    private Score score;

    // SFX
    private AudioSource laserSound;
    public GameObject laser;

    // Start is called before the first frame update
    void Start()
    {
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
        scoreObject = GameObject.FindGameObjectWithTag("Score");
        score = scoreObject.GetComponent<Score>();

        finalPoint = new Vector3(17.43292f, transform.position.y, transform.position.z); // Where the bird ends up
        laserSound = GetComponent<AudioSource>();
        bg = GameObject.FindGameObjectWithTag("Background");
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            Vector3 newPosition = transform.position + new Vector3(0f, -1 * 20f * Time.deltaTime, 0f);
            if (newPosition.y <= -7f)
            {
                Destroy(gameObject);
            }
            transform.position = newPosition;
        } 
        else
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, finalPoint, step);


            if (transform.position == finalPoint && !canShoot)
            {
                cooldown += Time.deltaTime;
                if ((cooldown >= 0.5f && timesShot == 0) || (cooldown >=3f && timesShot > 0))
                {
                    state = 3;
                    canShoot = true;
                    sprite.sprite = state3;
                    laserSound.Play();
                    cooldown = 0f;
                    StartCoroutine(ShootLaser());
                 
                }
            }
            spriteTimer += Time.deltaTime;

            if (spriteTimer >= .1f )
            {
                spriteTimer = 0;
                if (!canShoot) // Eyes are Green
                    if (state == 1) { sprite.sprite = state2; state = 2; } 
                    else { sprite.sprite = state1; state = 1; }
                else // Eyes Are Red (same animations)
                    if (state == 3) { sprite.sprite = state4; state = 4; }
                    else { sprite.sprite = state3; state = 3; }
            } 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            health--;

            if (health > 0)
            {
                StartCoroutine(FlashSprite());
                score.targetScore += 5;
            }
            else if (!dead)
            {
                sprite.color = Color.gray;
                state = 3;
                sprite.sprite = state3;
                transform.Rotate(0f, 0.0f, 180f, Space.World);
                dead = true;
                score.targetScore += 450;

                if (laserSound.isPlaying)
                    laserSound.Stop();

                WaveTwo wave = bg.GetComponent<WaveTwo>();

                if (wave.initialRoboDown == false)
                    wave.initialRoboDown = true;
                else
                    wave.robosDown++;
            }


        }

    }
   private System.Collections.IEnumerator FlashSprite()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = originalColor;
        
    }

    private System.Collections.IEnumerator ShootLaser()
    {
        yield return new WaitForSeconds(2.29f);
        Instantiate(laser, new Vector3(transform.position.x - 4.27632f, transform.position.y + 3.41f, 0), Quaternion.identity);
        canShoot = false;
        if (state == 3) { sprite.sprite = state2; state = 2; }
        else { sprite.sprite = state1; state = 1; }

        timesShot++;
    }
}
