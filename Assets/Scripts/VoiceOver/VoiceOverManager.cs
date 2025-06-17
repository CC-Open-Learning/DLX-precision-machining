using NodeCanvas.DialogueTrees;
using NodeCanvas.Tasks.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[DisallowMultipleComponent]
public class VoiceOverManager : MonoBehaviour
{
    public static VoiceOverManager Instance { get; private set; }

    [SerializeField, Tooltip("Reference to the audio source")]private AudioSource audioSource;

    private bool isPaused = false;
    private bool isAudioQueued = false;

    /// <summary>
    /// Repeat reminder messages every X seconds.
    /// </summary>
    private const float repeatDelay = 10f;
    float timeSinceLastAudio = 0f;

    //Clips and sections that need to be repeated
    private VoiceOverScriptableObject sectionToRepeat = null;
    private AudioClip clipToRepeat = null;

    public delegate void OnSubtitleRequested(string subtitle);
    public static OnSubtitleRequested onSubtitleRequested;

    public delegate void OnVoiceOverStart();
    public static OnVoiceOverStart onVoiceOverStart;

    public delegate void OnVoiceOverEnd();
    public static OnVoiceOverEnd onVoiceOverEnd;

    //Singleton pattern
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

    public void SetPause(bool paused)
    {
        if (paused) audioSource.Pause();
        if (!paused) audioSource.UnPause();
        isPaused = paused;
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }

    public bool IsSectionFinished()
    {
        return !isAudioQueued && !IsPlaying();
    }

    /// <summary>
    /// Play the repeating clips periodically
    /// </summary>
    private void Update()
    {
        if (IsPlaying())
            timeSinceLastAudio = 0f;

        timeSinceLastAudio += Time.deltaTime;

        if (timeSinceLastAudio < repeatDelay) return;

        if (sectionToRepeat != null)
        {
            StartCoroutine(_PlayVoiceOverClipsFromSection(sectionToRepeat, false));
            sectionToRepeat = null;
        } else if (clipToRepeat != null)
        {
            PlayAudioClip(clipToRepeat);
            clipToRepeat = null;
        }
    }

    private void OnEnable()
    {
        ScreenManager.onLeavingScene += StopAudio;
        ScreenManager.onLeavingScene += StopDelayedAudio;

        VoiceOverTask.onVoiceOverRequested += PlayVoiceOverClipsFromSection;
        Tab.onVoiceOverRequested += PlayVoiceOverClipsFromSection;
        MultipleChoiceQuizManager.onQuizComplete += StopDelayedRepeatingSection;
        ModuleIntroduction.onVoiceOverRequested += (_) => PlayVoiceOverClipsFromSection(_, false);
    }

    private void OnDisable()
    {
        ScreenManager.onLeavingScene -= StopAudio;
        ScreenManager.onLeavingScene -= StopDelayedAudio;

        VoiceOverTask.onVoiceOverRequested -= PlayVoiceOverClipsFromSection;
        Tab.onVoiceOverRequested -= PlayVoiceOverClipsFromSection;
        MultipleChoiceQuizManager.onQuizComplete -= StopDelayedRepeatingSection;
    }
   

    public void PlayVoiceOverClipsFromSection(VoiceOverScriptableObject section, bool repeat)
    {
        if (section == null) return;

        StopAudio();
        StopDelayedAudio();

        StartCoroutine(_PlayVoiceOverClipsFromSection(section, repeat));

    }

    public void PlayAudioClip(AudioClip clip, bool delayed = false)
    {
        if (delayed)
        {
            Debug.Log("clip requested delayed");
            clipToRepeat = clip;
            timeSinceLastAudio = 0;
        }
        else
        {
            StopAudio();
            audioSource.PlayOneShot(clip);
        }
    }

    public void StopAudio()
    {
        StopAllCoroutines();
        audioSource.Stop();
    }

    public void StopDelayedAudio()
    {
        StopDelayedRepeatingSection();
        StopDelayedVoiceClip();
    }

    public void StopDelayedRepeatingSection()
    {
        sectionToRepeat = null;
    }

    public void StopDelayedVoiceClip()
    {
        clipToRepeat = null;
    }

    public void PlayAudioClips(List<AudioClip> clips)
    {
        StopAudio();
        StartCoroutine(_PlayAudioClips(clips));
    }

    private IEnumerator _PlayAudioClips(List<AudioClip> audioClips)
    {
        isAudioQueued = true;

        foreach (var audioClip in audioClips)
        {
            audioSource.PlayOneShot(audioClip);
            yield return new WaitForSeconds(audioClip.length);
        }

        isAudioQueued = false;
    }

    private IEnumerator _PlayVoiceOverClipsFromSection(VoiceOverScriptableObject section, bool repeat)
    {
        isAudioQueued = true;
        onVoiceOverStart?.Invoke();

        if (repeat) sectionToRepeat = section;

        int i = 0;

        foreach (var audioClip in section.voiceOverClips)
        {
            string subtitle = section.voiceOverSubtitles[i++];

            onSubtitleRequested?.Invoke(subtitle);
            audioSource.Stop();
            audioSource.PlayOneShot(audioClip);

            float timer = 0f;

            while (timer < audioClip.length)
            {
                if (!isPaused)
                {
                    timer += Time.deltaTime;

                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        audioSource.Stop();
                        break;
                    }
                }

                yield return null;
            }

            yield return new WaitUntil(() => (!audioSource.isPlaying && !isPaused));
        }

        onVoiceOverEnd?.Invoke();
        isAudioQueued = false;
    }


}
