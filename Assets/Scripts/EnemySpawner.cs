using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;
    int startingWave;
    bool looping = true;

    [SerializeField] List<Sprite> enemySprites;
    [SerializeField] List<Sprite> enemyLaserSprites;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        float waveSpawnDelay = 10;
        int additionalEnemies = 0;

        do
        {
            yield return StartCoroutine(SpawnAllWaves(waveSpawnDelay, additionalEnemies));
            waveSpawnDelay -= Time.deltaTime;
            additionalEnemies++;
        } while (looping);
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SpawnAllWaves(float delay, int additionalEnemies)
    {
        foreach (WaveConfig w in waveConfigs)
        {
            StartCoroutine(SpawnAllEnemiesInWave(w, delay, additionalEnemies));
            yield return new WaitForSeconds(delay);

            delay -= Time.deltaTime;
        }
    }

    IEnumerator SpawnAllEnemiesInWave(WaveConfig thisWave, float delay, int additionalEnemies)
    {
        GameObject thisObject;
        int randomSpriteIndex = -1;
        int randomLaserIndex = -1;
        for (int i = 0; i < thisWave.EnemyCountPerWave + additionalEnemies; i++)
        {
            thisObject = Instantiate(thisWave.EnemyPrefab, thisWave.PathWaypoints[0].position, Quaternion.identity);
            thisObject.GetComponent<EnemyPathing>().WaveConfig = thisWave;
            if (thisWave.name == "Purple Wave")
            {
                thisObject.name = "Big Boy";
            }
            else
            {
                thisObject.name = "Enemy";

                if (enemySprites != null)
                {
                    randomSpriteIndex = Random.Range(0, enemySprites.Count);
                    thisObject.GetComponent<SpriteRenderer>().sprite = enemySprites[randomSpriteIndex];
                }

                if (enemyLaserSprites != null)
                {
                    randomLaserIndex = Random.Range(0, enemyLaserSprites.Count);
                    thisObject.GetComponent<Enemy>().randomLaserSprite = enemyLaserSprites[randomLaserIndex];
                }
            }

            yield return new WaitForSeconds(thisWave.WaveSpawnDelay);
        }
    }
}
