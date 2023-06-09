using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{

    // Movement
    public float moveSpeed = 5f;
    public float maxYPosition = 4.06f;
    public float minYPosition = -4.1f;
    public float horizSpeed = 10f;
    private int direction = 1; // 1 for moving up, -1 for moving down

    // Health
    public int health = 25;


    // Status
    public bool dead = false;
    public Sprite state1;
    public Sprite state2;
    public Sprite state3;
    public int state = 1; // 1 = state 1, 2 = state 2, propeller changes, 3 = dead


    // Misc
    private float timer = 0f; // Timer to change sprite
    private Color originalColor;
    private SpriteRenderer sprite;

    // Score
    public GameObject scoreObject;
    private Score score;

    // Start is called before the first frame update
    void Start()
    { 
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        originalColor = sprite.color;
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
        else
        {
            Vector3 newPosition = transform.position + new Vector3(-1 * horizSpeed * Time.deltaTime, direction * moveSpeed * Time.deltaTime, 0f);


           

            if (newPosition.y >= maxYPosition)
            {
                moveSpeed = 5f;
                newPosition.y = maxYPosition;
                direction = -1;
            }
            else if (newPosition.y <= minYPosition)
            {
                newPosition.y = minYPosition;
                direction = 1;

            }

            timer += Time.deltaTime;

            if (timer >= .1f && state != 3)
            {
                timer = 0;
                if (state == 1) { sprite.sprite = state2; state = 2; }
                else { sprite.sprite = state1; state = 1; }
            }
            transform.position = newPosition;

            if (transform.position.x <= -28f)
            {
                Destroy(gameObject);
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
                score.targetScore += 50;
            }
            else
            {
                sprite.color = Color.gray;
                state = 3;
                sprite.sprite = state3;
                transform.Rotate(0f, 0.0f, 180f, Space.World);
                dead = true;
                score.targetScore += 150;
            }


        }

    }

    private System.Collections.IEnumerator FlashSprite()
    {
        sprite.color = Color.red;
        state = 3;
        sprite.sprite = state3;

        yield return new WaitForSeconds(0.1f);

        sprite.color = originalColor;
        state = 1;
        sprite.sprite = state1;
    }


}
