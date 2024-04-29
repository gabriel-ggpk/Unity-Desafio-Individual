using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemySpawner : MonoBehaviour
    

{
    public Grid grid;

    [SerializeField] GameObject EnemyPF;
    [SerializeField] float spawnTime = 3f; 
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn(spawnTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator Spawn(float timer)
    {
        yield return new WaitForSeconds(timer);
        Vector3 spawnPos = new Vector3(Random.Range(-14, 20), Random.Range(-5, 10), 0);
        while(!CanSpawn(spawnPos)) spawnPos = new Vector3(Random.Range(-14, 20), Random.Range(-5, 10), 0);
        GameObject newEnemy = Instantiate(EnemyPF, new Vector3(Random.Range(-14,20), Random.Range(-5, 10),0),Quaternion.identity);
        StartCoroutine(Spawn(timer*0.95f));
    }

    // Assign this in the inspector with all tilemaps you want to check

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
