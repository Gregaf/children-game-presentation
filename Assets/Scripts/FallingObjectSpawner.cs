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
    public FallingObjectData[] objectVariations;
    public float spawnRadius = 5f;
    public float spawnInterval = 2f;
    public int maxSpawnAttempts = 30;

    private float nextSpawnTime;
    private List<Vector3> spawnPoints = new List<Vector3>();

    private void Start()
    {
        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObject();
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
            selectedObject = objectVariations[0];
        }

        int randomIndex = Random.Range(0, objectVariations.Length);
        FallingObjectData objectData = objectVariations[randomIndex];

        Vector3 spawnPosition = GenerateRandomPoint();

        GameObject spawnedObject = Instantiate(objectData.objectPrefab, spawnPosition, Quaternion.identity);

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

            if (validPoint)
            {
                spawnPoints.Add(randomPoint);
                return randomPoint;
            }
        }

        return Random.insideUnitSphere * spawnRadius + transform.position;
    }
}