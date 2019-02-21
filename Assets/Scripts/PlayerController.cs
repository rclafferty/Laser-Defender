using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    
    // Allows this object to be a singleton
    private static PlayerController thisPlayer = null;

    // Laser Beam to shoot when firing
    [SerializeField]
    private GameObject laserBeam;

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

    int numberOfEnemies;
    int score;

    private const int HEALTH_MAX = 10;
    private int health;

    private void Awake()
    {
        // Checks if another instance of this object exists
        if (thisPlayer == null)
        {
            // Sets the singleton
            thisPlayer = this;
            // Declares not to destroy this object between scenes
            DontDestroyOnLoad(this);
            if (SceneManager.GetActiveScene().name == "Game")
            {
                // Resets at bottom of play space
                SetActive(true);
            }
            else
            {
                // Hides from camera and disables input
                SetActive(false);
            }
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

        laserBeam = Resources.Load<GameObject>("Prefabs/PlayerLaser");

        numberOfEnemies = 0;

        health = HEALTH_MAX;
        GameObject.Find("HealthText").GetComponent<Text>().text = "Health:  " + health;

        score = 0;
        GameObject.Find("ScoreText").GetComponent<Text>().text = "Score:  " + score;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // If in a level, not a menu
        if (isActive)
        {
            // Handle "fire" input
            if (Input.GetButtonDown("Fire"))
            {
                Debug.Log("Shot fired");

                GameObject laser = Instantiate(laserBeam, transform.position, Quaternion.identity) as GameObject;
                //laser.GetComponent<Rigidbody2D>().velocity = new Vector3(0, )
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

    public void UpdateNumberOfEnemies(int enemies)
    {
        if (enemies < 0)
        {
            throw new System.InvalidOperationException("Can't have negative enemies");
        }
        else if (enemies == 0)
        {
            Debug.Log("All enemies are dead!");
            // Win!
            //GameObject.Find("EnemyFormation").GetComponent<EnemySpawner>().AllEnemiesAreDead();
        }
        else
        {
            numberOfEnemies = enemies;
        }
    }

    public void KilledEnemy()
    {
        score += 100;
        GameObject.Find("ScoreText").GetComponent<Text>().text = "Score:  " + score;
        UpdateNumberOfEnemies(GameObject.Find("EnemyFormation").GetComponent<EnemySpawner>().NumberOfCurrentEnemies);
    }

    public void Hit()
    {
        health--;
        GameObject.Find("HealthText").GetComponent<Text>().text = "Health:  " + health;

        if (health == 0)
        {
            // Lose! :(
            Destroy(gameObject);
        }
    }
}
