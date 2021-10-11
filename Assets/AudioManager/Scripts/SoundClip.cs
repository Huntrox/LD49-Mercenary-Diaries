using UnityEngine;
using UnityEngine.Audio;
using System;


namespace HuntroxGames.Utils.Audio
{
	[Serializable]
	public class SoundClip
	{
		public string clipname = string.Empty;
		public AudioClip clip;
		public float delay = 0;
		public float pitch=1f;
		public float volume=1f;
		public bool loop = false;
		public AudioMixerGroup group;

		public bool RandomPitch = false;
		public bool RandomVolume = false;
		public floatMinMax minMaxPitch = new floatMinMax(0.5f, 1);
		public floatMinMax minMaxVolume = new floatMinMax(0.5f, 1);
		//public float lastTimePlayed = 0;
		//public float mintriggerTime;
		//public int maxSimultaneous = 5;

		public SoundClip GetCopy()
		{
			return (SoundClip )this.MemberwiseClone();
		}


	}
}