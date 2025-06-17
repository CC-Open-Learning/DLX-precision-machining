using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] Image backgroundImage;
    [SerializeField] TextMeshProUGUI questionText;

    [SerializeField] List<AdventureButton> choiceButtons;

    [SerializeField] GameObject feedbackScreenGroup, optionScreenGroup;

    [SerializeField] AdventureButton feedbackButton;

    [SerializeField] Animator feedbackAnimator;

    [SerializeField] AdventureScreenScriptableObject currentScreen;

    [SerializeField] LoadLevelButton returnButton;

    [SerializeField] SaveStateScriptableObject saveState;
    // Start is called before the first frame update
    void Start()
    {
        SetupScreen(currentScreen);
    }

    public void DisplayNewScreen(AdventureScreenScriptableObject screen)
    {
        if (screen.IsLastScreen)
        {
            Debug.Log("last screen");
            FindObjectOfType<SaveManager>().SetStateComplete(saveState, true);
            returnButton.SetExitConfirmation(false);
        }

        StartCoroutine(_DisplayNewScreen(screen));
    }

    private IEnumerator _DisplayNewScreen(AdventureScreenScriptableObject screen)
    {
        if (ScreenManager.Instance == null)
        {
            SetupScreen(screen);
            yield return null;
        } else
        {
            yield return ScreenManager.Instance.FadeOut();
            SetupScreen(screen);
            yield return ScreenManager.Instance.FadeIn();
        }
    }

    public void SetupScreen(AdventureScreenScriptableObject screen)
    {
        feedbackScreenGroup.gameObject.SetActive(false);
        optionScreenGroup.gameObject.SetActive(false);

        if (screen.IsFeedback) SetupFeedbackScreen(screen);
        if (!screen.IsFeedback) SetupOptionScreen(screen);

        if (screen.QuestionVoiceOver != null)
            VoiceOverManager.Instance.PlayAudioClip(screen.QuestionVoiceOver);
    }

    private void SetupOptionScreen(AdventureScreenScriptableObject screen)
    {
        optionScreenGroup.gameObject.SetActive(true);

        backgroundImage = screen.backgroundImage;
        questionText.text = screen.QuestionText;

        foreach (var b in choiceButtons)
        {
            b.gameObject.SetActive(false);
        }

        for (int i = 1; i < screen.Choices.Count; i++)
        {
            choiceButtons[i - 1].gameObject.SetActive(true);
            choiceButtons[i - 1].OptionImage.sprite = screen.Choices[i].optionImage;
            choiceButtons[i - 1].SetDirection(screen.Choices[i].optionScreen);
            choiceButtons[i - 1].GetComponentInChildren<TextMeshProUGUI>().text = screen.Choices[i].optionText;
            choiceButtons[i - 1].GetComponent<DescriptiveAudio>().AudioClip = screen.Choices[i].descriptiveAudio;
        }
    }

    private void SetupFeedbackScreen(AdventureScreenScriptableObject screen)
    {
        feedbackScreenGroup.gameObject.SetActive(true);

        backgroundImage = screen.backgroundImage;
        questionText.text = screen.QuestionText;

        //always use the first choice
        feedbackButton.GetComponentInChildren<TextMeshProUGUI>().text = screen.Choices[1].optionText;
        feedbackButton.SetDirection(screen.Choices[1].optionScreen);
        feedbackButton.GetComponentInChildren<DescriptiveAudio>().AudioClip = screen.Choices[1].descriptiveAudio;

        if (screen.AnimationString != "")
            feedbackAnimator.Play(screen.AnimationString);
    }
}