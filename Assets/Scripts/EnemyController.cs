using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    PlayerController player;
    [SerializeField]
    GameObject laserBeam;

    [SerializeField]
    int health;

    [SerializeField]
    float firingTimer;
    const float FIRING_TIMER_MIN = 3.0f;
    const float FIRING_TIMER_MAX = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
        health = 2;

        laserBeam = Resources.Load<GameObject>("Prefabs/EnemyLaser");
        
        firingTimer = Random.Range(FIRING_TIMER_MIN, FIRING_TIMER_MAX);
    }

    // Update is called once per frame
    void Update()
    {
        firingTimer -= Time.deltaTime;
        if (firingTimer <= 0.0f)
        {
            Debug.Log("Enemy firing laser");

            // Fire the laser
            GameObject laser = Instantiate(laserBeam, transform.position, Quaternion.identity) as GameObject;
            

            // Reset timer
            firingTimer = Random.Range(FIRING_TIMER_MIN, FIRING_TIMER_MAX);
        }
    }

    public PlayerController Player
    {
        get
        {
            return player;
        }
        set
        {
            if (value != null)
            {
                player = value;
            }
        }
    }

    public void Hit()
    {
        Debug.Log("Hit Enemy");

        health--;

        if (health == 0)
        {
            Kill();
        }
    }

    void Kill()
    {
        GameObject.Find("EnemyFormation").GetComponent<EnemySpawner>().EnemyDied();
        Destroy(gameObject);
    }
}
