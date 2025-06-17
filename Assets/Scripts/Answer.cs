using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static VoiceOverManager;

/// <summary>
/// A class for simple answers that can either be correct or incorrect.
/// </summary>
public class Answer : MonoBehaviour
{
    [Tooltip("Whether or not the transparency on the image can be clicked.")]
    [SerializeField] private bool clickTransparency;

    [Tooltip("The sound clip that plays when the item is dropped")]
    public AudioClip AudioClip;

    [Tooltip("The content that gets displayed in the feedback")]
    public string FeedbackMessage;

    [Tooltip("Whether the object is a correct answer or not")]
    public bool CorrectAnswer;

    [Tooltip("Whether the answer is complete or not")]
    public bool Complete = false;

    [Tooltip("Reference to the answer feedback image")] 
    public GameObject FeedbackImage = null;

    private Image image;

    /// <summary>
    /// An event that is used when a drag and drop answer is successfully dropped.
    /// </summary>
    public delegate void OnAnswerDroppedSuccessfully();
    public static OnAnswerDroppedSuccessfully onAnswerDroppedSuccessfully;

    private void Start()
    {
        image = GetComponent<Image>();

        if (!clickTransparency)
            image.alphaHitTestMinimumThreshold = 0.1f;
    }

    /// <summary>
    /// Forces the answer to be set to complete.
    /// </summary>
    /// <param name="complete"></param>
    public void ForceSetComplete(bool complete)
    {
        Complete = complete;
    }

    /// <summary>
    /// Sets the status to completed, or resets it.
    /// </summary>
    /// <param name="reset"></param>
    public void Completed()
    {
        if (CorrectAnswer)
        {
            Complete = true;
            onAnswerDroppedSuccessfully?.Invoke();
        }
        else
        {
            Complete = false;
        }
    }
}
