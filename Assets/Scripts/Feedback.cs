using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script should be added to any game object that needs a feedback modal displayed.
/// </summary>

public class Feedback : MonoBehaviour
{
    [Header("Setting up the Modal")]
    [SerializeField, Tooltip("Reference to the Feedback Modal")] private FeedbackModal feedbackModal;
    [SerializeField, Tooltip("Feedback Content")] private string feedbackContentText;
    [SerializeField, Tooltip("Correct Answer")] private bool correctAnswer = false;

    public void UpdateFeedback()
    {
        feedbackModal.UpdateModal(feedbackContentText, correctAnswer);
    }
}