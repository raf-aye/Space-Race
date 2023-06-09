using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyBullet : MonoBehaviour
{

    public GameObject target;
    public float speed = 25f;
    public float rotationSpeed = 5f;

    public bool targetPlayer = true;
    public bool collidedPlayer = false;
    private SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.y >= 15f || transform.position.y <= -15f || transform.position.x >= 30f || transform.position.x <= -30f)
        {
            Destroy(gameObject);
        }

        Vector2 direction = target.transform.position - transform.position;

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
        if (collision.gameObject.CompareTag("Player") && targetPlayer)
        {
            targetPlayer = false;
            collidedPlayer = true;
        }

    }
}
