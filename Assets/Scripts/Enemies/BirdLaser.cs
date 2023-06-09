using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdLaser : MonoBehaviour
{

    public float horizSpeed = 25f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position + new Vector3(-1 * horizSpeed * Time.deltaTime, 0f, 0f);

        transform.position = newPosition;
        if (transform.position.x <= -28f)
        {
            Destroy(gameObject);
        }
    }
}
