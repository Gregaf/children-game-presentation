using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameSettingsSO gameSettings; // Reference to the ScriptableObject containing game settings
    public float remainingTime { get; private set; }
    public int currentScore { get; private set; } // Current game score

    private bool isGameRunning = false;

    private void Awake()
    {
        // Singleton pattern to ensure there's only one GameManager instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Load the game settings from the ScriptableObject
        if (gameSettings == null)
        {
            Debug.LogError("GameSettings ScriptableObject not assigned in GameManager.");
        }

        remainingTime = gameSettings.gameTimeLimit;
        currentScore = 0; // Initialize the score to zero
    }

    private void Start()
    {
        StartGame();

        Catcher.OnHealthDepleted += GameOver;
    }

    private void Update()
    {
        if (isGameRunning)
        {
            // Update the remaining time and check for game over
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            {
                GameOver();
            }
        }
    }

    public void StartGame()
    {
        // Start the game and timer
        isGameRunning = true;
    }

    public void StopGame()
    {
        // Stop the game and timer
        isGameRunning = false;
    }

    private void GameOver()
    {
        // Handle game over logic here
        Debug.Log("Game Over!");
        Debug.Log("Final Score: " + currentScore);
        isGameRunning = false;
    }
}
