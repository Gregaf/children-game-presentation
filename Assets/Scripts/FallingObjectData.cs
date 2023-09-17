using UnityEngine;

[CreateAssetMenu(fileName = "FallingObjectData", menuName = "ScriptableObjects/FallingObjectData")]
public class FallingObjectData : ScriptableObject
{
    public GameObject objectPrefab; // The prefab of the falling object
    public float minFallSpeed = 2f; // Minimum falling speed
    public float maxFallSpeed = 5f; // Maximum falling speed
    public float weight;
}