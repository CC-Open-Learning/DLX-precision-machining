using NodeCanvas.DialogueTrees;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class SubtitlesManager : MonoBehaviour
{
    [SerializeField] RectTransform subtitlesGroup, mainMenuTransform, tabbedLayoutTransform;
    [SerializeField] TMPro.TextMeshProUGUI actorSpeech;

    /// <summary>
    /// internal flag for whether or not we show subtitles
    /// </summary>
    bool subtitlesOn = false;

    public static SubtitlesManager Instance { get; private set; }

    private void Start()
    {
        ToggleSubtitleBox(false);
    }

    private void OnEnable()
    {
        SettingsMenu.onToggleSubtitlesRequested += ToggleSubtitles;

        VoiceOverManager.onSubtitleRequested += CreateSubtitle;
        VoiceOverManager.onVoiceOverEnd += ClearSubtitle;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        VoiceOverManager.onSubtitleRequested -= CreateSubtitle;
        VoiceOverManager.onVoiceOverEnd -= ClearSubtitle;
        SettingsMenu.onToggleSubtitlesRequested -= ToggleSubtitles;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void ToggleSubtitles(bool on)
    {
        subtitlesOn = on;
        if (!on) ToggleSubtitleBox(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearSubtitle();

        //the shape of the subtitle box is different in the homescene than it is in other scenes
        bool sceneUsesTabbedLayoutTransform = scene.name == "PPETabbedLayout" || scene.name == "FitForDutyTabbedLayout" || scene.name == "WhatToWearTabbedLayout";

        RectTransform target = sceneUsesTabbedLayoutTransform ? tabbedLayoutTransform : mainMenuTransform;

        subtitlesGroup.anchoredPosition = target.anchoredPosition;
        subtitlesGroup.sizeDelta = target.sizeDelta;
    }

    //singleton pattern
    void Awake()
    {
        if (Instance != null && Instance != this){
            Destroy(this.gameObject);
        } else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void ToggleSubtitleBox(bool enabled)
    {
        subtitlesGroup.gameObject.SetActive(enabled && subtitlesOn);
    }

    public void ClearSubtitle()
    {
        actorSpeech.text = "";
        ToggleSubtitleBox(false);
    }

    private void CreateSubtitle(string subtitle)
    {
        actorSpeech.text = subtitle;
        ToggleSubtitleBox(true);
    }

}
