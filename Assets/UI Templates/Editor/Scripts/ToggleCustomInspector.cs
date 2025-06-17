using System;
using UnityEditor;

namespace RemoteEducation.UI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Toggle), true)]
    public class ToggleCustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();

            DrawFlags();

            EditorGUILayout.Separator();

            DrawEvents();

            if (EditorGUI.EndChangeCheck())
            {
                Array.ForEach(targets, target => (target as Toggle).On = serializedObject.FindProperty("on").boolValue);
            }

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawFlags()
        {
            // Flags
            EditorGUILayout.PropertyField(serializedObject.FindProperty("on"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("turnOffSiblings"));
        }

        protected virtual void DrawEvents()
        {
            // Events
            EditorGUILayout.LabelField("Events", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onOn"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onOff"));
        }
    }
}