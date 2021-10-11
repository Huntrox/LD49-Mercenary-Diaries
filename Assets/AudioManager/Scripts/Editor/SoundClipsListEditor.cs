using UnityEditor;
using HuntroxGames.Utils.Audio;
using System;
using UnityEngine;
using HuntroxGames.Utils.EditorUtils;
using UnityEditorInternal;
using UnityEngine.Audio;

[CustomEditor(typeof(AudioDatabase))]
public class SoundClipsListEditor : Editor
{

    private SerializedObject m_serializedProperty;
    private ReorderableList m_ReorderableList;
    AudioDatabase audioClipsData;
    Rect listRect = new Rect(Vector2.zero, new Vector2(20, 20));
    private void OnEnable()
    {
#if !UNITY_2020_2_OR_NEWER
        audioClipsData = (AudioDatabase)target; 
        m_serializedProperty = new SerializedObject((AudioDatabase)target);
        SerializedProperty array = m_serializedProperty.FindProperty("SoundList");
        m_ReorderableList = new ReorderableList(m_serializedProperty, array, true, true, true, true);
        m_ReorderableList.drawElementCallback = DrawOptionData;
        m_ReorderableList.drawHeaderCallback = DrawHeader;
        m_ReorderableList.elementHeight += 16;
#endif
    }

    private void DrawHeader(Rect rect)
    {
        GUI.Label(rect, "Audio Database");
    }

    private void DrawOptionData(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty itemData = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
        SerializedProperty itemText = itemData.FindPropertyRelative("clipname");
        SerializedProperty clip = itemData.FindPropertyRelative("clip");

        RectOffset offset = new RectOffset(0, 0, -1, -3);
        rect = offset.Add(rect);
        rect.height = EditorGUIUtility.singleLineHeight;

        EditorGUI.PropertyField(rect, itemText, GUIContent.none);
        rect.y += EditorGUIUtility.singleLineHeight;
        rect.x += EditorGUIUtility.labelWidth;
        rect.width = EditorGUIUtility.labelWidth;
        EditorGUI.PropertyField(rect, clip, GUIContent.none);
        //rect.x -= EditorGUIUtility.labelWidth;
        //rect.y += EditorGUIUtility.singleLineHeight;

    }

    public override void OnInspectorGUI()
    {
#if UNITY_2020_2_OR_NEWER
        base.OnInspectorGUI();
#else
        if (GUILayout.Button("Refresh/Save"))
        {
            EditorUtility.SetDirty(serializedObject.targetObject);
            EditorUtility.SetDirty(audioClipsData);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }


        if (GUILayout.Button("Open Audio Manager"))
        {
            AudioManagerEditorWindow.ShowWindow();
        }
        GUILayout.Space(25);
        //   

        audioClipsData.audioMixer = (AudioMixer)EditorGUILayout.ObjectField("AudioMixer", audioClipsData.audioMixer, typeof(AudioMixer), false);

        m_serializedProperty.Update();
        m_ReorderableList.DoLayoutList();
        m_serializedProperty.ApplyModifiedProperties();
#endif
    }

}

