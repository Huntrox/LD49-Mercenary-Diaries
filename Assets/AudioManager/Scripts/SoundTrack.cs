using UnityEngine;
using UnityEngine.Audio;

namespace HuntroxGames.Utils.Audio
{
[System.Serializable]

    public class SoundTrack
    {

        public string name;
        public AudioClip[] tracks;
        public SoundtrackEvent musicTrackEvent;
        public string sceneName;
        public AudioMixerGroup group;
        public float volume = 1f;
        public bool loopLastElement;
        public bool fadeIn;
        public bool fadeOut;
        public bool isPlaying= false;
        public SoundTrack() { name = "new BGM"; }
        public SoundTrack(string p_name,AudioClip[] p_tracks,SoundtrackEvent p_musicTrackEvent,string p_sceneName,bool p_loopLastElement,bool p_fadeIn, bool p_fadeOut )
        {
            name = p_name;
            tracks = p_tracks;
            musicTrackEvent = p_musicTrackEvent;
            sceneName = p_sceneName;
            loopLastElement = p_loopLastElement;
            fadeIn = p_fadeIn;
            fadeOut = p_fadeOut;
        }
    }
    public enum SoundtrackEvent {OnSceneLoad,TriggerEvint}
}