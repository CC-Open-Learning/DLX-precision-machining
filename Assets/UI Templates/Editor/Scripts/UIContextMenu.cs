using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


namespace RemoteEducation.UI
{

    /// <summary>
    ///     Provides context menu options in the Hierarchy window and GameObject tab 
    ///     for creating instances of CORE UI elements from their template resources
    /// </summary>
    public class UIContextMenu : Editor
    {

        /// <summary>
        ///     Instantiates the GameObject specified by <paramref name="resourcePath"/> contained in a file path 
        ///     within the Unity Editor <see cref="Resources"/> set of folders
        /// </summary>
        /// <param name="resourcePath"></param>
        private static void CreateObject(string resourcePath)
        {
            GameObject clone = PrefabUtility.InstantiatePrefab(Resources.Load(resourcePath)) as GameObject;

            try
            {

                Undo.RegisterCreatedObjectUndo(clone, "Created an object");

                if (Selection.activeGameObject == null)
                {
                    var canvas = FindObjectsOfType<Canvas>()[0];
                    clone.transform.SetParent(canvas.transform, false);
                }
                else
                {
                    clone.transform.SetParent(Selection.activeGameObject.transform, false);
                }

            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                Debug.LogWarning($"Unable to insert {clone.name}");

                Destroy(clone);
            }

            if (Application.isPlaying == false)
                EditorSceneManager.MarkSceneDirty(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }


        [MenuItem("GameObject/CORE UI/Windows/Standard", false, 10)]
        static void Window()
        {
            CreateObject(UIResources.WindowTemplateResourcePath + "Window");

        }

        [MenuItem("GameObject/CORE UI/Windows/Modal", false, 10)]
        static void ModalWindow()
        {
            CreateObject(UIResources.WindowTemplateResourcePath + "Modal");
        }

        [MenuItem("GameObject/CORE UI/Windows/Dialog", false, 10)]
        static void DialogWindow()
        {
            CreateObject(UIResources.WindowTemplateResourcePath + "Dialog");
        }

        [MenuItem("GameObject/CORE UI/Switch", false, 10)]
        static void Switch()
        {
            CreateObject(UIResources.UITemplateResourcePath + "Switch");

        }

        [MenuItem("GameObject/CORE UI/Button/Standard", false, 10)]
        static void Button()
        {
            CreateObject(UIResources.UITemplateResourcePath + "Standard Button");

        }

        [MenuItem("GameObject/CORE UI/Button/Outline", false, 10)]
        static void ButtonOutline()
        {
            CreateObject(UIResources.UITemplateResourcePath + "Outline Button");
        }

        [MenuItem("GameObject/CORE UI/Button/Radial", false, 10)]
        static void ButtonRadial()
        {
            CreateObject(UIResources.UITemplateResourcePath + "Radial Button");
        }

        [MenuItem("GameObject/CORE UI/Button/Menu", false, 10)]
        static void ButtonMenu()
        {
            CreateObject(UIResources.UITemplateResourcePath + "Menu Button");
        }

    }
}
