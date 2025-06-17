using System;
using UnityEditor;

namespace RemoteEducation.UI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Dialog), true)]
    public class DialogCustomInspector : WindowCustomInspector
    {
        protected override void DrawEvents()
        {
            base.DrawEvents();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onConfirm"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onCancel"));
        }
    }
}