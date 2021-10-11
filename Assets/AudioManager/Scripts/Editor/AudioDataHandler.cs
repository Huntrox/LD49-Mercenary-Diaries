using UnityEngine;
using UnityEditor;
using HuntroxGames.Utils.Audio;
using System.Reflection;
using System;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace HuntroxGames.Utils.EditorUtils
{
    public static class AudioDataHandler
    {




        private static AudioDatabase soundClipsList;
        private static string soundClipsListPath;

        private static List<string> sounds_names_array = new List<string>();

        private static AudioMixer audioMixer;
        private static string audioMixerPath;

        public static AudioMixer GetAudioMixer
        {
            get
            {
                Initialize();
                return audioMixer;
            }
        }

        public static void Initialize()
        {
                soundClipsList = LoadOrCreateSoundsDataAsset();
                audioMixer = LoadAudioMixer();
        }







        #region sounds set up


        public static string[] GetSoundsListNames()
        {

            Initialize();

            List<string> t_names = new List<string>();
            foreach (var soundclip in soundClipsList.SoundList)
            {
                t_names.Add(soundclip.clipname);
            }


            return t_names.ToArray();
        }

        public static Dictionary<string, SoundClip> GetClipsDictionary()
        {
            Initialize();
            return soundClipsList.GetClipsDictionary();
        }
        public static SoundClip GetSoundClip(string key)
        {

            //  return soundClipsList.GetSoundClipCopy(key);
            return soundClipsList.GetSoundClip(key);
        }

        public static bool CheckClipExist(string key)
        {
            return soundClipsList.CheckItemInList(key);
        }
     


        #endregion





        public static AudioMixer LoadAudioMixer()
        {
            //GameMixer

            AudioMixer audio;
            audioMixerPath = EditorPrefs.GetString("audioMixerPath");
            if (AssetDatabase.LoadAssetAtPath(audioMixerPath, typeof(AudioMixer)))
            {

                audio = (AudioMixer)AssetDatabase.LoadAssetAtPath(audioMixerPath, typeof(AudioMixer));
             
                return audio;
            }
            else
            {
                //  AudioMixer

                audio = (AudioMixer)Resources.Load("AudioMixer",typeof(AudioMixer));
                if (audio != null)
                    return audio;
                else
                {
                    Debug.LogWarning("there is no AudioMixer selected please setup your AudioManager > Setup> AudioMixer" + audioMixerPath);
                    return null;
                }
            }
        }


        public static AudioDatabase LoadOrCreateSoundsDataAsset()
        {
            AudioDatabase list;

            soundClipsListPath = EditorPrefs.GetString("soundClipsListPath", "Assets/Resources/AudioDatabase.asset");
            if (AssetDatabase.LoadAssetAtPath(soundClipsListPath, typeof(AudioDatabase)))
            {
                list = (AudioDatabase)AssetDatabase.LoadAssetAtPath(soundClipsListPath, typeof(AudioDatabase));
                return list;
            }
            else
            {
                list = (AudioDatabase)Resources.Load("AudioDatabase", typeof(AudioDatabase));
                if (list) return list;
                else
                {
                    list = AudioDatabase.CreateInstance<AudioDatabase>();
                    AssetDatabase.CreateAsset(list, "Assets/Resources/AudioDataBase.asset");
                    EditorPrefs.SetString("soundClipsListPath", "Assets/Resources/AudioDatabase.asset");
                    AssetDatabase.SaveAssets();
                    return list;
                }

            }
        }




        #region  Editor Clips Preview
        public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "PlayClip",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null
            );
            method.Invoke(
                null,
                new object[] { clip, startSample, loop }
            );
        }
        public static void StopAllClips()
        {
            Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            MethodInfo method = audioUtilClass.GetMethod(
                "StopAllClips",
                BindingFlags.Static | BindingFlags.Public,
                null,
                new System.Type[] { },
                null
            );
            method.Invoke(
                null,
                new object[] { }
            );
        }




        #endregion


    }
}