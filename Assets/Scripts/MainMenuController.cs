using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private UIDocument _baseUIDocument;

    private const string _sceneToLoad = "scene";


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
    }

    private void QuitGame()
    {
        Debug.Log("Quit game");
        Application.Quit();
    }

    public void LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneToLoad);

        asyncLoad.allowSceneActivation = false;

        StartCoroutine(UpdateLoadingScreen(asyncLoad));
    }

    private IEnumerator UpdateLoadingScreen(AsyncOperation asyncOperation)
    {
        // Impose a second or two, for fake load time.
        while (!asyncOperation.isDone)
        {
            // Calculate the loading progress (0 to 1)
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // 0.9f is the completion threshold

            // Invoke load screen drop down
            // Update the progress bar

            yield return null;
        }

        asyncOperation.allowSceneActivation = true;
    }

}
