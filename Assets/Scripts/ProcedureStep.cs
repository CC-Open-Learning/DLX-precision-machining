using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureStep : MonoBehaviour
{
    [HideInInspector] public bool correctStep;

    public int stepNumber;

    [HideInInspector] public AudioClip AudioClip;
    [HideInInspector] public string FeedbackMessage;

    public string correctFeedbackMessage;
    public string incorrectFeedbackMessage;

    public AudioClip correctAudioClip;
    public AudioClip incorrectAudioClip;

}
