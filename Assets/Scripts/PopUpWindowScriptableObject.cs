using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPopUp", menuName = "ScriptableObjects/PopUpWindowScriptableObject", order = 2)]
public class PopUpWindowScriptableObject : ScriptableObject
{
    public string description;
    public string title;
    public Sprite popUpImage;
    public AudioClip audioClip;
}
