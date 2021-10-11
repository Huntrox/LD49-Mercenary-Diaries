using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HuntroxGames.Utils.Audio
{
	public static class AudioManagerUtils
	{
        public static AudioSource CreateAudioSource(Transform parent)
        {
            GameObject tempAudioSource = new GameObject("UniqueAudioSource");
            tempAudioSource.transform.parent = parent.transform;
            tempAudioSource.transform.position = Vector3.zero;
            return tempAudioSource.AddComponent<AudioSource>();
        }
        public static void RemoveAt<T>(ref T[] arr, int index)
        {
            for (int a = index; a < arr.Length - 1; a++)
            {
                arr[a] = arr[a + 1];
            }
            System.Array.Resize(ref arr, arr.Length - 1);
        }
#if UNITY_EDITOR
        public static string[] GetCurrentScenesName()
        {
            List<string> scenes = new List<string>();
            foreach (UnityEditor.EditorBuildSettingsScene scene in UnityEditor.EditorBuildSettings.scenes)
            {
                scenes.Add(System.IO.Path.GetFileNameWithoutExtension(scene.path));
            }
            return scenes.ToArray();
        }


        public static GUIStyle LabelStyle(int fontSize = 18, bool IsBoold = true)
        {
            GUIStyle gUI = new GUIStyle();

            gUI.fontSize = fontSize;
            if (IsBoold)
                gUI.fontStyle = FontStyle.Bold;
            gUI.alignment = TextAnchor.MiddleCenter;
            gUI.normal.textColor = GUI.color;
            return gUI;
        }
#endif
    }
}