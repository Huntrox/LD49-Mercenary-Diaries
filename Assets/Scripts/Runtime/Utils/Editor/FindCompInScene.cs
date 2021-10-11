using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HuntroxGames.Utils
{
    public static class FindCompInScene
    {
        private static SearchableEditorWindow hierarchy;
        [MenuItem("Assets/Find in Scene")]
        private static void FindInScene() => SetSearchFilter(Selection.activeObject.name, 0);
        [MenuItem("Assets/Find in Scene", true)]
        private static bool FindInSceneValidation() => Selection.activeObject is Object;


        [MenuItem("CONTEXT/Component/Find in Scene")]
        private static void findinSceneContextMenu(MenuCommand command)
        {
            Component component = (Component)command.context;
            SetSearchFilter(component.GetType().Name, 0);
        }
        public static void SetSearchFilter(string filter, int filterMode)
        {
            SearchableEditorWindow[] windows = (SearchableEditorWindow[])Resources.FindObjectsOfTypeAll(typeof(SearchableEditorWindow));
            foreach (SearchableEditorWindow window in windows)
            {
                if (window.GetType().ToString() == "UnityEditor.SceneHierarchyWindow")
                {
                    hierarchy = window;
                    break;
                }
            }
            if (hierarchy == null) return;
            MethodInfo setSearchType = typeof(SearchableEditorWindow).GetMethod("SetSearchFilter", BindingFlags.NonPublic | BindingFlags.Instance);
            object[] parameters = new object[] { filter, filterMode, false, false };
            setSearchType.Invoke(hierarchy, parameters);
        }

    }
}