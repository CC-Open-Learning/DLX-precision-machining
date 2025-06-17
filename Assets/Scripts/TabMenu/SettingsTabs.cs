using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class SettingsTabs : MonoBehaviour
{
    [SerializeField] private List<Button> Tabs;

    [Header("Selected Color Block")]
    [SerializeField] private ColorBlock selectedColorBlock;
    [Header("Deselected Color Block")]
    [SerializeField] private ColorBlock deSelectedColorBlock;

    void Start()
    {
        foreach (Button b in Tabs)
        {
            b.onClick.AddListener(() => SetSelectedButtonUI(b));
        }

        //Make the first tab selected on start
        SetSelectedButtonUI(Tabs[0]);
    }

    void SetSelectedButtonUI(Button selectedButton)
    {
        foreach (Button button in Tabs)
        {
            if (button == selectedButton) continue;

            button.colors = deSelectedColorBlock;
        }

        selectedButton.colors = selectedColorBlock;
    }
}
