using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelSwap : MonoBehaviour
{
    [SerializeField, Tooltip("New sprite")]private Sprite NewSprite;
    [SerializeField, Tooltip("New object size, if object needs to be resized")] public RectTransform NewObjectSize;
    [SerializeField, Tooltip("The panel that will receive the new sprite")] public GameObject Panel;
    [SerializeField, Tooltip("Reference to the object dropped on the panel")] public GameObject PanelObject;

    private Image objectImage;
    private Sprite originalSprite;
    private RectTransform objectSize;

    private void Start()
    {
        objectImage = GetComponent<Image>();
        originalSprite = objectImage.sprite;
        objectSize = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Swaps the current sprite for the new sprint.
    /// </summary>
    public void SwapSprite()
    {
        Image panelImg = Panel.GetComponent<Image>();

        if (!panelImg.enabled) 
            panelImg.enabled = true;

        panelImg.sprite = NewSprite;
        panelImg.preserveAspect = true;

        // If the correct answer is dropped on the panel we are setting the object replacement
        // in the Panel Settings to not allowed.
        if(GetComponent<Answer>().CorrectAnswer == true && Panel.GetComponent<PanelSettings>() != null)
        {
            Panel.GetComponent<PanelSettings>().ObjectReplacement = PanelSettings.ObjectReplace.NotAllowed;
        }
    }

    //Resizes the sprites in swap
    public void SwapAndResize()
    {
        objectImage.sprite = NewSprite;

        if (NewObjectSize != null)
            objectSize = NewObjectSize;
    }

    // Swaps back the sprite to the original one if the drop fails.
    public void SwapBackOnFail()
    {
        objectImage.sprite = originalSprite;
    }

    public void PositionObject()
    {
        RectTransform objectRectTransform = PanelObject.GetComponent<RectTransform>();

        objectRectTransform.anchorMax = NewObjectSize.anchorMax;
        objectRectTransform.anchorMin = NewObjectSize.anchorMin;
        objectRectTransform.position = NewObjectSize.position;
    }
}
