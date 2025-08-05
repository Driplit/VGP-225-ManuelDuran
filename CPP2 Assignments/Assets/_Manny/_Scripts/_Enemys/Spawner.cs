using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject[] itemsToSpawn; // Array of items to spawn
    public Transform[] spawnPoints;  // Array of spawn point locations
    public int maxSpawns = 1;       // Number of items to spawn
    public float spawnRadius = 5f;  // Radius within which items will spawn

    void Start()
    {
        // Check for configuration errors
        if (itemsToSpawn == null || itemsToSpawn.Length == 0)
        {
            Debug.LogError("No items assigned in itemsToSpawn array!");
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points assigned!");
            return;
        }

        // Spawn items at random spawn points
        SpawnItems();
    }

    void SpawnItems()
    {
        // Ensure we do not spawn more items than the available spawn points
        int spawnsToUse = Mathf.Min(maxSpawns, spawnPoints.Length);

        // Shuffle the spawn points to prevent reuse
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);
        ShuffleList(availableSpawnPoints);

        for (int i = 0; i < spawnsToUse; i++)
        {
            // Randomly select an item to spawn
            GameObject randomItem = itemsToSpawn[Random.Range(0, itemsToSpawn.Length)];

            // Use the next available spawn point
            Transform spawnPoint = availableSpawnPoints[i];

            // Get a random offset within a spherical radius of spawnRadius
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = 0; // Keep the spawning on the same ground level (if needed)

            // Calculate the final spawn position
            Vector3 spawnPosition = spawnPoint.position + randomOffset;

            // Spawn the item at the calculated position
            Instantiate(randomItem, spawnPosition, spawnPoint.rotation);
        }
    }

    // Fisher-Yates shuffle to randomize the order of spawn points
    void ShuffleList(List<Transform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Transform temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}