using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Net;
using Unity.VisualScripting;

public class Tab : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    /// <summary>
    /// Tab background
    /// </summary>
    [HideInInspector] public Image backgroundImage;

    /// <summary>
    /// Tab content
    /// </summary>
    [HideInInspector] public TextMeshProUGUI text;

    /// <summary>
    /// GameObject for the checkmark that marks completion of the tab.
    /// </summary>
    [HideInInspector] public GameObject Check;

    [HideInInspector] public GameObject Curtain;

    [HideInInspector] public DescriptiveAudio DescriptiveAudio;

    /// <summary>
    /// Transform of the tab
    /// </summary>
    public RectTransform rectTransform;

    /// <summary>
    /// Event when the tab is selected
    /// </summary>
    [SerializeField] private UnityEvent OnSelected;

    [SerializeField] private VoiceOverScriptableObject voiceOverObject = null;
    [SerializeField, Tooltip("-1 means the section does not repeat")] bool repeat = false;

    /// <summary>
    /// Tab group that this tab belongs too
    /// </summary>
    private TabGroup tabGroup;

    public delegate void OnVoiceOverRequested(VoiceOverScriptableObject section, bool repeat);
    public static OnVoiceOverRequested onVoiceOverRequested;

    private void Awake()
    {
        backgroundImage = GetComponentInChildren<Image>();
        tabGroup = GetComponentInParent<TabGroup>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        Check = gameObject.transform.GetChild(2).gameObject;
        DescriptiveAudio = gameObject.GetComponent<DescriptiveAudio>();

        if (backgroundImage == null)
        {
            Debug.LogError(gameObject + ": Error: child missing Image component");
        }

        try
        {
            Curtain = gameObject.transform.GetChild(3).gameObject;
        }
        catch
        {
            return;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabClick(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }

    public void Select()
    {
        onVoiceOverRequested?.Invoke(voiceOverObject, repeat);

        if(OnSelected != null)
        {
            OnSelected.Invoke();
        }
    }
}
