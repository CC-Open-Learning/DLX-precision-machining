using RemoteEducation.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class that controls the modal window's behaviour.
/// </summary>
public class ModalWindow : MonoBehaviour
{
    [SerializeField, Tooltip("Reference to the Modal Window inner widow")] private Image InnerWindow;
    [SerializeField, Tooltip("Reference to the Modal Window Content text")] private TMP_Text Content;
    [SerializeField, Tooltip("Reference to the Modal Window Title text")] private TMP_Text Title;
    [SerializeField, Tooltip("Reference to the Modal Window icon image")] private Image Icon;
    [SerializeField, Tooltip("Reference to the Modal Button text")] private TextMeshProUGUI buttonText;

    /// <summary>
    /// Changes the appearance of the modal window, depending on the answer selected.
    /// </summary>
    /// <param name="answer">The answer selected.</param>
    public void ChangeWindow(Answer answer)
    {
        InnerWindow.color = PrecisionMachiningGUI.DefaultColor;
        Icon.sprite = PrecisionMachiningGUI.DefaultSprite;

        if ((answer.CorrectAnswer))
        {
            InnerWindow.color = PrecisionMachiningGUI.CorrectColor;
            Icon.sprite = PrecisionMachiningGUI.CorrectSprite;
            Title.text = "Correct";
        }    
        else if (!answer.CorrectAnswer)
        {
            InnerWindow.color = PrecisionMachiningGUI.IncorrectColor;
            Icon.sprite = PrecisionMachiningGUI.IncorrectSprite;
            Title.text = "Incorrect";
        }

        Content.text = answer.FeedbackMessage;
        buttonText.text = "Got it!";
    }


    /// <summary>
    /// Changes the appearance of the modal window, depending on the answer selected.
    /// </summary>
    /// <param name="answer">The answer selected.</param>
    public void ChangeWindow(ProcedureStep step)
    {
        InnerWindow.color = PrecisionMachiningGUI.DefaultColor;
        Icon.sprite = PrecisionMachiningGUI.DefaultSprite;

        if ((step.correctStep))
        {
            InnerWindow.color = PrecisionMachiningGUI.CorrectColor;
            Icon.sprite = PrecisionMachiningGUI.CorrectSprite;
            Title.text = "Correct";
            buttonText.text = "Got it!";
        }
        else if (!step.correctStep)
        {
            InnerWindow.color = PrecisionMachiningGUI.IncorrectColor;
            Icon.sprite = PrecisionMachiningGUI.IncorrectSprite;
            Title.text = "Incorrect";
            buttonText.text = "Restart";
        }

        Content.text = step.FeedbackMessage;

    }

    /// <summary>
    /// Changes the appearance of the modal window, depending on the bubble dropped.
    /// </summary>
    /// <param name="bubble">The bubble that was successfully dropped.</param>
    /// <param name="correct">Whether or not the bubble was dropped in the correct panel.</param>
    public void ChangeWindow(Bubble bubble, bool correct)
    {
        InnerWindow.color = PrecisionMachiningGUI.DefaultColor;
        Icon.sprite = PrecisionMachiningGUI.DefaultSprite;

        if (correct)
        {
            InnerWindow.color = PrecisionMachiningGUI.CorrectColor;
            Icon.sprite = PrecisionMachiningGUI.CorrectSprite;
            Title.text = "Correct";
            Content.text = bubble.CorrectFeedback;
        }
        else
        {
            InnerWindow.color = PrecisionMachiningGUI.IncorrectColor;
            Icon.sprite = PrecisionMachiningGUI.IncorrectSprite;
            Title.text = "Incorrect";
            Content.text = bubble.IncorrectFeedback;
        }

        buttonText.text = "Got it!";
    }

    /// <summary>
    /// Changes the appearance of the modal window.
    /// </summary>
    /// <param name="state">The desired state of the modal window.</param>
    /// <param name="title">The title that shows on the header of the modal window.</param>
    /// <param name="content">The content that shows in the middle of the modal window.</param>
    /// <param name="button">The text that appears on the button of the modal window.</param>
    public void ChangeWindow(PrecisionMachiningGUI.WindowState state, string title, string content, string button = "")
    {
        InnerWindow.color = PrecisionMachiningGUI.DefaultColor;
        Icon.sprite = PrecisionMachiningGUI.DefaultSprite;

        switch (state)
        {
            case PrecisionMachiningGUI.WindowState.Default:
                InnerWindow.color = PrecisionMachiningGUI.DefaultColor;
                Icon.sprite = PrecisionMachiningGUI.DefaultSprite;
                break;
            case PrecisionMachiningGUI.WindowState.Correct:
                InnerWindow.color = PrecisionMachiningGUI.CorrectColor;
                Icon.sprite = PrecisionMachiningGUI.CorrectSprite;
                break;
            case PrecisionMachiningGUI.WindowState.Incorrect:
                InnerWindow.color = PrecisionMachiningGUI.IncorrectColor;
                Icon.sprite = PrecisionMachiningGUI.IncorrectSprite;
                break;
        }

        Title.text = title;
        Content.text = content;
        buttonText.text = String.IsNullOrEmpty(button) ? "Close" : button;
    }
}
