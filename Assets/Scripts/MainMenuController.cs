using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public AudioClip buttonClickSfx;

    [SerializeField] private UIDocument _baseUIDocument;
    [SerializeField] private float _minimumLoadTime;

    private const string _sceneToLoad = "MainLevel";

    private bool _loadingScene = false;

    private void Start()
    {
        var root = _baseUIDocument.rootVisualElement;

        Button startButton = root.Q<Button>("start-button");
        Button quitButton = root.Q<Button>("quit-button");

        startButton.clicked += StartGame;
        quitButton.clicked += QuitGame;

    }

    private void StartGame()
    {
        Debug.Log("Start game");

        if (!_loadingScene)
        {
            LoadSceneAsync();
        }
    }

    private void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneToLoad);

        StartCoroutine(UpdateLoadingScreen(asyncLoad));
    }

    private IEnumerator UpdateLoadingScreen(AsyncOperation asyncOperation)
    {
        _loadingScene = true;
        asyncOperation.allowSceneActivation = false;

        // Impose a second or two, for fake load time.
        while (!asyncOperation.isDone)
        {
            // Calculate the loading progress (0 to 1)
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 0.9f is the completion threshold

            Debug.Log(progress);
            // Invoke load screen drop down

            // Update the progress bar

            yield return null;
        }

        _loadingScene = false;
        asyncOperation.allowSceneActivation = true;
    }

    private void OnEnable()
    {
        var root = _baseUIDocument.rootVisualElement;

        Button startButton = root.Q<Button>("start-button");
        Button quitButton = root.Q<Button>("quit-button");

        startButton.clicked += StartGame;
        quitButton.clicked += QuitGame;

        root.RegisterCallback<MouseOverEvent>(OnPress, TrickleDown.TrickleDown);
    }

    private void OnDisable()
    {
        var root = _baseUIDocument.rootVisualElement;

        Button startButton = root.Q<Button>("start-button");
        Button quitButton = root.Q<Button>("quit-button");

        startButton.clicked -= StartGame;
        quitButton.clicked -= QuitGame;

        root.UnregisterCallback<MouseOverEvent>(OnPress, TrickleDown.TrickleDown);
    }

    private void OnPress(EventBase evt)
    {
        if (evt.target is Button)
        {
            AudioManager.Instance.PlaySound(buttonClickSfx);
        }
    }

}
