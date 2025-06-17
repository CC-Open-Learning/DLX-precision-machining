using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnSaveState : MonoBehaviour
{
    [SerializeField, Tooltip("List of states required to be completed for the object to be disabled")] private List<SaveStateScriptableObject> requiredSaveStates;

    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(!SaveManager.Instance.GetStatesComplete(requiredSaveStates));   
    }
}
