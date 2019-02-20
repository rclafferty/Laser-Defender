using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    // Stores the enemy prefab to be used for every enemy
    GameObject enemyPrefab;
    // Stores all sprites for the different types of enemies
    Sprite[] enemySprites;
    // Stores all the spawn points for the enemies
    GameObject[] enemySpawnPoints;

    // Speed of the enemy formation
    const float SPEED = 2.0f;

    // Flag to show if the formation is moving left
    bool isMovingLeft;
    bool isMovingLeftWaiting;
    // Flag to show if the formation is moving right
    bool isMovingRight;
    bool isMovingRightWaiting;

    // Timer before moving again
    float moveTimer;
    const float MOVE_TIMER_MAX = 5.0f;

    // Padding for formation position
    const float PADDING = 0.5f;

    float formationHeight;
    float formationWidth;

    // Camera/Graphics variables
    float xmin;
    float xmax;
    float ymin;
    float ymax;

	// Use this for initialization
	void Start () {
        // Load the prefab for the Enemy object
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        
        // Load the sprites for the Enemy object
        enemySprites = Resources.LoadAll<Sprite>("ShipSprites/Enemy");

        // Store pointers to all enemy spawn points
        enemySpawnPoints = GameObject.FindGameObjectsWithTag("Waypoint");

        isMovingLeft = true;
        isMovingRight = false;

        float minHeight = 999;
        float maxHeight = -999;
        float minWidth = 999;
        float maxWidth = -999;

        Vector3 thisPosition;
        // Spawn an enemy at each spawn point
        foreach (GameObject g in enemySpawnPoints)
        {
            thisPosition = g.transform.position;
            SpawnEnemy(thisPosition);

            if (thisPosition.x < minWidth)
            {
                minWidth = thisPosition.x;
            }
            if (thisPosition.x > maxWidth)
            {
                maxWidth = thisPosition.x;
            }
            if (thisPosition.y < minHeight)
            {
                minHeight = thisPosition.y;
            }
            if (thisPosition.y > maxHeight)
            {
                maxHeight = thisPosition.y;
            }
        }

        // Calculate the formation height and width
        formationHeight = maxHeight - minHeight + PADDING;
        formationWidth = maxWidth - minWidth + PADDING;

        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distanceToCamera));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, distanceToCamera));
        Vector3 upmost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 1f, distanceToCamera));
        Vector3 downmost = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, distanceToCamera));

        xmax = rightmost.x - PADDING;
        xmin = leftmost.x + PADDING;
        ymax = upmost.y - PADDING;
        ymin = downmost.y + PADDING;
	}

    void Update ()
    {
        if (isMovingRight)
        {
            transform.position += Vector3.right * SPEED * Time.deltaTime;
        }
        else if (isMovingLeft)
        {
            transform.position += Vector3.left * SPEED * Time.deltaTime;
        }

        float rightEdgeOfFormation = transform.position.x + (formationWidth * 0.5f);
        float leftEdgeOfFormation = transform.position.x - (formationWidth * 0.5f);
        if (leftEdgeOfFormation < xmin || rightEdgeOfFormation > xmax)
        {
            // Just hit the edge
            if (!isMovingLeftWaiting && !isMovingRightWaiting)
            {
                moveTimer = MOVE_TIMER_MAX;

                // If it was moving left
                if (isMovingLeft)
                {
                    // Wait to move right
                    isMovingLeft = false;
                    isMovingRightWaiting = true;
                }
                // If it was moving right
                else if (isMovingRight)
                {
                    // Wait to move left
                    isMovingRight = false;
                    isMovingLeftWaiting = true;
                }
            }
            // Waiting to move
            else
            {
                moveTimer -= Time.deltaTime;

                if (moveTimer <= 0.0f)
                {
                    moveTimer = 0.0f;
                    
                    if (isMovingRightWaiting)
                    {
                        // Waiting to move right
                        isMovingRightWaiting = false;
                        isMovingRight = true;
                    }
                    else if (isMovingLeftWaiting)
                    {
                        // Waiting to move left
                        isMovingLeftWaiting = false;
                        isMovingLeft = true;
                    }
                }
            }
        }
    }
	
    /// <summary>
    /// Spawns and enemy at the given location
    /// </summary>
    /// <param name="position">Starting position of the new enemy</param>
    void SpawnEnemy(Vector3 position)
    {
        // Instantiates an enemy object
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity) as GameObject;
        
        // Sets the parent object of the spawned enemy
        enemy.transform.parent = transform;

        // Choose a random sprite for the prefab
        int index = (int)Random.Range(0, enemySprites.Length - 1);
        enemy.GetComponent<SpriteRenderer>().sprite = enemySprites[index];
    }
}
