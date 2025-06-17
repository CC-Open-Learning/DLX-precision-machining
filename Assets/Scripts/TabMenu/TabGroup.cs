using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class TabGroup : MonoBehaviour
{
    /// <summary>
    /// The list of tabs
    /// </summary>
    public List<Tab> tabs { get; private set; }

    /// <summary>
    /// The tab that was selected
    /// </summary>
    public Tab selectedTab { get; private set; }

    [Tooltip("The parent tab, if the tab group is a collection of subtopics.")] public Tab parentTab; 


    [SerializeField, Tooltip("Color of the tab when it's idle")] 
    private Color tabIdle;

    [SerializeField, Tooltip("Color of the tab when it's being hovered over")] 
    private Color tabHover;
    [SerializeField, Tooltip("Color of the tab when it's active")] 
    private Color tabActive;

    [SerializeField, Tooltip("Color of the tab text when it's idle")] 
    private Color textColorIdle;

    [SerializeField, Tooltip("Color of the tab text when it's active")]
    private Color textColorActive;


    [SerializeField, Tooltip("The main body that contains the content")]
    private Transform body;

    [SerializeField, Tooltip("Size of the bottom border of the tab")]
    private float bottom = 5f;

    [SerializeField] bool autoCompleteOnEnter = true;

    private int quizTab;

    enum Orientation
    {
        Horizontal,
        Vertical,
        SingleGroup
    }

    [SerializeField, Tooltip("Orientation of the tab group, this is important for the border effects")] 
    private Orientation orientation;


    [Tooltip("List of objects swapt by the tabs")] 
    public List<GameObject> objectsToSwap;


    [SerializeField, Tooltip("Invoked when the user has gone through all the tabs in the tab group")] 
    private UnityEvent onTabGroupCompleted;

    /// <summary>
    /// True if the tab group has been initialized
    /// </summary>
    private bool initialized = false;


    private void OnEnable()
    {
        if (selectedTab != null)
            OnTabSelected(selectedTab);
    }

    private void OnDisable()
    {
        if(!initialized)
        {
            initialized = true;
        }
    }

    public void Start()
    {
        if (tabs == null)
        {
            tabs = new List<Tab>();
        }

        foreach (Transform child in transform)
        {
            if (child.GetComponent<Tab>() != null)
            {
                tabs.Add(child.GetComponent<Tab>());
            }
        }

        if (orientation == Orientation.Horizontal)
            quizTab = tabs.Count - 1;

        OnTabSelected(tabs[0]);
        StartCoroutine(InitializeContent());
    }


    /// <summary>
    /// Initializes content, placed in a coroutine to avoid race conditions
    /// </summary>
    /// <returns></returns>
    IEnumerator InitializeContent()
    {
        yield return new WaitForEndOfFrame();
        objectsToSwap[0].SetActive(true);
        if(gameObject.activeSelf)
        {
            initialized = true;
        }
    }

    public void OnTabEnter(Tab t)
    {
        ResetTabs();

        if(selectedTab == null || t != selectedTab)
            t.backgroundImage.color = tabHover;
    }

    public void OnTabExit(Tab t)
    {
        ResetTabs();
    }

    /// <summary>
    /// Select the tab on click. If you're already on the selected tab, or if
    /// the clicked tab is an unavailable quiz, ignore click
    /// </summary>
    /// <param name="t"></param>
    public void OnTabClick(Tab t)
    {
        if (t == selectedTab || (t == tabs[quizTab] && !IsQuizOpen()))
            return;

        OnTabSelected(t);
    }

    /// <summary>
    /// Assign selected tab, check if all tabs are completed and swap objects
    /// tied to the tab group
    /// </summary>
    /// <param name="t"></param>
    public void OnTabSelected(Tab t)
    {
        DescriptiveAudio.HasMouseMoved = false;

        if (t == null)
            return;

        //If t is not the selected tab, clear the body
        if(t != selectedTab)
        {
            ClearBody();
            selectedTab = t;
        }

        selectedTab.Select();

        Debug.Log(selectedTab.gameObject.name);

        ResetTabs();
      
        if(t.backgroundImage != null && t.text != null)
        {
            t.backgroundImage.color = tabActive;
            t.text.color = textColorActive;
        }

        if(orientation == Orientation.Horizontal)
        {
            t.rectTransform.offsetMin = new Vector2(t.rectTransform.offsetMin.x, 0);

            //If the tab is not the last one in the group (quiz), activate the check mark
            if (t != tabs[quizTab] && autoCompleteOnEnter)
                t.Check.GetComponent<SaveState>().Complete = true;

            if (IsTabGroupCompleted(true))
            {

                if(tabs[quizTab].Curtain.gameObject != null) 
                    tabs[quizTab].Curtain.gameObject.SetActive(false);
          
                if (tabs[quizTab].DescriptiveAudio != null && !tabs[quizTab].DescriptiveAudio.enabled)
                    tabs[quizTab].DescriptiveAudio.enabled = true;
            }
        }   

        int index = t.transform.GetSiblingIndex();

        for (int i = 0; i < tabs.Count; i++)
        {
            if(i == index)
            {
               objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
        }  
    }

    /// <summary>
    /// Clears the content and stops any voiceovers 
    /// </summary>
    public void ClearBody()
    {
        foreach (Transform child in body)
        {
            child.gameObject.SetActive(false);
        }
    }


    /// <summary>
    /// Clear all tab images except for the selected one and assign the proper bottom border
    /// </summary>
    public void ResetTabs()
    {
        foreach (Tab t in tabs)
        {
            if (selectedTab != null && t == selectedTab)
                continue;


            if(t.backgroundImage != null) t.backgroundImage.color = tabIdle;
            if(t.text != null) t.text.color = textColorIdle;

            if(orientation == Orientation.Horizontal)
            {
                t.rectTransform.offsetMin = new Vector2(t.rectTransform.offsetMin.x, bottom);
            }
        }
    }

    /// <summary>
    /// Checks if the tab group has been completed
    /// </summary>
    /// <returns></returns>
    private bool IsTabGroupCompleted(bool checkingForQuiz)
    {
        int completedTabs = 0;
        int tabCount = tabs.Count;

        if (checkingForQuiz)
            tabCount = quizTab;

        for (int i = 0; i < tabCount; i++)
        {
            if (tabs[i].Check.GetComponent<SaveState>().Complete)
                completedTabs++;
        }

        return completedTabs == tabCount;
    }

    /// <summary>
    /// Checks if the quiz for the current subtopic is able to be clicked.
    /// </summary>
    /// <returns></returns>
    private bool IsQuizOpen()
    {
        if (orientation == Orientation.Horizontal && tabs[quizTab].Curtain != null && tabs[quizTab].Curtain.activeSelf)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Sets the tab group to complete after the quiz is finished. Only works for tab groups with a parent.
    /// </summary>
    public void CompleteTabGroupAfterQuiz()
    {
        if (IsTabGroupCompleted(true) && parentTab != null)
        {

            tabs[quizTab].Check.GetComponent<SaveState>().Complete = true;
            parentTab.Check.GetComponent<SaveState>().Complete = true;

            //Check if parent tab group is completed as well
            TabGroup parentTabGroup = parentTab.GetComponentInParent<TabGroup>();
            if (parentTabGroup != null && parentTabGroup.IsTabGroupCompleted(false))
                parentTabGroup.onTabGroupCompleted.Invoke();
        } 
    }
}
