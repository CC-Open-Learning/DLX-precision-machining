using RemoteEducation.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Toggle = UnityEngine.UI.Toggle;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

    [SerializeField, Tooltip("Toggle button for the voice-over")] Toggle voiceOverToggle;
    [SerializeField, Tooltip("Toggle button for subtitles")] Toggle subtitlesToggle;
    [SerializeField, Tooltip("Slider for the voice-over volume")] Slider voiceOverSlider;
    [SerializeField, Tooltip("Toggle for descriptive audio")] Toggle descriptiveAudioToggle;

    [SerializeField, Tooltip("Section for descriptive audio")] GameObject descriptiveAudioSection;

    private bool voiceOverEnabled = true;
    [HideInInspector] public bool voiceOverDescription = false;

    // bool that keeps track of the descriptive audio status.
    public bool DescriptiveAudioToggle = false;
    public bool VoiceOverToggle = true;
    public bool SubtitleToggle = false;

    //Minimum value for the audio mixer so that nothing can be heard
    private const float muteVolume = -80;
    private const float onVolume = -3;

    public delegate void OnToggleSubtitlesRequested(bool on);
    public static OnToggleSubtitlesRequested onToggleSubtitlesRequested;

    private void Awake()
    {
        subtitlesToggle.onValueChanged.AddListener(ToggleSubtitles);
        descriptiveAudioToggle.onValueChanged.AddListener(ToggleDescriptiveAudio);
        voiceOverToggle.onValueChanged.AddListener(ToggleVoiceOver);
        voiceOverSlider.onValueChanged.AddListener(SetVolume);
        DescriptiveAudioToggle = descriptiveAudioToggle.isOn;
        VoiceOverToggle = voiceOverToggle.isOn;
        SubtitleToggle = subtitlesToggle.isOn;
    }

    private void Start()
    {
        SubtitlesManager.Instance.ToggleSubtitles(true);
        ToggleDescriptiveAudio(false);
        ToggleVoiceOver(true);
        SetVolume(onVolume);
    }

    public void ToggleSubtitles(bool on)
    {
        onToggleSubtitlesRequested?.Invoke(on);
        subtitlesToggle.isOn = on;
        SubtitleToggle = subtitlesToggle.isOn;
    }

    public void SetVolume(float volume)
    {
        //If the slider is turned all the way to the left, completely silence the voice over
        if (volume <= voiceOverSlider.minValue)
            volume = muteVolume;

        if(voiceOverEnabled)
            audioMixer.SetFloat("voiceOverVolume", volume);
    }

    public void ToggleVoiceOver(bool on)
    {
        voiceOverEnabled = on;
        voiceOverToggle.isOn = on;
        VoiceOverToggle = voiceOverToggle.isOn;

        // Descriptive audio toggle is enabled and disabled accordin to the voice-over state
        descriptiveAudioToggle.interactable = on;

        if (!on) descriptiveAudioToggle.isOn = false;

        if (!voiceOverEnabled || voiceOverSlider.value == voiceOverSlider.minValue)
        {
            audioMixer.SetFloat("voiceOverVolume", muteVolume);
        }
        else
        {
            audioMixer.SetFloat("voiceOverVolume", voiceOverSlider.value);
        }
    }

    public void ToggleDescriptiveAudio(bool on)
    {
        voiceOverDescription = on;
        descriptiveAudioToggle.isOn = on;
        DescriptiveAudioToggle = descriptiveAudioToggle.isOn;

        // If the descriptive audio is enabled using the keyboard shortcut it will also enable the voice-over
        if (voiceOverDescription)
        {
            ToggleVoiceOver(true);
        }
    }

}
