using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedVoiceOver : MonoBehaviour
{
    [SerializeField, Tooltip("The audio clip for the delayed voiceover.")]
    private AudioClip audioClip;
    
    /// <summary>
    /// Plays delayed audio clip
    /// </summary>
    public void Play()
    {
        VoiceOverManager.Instance.PlayAudioClip(audioClip, true);
    }
}
