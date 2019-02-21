using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    // Stores the enemy prefab to be used for every enemy
    GameObject enemyPrefab;
    // Stores all sprites for the different types of enemies
    Sprite[] enemySprites;
    // Stores all the spawn points for the enemies
    GameObject[] enemySpawnPoints;

    GameObject[] currentEnemies;

    [SerializeField]
    int numberOfCurrentEnemies;

    // Speed of the enemy formation
    const float SPEED = 2.0f;

    // Flag to show if the formation is moving left
    [SerializeField]
    bool isMovingLeft;
    [SerializeField]
    bool isMovingLeftWaiting;
    // Flag to show if the formation is moving right
    [SerializeField]
    bool isMovingRight;
    [SerializeField]
    bool isMovingRightWaiting;

    // Timer before moving again
    [SerializeField]
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

    bool respawnEnemies;

    bool initialTimer;

    [SerializeField]
    float respawnEnemiesTimer;
    const float RESPAWN_ENEMIES_TIMER_MAX = 1.0f;
    
	// Use this for initialization
	void Start () {
        // Load the prefab for the Enemy object
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        
        // Load the sprites for the Enemy object
        enemySprites = Resources.LoadAll<Sprite>("ShipSprites/Enemy");

        // Store pointers to all enemy spawn points
        enemySpawnPoints = GameObject.FindGameObjectsWithTag("Waypoint");
        currentEnemies = new GameObject[enemySpawnPoints.Length];
        numberOfCurrentEnemies = 0;

        isMovingLeft = false;
        isMovingRight = false;
        isMovingRightWaiting = true;
        isMovingLeftWaiting = false;

        float minHeight = 999;
        float maxHeight = -999;
        float minWidth = 999;
        float maxWidth = -999;

        Vector3 thisPosition;
        // Spawn an enemy at each spawn point
        // foreach (GameObject g in enemySpawnPoints)
        for (int i = 0; i < currentEnemies.Length; i++)
        {
            int index = RandomIndex();
            if (currentEnemies[index] != null)
            {
                // Already taken
                i--;
                continue;
            }

            GameObject g = enemySpawnPoints[index];
            thisPosition = g.transform.position;
            SpawnEnemy(thisPosition, index);

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

        respawnEnemies = false;
        initialTimer = true;
        moveTimer = MOVE_TIMER_MAX;
	}

    void Update ()
    {
        if (initialTimer)
        {
            moveTimer -= Time.deltaTime;
            if (moveTimer <= 0)
            {
                initialTimer = false;

                if (isMovingRightWaiting)
                {
                    isMovingRightWaiting = false;
                    isMovingRight = true;
                }
                else
                {
                    isMovingLeftWaiting = false;
                    isMovingLeft = true;
                }
            }

            return;
        }

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

        if (respawnEnemies)
        {
            respawnEnemiesTimer -= Time.deltaTime;
            if (respawnEnemiesTimer <= 0.0f)
            {
                RespawnNextEnemyRandom();
                respawnEnemiesTimer = RESPAWN_ENEMIES_TIMER_MAX;
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
        GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        enemy.GetComponent<EnemyController>().Player = GameObject.Find("Player").GetComponent<PlayerController>();

        for (int i = 0; i < currentEnemies.Length; i++)
        {
            if (currentEnemies[i] == null)
            {
                currentEnemies[i] = enemy;
                enemy.transform.parent = enemySpawnPoints[i].transform;
                break;
            }
        }

        // Sets the parent object of the spawned enemy
        //enemy.transform.parent = transform;

        // Choose a random sprite for the prefab
        int index = RandomIndex(0, enemySprites.Length);
        enemy.GetComponent<SpriteRenderer>().sprite = enemySprites[index];

        numberOfCurrentEnemies++;
        if (numberOfCurrentEnemies == currentEnemies.Length)
        {
            respawnEnemies = false;
        }
    }

    /// <summary>
    /// Spawns and enemy at the given location
    /// </summary>
    /// <param name="position">Starting position of the new enemy</param>
    void SpawnEnemy(Vector3 position, int index)
    {
        // Instantiates an enemy object
        GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        //enemy.GetComponent<EnemyController>().Player = GameObject.Find("Player").GetComponent<PlayerController>();

        currentEnemies[index] = enemy;
        enemy.transform.parent = enemySpawnPoints[index].transform;

        // Sets the parent object of the spawned enemy
        //enemy.transform.parent = transform;

        // Choose a random sprite for the prefab
        int spriteIndex = RandomIndex(0, enemySprites.Length);
        enemy.GetComponent<SpriteRenderer>().sprite = enemySprites[spriteIndex];

        numberOfCurrentEnemies++;
        if (numberOfCurrentEnemies == currentEnemies.Length)
        {
            respawnEnemies = false;
        }
    }

    public int NumberOfCurrentEnemies
    {
        get
        {
            return numberOfCurrentEnemies;
        }
    }

    /*void RespawnNextEnemy()
    {
        for(int i = 0; i < currentEnemies.Length; i++)
        {
            if (currentEnemies[i] == null)
            {
                SpawnEnemy(enemySpawnPoints[i].transform.position);
                return;
            }
        }
    }*/

    void RespawnNextEnemyRandom()
    {
        for (int i = 0; i < currentEnemies.Length; i++)
        {
            int index = RandomIndex(0, currentEnemies.Length);
            Debug.Log(index);
            if (currentEnemies[index] == null)
            {
                SpawnEnemy(enemySpawnPoints[i].transform.position, index);
                return;
            }
            else
            {
                i--;
                continue;
            }
        }
    }

    public void AllEnemiesAreDead()
    {
        respawnEnemies = true;

        respawnEnemiesTimer = RESPAWN_ENEMIES_TIMER_MAX;
    }

    public void EnemyDied()
    {
        numberOfCurrentEnemies--;

        if (numberOfCurrentEnemies == 0)
        {
            AllEnemiesAreDead();
        }
    }

    int RandomIndex()
    {
        return (int)Mathf.Round(Random.Range(0, currentEnemies.Length));
    }

    int RandomIndex(int min, int max)
    {
        return (int)Mathf.Round(Random.Range(min, max));
    }

    int RandomIndex(GameObject[] g)
    {
        return (int)Mathf.Round(Random.Range(0, g.Length));
    }
}
