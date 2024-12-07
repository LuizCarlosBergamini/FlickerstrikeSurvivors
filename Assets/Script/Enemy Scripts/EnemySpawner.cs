using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    [System.Serializable]
    public class Wave {
        public string waveName;
        public List<EnemyGroup> enemyGroups; // list of enemy prefabs to spawn
        public int waveQuota; // number of enemies to spawn in the wave
        public float spawnInterval; // time elapsed between each enemy spawn
        public int spawnCount; // number of enemies spawned so far
    }

    [System.Serializable]
    public class EnemyGroup {
        public string enemyName; // list of names for each enemy type
        public GameObject enemyPrefab; // list of enemy prefabs to spawn
        public int enemyCount; // list of quotas for each enemy type
        public int spawnCount; // number of enemies spawned so far

    }

    public List<Wave> waves; // list of all waves in the game
    public int currentWaveCount; // index of the current wave
    public float waveInterval; // the interval between each wave

    Transform player;

    [Header("Spawner Attributes")]
    float spawnTimer; // time used to determine the next spawn

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
        CalculateWaveQuota();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0) // Check if the wave has ended and the next should start
        {
            StartCoroutine(BeginNextWave());
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        // Wave for 'waveInterval' seconds before starting the next wave
        yield return new WaitForSeconds(waveInterval);

        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
        }
        else
        {
            Debug.LogWarning("All waves have been completed.");
        }

    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (EnemyGroup enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning("Wave " + waves[currentWaveCount].waveName + " has a quota of " + currentWaveQuota + " enemies.");
    }

    void SpawnEnemies()
    {
        Debug.Log("Spawning enemies...");
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota)
        {
            foreach (EnemyGroup enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                if (enemyGroup.spawnCount < enemyGroup.enemyCount && player != null)
                {
                    Vector2 spawnPosition = new Vector2(player.transform.position.x + Random.Range(-10, 10), player.transform.position.y + Random.Range(-10, 10));
                    Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);
                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                }
            }
        }
    }
}
