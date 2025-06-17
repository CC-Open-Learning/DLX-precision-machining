using ScratchCardAsset;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 cursorHotspot = Vector2.zero;

    private void OnEnable()
    {
        Cursor.SetCursor(cursorTexture, cursorHotspot, cursorMode);
    }

    private void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    public void ActivateCustomCursor()
    {
        enabled = true;
    }

    public void DeactivateCustomCursor()
    {
        enabled = false;
    }

    public void ActivateAfterObjectDismissed(GameObject dismissedObj)
    {
        StartCoroutine(ActivateAfterObjectDismissedCoroutine(dismissedObj));
    }

    private IEnumerator ActivateAfterObjectDismissedCoroutine(GameObject dismissedObj)
    {
        while (dismissedObj.activeSelf)
            yield return null;

        ActivateCustomCursor();
    }

    private Texture2D ChangeTextureFormat(Texture2D originalTexture, TextureFormat newFormat)
    {
        Texture2D newTexture = new Texture2D(2, 2, newFormat, false);
        newTexture.SetPixels(originalTexture.GetPixels());
        newTexture.Apply();

        return newTexture;
    }
}
