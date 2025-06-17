using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// This Singleton manages the saveStates for all the Segments, mini-games, 
/// topics, subtopics and quiz questions.
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    // Variable used to track if the introduction module voice-over has played once.
    public static bool isWelcomeVoicePlayed { get; set; } = false;

    public delegate void OnSaveStateChangeRegistered();
    public static OnSaveStateChangeRegistered onSaveStateChangeRegistered;

    //states that need to be tracked
    [SerializeField] SaveStateScriptableObject[] SaveStates;

    private Dictionary<SaveStateScriptableObject, bool> saveStates;

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

        SetupSaveStates();
    }

    /// <summary>
    /// Creates a dictionary of save states and their value
    /// </summary>
    /// <param name="completed">Sets state value to true of false according to users completion status.</param>
    public void SetupSaveStates(bool completed = false)
    {
        saveStates = new Dictionary<SaveStateScriptableObject, bool>();

        foreach(SaveStateScriptableObject c in SaveStates)
        {
            saveStates.Add(c, completed);
        }
    }

    public void LoadCurrentStates()
    {
        foreach(SaveState s in FindObjectsOfType<SaveState>(true))
        {
            if (!s.gameObject.GetComponent<Button>())
            {
                bool enabled = saveStates[s.TrackedSaveState];
                s.GetComponent<Image>().enabled = enabled;
            }
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
        SaveState.onSaveStateChanged += SetStateComplete;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        SaveState.onSaveStateChanged -= SetStateComplete;
    }

    public void SetStateComplete(SaveStateScriptableObject state, bool complete)
    {
        saveStates[state] = complete;
        onSaveStateChangeRegistered?.Invoke();
    }

    public bool GetStatesComplete(List<SaveStateScriptableObject> states)
    {
        foreach(var s in states)
        {
            if (saveStates[s] == false)
                return false;
        }
        return true;
    }

    void OnLevelFinishedLoading(Scene s, LoadSceneMode l)
    {
        LoadCurrentStates();
    }
}
