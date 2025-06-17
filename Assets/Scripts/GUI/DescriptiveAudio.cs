using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// A class that plays audio when the mouse is hovered over a UI element.
/// </summary>
public class DescriptiveAudio : MonoBehaviour, IPointerEnterHandler, IDragHandler, IPointerDownHandler
{
    /// <summary>
    /// Whether or not the mouse has moved since the last tab click.
    /// </summary>
    public static bool HasMouseMoved { get; set; }

    private SettingsMenu settingsMenu;

    [Tooltip("The audio clip for the descriptive audio.")] public AudioClip AudioClip;

    void Start()
    {
        settingsMenu = FindObjectOfType<SettingsMenu>(true);
    }

    /// <summary>
    /// Play the audio when mouse hover over the object
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (AudioClip != null && settingsMenu.voiceOverDescription && HasMouseMoved)
        {
            VoiceOverManager.Instance.PlayAudioClip(AudioClip);
            SubtitlesManager.Instance.ClearSubtitle();
        }
    }

    /// <summary>
    /// Stop the audio when the mouse is clicked
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (AudioClip != null && settingsMenu.voiceOverDescription && GetComponent<Button>() != null) 
        {
            VoiceOverManager.Instance.StopAudio();
        }
    }

    /// <summary>
    /// Stop the audio when the mouse is dragging
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {
        if (AudioClip != null && settingsMenu.voiceOverDescription)
        {
            VoiceOverManager.Instance.StopAudio();
        }
    }
}
