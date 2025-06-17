using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopUpWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Image popUpImage;
    private AudioClip clip;

    
    public void SetPopUpWindowContent(PopUpWindowScriptableObject p)
    {
        title.text = p.title;
        description.text = p.description;
        popUpImage.sprite = p.popUpImage;
    }
}
