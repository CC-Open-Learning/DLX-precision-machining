using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.DialogueTrees;
using TMPro;

//This class is using NodCanvas functions and tied to the "Say" action
public class PrecisionMachiningDialogue : MonoBehaviour
{
    //Options...
    [Header("Input Options")]
    public bool canSkip;

    //Group...
    [Header("Subtitles")]
    public RectTransform subtitlesGroup;
    public TextMeshProUGUI actorSpeech;
    private AudioSource playSource;
    private bool subtitlesOn;

    private bool skipDialogue;
    bool anyKeyDown
    {
        get
        {
            #if ENABLE_LEGACY_INPUT_MANAGER
                            return Input.anyKeyDown;
            #elif ENABLE_INPUT_SYSTEM
                            return UnityEngine.InputSystem.Keyboard.current.anyKey.wasPressedThisFrame || UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame;
            #else
                            return Input.anyKeyDown;
            #endif
        }
    }

    void Awake() 
    { 
        Subscribe(); 
        Hide();
        playSource = GetComponent<AudioSource>();
    }
    void OnEnable() { UnSubscribe(); Subscribe(); }
    void OnDisable() { UnSubscribe(); }

    void Subscribe()
    {
        DialogueTree.OnDialogueStarted += OnDialogueStarted;
        DialogueTree.OnDialoguePaused += OnDialoguePaused;
        DialogueTree.OnDialogueFinished += OnDialogueFinished;
        DialogueTree.OnSubtitlesRequest += OnSubtitlesRequest;        
    }

    void UnSubscribe()
    {
        DialogueTree.OnDialogueStarted -= OnDialogueStarted;
        DialogueTree.OnDialoguePaused -= OnDialoguePaused;
        DialogueTree.OnDialogueFinished -= OnDialogueFinished;
        DialogueTree.OnSubtitlesRequest -= OnSubtitlesRequest;
    }

    #region Subtitles

    public void ToggleSubtitles()
    {
        subtitlesOn = !subtitlesOn;
    }

    public void SkipDialogue()
    {
        skipDialogue = true;
    }

    void Hide()
    {
        SubtitlesManager.Instance.ClearSubtitle();
    }

    void OnDialogueStarted(DialogueTree dlg)
    {
        //nothing special...
    }

    void OnDialoguePaused(DialogueTree dlg)
    {
        subtitlesGroup.gameObject.SetActive(false);
        StopAllCoroutines();
        if (playSource != null) playSource.Stop();
    }

    void OnDialogueFinished(DialogueTree dlg)
    {
        subtitlesGroup.gameObject.SetActive(false);
        StopAllCoroutines();
        if (playSource != null) playSource.Stop();
    }


    void OnSubtitlesRequest(SubtitlesRequestInfo info)
    {
        StartCoroutine(Internal_OnSubtitlesRequestInfo(info));
    }

    IEnumerator Internal_OnSubtitlesRequestInfo(SubtitlesRequestInfo info)
    {
        var text = info.statement.text;
        var audio = info.statement.audio;
        var actor = info.actor;

        actorSpeech.text = text;
        actorSpeech.color = actor.dialogueColor;

        if (audio != null)
        {
            playSource.clip = audio;
            playSource.Play();
            actorSpeech.text = text;
            var timer = 0f;
            while (timer < audio.length)
            {
                subtitlesGroup.gameObject.SetActive(subtitlesOn); //Subtitles only appear for audio

                if (skipDialogue && canSkip)
                {
                    playSource.Stop();
                    break;
                }
                timer += Time.deltaTime;
                yield return null;
            }
            //playSource.clip = null;
        }

        subtitlesGroup.gameObject.SetActive(false);
        
        skipDialogue = false;
        if (info.Continue != null)
        {
            info.Continue();
        }
    }

    #endregion
}

