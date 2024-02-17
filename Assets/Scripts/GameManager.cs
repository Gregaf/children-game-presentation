using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameSettingsSO gameSettings;
    public float remainingTime { get; private set; }
    public int currentScore { get; private set; }

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
        currentScore = 0;
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

            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0f)
            {
                GameOver();
            }
        }
    }

    public void StartGame()
    {
        isGameRunning = true;
    }

    public void StopGame()
    {
        isGameRunning = false;
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
        Debug.Log("Final Score: " + currentScore);
        isGameRunning = false;
    }
}
