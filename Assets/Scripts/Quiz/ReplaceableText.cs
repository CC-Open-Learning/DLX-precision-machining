using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReplaceableText : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private string questionText;

    private void Awake()
    {
        questionText = text.text;
    }

    // Replaces the text with the correct answer in the fill in the blank questions
    public void ReplaceText(string s)
    {
        var tokens = questionText.Split('_');
        text.text = $"{tokens[0]}{s}{tokens[tokens.Length - 1]}";
    }
}
