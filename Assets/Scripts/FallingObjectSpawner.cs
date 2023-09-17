using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(FallingObjectSpawner))]
public class FallingObjectSpawnerEditor : Editor
{
    private void OnSceneGUI()
    {
        FallingObjectSpawner spawner = (FallingObjectSpawner)target;
        Handles.color = Color.blue;
        Vector3 spawnerPosition = spawner.transform.position;
        spawnerPosition.y += 0.01f; // Raise the circle slightly above the ground to avoid z-fighting
        Handles.DrawWireDisc(spawnerPosition, Vector3.up, spawner.spawnRadius);
    }
}
#endif

public class FallingObjectSpawner : MonoBehaviour
{
    public FallingObjectData[] objectVariations; // Array of FallingObjectData for object variations
    public float spawnRadius = 5f; // Radius within which objects will be spawned
    public float spawnInterval = 2f; // Time interval between spawns
    public int maxSpawnAttempts = 30; // Maximum attempts to find a suitable spawn point

    private float nextSpawnTime;
    private List<Vector3> spawnPoints = new List<Vector3>();

    private void Start()
    {
        // Initialize the next spawn time
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        // Check if it's time to spawn a new object
        if (Time.time >= nextSpawnTime)
        {
            SpawnObject();
            // Set the next spawn time
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnObject()
    {
        // Calculate the total weight of all object variations
        float totalWeight = 0f;
        foreach (var objectDataWithWeight in objectVariations)
        {
            totalWeight += objectDataWithWeight.weight;
        }

        // Generate a random value within the total weight range
        float randomValue = Random.Range(0f, totalWeight);

        // Select the object based on the weighted probabilities
        FallingObjectData selectedObject = null;
        foreach (var objectDataWithWeight in objectVariations)
        {
            if (randomValue <= objectDataWithWeight.weight)
            {
                selectedObject = objectDataWithWeight;
                break;
            }
            randomValue -= objectDataWithWeight.weight;
        }

        if (selectedObject == null)
        {
            // Fallback to the first object if selection fails (due to rounding errors)
            selectedObject = objectVariations[0];
        }

        // Randomly select an object variation
        int randomIndex = Random.Range(0, objectVariations.Length);
        FallingObjectData objectData = objectVariations[randomIndex];

        // Generate a random position within the spawn radius using Poisson Disk Sampling
        Vector3 spawnPosition = GenerateRandomPoint();

        // Create a clone of the selected object prefab at the spawn position
        GameObject spawnedObject = Instantiate(objectData.objectPrefab, spawnPosition, Quaternion.identity);

        // Apply a random falling speed to the spawned object
        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float randomFallSpeed = Random.Range(objectData.minFallSpeed, objectData.maxFallSpeed);
            rb.velocity = Vector3.down * randomFallSpeed;
        }
    }

    private Vector3 GenerateRandomPoint()
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            // Generate a random point within the spawn radius
            Vector2 randomPoint2D = Random.insideUnitCircle * spawnRadius;
            Vector3 randomPoint = new Vector3(randomPoint2D.x, 0f, randomPoint2D.y) + transform.position;

            // Check if the point is far enough from existing spawn points
            bool validPoint = true;
            foreach (Vector3 existingPoint in spawnPoints)
            {
                if (Vector3.Distance(randomPoint, existingPoint) < spawnRadius * 0.5f)
                {
                    validPoint = false;
                    break;
                }
            }

            // If the point is valid, add it to the list of spawn points and return it
            if (validPoint)
            {
                spawnPoints.Add(randomPoint);
                return randomPoint;
            }
        }

        // If no valid point is found, return a random point within the spawn radius
        return Random.insideUnitSphere * spawnRadius + transform.position;
    }
}