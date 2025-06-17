using System.Collections;
using UnityEngine;

namespace VARLab.SCORM
{
    public delegate void OnInitializeEvent();

    public class ScormIntegrator : MonoBehaviour
    {
        public static ScormIntegrator instance;

        public static event OnInitializeEvent OnInitialize;

        public static string LearnerId { get; private set; }
        public static bool Initialized { get; private set; } = false;

        [SerializeField] private AzureManager azureManager;

        private const string segmentsCompleted = "true";

        private void Start()
        {
            instance = this;
        }

        public void ScormManager_OnScormMessage(ScormManager.Event e)
        {
            switch (e)
            {
                case ScormManager.Event.Initialized:
                    StartCoroutine(Startup());
                    break;
            }
        }

        public IEnumerator Startup()
        {
            yield return new WaitForSeconds(2); // Race condition, should be a callback or a timeout-based wait until.
            InitializeData();
        }

        private void InitializeData()
        {
            Debug.Log("ScormIntegrator Initialize Data entered");
            LearnerId = ScormManager.GetLearnerId();
            Initialized = true;
            ScormIntegrator.OnInitialize?.Invoke();
            Debug.Log($"eConestoga Learner ID: {LearnerId}");
            StartCoroutine(CheckCompletion());
        }

        public void _Completed(bool iscomplete)
        {
            if (ScormManager.Initialized)
            {
                if (iscomplete)
                    ScormManager.SetCompletionStatus(StudentRecord.CompletionStatusType.completed);
                else
                    ScormManager.SetCompletionStatus(StudentRecord.CompletionStatusType.incomplete);

                ScormManager.Commit();
            }
            Debug.Log(azureManager.LoadedData);  
        }

        public IEnumerator CheckCompletion()
        {
            while (!azureManager.HasLoaded)
            {
                yield return null;
            }
            if (azureManager.LoadedData == azureManager.DEFAULT_CALL)
            {
                _Completed(true);
            }
            else
            {
                _Completed(false);
                Debug.Log("is not completed");
            }
        }
    }
}