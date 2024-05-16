using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemySpawner : MonoBehaviour
    

{
    public Grid grid;

    [SerializeField] GameObject EnemyPF;
    [SerializeField] int difficulty = 2;
    public List<EnemyConfig> enemyConfigs;
    public float baseSpawnInterval = 5f;
    private float spawnInterval = 0f;
    private float elapsedTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        UpdateSpawnInterval();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= spawnInterval)
        {
            SpawnMobs();
            elapsedTime = 0f;
        }
    }
    void SpawnMobs()
    {
        int points = difficulty;
        List<EnemyConfig> spawnableEnemies = new List<EnemyConfig>();

        // Determine which enemies can be spawned based on available points
        foreach (EnemyConfig config in enemyConfigs)
        {
            if (config.spawnCost <= points)
            {
                spawnableEnemies.Add(config);
            }
        }

        // Sort by cost descending to prioritize higher-cost enemies
        spawnableEnemies.Sort((a, b) => b.spawnCost.CompareTo(a.spawnCost));

        foreach (EnemyConfig config in spawnableEnemies)
        {
            if (points >= config.spawnCost)
            {
                SpawnEnemy(config);
                points -= config.spawnCost;
                Debug.Log("Spawned enemy with config: " + config.name + ", Remaining Points: " + points);
            }
        }
        difficulty = (int)(difficulty + (2 + Math.Ceiling( 0.1f * (difficulty / 100))));
    }
    void SpawnEnemy(EnemyConfig config)
    {
        Vector3 spawnPos = new Vector3(UnityEngine.Random.Range(-14, 20), UnityEngine.Random.Range(-5, 10), 0);
        while (!CanSpawn(spawnPos)) spawnPos = new Vector3(UnityEngine.Random.Range(-14, 20), UnityEngine.Random.Range(-5, 10), 0);
        GameObject enemyObject = Instantiate(EnemyPF, spawnPos, Quaternion.identity);
        EnemyLogic enemy = enemyObject.GetComponent<EnemyLogic>();
        enemy.ApplyConfig(config);
    }
    // Assign this in the inspector with all tilemaps you want to check
    void UpdateSpawnInterval()
    {
        int difficultyLevel = difficulty;
        spawnInterval = (float)(baseSpawnInterval / Math.Log(difficultyLevel,2));
    }
    public bool CanSpawn(Vector3 worldPosition)
    {
        Vector3Int cellPosition = grid.WorldToCell(worldPosition);

        foreach (Tilemap tilemap in grid.GetComponentsInChildren<Tilemap>())
        {
            if (tilemap.HasTile(cellPosition))
            {
                return false; // There's a tile here, so we can't spawn
            }
        }
        return true; // No tiles found on any tilemap, safe to spawn
    }

}
