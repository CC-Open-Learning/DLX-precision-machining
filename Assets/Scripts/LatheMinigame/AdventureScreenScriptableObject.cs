using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/AdventureScreenScriptableObject", order = 1)]
public class AdventureScreenScriptableObject : ScriptableObject
{
    public string QuestionText;
    public AudioClip QuestionVoiceOver;
    public Image backgroundImage;

    public List<Options> Choices;

    public bool IsFeedback = false;
    public string AnimationString;

    public bool IsLastScreen = false;

    [System.Serializable]
    public struct Options {
        public AdventureScreenScriptableObject optionScreen;
        public string optionText;
        public Sprite optionImage;
        public AudioClip descriptiveAudio;
    }
}