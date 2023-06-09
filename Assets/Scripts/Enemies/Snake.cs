using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    public float timer = 0f;
    public int state = 1;
    public float speed = 10f;
    public float rotationSpeed = 5f;

    public Sprite state1;
    public Sprite state2;

    private SpriteRenderer sprite;
    private GameObject target;
    public bool targetPlayer = true;
    public bool collidedPlayer = false;
    private Color originalColor;
     
    public bool dead = false;
    public int health = 5;
    private Score score;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        target = GameObject.FindGameObjectWithTag("Player");
        originalColor = sprite.color;
        GameObject scoreObject = GameObject.FindGameObjectWithTag("Score");
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
            timer += Time.deltaTime;

            if (timer >= .1f && state != 3)
            {
                timer = 0;
                if (state == 1) { sprite.sprite = state2; state = 2; }
                else { sprite.sprite = state1; state = 1; }
            }

            Vector2 direction = target.transform.position - transform.position;


            if (transform.position.y >= 15f || transform.position.y <= -15f || transform.position.x >= 30f || transform.position.x <= -30f)
            {
                Destroy(gameObject);
            }

            if (direction.x >= 0f || !targetPlayer)
            {
                // If the target is to the left of the missile, do not target
                Color c = sprite.color;
                c.a = 0.5f;
                sprite.color = c;

                speed = 25f;
                transform.Translate(transform.right * -speed * Time.deltaTime, Space.World);
                targetPlayer = false;
                return;
            }

            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

            // Rotate towards the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle = NormalizeAngle(angle); // Normalize the angle between -90 and 90 degrees
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private float NormalizeAngle(float angle)
    {
        // Normalize the angle to be between -90 and 90 degrees
        angle %= 360f;
        if (angle > 90f)
        {
            angle -= 180f;
        }
        else if (angle < -90f)
        {
            angle += 180f;
        }
        return angle;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet") && !dead && targetPlayer)
        {
            health--;

            if (health > 0)
            {
                StartCoroutine(FlashSprite());
                score.targetScore += 50;
            }
            else if (!dead)
            {
                sprite.color = Color.gray;
                transform.Rotate(0f, 0.0f, 180f, Space.World);
                dead = true;
                score.targetScore += 150;
            }


        } else if (collision.gameObject.CompareTag("Player") && !dead && targetPlayer)
        {
            targetPlayer = false;
            collidedPlayer = true;
        }

    }

    private System.Collections.IEnumerator FlashSprite()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = originalColor;
    }
}
