using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroids : MonoBehaviour
{

    public float moveSpeed = 5f;
    public float maxYPosition = 4.06f;
    public float minYPosition = -4.1f;
    public int health = 15;
    public bool dead = false;

    public Sprite state1;
    public Sprite state2;
    public Sprite state3;
    public Sprite state4;

    public int state = 1; // 1 = state 1, 2 = state 2, propeller changes

    private float timer = 0f; // Timer to change sprite

    private int direction = 1; // 1 for moving up, -1 for moving down
    private Color originalColor;
    private SpriteRenderer sprite;

    // Score
    public GameObject scoreObject;
    private Score score;


    // Start is called before the first frame update
    void Start()
    {
        System.Random random = new System.Random();
        float randomY = (float)random.NextDouble() * (3.78f + 1.54f) - 1.54f;
        float randomRot = (float)random.NextDouble() * (360 - 0);

        sprite = gameObject.AddComponent<SpriteRenderer>();

        sprite.sprite = state1;
        originalColor = sprite.color;

        if (transform.position.y == 0f)
            transform.position = new Vector3(28f, randomY, 0f);
        transform.Rotate(0f, 0.0f, randomRot, Space.World);
        transform.localScale = new Vector3(7f, 7f, 7f);

        scoreObject = GameObject.FindGameObjectWithTag("Score");
        score = scoreObject.GetComponent<Score>();

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
        else if (Time.timeScale != 0f)
        {
            transform.Rotate(0f, 0f, 1f, Space.Self);
            Vector3 newPosition = transform.position + new Vector3(-1 * moveSpeed * Time.deltaTime, 0f, 0f);
            transform.position = newPosition;
        }

        if (transform.position.x <= -28f)
        {
            Destroy(gameObject);
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
                score.targetScore += 100;
            }
            else
            {
                state = 4;
                sprite.sprite = state4;
                transform.localScale = new Vector3(.5f, .5f, .5f);
                transform.Rotate(0f, 0.0f, 180f, Space.World);
                dead = true;
                score.targetScore += 250;
            }


        }

    }

    private System.Collections.IEnumerator FlashSprite()
    {
        state = 2;
        sprite.sprite = state2;

        yield return new WaitForSeconds(0.1f);

        sprite.color = originalColor;
        state = 1;
        sprite.sprite = state1;
    }
}
