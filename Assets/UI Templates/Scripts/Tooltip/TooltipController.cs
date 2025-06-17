/*
 *  FILE        : TooltipController.cs
 *  PROJECT     : CORE Engine
 *  AUTHOR      : Chowon Jung, Davig Inglis
 *  DESCRIPTION :
 *      Controller to provide tooltips when interacting with GameObjects. 
 *      
 *      The TooltipController.cs file is a modification of the STController.cs script
 *      provided in the SimpleTooltip package by 'snorbertas' (https://github.com/snorbertas/simple-tooltip/)
 *      under the MIT License
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RemoteEducation.UI
{
    /// <summary>
    ///     Controller to provide tooltips when interacting with GameObjects
    /// </summary>
    [DisallowMultipleComponent]
    public class TooltipController : MonoBehaviour
    {
        
        /// <summary>Arbitrary position which will cause the tooltip to be hidden on any screen resolution</summary>
        private readonly Vector2 OffscreenPosition = new(-1000, -1000);

        [System.NonSerialized]
        public bool FixedPosition = false;

        public bool Showing { get; private set; } = false;

        private Image panel;
        private TextMeshProUGUI tooltipText;
        private RectTransform anchor;
        [SerializeField] private RectTransform window;
        private int showInFrames = -1;
        private Vector3 position;

        public static TooltipController Instance { get; private set; }

        private void Awake()
        {
            // Use public member to perform first find-and-check
            if(Instance && Instance != this)
            {
                Debug.LogWarning($"Duplicate TooltipController added to scene, deleting this duplicate instance named {gameObject.name}");
                Destroy(gameObject);
                return;
            }

            Instance = this;

            // Load up both text layers
            tooltipText = GetComponentInChildren<TextMeshProUGUI>();

            // Keep a reference for the panel image and transform
            panel = window.GetComponent<Image>();
            anchor = GetComponent<RectTransform>();

            position = Input.mousePosition;

            // Hide at the start
            HideTooltip();
        }

        void Update()
        {
            UpdateShow();
        }

        private void UpdateShow()
        {
            if (Showing)
            {
                anchor.position = GetTooltipPosition();
            }

            if (showInFrames == -1)
            {
                return;
            }

            if (showInFrames == 0)
            {
                Showing = true;
            }

            showInFrames -= 1;
        }

        public void UpdatePosition(Vector3 pos)
        {
            position = pos;
        }

        public void SetRawText(string text)
        {
            tooltipText.text = text;
        }

        public void SetCustomStyledText(string text, TooltipStyle style)
        {
            // Update the panel sprite and color
            panel.sprite = style.slicedSprite;
            panel.color = style.color;

            // Update the font asset, size and default color
            tooltipText.font = style.fontAsset;
            tooltipText.color = style.defaultColor;

            // Convert all tags to TMPro markup
            var styles = style.fontStyles;
            for (int i = 0; i < styles.Length; i++)
            {
                string addTags = "</b></i></u></s>";
                addTags += "<color=#" + ColorToHexString(styles[i].color) + ">";
                if (styles[i].bold) addTags += "<b>";
                if (styles[i].italic) addTags += "<i>";
                if (styles[i].underline) addTags += "<u>";
                if (styles[i].strikethrough) addTags += "<s>";
                text = text.Replace(styles[i].tag, addTags);
            }

            tooltipText.text = text;
        }


        public void ShowTooltip()
        {
            if (!Showing)
            {
                // After 2 frames, showNow will be set to TRUE
                // after that the frame count wont matter
                if (showInFrames == -1)
                    showInFrames = 2;
            }
        }

        public void HideTooltip()
        {
            showInFrames = -1;
            Showing = false;
            anchor.position = OffscreenPosition;
        }


        /// <summary>
        ///     Calculates the appropriate position for the Tooltip anchor point
        ///     based on screen position and the <c>FixedPosition</c> property
        /// </summary>
        /// <returns>Point at which to place the Tooltip anchor</returns>
        public Vector2 GetTooltipPosition()
        {
            // No further calculation needed if the fixed position is being used
            if (FixedPosition) { return position; }


            Vector2 pos = Input.mousePosition;

            float xLowerBound = window.anchoredPosition.x + (window.rect.width / 2);
            float xUpperBound = Screen.width - xLowerBound;
            
            float yLowerBound = window.anchoredPosition.y + (window.rect.height / 2);
            float yUpperBound = Screen.height - yLowerBound;

            // Adjust X position offset based on screen width
            if (pos.x > xUpperBound)
            {
                pos.x = xUpperBound;
            }
            else if (pos.x < xLowerBound)
            {
                pos.x = xLowerBound;
            }


            // Due to the (0.5, 0) anchor setup of the tooltip defaults, lower-bound 
            // Y validation does not work appropriately. With the default settings, the
            // tooltip should always be visible as the mouse is repositioned
            if (pos.y > yUpperBound)
            {
                pos.y = yUpperBound;
            }
            //else if (pos.y < yLowerBound)
            //{
            //    pos.y = yLowerBound;
            //}

            return pos;
        }

        /// <summary>
        ///     Determines the hexadecimal string corresponding to
        ///     the Color object. Supports RGB colors.
        /// </summary>
        /// <param name="color"></param>
        /// <returns>Hexadecimal color string</returns>
        public static string ColorToHexString(Color color)
        {
            int r = (int)(color.r * 255);
            int g = (int)(color.g * 255);
            int b = (int)(color.b * 255);
            return r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
        }


        /// <summary>
        ///     Instantiates the <see cref="TooltipController"/> responsible for managing 
        ///     and showing all <see cref="SimpleTooltip"/> and <see cref="Tooltip"/> content.
        ///     The reference is then stored as the static <see cref="Instance"/> member.
        /// </summary>
        /// <returns>Boolean indicating whether a TooltipController object was successfully loaded</returns>
        public static bool LoadStaticInstance()
        {
            Instance = FindObjectOfType<TooltipController>();

            if (!Instance)
            {
                Instance = Instantiate(Resources.Load<GameObject>(UIResources.TooltipCanvasAssetPath)).GetComponentInChildren<TooltipController>();
            }

            return Instance;
        }
    }
}