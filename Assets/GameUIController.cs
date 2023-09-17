using UnityEngine;
using UnityEngine.UIElements;

public class GameUIController : MonoBehaviour
{
    private UIDocument _rootDocument;

    private Label _remainingTimeLabel;

    private void Awake()
    {
        _rootDocument = GetComponent<UIDocument>();

        var rootElement = _rootDocument.rootVisualElement;

        _remainingTimeLabel = rootElement.Q<Label>("timer-label");
    }

    private void LateUpdate()
    {
        _remainingTimeLabel.text = $"{(int)GameManager.Instance.remainingTime} s";

        Debug.Log(GameManager.Instance.remainingTime);
    }
}
