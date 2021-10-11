using UnityEngine;
using UnityEngine.EventSystems;
using HuntroxGames.Utils;


namespace HuntroxGames.Utils.Audio {
    public class SoundTrigger : EventsHandler
    {


#pragma warning disable 649
        [SerializeField] private SoundClip sound;
        private AudioSource audioSource;
#pragma warning restore 649

        

        protected override void EventHandler(TriggerEvent t_event)
        {
            if (t_event == triggerEvent)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                    AudioController.PlaySoundClip(sound);
                else
                    AudioController.PlaySoundClip(sound,false,audioSource);
            }
        }


    }



}
