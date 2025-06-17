using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TopicNavigation : MonoBehaviour
{
    [SerializeField, Tooltip("Main tab group")]
    private TabGroup mainTabGroup;

    /// <summary>
    /// Child tab groups
    /// </summary>
    private List<TabGroup> childTabGroups = new List<TabGroup>();

    private void Start()
    {
        foreach (GameObject o in mainTabGroup.objectsToSwap)
        {
            TabGroup tempTabGroup = o.GetComponent<TabGroup>();

            if (tempTabGroup != null)
            {
                childTabGroups.Add(tempTabGroup);
            }
        }
    }

    private void Update()
    {
        // checking if the feedback modal is closed to enable keyboard navigation functionality
        if(!PrecisionMachiningGUI.Instance.ModalActive && !PrecisionMachiningGUI.Instance.PopupActive)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MoveUp();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MoveDown();
            }
        } 
    }

    /// <summary>
    /// Move up a tab, if the last tab has been reached move to the next higher level tab (topic)
    /// </summary>
    public void MoveUp()
    {
        TabGroup activeChildTabGroup = GetActiveChildTab();

        int nextTabIndex = 0;

        if(activeChildTabGroup != null)
        {
            for (int i = 0; i < activeChildTabGroup.tabs.Count; i++)
            {
                if (activeChildTabGroup.tabs[i] == activeChildTabGroup.selectedTab)
                {
                    nextTabIndex = i;
                    nextTabIndex++;
                    break;
                }
            }

            if (nextTabIndex > activeChildTabGroup.tabs.Count - 1)
            {
                //Next topic
                nextTabIndex = 0;

                for (int i = 0; i < mainTabGroup.tabs.Count; i++)
                {
                    if (mainTabGroup.tabs[i] == mainTabGroup.selectedTab)
                    {
                        nextTabIndex = i;
                        nextTabIndex++;
                        break;
                    }
                }

                if (nextTabIndex < mainTabGroup.tabs.Count)
                {
                    mainTabGroup.OnTabSelected(mainTabGroup.tabs[nextTabIndex]);

                    activeChildTabGroup = GetActiveChildTab();


                    if (activeChildTabGroup.tabs != null)
                    {
                        activeChildTabGroup.OnTabSelected(activeChildTabGroup.tabs.First());
                    }
                }
            }
            else
            {
                activeChildTabGroup.OnTabSelected(activeChildTabGroup.tabs[nextTabIndex]);
            }
        }
        else
        {

            for (int i = 0; i < mainTabGroup.tabs.Count; i++)
            {
                if (mainTabGroup.tabs[i] == mainTabGroup.selectedTab)
                {
                    nextTabIndex = i;
                    nextTabIndex++;
                    break;
                }
            }

            if (nextTabIndex < mainTabGroup.tabs.Count)
            {
                mainTabGroup.OnTabSelected(mainTabGroup.tabs[nextTabIndex]);
            }
        }
    }

    /// <summary>
    /// Move down a tab, if the last tab has been reached move to the previous higher level tab (topic)
    /// </summary>
    public void MoveDown()
    {
        TabGroup activeChildTabGroup = GetActiveChildTab();

        int prevTabIndex = 0;


        if (activeChildTabGroup != null)
        {
            for (int i = 0; i < activeChildTabGroup.tabs.Count; i++)
            {
                if (activeChildTabGroup.tabs[i] == activeChildTabGroup.selectedTab)
                {
                    prevTabIndex = i;
                    prevTabIndex--;
                    break;
                }
            }

            if (prevTabIndex < 0)
            {
                //Next topic
                prevTabIndex = 0;

                for (int i = 0; i < mainTabGroup.tabs.Count; i++)
                {
                    if (mainTabGroup.tabs[i] == mainTabGroup.selectedTab)
                    {
                        prevTabIndex = i;
                        prevTabIndex--;
                        break;
                    }
                }

                if (prevTabIndex >= 0)
                {
                    mainTabGroup.OnTabSelected(mainTabGroup.tabs[prevTabIndex]);

                    activeChildTabGroup = GetActiveChildTab();
                    activeChildTabGroup.OnTabSelected(activeChildTabGroup.tabs.Last());
                }
            }
            else
            {
                activeChildTabGroup.OnTabSelected(activeChildTabGroup.tabs[prevTabIndex]);
            }
        }
        else
        {
            for (int i = 0; i < mainTabGroup.tabs.Count; i++)
            {
                if (mainTabGroup.tabs[i] == mainTabGroup.selectedTab)
                {
                    prevTabIndex = i;
                    prevTabIndex-- ;
                    break;
                }
            }

            if (prevTabIndex >= 0)
            {
                mainTabGroup.OnTabSelected(mainTabGroup.tabs[prevTabIndex]);
            }
        }
    }

    /// <summary>
    /// Get the active tab
    /// </summary>
    /// <returns></returns>
    private TabGroup GetActiveChildTab()
    {
        foreach (TabGroup t in childTabGroups)
        {
            if (t.gameObject.activeSelf == true)
            {
                return t;
            }
        }

        return null;
    }
}
