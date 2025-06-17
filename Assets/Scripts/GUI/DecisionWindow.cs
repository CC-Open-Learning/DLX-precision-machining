using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// A class for the decision window's behavior.
/// </summary>
public class DecisionWindow : MonoBehaviour
{
    [SerializeField, Tooltip("The window title TMP.")] private TextMeshProUGUI title;
    [SerializeField, Tooltip("The window content TMP.")] private TextMeshProUGUI content;
    [SerializeField, Tooltip("The TMP component on the first option.")] private TextMeshProUGUI firstOptionText;
    [SerializeField, Tooltip("The button component on the first option.")] private Button firstOptionButton;

    private const string CONTINUE = "Continue";
    private const string PLAY_AGAIN = "Play Again";
    private const string HOME_CONTENT = "Are you sure you want to return to the menu before completing this segment?";

    /// <summary>
    /// Changes the decision window when the home button is clicked.
    /// </summary>
    public void ChangeForHomeButton()
    {
        content.text = HOME_CONTENT;
        firstOptionText.text = CONTINUE;

        firstOptionButton.onClick.RemoveAllListeners();
        firstOptionButton.onClick.AddListener(PrecisionMachiningGUI.Instance.CloseDecision);
    }

    /// <summary>
    /// Changes the decision window for the end of a minigame.
    /// </summary>
    /// <param name="content">The content displayed in the decision window.</param>
    public void ChangeForMinigame(string content)
    {
        this.title.text = "Congratulations";
        this.content.text = content;
        firstOptionText.text = PLAY_AGAIN;

        firstOptionButton.onClick.RemoveAllListeners();
        firstOptionButton.onClick.AddListener(() => ScreenManager.Instance.LoadScene(SceneManager.GetActiveScene().name, false));
    }
}
