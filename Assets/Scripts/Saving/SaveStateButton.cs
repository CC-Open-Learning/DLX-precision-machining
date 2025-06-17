using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(Button))]
[DisallowMultipleComponent]
public class SaveStateButton : MonoBehaviour
{
    [SerializeField] private List<SaveStateScriptableObject> requiredSaveStates;

    // Start is called before the first frame update
    void Start()
    {
        CheckRequiredStatesToEnable();
    }

    private void OnEnable()
    {
        SaveState.onSaveStateChanged += OnSaveStateChanged;
    }

    private void OnDisable()
    {
        SaveState.onSaveStateChanged -= OnSaveStateChanged;
    }

    void OnSaveStateChanged(SaveStateScriptableObject s, bool value)
    {
        CheckRequiredStatesToEnable();
    }

    void CheckRequiredStatesToEnable()
    {
        bool statesEnabled = SaveManager.Instance.GetStatesComplete(requiredSaveStates);
        GetComponent<Image>().enabled = statesEnabled;
        GetComponent<Button>().enabled = statesEnabled;
        foreach(Transform child in this.transform)
        {
            child.gameObject.SetActive(statesEnabled);
        }
    }
}
