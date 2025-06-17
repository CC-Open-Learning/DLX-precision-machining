using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FeedbackModal : MonoBehaviour
{
    [Header("Setting up the Modal")]
    [SerializeField, Tooltip("Reference to the Feedback Modal Title")] private TextMeshProUGUI titleText;
    [SerializeField, Tooltip("Reference to the Feedback Modal Content")] private TextMeshProUGUI feedbackText;
    [SerializeField, Tooltip("Reference to the Feedback Modal Button")] private TextMeshProUGUI buttonText;

    public void UpdateModal(string content, bool answerCorrect)
    {
        titleText.text = answerCorrect ? "Good Job!" : "Try Again";
        titleText.color = answerCorrect ? Color.green : Color.red;
        feedbackText.text = content;
        buttonText.text = "Got It!";
    }
}
