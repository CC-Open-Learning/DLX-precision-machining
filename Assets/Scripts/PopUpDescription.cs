using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpDescription : MonoBehaviour
{
    [SerializeField] private PopUpWindow popUpWindow;
    [SerializeField] PopUpWindowScriptableObject popUpWindowScriptableObject;

    private Button button;

    /// <summary>
    /// On Awake set up the button listener fot the pop up window.
    /// </summary>
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(() => PrecisionMachiningGUI.Instance.ShowPopup(popUpWindowScriptableObject));
    }
}
