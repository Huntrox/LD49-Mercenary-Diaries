using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using HuntroxGames.Utils.Audio;
using HuntroxGames.Utils;


[CustomEditor(typeof(SoundTrigger))]
[CanEditMultipleObjects]
public class SoundTriggerEditor : Editor
{


    private SerializedProperty triggerEvent;
    private SerializedProperty sound;
    private SerializedProperty tag;
    private SerializedProperty usedByEventsManager;



    private void OnEnable()
    {
        triggerEvent = serializedObject.FindProperty("triggerEvent");
        sound = serializedObject.FindProperty("sound");

        tag = serializedObject.FindProperty("tag");
        usedByEventsManager = serializedObject.FindProperty("usedByEventsManager");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.UpdateIfRequiredOrScript();




        if (!usedByEventsManager.boolValue)
        {
            EditorGUILayout.PropertyField(triggerEvent, new GUIContent("Trigger Event"));
        }
        EditorGUILayout.PropertyField(sound, new GUIContent("Sound Clip"));


        if (triggerEvent.enumValueIndex == (int)TriggerEvent.OnCollisionEnter ||
              triggerEvent.enumValueIndex == (int)TriggerEvent.OnCollisionEnter2D ||
                triggerEvent.enumValueIndex == (int)TriggerEvent.OnCollisionExit ||
                  triggerEvent.enumValueIndex == (int)TriggerEvent.OnCollisionExit2D ||
                    triggerEvent.enumValueIndex == (int)TriggerEvent.OnTriggerEnter ||
                      triggerEvent.enumValueIndex == (int)TriggerEvent.OnTriggerEnter2D ||
                        triggerEvent.enumValueIndex == (int)TriggerEvent.OnTriggerExit ||
                          triggerEvent.enumValueIndex == (int)TriggerEvent.OnTriggerEnter2D
            )
        {

            tag.stringValue = EditorGUILayout.TagField("Trigger Tag",tag.stringValue);
        }
           

        EditorGUILayout.Space();




        serializedObject.ApplyModifiedProperties();
    }




}
