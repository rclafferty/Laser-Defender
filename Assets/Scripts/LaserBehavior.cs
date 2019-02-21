using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    // The laser's rigidbody
    Rigidbody2D rigidbody;

    // Speed of the laser beam
    const float SPEED = 2.0f;

    // Boundary variables
    private float ymin;
    private float ymax;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(0, SPEED);

        // Find distance between camera and this object
        float distance = transform.position.z - Camera.main.transform.position.z;
        // Find the top boundary for the camera view
        Vector3 upmost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, distance));
        // Find the bottom boundary for the camera view
        Vector3 downmost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distance));

        // Set play space boundaries (with padding)
        ymin = downmost.y;
        ymax = upmost.y;
    }

    // Update is called once per frame
    void Update()
    {
        // If outside the playspace
        if (transform.position.y > ymax)
        {
            Destroy(gameObject);
        }

        if (rigidbody.velocity == Vector2.zero)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string name = collision.name;
        //Debug.Log(name);

        if (name.ToLower().Contains("enemy") && !name.ToLower().Contains("laser"))
        {
            collision.gameObject.GetComponent<EnemyController>().Hit();

            Destroy(gameObject);
        }
    }
}
