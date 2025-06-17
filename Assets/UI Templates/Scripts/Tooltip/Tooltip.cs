/*
 *  FILE    : Tooltip.cs
 *  PROJECT : CORE Engine
 *  AUTHOR  : David Inglis, Chowon Jung
 *  DATE    : 2020-08-25
 *  DESC    : 
 *      This script allows a controller on a GameObject specify when 
 *      to show, hide, and update the attached tooltip
 *      
 *      The SimpleTooltip.cs file is a modification of the SimpleTooltip.cs script
 *      provided in the SimpleTooltip package by 'snorbertas' (https://github.com/snorbertas/simple-tooltip/)
 *      under the MIT License
 */

using UnityEngine;
using UnityEngine.Serialization;

namespace RemoteEducation.UI
{
    /// <summary>
    ///     Allows a <see cref="Component"/> on a <see cref="GameObject"/> to manually control
    ///     when to show, hide and update a UI-layer tooltip through the <see cref="TooltipController"/>
    /// </summary>
    /// <remarks>
    ///     This class and the <see cref="SimpleTooltip"/> class are modified from
    ///     the <see href="https://github.com/snorbertas/simple-tooltip/">SimpleTooltip</see> 
    ///     package by <see href="https://github.com/snorbertas">snorbertas</see> under the MIT License
    /// </remarks>
    [DisallowMultipleComponent]
    public class Tooltip : MonoBehaviour
    {

        public TooltipStyle simpleTooltipStyle;

        [Tooltip("The text displayed when the tooltip is enabled and active")]
        [FormerlySerializedAs("infoLeft")]
        [TextArea] public string tooltipText = "Hello";

        /// <summary>
        ///     Reference to the <see cref="TooltipController"/> in the Scene
        /// </summary>
        protected static TooltipController tooltipController => TooltipController.Instance;

        public bool Showing { get; protected set; } = false;

        public bool Fixed
        {
            get => tooltipController.FixedPosition;

            set
            {
                tooltipController.FixedPosition = value;
                tooltipController.UpdatePosition(Input.mousePosition);
            }
        }

        protected void Awake()
        {

            if (!tooltipController)
            {
                // Add a new tooltip prefab if one does not exist yet
                TooltipController.LoadStaticInstance();
            }

            if (!tooltipController)
            {
                Debug.LogWarning("Could not find the Tooltip prefab");
                Debug.LogWarning("Make sure you don't have any other prefabs named `SimpleTooltip`");
            }

            // Always make sure there's a style loaded
            CheckStyleLoaded();
        }

        private void Reset()
        {
            // Load the default style if none is specified
            CheckStyleLoaded();

            // If UI, nothing else needs to be done
            if (GetComponent<RectTransform>())
            {
                return;
            }

            // If has a collider, nothing else needs to be done
            if (GetComponent<Collider>())
            {
                return;
            }

            // There were no colliders found when the component is added so we'll add a box collider by default
            // If you are making a 2D game you can change this to a BoxCollider2D for convenience
            // You can obviously still swap it manually in the editor but this should speed up development
            gameObject.AddComponent<BoxCollider>();
        }


        /// <summary>
        ///     Ensures that the tooltip is hidden when the MonoBehaviour is disabled
        /// </summary>
        public void OnDisable()
        {
            HideTooltip();
        }


        /// <summary>
        ///     Indicates to the global <see cref="TooltipController"/> that this tooltip 
        ///     should be displayed, if this tooltip Component is enabled and no other
        ///     tooltip is being shown currently
        /// </summary>
        /// <remarks>
        ///     This method respects the <see cref="Behaviour.enabled"/> state of 
        ///     the <see cref="Behaviour"/> lifecycle
        /// </remarks>
        public void ShowTooltip()
        {
            if (enabled && (Showing || !tooltipController.Showing))
            {
                Showing = true;

                UpdateTooltip();

                // Then tell the controller to show it
                tooltipController.ShowTooltip();
            }
        }

        /// <summary>
        ///     Updates the global <see cref="TooltipController"/> with the content and 
        ///     style of this tooltip
        /// </summary>
        public void UpdateTooltip()
        {
            if (Showing)
            {
                tooltipController.SetCustomStyledText(tooltipText, simpleTooltipStyle);
            }
        }


        /// <summary>
        ///     Indicates to the global <see cref="TooltipController"/> that this tooltip 
        ///     should be no longer be displayed
        /// </summary>
        public void HideTooltip()
        {
            if (!Showing)
            {
                return;
            }

            Showing = false;
            tooltipController.HideTooltip();
        }

        /// <summary>
        ///     Loads the default <see cref="TooltipStyle"/>
        /// </summary>
        private void CheckStyleLoaded()
        {
            if (simpleTooltipStyle) { return; }
            
            simpleTooltipStyle = Resources.Load<TooltipStyle>(UIResources.DefaultTooltipStyleResource);
        }


    }
}