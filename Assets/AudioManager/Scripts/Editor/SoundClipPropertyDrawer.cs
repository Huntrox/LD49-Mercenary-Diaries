using System;
using HuntroxGames.Utils.Audio;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace HuntroxGames.Utils.EditorUtils
{
	[CustomPropertyDrawer(typeof(SoundClip))]
	public class SoundClipPropertyDrawer : PropertyDrawer
	{
		SerializedProperty name_sp;
		SerializedProperty AudioClip_sp;
		SerializedProperty delay_sp;
		SerializedProperty pitch_sp;
		SerializedProperty volume_sp;
		SerializedProperty loop_sp;
		SerializedProperty randomVolume_sp;
		SerializedProperty randomPitch_sp;

		SerializedProperty minPitch_sp;
		SerializedProperty maxPitch_sp;


		SerializedProperty minVolume_sp;
		SerializedProperty maxVolume_sp;

		SerializedProperty group_sp;



		SoundClip clip;

		string[] m_audioClips_choices;
		string[] audioMixerGroupsNames;


		int audioMixerGroups_index = 0;

		int m_choice_index = 0;

		bool showFoldout;

		Texture playicon;
		Texture stopicon;

		GUIContent playbtn_content;
		GUIContent stopbtn_content;
		Rect oriRect;
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{

			if (property.isExpanded)
			{
				if (clip == null) return 20;
				return +185;
			}
			else return 20;




		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			var target = property.serializedObject.targetObject;

			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			position.width -= 34;
			position.height = 18;
			oriRect = new Rect(position);

			Rect t_Rect = new Rect(position);
			t_Rect.x += 10;
			//t_Rect.y += 10;
			t_Rect.width = 30;
			t_Rect.height = 25;


			property.isExpanded = EditorGUI.Foldout(t_Rect, property.isExpanded, "");
			m_audioClips_choices = AudioDataHandler.GetSoundsListNames();
			audioMixerGroupsNames = new string[AudioDataHandler.GetAudioMixer.FindMatchingGroups("Master").Length];

			for (int i = 0; i < AudioDataHandler.GetAudioMixer.FindMatchingGroups("Master").Length; i++)
			{
				audioMixerGroupsNames[i] = AudioDataHandler.GetAudioMixer.FindMatchingGroups("Master")[i].name;
			}



			init_properties(property);


			if (AudioDataHandler.CheckClipExist(name_sp.stringValue))
				m_choice_index = Array.IndexOf(m_audioClips_choices, name_sp.stringValue);
			else
				m_choice_index = 0;


			position.x += 15;
			position.width -= 15;

			m_choice_index = EditorGUI.Popup(position, m_choice_index, m_audioClips_choices);

			if (m_choice_index >= 0)
				name_sp.stringValue = m_audioClips_choices[m_choice_index];


			//	AudioClip_sp.objectReferenceValue = S

			position.x += position.width + 2;
			position.width = 17;
			position.height = 17;




			if (GUI.Button(position, playbtn_content))
			{
				if (clip.clip)
					AudioController.PlaySoundEditMode(clip);
			}
			position.x += position.width + 2;

			if (GUI.Button(position, stopbtn_content))
			{
				if (!string.IsNullOrEmpty(name_sp.stringValue))
				{
					AudioController.StopAudioSource();
					AudioDataHandler.StopAllClips();
				}
			}

			DrawFoldoutProperty(position, property);

			ApplyProperties(property);



			EditorGUI.EndProperty();
		}

		private void ApplyProperties(SerializedProperty property)
		{
			if (!string.IsNullOrEmpty(name_sp.stringValue))
			{
				if (AudioDataHandler.CheckClipExist(name_sp.stringValue))
				{
					clip = AudioDataHandler.GetSoundClip(name_sp.stringValue);
					AudioClip_sp.objectReferenceValue = clip.clip;
					delay_sp.floatValue = clip.delay;
					pitch_sp.floatValue = clip.pitch;
					volume_sp.floatValue = clip.volume;
					loop_sp.boolValue = clip.loop;
					group_sp.objectReferenceValue = clip.group;
					randomVolume_sp.boolValue = clip.RandomVolume;
					randomPitch_sp.boolValue = clip.RandomPitch;
					minPitch_sp.floatValue = clip.minMaxPitch._MinValue;
					maxPitch_sp.floatValue = clip.minMaxPitch._MaxValue;
					minVolume_sp.floatValue = clip.minMaxVolume._MinValue;
					maxVolume_sp.floatValue = clip.minMaxVolume._MaxValue;
				}
			}
			else
			{

				name_sp.stringValue = m_audioClips_choices[0];
			}

			//clip.clipname = name_sp.stringValue;
			//clip.clip = (AudioClip)AudioClip_sp.objectReferenceValue;
			//clip.delay = delay_sp.floatValue;
			//clip.pitch = pitch_sp.floatValue;
			//clip.volume = volume_sp.floatValue;
			//clip.loop = loop_sp.boolValue;
			//clip.group = (AudioMixerGroup)group_sp.objectReferenceValue;

			//var target = property.serializedObject.targetObject;
			//clip = fieldInfo.GetValue(target) as SoundClip;
			//Debug.Log(target.ToString());
			property.serializedObject.ApplyModifiedProperties();
		}
		private void DrawFoldoutProperty(Rect position, SerializedProperty property)
		{
			if (property.isExpanded)
			{

				if (clip == null) return;
				//	position.y += 20;
				//position.x -= position.width+20;
				oriRect.y += 25;
				oriRect.x -= position.width + 5;
				oriRect.width += 20;

				GUIStyle style = new GUIStyle(GUI.skin.box);
				Rect boxRect = oriRect;

				boxRect.width += 114;//62
				boxRect.height += 135;
				boxRect.x -= position.width + 62;

				GUI.Box(boxRect, "Options", style);
				oriRect.y += 25;
				oriRect.x -= position.width + 38;
				EditorGUI.LabelField(oriRect, "Delay");

				oriRect.x += position.width + 45;

				clip.delay = EditorGUI.Slider(oriRect, clip.delay, 0f, 5f);

				oriRect.y += 20;
				oriRect.x -= position.width + 45;

				EditorGUI.LabelField(oriRect, "Pitch");
				oriRect.x += position.width + 45;
				if (clip.RandomPitch)
				{
					Rect randPitchRect = oriRect;
					randPitchRect.width -= 65;
					EditorGUI.MinMaxSlider(randPitchRect, ref clip.minMaxPitch._MinValue, ref clip.minMaxPitch._MaxValue, 0, 3);
					randPitchRect.x += randPitchRect.width + 5;
					randPitchRect.width = 30;
					clip.minMaxPitch.MinValue = EditorGUI.FloatField(randPitchRect, clip.minMaxPitch.MinValue);
					randPitchRect.x += 30;
					clip.minMaxPitch.MaxValue = EditorGUI.FloatField(randPitchRect, clip.minMaxPitch.MaxValue);
				}
				else
					clip.pitch = EditorGUI.Slider(oriRect, clip.pitch, 0, 3f);

				oriRect.y += 20;
				oriRect.x -= position.width + 45;
				EditorGUI.LabelField(oriRect, "Volume");
				oriRect.x += position.width + 45;


				if (clip.RandomVolume)
				{
					Rect randVolumehRect = oriRect;
					randVolumehRect.width -= 65;
					EditorGUI.MinMaxSlider(randVolumehRect, ref clip.minMaxVolume._MinValue, ref clip.minMaxVolume._MaxValue, 0, 1);
					randVolumehRect.x += randVolumehRect.width + 5;
					randVolumehRect.width = 30;
					clip.minMaxVolume.MinValue = EditorGUI.FloatField(randVolumehRect, clip.minMaxVolume.MinValue);
					randVolumehRect.x += 30;
					clip.minMaxVolume.MaxValue = EditorGUI.FloatField(randVolumehRect, clip.minMaxVolume.MaxValue);

				}
				else
					clip.volume = EditorGUI.Slider(oriRect, clip.volume, 0f, 1f);

				oriRect.y += 20;
				oriRect.x -= position.width + 45;
				EditorGUI.LabelField(oriRect, "Loop :");
				oriRect.x += position.width + 45;
				Rect boolsRect = oriRect;
				boolsRect.width = 20;
				clip.loop = EditorGUI.Toggle(boolsRect, clip.loop);
				boolsRect.x += position.width + 3;
				boolsRect.width = oriRect.width;
				EditorGUI.LabelField(boolsRect, "Random Pitch :");
				boolsRect.x += position.width + 80;

				clip.RandomPitch = EditorGUI.Toggle(boolsRect, clip.RandomPitch);

				oriRect.y += 20;
				oriRect.x -= position.width + 45;
				EditorGUI.LabelField(oriRect, "Random Volume :");
				oriRect.x += position.width + 85;
				oriRect.width = 20;
				clip.RandomVolume = EditorGUI.Toggle(oriRect, clip.RandomVolume);
				oriRect.y += 25;
				oriRect.x -= position.width + 85;
				oriRect.width = position.width + 65;
				EditorGUI.LabelField(oriRect, " AM Group :");
				oriRect.x += position.width + 55;
				oriRect.width += 75;
				audioMixerGroups_index = Array.IndexOf(audioMixerGroupsNames, clip.group.name);
				audioMixerGroups_index = GUI.Toolbar(oriRect, audioMixerGroups_index, audioMixerGroupsNames);
				clip.group = AudioDataHandler.GetAudioMixer.FindMatchingGroups("Master")[audioMixerGroups_index];
				oriRect.y += 5;
			}
		}

		private void init_properties(SerializedProperty property)
		{
			name_sp = property.FindPropertyRelative("clipname");
			AudioClip_sp = property.FindPropertyRelative("clip");
			delay_sp = property.FindPropertyRelative("delay");
			pitch_sp = property.FindPropertyRelative("pitch");
			volume_sp = property.FindPropertyRelative("volume");
			loop_sp = property.FindPropertyRelative("loop");
			randomPitch_sp = property.FindPropertyRelative("RandomPitch");
			randomVolume_sp = property.FindPropertyRelative("RandomVolume");
			minPitch_sp=property.FindPropertyRelative("minMaxPitch").FindPropertyRelative("_MinValue");
			maxPitch_sp = property.FindPropertyRelative("minMaxPitch").FindPropertyRelative("_MaxValue");
			minVolume_sp = property.FindPropertyRelative("minMaxVolume").FindPropertyRelative("_MinValue");
			maxVolume_sp = property.FindPropertyRelative("minMaxVolume").FindPropertyRelative("_MaxValue");


			group_sp = property.FindPropertyRelative("group");


			playicon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/AudioManager/Resources/texture/play_btn.png", typeof(Texture));
			stopicon = (Texture)AssetDatabase.LoadAssetAtPath("Assets/AudioManager/Resources/texture/stop_btn.png", typeof(Texture));

			playbtn_content = new GUIContent(playicon);
			stopbtn_content = new GUIContent(stopicon);


		}
	}
}