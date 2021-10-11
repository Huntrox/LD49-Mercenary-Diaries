using UnityEditor.UI;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ExtendedButton), true)]
[CanEditMultipleObjects]
public class ButtonExtensionEditor : SelectableEditor
{

    SerializedObject serialized;
    SerializedProperty m_EnterProperty;
    SerializedProperty m_ExitProperty;
    SerializedProperty m_OnClickProperty;

    /*    SerializedProperty m_OnEnterAnimationPropert;
        SerializedProperty m_OnClickAnimationPropert;      
        SerializedProperty m_EnterAnimationPropert;
        SerializedProperty m_ClickAnimationPropert;*/


    protected override void OnEnable()
    {
        base.OnEnable();
        serialized = new SerializedObject(target);
        m_EnterProperty = serializedObject.FindProperty("m_OnEnter");
        m_ExitProperty = serializedObject.FindProperty("m_OnExit");
        m_OnClickProperty = serializedObject.FindProperty("m_OnClick");

        //	m_OnEnterAnimationPropert = serialized.FindProperty("m_OnEnterAnimation");
        //	m_OnClickAnimationPropert = serialized.FindProperty("m_OnClickAnimation");
        //	m_EnterAnimationPropert = serialized.FindProperty("m_EnterAnimation");
        //	m_ClickAnimationPropert = serialized.FindProperty("m_ClickAnimation");


    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        serializedObject.Update();


        EditorGUILayout.PropertyField(m_OnClickProperty);
        EditorGUILayout.PropertyField(m_EnterProperty);
        EditorGUILayout.PropertyField(m_ExitProperty);

        EditorGUILayout.Space();
        //EditorGUILayout.PropertyField(m_ClickAnimationPropert);

        //if (m_ClickAnimationPropert.boolValue)
        //{

        //	SerializedProperty encounters = m_OnClickAnimationPropert.FindPropertyRelative("m_AnimationType");
        //	EditorGUILayout.PropertyField(encounters);

        //	//  EditorGUILayout.PropertyField(prop, true, new GUILayoutOption[0]);

        //	//  EditorGUILayout.ObjectField("m_OnClickAnimation:", MonoScript.FromMonoBehaviour((ButtonExtensionAnimation)target), typeof(ButtonExtensionAnimation), false);

        //}
        //// EditorGUILayout.PropertyField(m_OnEnterAnimationPropert);




        serializedObject.ApplyModifiedProperties();
    }




}
