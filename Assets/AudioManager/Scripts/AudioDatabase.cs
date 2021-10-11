using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HuntroxGames.Utils.Audio {
    [System.Serializable]
    public class AudioDatabase : ScriptableObject
    {
        [SerializeField] public List<SoundClip> SoundList = new List<SoundClip>();
        [SerializeField] public List<SoundTrack> MusicTracksList = new List<SoundTrack>();

        public AudioMixer audioMixer;
        public int maxSoundPlayCountPerFrame = 25;
        public int maxSameSoundPlayCountPerFrame = 3;
        public List<string> audioMixerSnapshots = new List<string>();


        public SoundClip GetSoundClip(string key)
        {
            for (int i = 0; i < SoundList.Count; i++)
            {
                if (SoundList[i].clipname.ToLower() == key.ToLower())
                    return SoundList[i];
            }
            return null;
        }


        public Dictionary<string,SoundClip> GetClipsDictionary()
        {
            Dictionary<string, SoundClip> dictionary = new Dictionary<string, SoundClip>();
            SoundList.ForEach(s => dictionary.Add(s.clipname, s));
            return dictionary;
        }
        public Dictionary<string, SoundTrack> GetSoundTracksDictionary()
        {
            Dictionary<string, SoundTrack> dictionary = new Dictionary<string, SoundTrack>();
            foreach (SoundTrack m in MusicTracksList)
            {
                if (!dictionary.ContainsKey(m.name))
                {
                    m.isPlaying = false;
                    dictionary.Add(m.name, m);
                }
            }
            return dictionary;
        }

        public bool CheckItemInList(string key)
        {

            for (int i = 0; i < SoundList.Count; i++)
            {
                if (SoundList[i].clipname.ToLower() == key.ToLower())
                {
                    return true;
                }

            }
            return false;
        }
        public bool check_Duplication(string key)
        {

            for (int i = 0; i < MusicTracksList.Count; i++)
            {
                if (MusicTracksList[i].name.ToLower() == key.ToLower())
                {
                    return false;
                }
            }
            return true;
        }

        public void UpdateSoundClip(SoundClip clip)
        {
            if (CheckItemInList(clip.clipname))
            {
                for (int i = 0; i < SoundList.Count; i++)
                {
                    if (SoundList[i].clipname == clip.clipname)
                    {

                        SoundList[i] = clip;
                        return;
                    }

                }

            }
            else
            {
                SoundList.Add(clip);
            }
        }

        public AudioMixerSnapshot[] GetSnapshots()
        {
            if (audioMixer == null)
                audioMixer = (AudioMixer)Resources.Load("AudioMixer", typeof(AudioMixer));

            List<AudioMixerSnapshot> audioMixerSnapshot = new List<AudioMixerSnapshot>();
            audioMixerSnapshots.ForEach(a => audioMixerSnapshot.Add(audioMixer.FindSnapshot(a)));
            return audioMixerSnapshot.ToArray();
        }
        

        public string[] SoundTracks()
        {
            List<string> list = new List<string>();
            MusicTracksList.ForEach(x => list.Add(x.name));
            return list.ToArray();
        }

    }


}