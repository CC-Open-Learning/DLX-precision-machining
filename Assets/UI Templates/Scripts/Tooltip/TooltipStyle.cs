using System;
using TMPro;
using UnityEngine;

namespace RemoteEducation.UI
{
    /// <summary>
    ///     Defines style for use with <see cref="Tooltip"/> objects
    /// </summary>
    [Serializable]
    [CreateAssetMenu]
    public class TooltipStyle : ScriptableObject
    {
        [Serializable]
        public struct Style
        {
            public string tag;
            public Color color;
            public bool bold;
            public bool italic;
            public bool underline;
            public bool strikethrough;
        }

        [Header("Tooltip Panel")]
        public Sprite slicedSprite;
        public Color color = Color.gray;

        [Header("Font")]
        public TMP_FontAsset fontAsset;
        public Color defaultColor = Color.white;

        [Header("Formatting")]
        public Style[] fontStyles;
    }
}