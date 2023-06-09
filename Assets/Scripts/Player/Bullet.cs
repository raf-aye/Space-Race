using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float moveSpeed = 25f;
    
    // Start is called before the first frame update
    void Start()
    {
        AudioSource a = GetComponent<AudioSource>();
        a.Play();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(moveSpeed, 0f, 0f) * Time.deltaTime;
        transform.Translate(movement);

        if (transform.position.x > 28f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
