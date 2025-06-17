using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/VoiceOverScriptableObject", order = 1)]
public class VoiceOverScriptableObject : ScriptableObject
{
    public List<AudioClip> voiceOverClips;
    public List<string> voiceOverSubtitles;
}