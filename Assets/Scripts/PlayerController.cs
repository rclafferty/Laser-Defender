using UnityEngine;

public class PlayerController : MonoBehaviour {
    
    // Allows this object to be a singleton
    private static PlayerController thisPlayer = null;

    // Physics variables
    private float speed;

    // Input variables
    private float horizontalInput;
    private float verticalInput;

    // Camera/Graphics variables
    private float padding;
    private float xmin;
    private float xmax;
    private float ymin;
    private float ymax;

    // Tracks if the player is active in this scene
    //     If not, hides the player from the camera
    private bool isActive;
    
    private void Awake()
    {
        // Checks if another instance of this object exists
        if (thisPlayer == null)
        {
            // Sets the singleton
            thisPlayer = this;
            // Declares not to destroy this object between scenes
            DontDestroyOnLoad(this);
            // Hides from camera and disables input
            SetActive(false);
        }
        else
        {
            // Another instance already exists
            Destroy(this.gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        // Initialize physics values
        speed = 15f;

        // Initialize input values
        horizontalInput = 0;
        verticalInput = 0;

        // Initialize camera values
        padding = 1f;
        
        // Find distance between camera and this object
        float distance = transform.position.z - Camera.main.transform.position.z;
        // Find the leftmost boundary for the camera view
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
        // Find the rightmost boundary for the camera view
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, distance));
        // Find the top boundary for the camera view
        Vector3 upmost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, distance));
        // Find the bottom boundary for the camera view
        Vector3 downmost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distance));
        
        // Set play space boundaries (with padding)
        xmin = leftmost.x + padding;
        xmax = rightmost.x - padding;
        ymin = downmost.y + padding;
        ymax = upmost.y - padding;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // If in a level, not a menu
        if (isActive)
        {
            // Handle "fire" input
            if (Input.GetAxis("Fire") > 0)
            {
                Debug.Log("Shot fired");
            }

            // Take input from WASD or Arrow Keys
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            
            // Handle movement
            if (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0)
            {
                transform.position += new Vector3(horizontalInput * speed * Time.deltaTime, verticalInput * speed * Time.deltaTime, 0f);
            }

            // Make sure player is not outside of play space boundaries
            float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
            float newY = Mathf.Clamp(transform.position.y, ymin, ymax);
            transform.position = new Vector3(newX, newY, transform.position.z);
        }
	}

    public void SetActive(bool tf)
    {
        if (tf)
        {
            // Reset at bottom of camera view
            transform.position = new Vector3(0, -4f, 0);
            isActive = true;
        }
        else
        {
            // Hide from camera
            transform.position = new Vector3(0, -200f, 0);
            isActive = false;
        }
    }
}
