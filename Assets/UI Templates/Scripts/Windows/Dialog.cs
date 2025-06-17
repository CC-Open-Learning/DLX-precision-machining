/*
*  FILE          :	PolarDialog.cs
*  PROJECT       :	Core Engine (Interaction system)
*  PROGRAMMER    :	Duane Cressman, Chowon Jung
*  FIRST VERSION :	2020-7-15
*  DESCRIPTION   :  This file contains the MainMenuDialog class. 
*  
*  Class Description: This class is used to manager a "Go back to Main Menu" dialog. Right now it is super basic.
*/

using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace RemoteEducation.UI
{
    public class Dialog : Window
    {
        /// <summary>This allows you to perform an action when this toggle turns on.</summary>


        /// <summary>This allows you to perform an action when this toggle turns off.</summary>


        [SerializeField, FormerlySerializedAs("OnConfirm")] private UnityEvent onConfirm;

        [SerializeField, FormerlySerializedAs("OnCancel")] private UnityEvent onCancel;

        /// <summary>Event fired when the Toggle is switched on</summary>
        public UnityEvent OnConfirm { get { if (onConfirm == null) onConfirm = new UnityEvent(); return onConfirm; } }


        /// <summary>Event fired when the Toggle is switched off</summary>
        public UnityEvent OnCancel { get { if (onCancel == null) onCancel = new UnityEvent(); return onCancel; } }

        [Header("Text Content")]
        [SerializeField] private TextMeshProUGUI titleText;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private TextMeshProUGUI confirmText;
        [SerializeField] private TextMeshProUGUI cancelText;

        //public bool customBehaviour = false;


        public void SetDialogText(string title, string description, string confirm = "Confirm", string cancel = "Cancel")
        {
            titleText.text = title;
            descriptionText.text = description;
            confirmText.text = confirm;
            cancelText.text = cancel;
        }

        public void Confirm()
        {
            OnConfirm?.Invoke();
        }

        public void Cancel()
        {
            OnCancel?.Invoke();

        }

        [Obsolete]
        public void ConfirmDialog()
        {
            Confirm();
        }

        [Obsolete]
        public void CancelDialog()
        {
            Cancel();
        }
    }

/* considering custom editor functionality for Window management, but I dunno yet
    [CustomEditor(typeof(Dialog), true)]
    public class Dialog_Editor : LeanInspector<LeanToggle>
    {
        protected override void DrawInspector()
        {
            Draw("titleText");
            Draw("descriptionText");
            Draw("confirmText");
            Draw("cancelText");

            EditorGUILayout.Separator();

            Draw("Closeable", "Indicates whether this Window should be closeable by the LeanWindowCloser when it is the topmost Window");

            if (Draw("on", "This lets you change the current toggle state of this UI element.") == true)
            {
                Each(t => t.On = serializedObject.FindProperty("on").boolValue, true);
            }

            if (Draw("turnOffSiblings", "If you enable this, then any sibling GameObjects (i.e. same parent GameObject) will automatically be turned off. This allows you to make radio boxes, or force only one panel to show at a time, etc.") == true)
            {
                Each(t => t.TurnOffSiblings = serializedObject.FindProperty("turnOffSiblings").boolValue, true);
            }

            EditorGUILayout.Separator();

            Draw("onTransitions", "This allows you to perform a transition when this toggle turns on. You can create a new transition GameObject by right clicking the transition name, and selecting Create. For example, the LeanGraphicColor (Graphic.color Transition) component can be used to change the color.\n\nNOTE: Any transitions you perform here should be reverted in the <b>Off Transitions</b> setting using a matching transition component.");
            Draw("offTransitions", "This allows you to perform a transition when this toggle turns off. You can create a new transition GameObject by right clicking the transition name, and selecting Create. For example, the LeanGraphicColor (Graphic.color Transition) component can be used to change the color.");

            EditorGUILayout.Separator();

            Draw("onOn");
            Draw("onOff");
            Draw("OnConfirm");
            Draw("OnCancel");
        }
    }
*/
}