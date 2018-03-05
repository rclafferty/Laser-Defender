using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    GameObject enemyPrefab;

	// Use this for initialization
	void Start () {
        enemyPrefab = Resources.Load<GameObject>("Prefabs/Enemy");
        GameObject enemy = Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity) as GameObject;
        enemy.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
