using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
public class GameSettingsSO : ScriptableObject
{
    public float gameTimeLimit = 60f; // Default game time limit in seconds
}