using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages the introduction/main menu module voice-over. It only plays when the user
/// first enters the module. If the user returns to the main menu during their play the introduction message 
/// won't be played again.
/// </summary>
public class ModuleIntroduction : MonoBehaviour
{
    [SerializeField] private VoiceOverScriptableObject voiceOverObject = null;

    public delegate void OnVoiceOverRequested(VoiceOverScriptableObject section);
    public static OnVoiceOverRequested onVoiceOverRequested;

    void Start()
    {
        //only play the voiceover once on start
        if (!SaveManager.isWelcomeVoicePlayed){
            onVoiceOverRequested?.Invoke(voiceOverObject);
            SaveManager.isWelcomeVoicePlayed = true;
        }else {
            VoiceOverManager.Instance.StopAudio();
            VoiceOverManager.Instance.StopDelayedAudio();
        }
    }
}
