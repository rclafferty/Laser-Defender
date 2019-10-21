using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] GameObject laser;
    [SerializeField] int health;

    float shotCounter;
    [SerializeField] float minTimeBetweenShots;
    [SerializeField] float maxTimeBetweenShots;

    public Sprite randomLaserSprite;

    float PROJECTILE_SPEED = 8f;

    [SerializeField] GameObject explosionPrefab;

    [SerializeField] AudioClip explosionSFX;

    [SerializeField] [Range(0, 1)] float explosionSFXVolume = 0.75f;

    [SerializeField] AudioClip fireSFX;
    [SerializeField] [Range(0, 1)] float fireSFXVolume = 0.15f;

    string laserText;

    // Start is called before the first frame update
    void Start()
    {
        if (name == "Big Boy")
        {
            health = 600;
            PROJECTILE_SPEED = 2f;
            laserText = "Enemy Bomb";
        }
        else
        {
            health = 100;
            PROJECTILE_SPEED = 8f;
            laserText = "Enemy Laser";
        }

        minTimeBetweenShots = 0.2f;
        maxTimeBetweenShots = 3f;
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;

        if (shotCounter <= 0)
        {
            // Shoot
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    void Fire()
    {
        GameObject laserObject = Instantiate(laser, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
        laserObject.GetComponent<Rigidbody2D>().velocity = Vector2.down * PROJECTILE_SPEED;
        laserObject.GetComponent<SpriteRenderer>().sprite = randomLaserSprite;
        laserObject.name = laserText;

        AudioSource.PlayClipAtPoint(fireSFX, Camera.main.transform.position, fireSFXVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Enemy Laser" || collision.name == "Enemy Bomb")
            return;

        if (collision.GetComponent<Enemy>() != null)
            return;

        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        Destroy(collision.gameObject);
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        if (damageDealer == null)
        {
            return;
        }

        health -= damageDealer.Damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
        GameObject g = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(g.gameObject, 1f);

        AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position, explosionSFXVolume);

        if (name == "Big Boy")
        {
            GameObject.Find("Game Session").GetComponent<GameSession>().AddToScore(200);
        }
        else
        {
            GameObject.Find("Game Session").GetComponent<GameSession>().AddToScore(100);
        }
    }

    public GameObject LaserPrefab
    {
        get
        {
            return laser;
        }
        set
        {
            if (value != null)
            {
                laser = value;
            }
        }
    }
}
