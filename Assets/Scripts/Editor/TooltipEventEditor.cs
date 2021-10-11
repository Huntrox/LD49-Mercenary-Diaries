using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TooltipTriggerEvent))]
[CanEditMultipleObjects]
public class TooltipEventEditor : Editor
{

    private SerializedProperty triggerEvent;
    private SerializedProperty sp_string_header;
    private SerializedProperty sp_string_content;
    private SerializedProperty sp_img;
    private SerializedProperty sp_delay;

    private void OnEnable()
    {
        triggerEvent = serializedObject.FindProperty("triggerEvent");
        sp_string_header = serializedObject.FindProperty("header");
        sp_string_content = serializedObject.FindProperty("Content");
        sp_delay = serializedObject.FindProperty("delay");
        sp_img = serializedObject.FindProperty("price");

    }
    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();

        EditorGUILayout.PropertyField(triggerEvent, new GUIContent("Trigger Event"));
        EditorGUILayout.PropertyField(sp_string_header, new GUIContent("Header"));
        EditorGUILayout.PropertyField(sp_string_content, new GUIContent("Content"));
        EditorGUILayout.PropertyField(sp_string_content, new GUIContent("Price"));
        EditorGUILayout.PropertyField(sp_delay, new GUIContent("Delay"));

        serializedObject.ApplyModifiedProperties();

    }



}
