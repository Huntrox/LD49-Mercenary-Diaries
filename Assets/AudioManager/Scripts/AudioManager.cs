using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuntroxGames.Utils;
using DG.Tweening;
using System.Linq;

namespace HuntroxGames.Utils.Audio
{
    [ExecuteAlways]
    public class AudioManager : Singleton<AudioManager>
    {
        #region Singleton
        public static new AudioManager instance
        {

            get
            {
                _instance = FindObjectOfType<AudioManager>();
                if (_instance != null)
                    return _instance;
                else
                {
                    GameObject t_go = new GameObject("[AudioManager]");
                    _instance = t_go.AddComponent<AudioManager>();
                    t_go.hideFlags = HideFlags.HideInInspector;
                    AudioController.init();
                    return _instance;
                }
            }
            set
            {
                _instance = value;
            }
        }
        #endregion

        private Dictionary<SoundClip, int> audioPool = new Dictionary<SoundClip, int>();
        private int maxSoundPlayCountPerFrame = 25;
        private int maxSameSoundPlayCountPerFrame = 3;
        private const float defaultSoundVolume = 1f;
        private const float defaultTimeTillNextTrgger = 0.005f;
        private Dictionary<string, SoundTrack> bgm_Dictionary = new Dictionary<string, SoundTrack>();
        private string currentPlayingBGM = string.Empty;
        private Queue<AudioSource> uniqueAudioSources = new Queue<AudioSource>();
        private AudioDatabase AudioDatatabase;
        protected override void Awake()
        {
            base.Awake();
            initialize();
        }

        private void initialize()
        {
            if (AudioDatatabase == null)
                AudioDatatabase = (AudioDatabase)Resources.Load("AudioDatabase", typeof(AudioDatabase));

            maxSoundPlayCountPerFrame = AudioDatatabase.maxSoundPlayCountPerFrame;
            maxSameSoundPlayCountPerFrame = AudioDatatabase.maxSameSoundPlayCountPerFrame;
            bgm_Dictionary = AudioDatatabase.GetSoundTracksDictionary();
        }

        #region BGM

        public void RequstNextBGM(string name, SoundtrackEvent musicTrack, AudioSource m_soundTrackSource)
        {
            if (bgm_Dictionary.IsNullOrEmpty()) initialize();


            SoundTrack track;
            if (bgm_Dictionary.ContainsKey(name)) track = bgm_Dictionary[name];
            else return;
            if (track.isPlaying) return;
            if (track.musicTrackEvent == musicTrack)
            {
                StopAllCoroutines();
                if (musicTrack == SoundtrackEvent.OnSceneLoad)
                    ClearAudioPool();
                StartCoroutine(playBGM(name, m_soundTrackSource));
            }
        }
        private void ClearAudioPool()
        {
            audioPool.Clear();
        }

        public void TransitionTo(int snapshot, float time)
        {

            if (AudioDatatabase == null)
                AudioDatatabase = (AudioDatabase)Resources.Load("AudioDatabase", typeof(AudioDatabase));
            AudioDatatabase.GetSnapshots()[snapshot].TransitionTo(time);
        }
        IEnumerator playBGM(string name, AudioSource m_soundTrackSource)
        {

            if (!string.IsNullOrEmpty(currentPlayingBGM) && bgm_Dictionary.ContainsKey(currentPlayingBGM))
            {
                bgm_Dictionary[currentPlayingBGM].isPlaying = false;
                if (bgm_Dictionary[currentPlayingBGM].fadeOut)
                {
                    Tween tween = m_soundTrackSource.DOFade(0, 2f).SetEase(Ease.InOutSine);
                    yield return tween.WaitForCompletion();
                }
            }
            m_soundTrackSource.Stop();
            SoundTrack musicTrack = bgm_Dictionary[name];
            musicTrack.isPlaying = true;


            m_soundTrackSource.outputAudioMixerGroup = musicTrack.group;
            currentPlayingBGM = name;
            if (musicTrack.loopLastElement)
            {

                Queue<AudioClip> tracksPlayList = new Queue<AudioClip>();
                musicTrack.tracks.ToList().ForEach(x => tracksPlayList.Enqueue(x));
                while (true)
                {
                    if (!m_soundTrackSource.isPlaying)
                    {

                        AudioClip t_sr = tracksPlayList.Dequeue();
                        m_soundTrackSource.clip = t_sr;
                        m_soundTrackSource.Play();

                        if (musicTrack.fadeIn && t_sr == musicTrack.tracks[0])
                        {
                            Tween tween = m_soundTrackSource.DOFade(musicTrack.volume, 1f).SetEase(Ease.InOutSine);
                            yield return tween.WaitForCompletion();
                        }
                        else
                            m_soundTrackSource.volume = musicTrack.volume;

                        if (tracksPlayList.Count == 0)
                            m_soundTrackSource.loop = true;
                        else
                            m_soundTrackSource.loop = false;
                    }
                    else
                    {

                        yield return null;
                    }

                }


            }
            else
            {
                int currentSoundtrack = 0;

                while (true)
                {
                    if (!m_soundTrackSource.isPlaying)
                    {

                        AudioClip t_sr = musicTrack.tracks[currentSoundtrack];
                        m_soundTrackSource.clip = t_sr;
                        m_soundTrackSource.Play();
                        m_soundTrackSource.loop = false;

                        if (musicTrack.fadeIn && t_sr == musicTrack.tracks[0])
                        {
                            Tween tween = m_soundTrackSource.DOFade(musicTrack.volume, 1f).SetEase(Ease.InOutSine);
                            yield return tween.WaitForCompletion();
                        }
                        else
                            m_soundTrackSource.volume = musicTrack.volume;

                    }
                    else
                    {
                        if (m_soundTrackSource.clip != null)
                        {
                            yield return new WaitForSeconds(m_soundTrackSource.clip.length);
                            currentSoundtrack = ((currentSoundtrack + 1) % musicTrack.tracks.Length);
                        }
                        else
                            yield return null;
                    }

                }


            }



        }
        #endregion
        #region SFX

        public void PlaySoundClip(SoundClip p_soundClip, bool PlayOneShot, AudioSource targetAudioSource, bool bypassMAPF = false)
        {
            if (!bypassMAPF)
            {
                if (GetSoundClipPlayCount(p_soundClip) >= maxSameSoundPlayCountPerFrame || GetTotallPlayCount() >= maxSoundPlayCountPerFrame)
                    return;
            }

            if (PlayOneShot)
				StartCoroutine(this.PlayOneShot(p_soundClip, targetAudioSource));
            else
                StartCoroutine(PlaySound(p_soundClip, targetAudioSource));
        }
        public void PlayUniqueSFX(SoundClip p_soundClip) => StartCoroutine(PlayUniqueSound(p_soundClip));

        public void PlayWithDelay(SoundClip p_soundClip, float volume, bool PlayOneShot, AudioSource targetAudioSource)
        {
            if (GetSoundClipPlayCount(p_soundClip) >= maxSameSoundPlayCountPerFrame || GetTotallPlayCount() >= maxSoundPlayCountPerFrame)
                return;
            StartCoroutine(playSoundClipWithVolume(p_soundClip, volume, PlayOneShot, targetAudioSource));
        }

        public void PlayWithVolume(float delay, SoundClip p_soundClip, bool PlayOneShot, AudioSource targetAudioSource)
        {
            StartCoroutine(playSoundClipWithDelay(delay, p_soundClip, PlayOneShot, targetAudioSource));
        }




        private IEnumerator playSoundClipWithVolume(SoundClip p_soundClip, float volume, bool PlayOneShot, AudioSource targetAudioSource)
        {
            if (p_soundClip == null || p_soundClip.clip == null) yield break;

            GetSoundClipPlayCount(p_soundClip);
            audioPool[p_soundClip]++;


            float calculatedVolume = volume;

            if (audioPool[p_soundClip] > 1)
                calculatedVolume = volume - (audioPool[p_soundClip] / maxSoundPlayCountPerFrame) * defaultSoundVolume;

            if (PlayOneShot)
            {

                targetAudioSource.pitch = (p_soundClip.RandomPitch ? p_soundClip.minMaxPitch.Random
                 : p_soundClip.pitch);
                yield return new WaitForSeconds(p_soundClip.delay);
                targetAudioSource.PlayOneShot(p_soundClip.clip, calculatedVolume);

            }
            else
            {
                SetupAudioSource(targetAudioSource, p_soundClip);
                targetAudioSource.PlayDelayed(p_soundClip.delay);
            }

            yield return new WaitForSecondsRealtime(defaultTimeTillNextTrgger);
            audioPool[p_soundClip] = 0;
        }

        private IEnumerator playSoundClipWithDelay(float delay, SoundClip p_soundClip, bool PlayOneShot, AudioSource targetAudioSource)
        {
            if (p_soundClip == null || p_soundClip.clip == null) yield break;
            if (PlayOneShot)
            {
                float volume = (p_soundClip.RandomVolume ? p_soundClip.minMaxVolume.Random
                  : p_soundClip.volume);
                targetAudioSource.pitch = (p_soundClip.RandomPitch ? p_soundClip.minMaxPitch.Random
                    : p_soundClip.pitch);

                yield return new WaitForSeconds(delay);
                targetAudioSource.PlayOneShot(p_soundClip.clip, volume);
                yield break;
            }
            else
            {
                SetupAudioSource(targetAudioSource, p_soundClip);
                targetAudioSource.PlayDelayed(delay);
                yield break;
            }
        }



        private IEnumerator PlayOneShot(SoundClip p_soundClip, AudioSource targetAudioSource)
        {

            if (p_soundClip == null || p_soundClip.clip == null) yield break;

            GetSoundClipPlayCount(p_soundClip);
            audioPool[p_soundClip]++;



            float volume = (p_soundClip.RandomVolume ? p_soundClip.minMaxVolume.Random : p_soundClip.volume);
            targetAudioSource.pitch = (p_soundClip.RandomPitch ? p_soundClip.minMaxPitch.Random : p_soundClip.pitch);
            targetAudioSource.outputAudioMixerGroup = p_soundClip.group;


            float calculatedVolume = volume;
            if (audioPool[p_soundClip] > 1)
                calculatedVolume = volume - (audioPool[p_soundClip] / maxSoundPlayCountPerFrame) * defaultSoundVolume;



            yield return new WaitForSeconds(p_soundClip.delay);
            targetAudioSource.PlayOneShot(p_soundClip.clip, calculatedVolume);

            yield return new WaitForSecondsRealtime(defaultTimeTillNextTrgger);
            audioPool[p_soundClip] = 0;

            yield break;

        }
        private IEnumerator PlaySound(SoundClip p_soundClip, AudioSource targetAudioSource)
        {

            if (p_soundClip == null || p_soundClip.clip == null) yield break;
            SetupAudioSource(targetAudioSource, p_soundClip);
            targetAudioSource.PlayDelayed(p_soundClip.delay);
            yield break;
        }
        private IEnumerator PlayUniqueSound(SoundClip p_soundClip)//Unique SFX bypassing maximum audioclips playing in the same frame
        {

            if (p_soundClip == null || p_soundClip.clip == null) yield break;

            AudioSource targetAudioSource = GetAudioSource();

            SetupAudioSource(targetAudioSource, p_soundClip);

            targetAudioSource.PlayDelayed(p_soundClip.delay);

            while (targetAudioSource.isPlaying)
            {
                yield return null;
            }
            uniqueAudioSources.Enqueue(targetAudioSource);
            yield break;
        }




        private int GetSoundClipPlayCount(SoundClip p_soundClip)
        {
            if (!audioPool.ContainsKey(p_soundClip))
            {
                audioPool.Add(p_soundClip, 0);
                return audioPool[p_soundClip];
            }
            else
                return audioPool[p_soundClip];
        }

        private int GetTotallPlayCount()
        {
            int count = 0;
            foreach (KeyValuePair<SoundClip, int> audioCount in audioPool)
            {
                count += audioCount.Value;
            }
            return count;
        }
        #endregion

        private void SetupAudioSource(AudioSource targetAudioSource, SoundClip p_soundClip)
        {
            targetAudioSource.volume = (p_soundClip.RandomVolume ? p_soundClip.minMaxVolume.Random
                : p_soundClip.volume);
            targetAudioSource.pitch = (p_soundClip.RandomPitch ? p_soundClip.minMaxPitch.Random
                : p_soundClip.pitch);
            targetAudioSource.loop = p_soundClip.loop;
            targetAudioSource.clip = p_soundClip.clip;
            targetAudioSource.outputAudioMixerGroup = p_soundClip.group;
        }

        private AudioSource GetAudioSource()
		{
            if (uniqueAudioSources.IsNullOrEmpty())
                return AudioManagerUtils.CreateAudioSource(transform);
            else
                return uniqueAudioSources.Dequeue();
        }
    }
}