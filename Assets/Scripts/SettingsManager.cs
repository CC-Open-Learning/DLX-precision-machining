using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [SerializeField, Tooltip("Settings menu object")] private SettingsMenu settingsMenu;
    [SerializeField, Tooltip("Close button in the settings menu")] private Button closeButton;

    /// <summary>
    /// Singleton Pattern
    /// </summary>
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        ToggleSettingsMenu(false);
        closeButton.onClick.AddListener(() => ToggleSettingsMenu(false));
    }

    private void Update()
    {
        //Keyboard shortcuts, static for now
        if (Input.GetKeyDown(KeyCode.S))
        {
            settingsMenu.ToggleSubtitles(!settingsMenu.SubtitleToggle);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            settingsMenu.ToggleVoiceOver(!settingsMenu.VoiceOverToggle);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            settingsMenu.ToggleDescriptiveAudio(!settingsMenu.DescriptiveAudioToggle);
        }
    }

    private void OnEnable()
    {
        Settings.onSettingsMenuRequested += ToggleSettingsMenu;
    }

    private void OnDisable()
    {
        Settings.onSettingsMenuRequested -= ToggleSettingsMenu;
    }

    public void ToggleSettingsMenu(bool on)
    {
        settingsMenu.gameObject.SetActive(on);
        VoiceOverManager.Instance.SetPause(on);
    }
}
