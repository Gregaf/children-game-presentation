using UnityEngine;

[CreateAssetMenu(fileName = "FallingObjectData", menuName = "ScriptableObjects/FallingObjectData")]
public class FallingObjectData : ScriptableObject
{
    public GameObject objectPrefab;
    public float minFallSpeed = 2f;
    public float maxFallSpeed = 5f;
    public float weight;
}