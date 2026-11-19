using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private float spawnInterval = 1.4f;
    [SerializeField] private float spawnHeight = 5.5f;
    [SerializeField] private Vector2 horizontalRange = new Vector2(-7f, 7f);
    [SerializeField] private bool useWaves;
    [SerializeField] private List<int> enemiesPerWave = new List<int> { 6, 10, 14 };

    private float elapsed;
    private int waveIndex;
    private int spawnedInWave;

    public void Configure(GameObject[] prefabs, bool waveMode)
    {
        enemyPrefabs = prefabs;
        useWaves = waveMode;
    }

    private void Update()
    {
        if (GameSession.Instance != null && GameSession.Instance.IsGameOver)
            return;

        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
            return;

        elapsed += Time.deltaTime;
        if (elapsed < spawnInterval)
            return;

        elapsed = 0f;
        SpawnEnemy();

        if (!useWaves || enemiesPerWave.Count == 0)
            return;

        spawnedInWave++;
        if (spawnedInWave >= enemiesPerWave[waveIndex])
        {
            spawnedInWave = 0;
            waveIndex = (waveIndex + 1) % enemiesPerWave.Count;
            spawnInterval = Mathf.Max(0.45f, spawnInterval - 0.15f);
            Debug.Log("Wave " + (waveIndex + 1) + " started!");
        }
    }

    private void SpawnEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        float x = Random.Range(horizontalRange.x, horizontalRange.y);
        Vector3 position = new Vector3(x, spawnHeight, 0f);
        Instantiate(enemyPrefabs[index], position, Quaternion.identity);
    }
}
