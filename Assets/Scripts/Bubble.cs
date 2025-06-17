using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class for the bubbles in the Fit For Duty minigame.
/// </summary>
public class Bubble : MonoBehaviour
{
    /// <summary>
    /// The type of answer inside the bubble.
    /// </summary>
    public enum BubbleAnswer 
    { 
        Do, 
        Dont };

    /// <summary>
    /// The type of bubble.
    /// </summary>
    public enum BubbleType
    {
        Thought,
        Speech
    }

    [Tooltip("Whether the answer is \"Do\" or \"Don't.\"")]
    public BubbleAnswer AnswerType;

    [SerializeField, Tooltip("The type of bubble this should appear in")] public BubbleType bubbleType;

    [Tooltip("The sound clip that plays when the item is dropped in the right spot.")]
    public AudioClip CorrectAudioClip;

    [Tooltip("The content that gets displayed in the feedback, if correct.")]
    public string CorrectFeedback;

    [Tooltip("The sound clip that plays when the item is dropped in the wrong spot.")]
    public AudioClip IncorrectAudioClip;

    [Tooltip("The content that gets displayed in the feedback, if incorrect.")]
    public string IncorrectFeedback;

    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = 0.1f;
    }
}
