using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VARLab.SCORM;

/// <summary>
/// Class that tracks the module completion and updates scorm and azure on completion.
/// </summary>
[RequireComponent(typeof(ScormIntegrator))]
[DisallowMultipleComponent]
public class UpdateSCORMOnComplete : MonoBehaviour
{
    [SerializeField, Tooltip("List of required saved states for module to be complete")]
    private List<SaveStateScriptableObject> requiredSaveStates;

    private ScormIntegrator scormIntegrator;

    [SerializeField] private AzureManager azureManager;

    [SerializeField] private GameObject loadPanel;

    private void Awake()
    {
        scormIntegrator = GetComponent<ScormIntegrator>();
        // Checks the azure completion status after it has loaded
        // If completion status is completed set all states to true
        if (!azureManager.HasLoaded)
        {
#if UNITY_EDITOR || !UNITY_WEBGL
            LoadData(); 
#else
            ScormIntegrator.OnInitialize += LoadData;
#endif
        }

        else
        {
            SaveManager.Instance.SetupSaveStates(true);
        }
    }

    private void Update()
    {
#if UNITY_EDITOR || !UNITY_WEBGL
        // Completes all the segments
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            SaveManager.Instance.SetupSaveStates(true);
        }
#endif
    }

    /// <summary>
    /// Loads the data from azure.
    /// </summary>
    private void LoadData()
    {
        azureManager.Load(azureManager.GetContainerName());
        StartCoroutine(CheckCompletion());
    }

    /// <summary>
    /// After Azure has loaded, if module has been completed, the save manager sets all 
    /// of the save states to true.
    /// </summary>
    public IEnumerator CheckCompletion()
    {
        while (!azureManager.HasLoaded)
        {
            yield return null;
        }
        if (azureManager.LoadedData == azureManager.DEFAULT_CALL)
        {
            SaveManager.Instance.SetupSaveStates(true);
            Debug.Log($"All segments completed and save states updated.");
        }
    }

    private void OnEnable()
    {
        SaveManager.onSaveStateChangeRegistered += UpdateScormCompletionStatus;
    }

    private void OnDisable()
    {
        SaveManager.onSaveStateChangeRegistered -= UpdateScormCompletionStatus;
    }

    /// <summary>
    /// Method called to update scorm and azure completion.
    /// </summary>
    private void UpdateScormCompletionStatus()
    {
        bool segmentsCompleted = SaveManager.Instance.GetStatesComplete(requiredSaveStates);

        // if states enabled true set completion status on LMS
        if (scormIntegrator != null && segmentsCompleted && azureManager.LoadedData != azureManager.DEFAULT_CALL)
        {
            Debug.Log($"Saving completion data....");
            azureManager.Save(azureManager.GetContainerName(), azureManager.DEFAULT_CALL);
            azureManager.LoadedData = azureManager.DEFAULT_CALL;
            ScormIntegrator.instance._Completed(true);
        }
        else if (scormIntegrator != null && segmentsCompleted && azureManager.LoadedData == azureManager.DEFAULT_CALL)
        {
            Debug.Log($"Data already Saved....");
        }
    }
}