using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    // Stores the enemy prefab to be used for every enemy
    GameObject enemyPrefab;
    // Stores all sprites for the different types of enemies
    Sprite[] enemySprites;
    // Stores all the spawn points for the enemies
    GameObject[] enemySpawnPoints;

	// Use this for initialization
	void Start () {
        // Load the prefab for the Enemy object
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        
        // Load the sprites for the Enemy object
        enemySprites = Resources.LoadAll<Sprite>("ShipSprites/Enemy");

        // Store pointers to all enemy spawn points
        enemySpawnPoints = GameObject.FindGameObjectsWithTag("Waypoint");

        // Spawn an enemy at each spawn point
        foreach (GameObject g in enemySpawnPoints)
        {
            SpawnEnemy(g.transform.position);
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
