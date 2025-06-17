using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighlightFlash : MonoBehaviour
{
    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.color = Color.cyan;
    }

    private void Start()
    {
        InvokeRepeating("HighlightToggle", 0.5f, 0.5f);
    }

    private void OnDisable()
    {
        CancelInvoke();
        image.color = Color.cyan;
    }

    private void HighlightToggle()
    {
        image.color = image.color == Color.cyan ? Color.clear : Color.cyan;
    }
}
