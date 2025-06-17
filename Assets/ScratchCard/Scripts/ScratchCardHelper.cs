using ScratchCardAsset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(ScratchCard), typeof(EraseProgress))]
public class ScratchCardHelper : MonoBehaviour
{
    private EraseProgress e;
    private bool completed;

    [SerializeField] UnityEvent onScratchCardCompleted;

    private void Awake()
    {
        e = GetComponent<EraseProgress>();
        onScratchCardCompleted.AddListener(() => Debug.Log("Scratch card completed unity event called."));
    }

    private void Update()
    {
        if(e.GetProgress() == 1 && !completed)
        {
            onScratchCardCompleted.Invoke();
            completed = true;
        }
    }

    public void ResetCompletion()
    {
        Debug.Log("ScratchCardHelper.ResetCompletion method accessed.");
        completed = false;
    }
}
