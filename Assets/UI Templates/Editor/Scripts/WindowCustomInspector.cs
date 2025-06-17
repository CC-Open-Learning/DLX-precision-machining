using System;
using UnityEditor;

namespace RemoteEducation.UI
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(Window), true)]
    public class WindowCustomInspector : ToggleCustomInspector
    {
        protected override void DrawFlags()
        {
            base.DrawFlags();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Closeable"));
        }
    }
}