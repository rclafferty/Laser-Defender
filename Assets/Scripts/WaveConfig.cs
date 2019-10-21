using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float timeBetweenSpawns = 1f;
    [SerializeField] float spawnRandomFactor = 0.3f;
    [SerializeField] int numberOfEnemies = 5;
    [SerializeField] float moveSpeed = 2;

    public GameObject EnemyPrefab
    {
        get
        {
            return enemyPrefab;
        }
    }

    public List<Transform> PathWaypoints
    {
        get
        {
            List<Transform> waypoints = new List<Transform>();

            foreach (Transform child in pathPrefab.transform)
            {
                waypoints.Add(child);
                // Debug.Log("Child: " + child.ToString());
            }

            return waypoints;
        }
    }

    public int EnemyCountPerWave
    {
        get
        {
            return numberOfEnemies;
        }
    }

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            if (value > 0)
            {
                moveSpeed = value;
            }
        }
    }

    public float WaveSpawnDelay
    {
        get
        {
            return timeBetweenSpawns;
        }
    }

    public float RandomSpawnNoise
    {
        get
        {
            return spawnRandomFactor;
        }
    }
}
