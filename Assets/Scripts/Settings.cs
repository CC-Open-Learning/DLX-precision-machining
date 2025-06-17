using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public delegate void OnSettingsMenuRequested(bool on);
    public static OnSettingsMenuRequested onSettingsMenuRequested;

    public void ShowSettings()
    {
        onSettingsMenuRequested?.Invoke(true);
    }

    public void Awake()
    {
        SubtitlesManager.Instance.ToggleSubtitles(true);
    }
}
