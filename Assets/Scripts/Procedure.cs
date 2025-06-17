using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Procedure : MonoBehaviour
{
    [SerializeField] private ProcedureStep[] steps;

    [SerializeField, Tooltip("Reference to the modal window")] private ModalWindow modalWindow;

    [SerializeField] private UnityEvent OnCompletedProdecure;

    private int stepCount;
    private int lastStepNumber;

    private void Awake()
    {
        stepCount = steps.Length - 1;

        for (int i = 0; i <= stepCount; i++)
        {
            Button btn = steps[i].GetComponent<Button>();

            int stepNum = steps[i].stepNumber;

            if (btn != null)
            {
                btn.onClick.AddListener(() => StepSelected(stepNum));
            }

            if(lastStepNumber < stepNum) lastStepNumber = stepNum;
        }

        ResetStepValues();
    }
    private void ResetStepValues()
    {
        foreach(ProcedureStep s in steps)
        {
            s.correctStep = s.stepNumber == 0;
        }
    }

    private void StepSelected(int index)
    {
        ProcedureStep selectedStep = steps[index];

        if (selectedStep.correctStep)
        {
            foreach(ProcedureStep s in steps)
            {
                if(s.stepNumber == selectedStep.stepNumber + 1)
                {
                    s.correctStep = true;
                }
            }

            if (selectedStep.stepNumber == lastStepNumber)
            {
                foreach (ProcedureStep s in steps)
                {
                    Destroy(s.gameObject);
                }

                OnCompletedProdecure.Invoke();

            }

            selectedStep.FeedbackMessage = selectedStep.correctFeedbackMessage;
            selectedStep.AudioClip = selectedStep.correctAudioClip;

            modalWindow.GetComponent<CanvasGroup>().alpha = 1;
            PrecisionMachiningGUI.Instance.ShowFeedBack(steps[index]);

            steps[index].correctStep = false;
        }
        else
        {
            selectedStep.FeedbackMessage = selectedStep.incorrectFeedbackMessage;
            selectedStep.AudioClip = selectedStep.incorrectAudioClip;

            modalWindow.GetComponent<CanvasGroup>().alpha = 1;
            PrecisionMachiningGUI.Instance.ShowFeedBack(steps[index]);

            ResetStepValues();
        }




    }



}
