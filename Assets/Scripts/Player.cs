using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject laser;
    const float PROJECTILE_FIRING_RATE = 0.1f;
    const float PROJECTILE_SPEED = 10f;
    int health;
    Coroutine firingCoroutine;

    // Clamp to screen
    [SerializeField] float xmin, xmax, ymin, ymax;
    float padding;

    // Movement variables
    float speed;

    // Input values
    float horizontalInput, verticalInput;
    const float DEAD_VALUE = 0.2f;

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] AudioClip explosionSFX;
    [SerializeField] [Range(0, 1)] float explosionSFXVolume = 0.75f;

    [SerializeField] AudioClip fireSFX;
    [SerializeField] [Range(0, 1)] float fireSFXVolume = 0.15f;

    // Start is called before the first frame update
    void Start()
    {
        speed = 15f;
        health = 200;
        padding = 1f;

        ClampValuesToScreen();
        UpdateHealthGUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            firingCoroutine = StartCoroutine(FireContinuously());
        }

        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }

        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontalInput) < DEAD_VALUE)
        {
            horizontalInput = 0;
        }
        if (Mathf.Abs(verticalInput) < DEAD_VALUE)
        {
            verticalInput = 0;
        }

        if (Mathf.Abs(horizontalInput) >= DEAD_VALUE || Mathf.Abs(verticalInput) >= DEAD_VALUE)
        {
            horizontalInput *= speed * Time.deltaTime;
            verticalInput *= speed * Time.deltaTime;
            Vector3 newPosition = transform.position + new Vector3(horizontalInput, verticalInput);

            newPosition.x = Mathf.Clamp(newPosition.x, xmin, xmax);
            newPosition.y = Mathf.Clamp(newPosition.y, ymin, ymax);

            transform.position = newPosition;
        }
    }

    void ClampValuesToScreen()
    {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        xmin = bounds.x;
        xmax = bounds.x;
        ymin = bounds.y;
        ymax = bounds.y;

        xmin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x + padding;
        xmax = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x - padding;
        ymin = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).y + padding;
        ymax = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance)).y - padding;
    }

    IEnumerator FireContinuously()
    {
        GameObject laserObject;
        while (true)
        {
            laserObject = Instantiate(laser, transform.position, Quaternion.identity);
            laserObject.name = "Laser";
            laserObject.GetComponent<Rigidbody2D>().velocity = Vector2.up * PROJECTILE_SPEED;

            AudioSource.PlayClipAtPoint(fireSFX, Camera.main.transform.position, fireSFXVolume);

            yield return new WaitForSeconds(PROJECTILE_FIRING_RATE);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Enemy")
        {
            Die();
            return;
        }

        if (collision.name == "Laser")
        {
            return;
        }

        DamageDealer damageDealer = collision.GetComponent<DamageDealer>();
        ProcessHit(damageDealer);
        Destroy(collision.gameObject);
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
            health = 0;
            Die();
        }

        UpdateHealthGUI();
    }

    void Die()
    {
        Destroy(gameObject);
        GameObject g = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(g.gameObject, 1f);

        AudioSource.PlayClipAtPoint(explosionSFX, Camera.main.transform.position, explosionSFXVolume);
        GameObject.Find("Level").GetComponent<Level>().LoadGameOver();
    }

    void UpdateHealthGUI()
    {
        GameObject.Find("Health Text").GetComponent<TextMeshProUGUI>().text = "Health: " + health.ToString();
    }
}
