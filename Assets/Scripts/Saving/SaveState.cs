using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[DisallowMultipleComponent]
public class SaveState : MonoBehaviour
{
    public SaveStateScriptableObject TrackedSaveState;

    private bool complete;

    public delegate void OnSaveStateChanged(SaveStateScriptableObject s, bool value);
    public static OnSaveStateChanged onSaveStateChanged;

    public bool Complete { get { return complete; } 
        set { 
            complete = value; 
            GetComponent<Image>().enabled = value;
            onSaveStateChanged?.Invoke(TrackedSaveState, value);
        } 
    }

    private void Awake()
    {
        //make sure this image doesn't get in the way of the cursor
        GetComponent<Image>().raycastTarget = false;
    }

    public bool IsSaveStateObjectComplete()
    {
        List<SaveStateScriptableObject> saveList = new List<SaveStateScriptableObject>() { TrackedSaveState };
        Debug.Log(SaveManager.Instance.GetStatesComplete(saveList));
        return SaveManager.Instance.GetStatesComplete(saveList);
    }
}
