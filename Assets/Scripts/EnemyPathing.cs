using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathing : MonoBehaviour
{
    WaveConfig waveConfig;
    List<Transform> waypoints;
    int waypointIndex;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (waypointIndex < waypoints.Count)
        {
            Vector3 targetPosition = waypoints[waypointIndex].transform.position;
            float movementThisFrame = waveConfig.MoveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);

            if (transform.position == targetPosition)
                waypointIndex++;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public WaveConfig WaveConfig
    {
        set
        {
            if (value != null)
            {
                waveConfig = value;
                waypoints = waveConfig.PathWaypoints;
            }
        }
    }
}
