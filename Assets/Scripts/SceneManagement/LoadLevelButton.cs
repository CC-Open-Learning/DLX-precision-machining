using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelButton : MonoBehaviour
{
    [SerializeField, Tooltip("Name of the scene that should load onButtonClick")] 
    private string loadLevelName;
    [SerializeField, Tooltip("Does this button require a exit confirmation before loading new scene")] 
    private bool exitConfirmation;
    [SerializeField, Tooltip("List of required save states")]
    private List<SaveStateScriptableObject> requiredSaveStates;

    public delegate void OnLoadSceneButtonClicked(string loadLevelName, bool hasExitConfirmation);
    public static OnLoadSceneButtonClicked onLoadSceneButtonClicked;

    // Start is called before the first frame update
    void Start()
    {
        AddLoadSceneListener();
        CheckRequiredStates();
    }

    public void OnEnable()
    {
        SaveState.onSaveStateChanged += OnSaveStateChanged;
    }

    public void AddLoadSceneListener()
    {
        this.GetComponent<Button>().onClick.AddListener(() =>
        {
            onLoadSceneButtonClicked?.Invoke(loadLevelName, exitConfirmation);
        });
    }

    private void OnSaveStateChanged(SaveStateScriptableObject s, bool value)
    {
        CheckRequiredStates();
    }

    /// <summary>
    /// This checks if there are any required states in the LoadLevelButton and if they
    /// have been completed it disables the exit confirmation.
    /// </summary>
    private void CheckRequiredStates()
    {
        if (requiredSaveStates.Count > 0)
        {
            bool statesCompleted = SaveManager.Instance.GetStatesComplete(requiredSaveStates);
            if (statesCompleted)
            {
                exitConfirmation = false;
            }
        }
    }

    public void SetExitConfirmation(bool enabled)
    {
        exitConfirmation = enabled;
    }
}
