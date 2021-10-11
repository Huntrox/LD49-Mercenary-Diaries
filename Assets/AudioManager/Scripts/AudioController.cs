using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace HuntroxGames.Utils.Audio
{
	public static class AudioController
    {

        private static AudioSource m_sfxSource;
        private static AudioSource m_soundTrackSource;
        private static GameObject m_AudioManager;
        private static AudioDatabase soundClipsList;
        private static Dictionary<string, SoundClip> audioDatabase;

        public static void init()
        {

            if (!soundClipsList)
            {
                soundClipsList = (AudioDatabase)Resources.Load("AudioDatabase", typeof(AudioDatabase));
                audioDatabase = soundClipsList.GetClipsDictionary();
            }
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.AssetDatabase.Refresh();
                audioDatabase = soundClipsList.GetClipsDictionary();
            }
#endif

            if (m_AudioManager == null)
            {

                m_AudioManager = GameObject.Find("[AudioManager]");
                if (m_AudioManager == null)
                {
                    GameObject t_go = new GameObject("[AudioManager]");
                    t_go.AddComponent<AudioManager>();
                    m_AudioManager = t_go;
                    t_go.hideFlags = HideFlags.HideInHierarchy;
                }
            }

            if (m_soundTrackSource == null)
            {

                if (GameObject.Find("BackgroundMusic AudioSource") == null)
                {
                    GameObject t_go = new GameObject("BackgroundMusic AudioSource");
                    t_go.transform.parent = m_AudioManager.transform;
                    m_soundTrackSource=t_go.AddComponent<AudioSource>();
                }
            }
            if (m_sfxSource == null)
            {

                if (GameObject.Find("SFX AudioSource") == null)
                {
                    GameObject t_go = new GameObject("SFX AudioSource");
                    t_go.transform.parent = m_AudioManager.transform;
                    t_go.AddComponent<AudioSource>();
                    t_go.GetComponent<AudioSource>().playOnAwake = false;
                    m_sfxSource = t_go.GetComponent<AudioSource>();
				}
				else
				{
                    m_sfxSource= GameObject.Find("SFX AudioSource").GetComponent<AudioSource>();
                }
            }
        }
        /// <summary>
        /// Performs an interpolated transition towards this snapshot over the time interval specified.
        /// </summary>
        /// <param name="snapshot"></param>
        /// <param name="time"></param>
        public static void TransitionTo(int snapshot ,float time =1f)
		{
            init();
            AudioManager.instance.TransitionTo(snapshot, time);
        }

        public static void PlayBGM(string name, SoundtrackEvent trackEvent)
        {
            init();
            AudioManager.instance.RequstNextBGM(name, trackEvent, m_soundTrackSource);
        }

        /// <summary>
        ///  play sound with sound clip parameters
        /// </summary>
        /// <param name="p_soundClip"></param>
        /// <param name="PlayOneShot"></param>
        /// <param name="targetAudioSource"></param>
        public static void PlaySoundClip(SoundClip p_soundClip, bool PlayOneShot = true, AudioSource targetAudioSource = null,bool bypassMAPF= false)
        {
            init();
            audioDatabase.TryGetValue(p_soundClip.clipname, out p_soundClip);
            AudioManager.instance.PlaySoundClip(p_soundClip, PlayOneShot,
                (targetAudioSource != null ? targetAudioSource : m_sfxSource), bypassMAPF);

        }


        /// <summary>
        /// Unique SFX bypassing maximum audioclips playing at the same frame
        /// </summary>
        /// <param name="p_soundClip"></param>
        public static void PlayeUniqueSFX(SoundClip p_soundClip)
        {
            init();
            audioDatabase.TryGetValue(p_soundClip.clipname, out p_soundClip);
            AudioManager.instance.PlayUniqueSFX(p_soundClip);
        }

        public static void PlaySoundEditMode(SoundClip p_soundClip)
        {
            init();
            AudioManager.instance.PlaySoundClip(p_soundClip, false,m_sfxSource,true);
        }
        /// <summary>
        /// play sound with custom volume,this will bypass  sound clip volume parameters
        /// </summary>
        /// <param name="p_soundClip"></param>
        /// <param name="volume"></param>
        /// <param name="PlayOneShot"></param>
        /// <param name="targetAudioSource"></param>
        public static void PlaySoundClip(SoundClip p_soundClip,float volume, bool PlayOneShot = true, AudioSource targetAudioSource = null)
        {
            init();
            audioDatabase.TryGetValue(p_soundClip.clipname, out p_soundClip);
            AudioManager.instance.PlayWithDelay(p_soundClip,volume,PlayOneShot,
                (targetAudioSource != null ? targetAudioSource : m_sfxSource));
        }
        /// <summary>
        ///  play sound with custom delay,this will bypass  sound clip delay parameters
        /// </summary>
        /// <param name="delay"></param>
        /// <param name="p_soundClip"></param>
        /// <param name="PlayOneShot"></param>
        /// <param name="targetAudioSource"></param>
        public static void PlaySoundClip(float delay,SoundClip p_soundClip, bool PlayOneShot = true, AudioSource targetAudioSource = null)
        {
            init();
            audioDatabase.TryGetValue(p_soundClip.clipname, out p_soundClip);
            AudioManager.instance.PlayWithVolume(delay,p_soundClip, PlayOneShot,
                (targetAudioSource != null ? targetAudioSource : m_sfxSource));
        }

        public static void StopAudioSource(AudioSource targetAudioSource = null)=>
            (targetAudioSource != null ? targetAudioSource : m_sfxSource).Stop();

    }
}
