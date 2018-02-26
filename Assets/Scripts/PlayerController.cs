using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private static PlayerController thisPlayer = null;

    private float speed;
    private float horizontalInput;
    private float verticalInput;
    private float padding;

    private float xmin;
    private float xmax;
    private float ymin;
    private float ymax;

    private bool isActive;

    private void Awake()
    {
        if (thisPlayer == null)
        {
            thisPlayer = this;
            DontDestroyOnLoad(this);
            SetActive(false);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        speed = 15f;
        horizontalInput = 0;
        verticalInput = 0;
        padding = 1f;
        
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, distance));
        Vector3 upmost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, distance));
        Vector3 downmost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distance));

        Debug.Log("upmost: " + upmost);
        Debug.Log("downmost: " + downmost);
        Debug.Log("leftmost: " + leftmost);
        Debug.Log("rightmost: " + rightmost);
        xmin = leftmost.x + padding;
        xmax = rightmost.x - padding;
        ymin = downmost.y + padding;
        ymax = upmost.y - padding;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isActive)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            if (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0)
            {
                transform.position += new Vector3(horizontalInput * speed * Time.deltaTime, verticalInput * speed * Time.deltaTime, 0f);
            }

            float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
            float newY = Mathf.Clamp(transform.position.y, ymin, ymax);
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
	}

    public void SetActive(bool tf)
    {
        if (tf)
        {
            Debug.Log("Set active");
            isActive = true;
            transform.position = new Vector3(0, -4f, 0);
        }
        else
        {
            Debug.Log("Set INACTIVE");
            // Hide from camera
            transform.position = new Vector3(0, -200f, 0);
            isActive = false;
        }
    }
}
