using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HuntroxGames.Utils
{
	[System.Serializable]
	public class ActionTimer
	{
		public float cooldown = 0.5f;
		private float timer = 0;
		private UnityAction onStartCallback;
		private UnityAction onFinishCallback;
		public bool IsActivated => timer > 0;
		private bool alreadyTriggerd= true;
		public ActionTimer(float cooldown)
		{
			this.cooldown = cooldown;
		}

		public ActionTimer(float cooldown, UnityAction onStartCallback, UnityAction onFinishCallback) : this(cooldown)
		{
			this.onStartCallback = onStartCallback;
			this.onFinishCallback = onFinishCallback;
		}
		public void SetCallbacks(UnityAction onStartCallback, UnityAction onFinishCallback)
		{
			this.onStartCallback = onStartCallback;
			this.onFinishCallback = onFinishCallback;
		}
		public void Activate()
		{
			timer = cooldown;
			onStartCallback?.Invoke();
			alreadyTriggerd = false;
		}
		public void Deactivate()
		{
			timer = 0;
			onFinishCallback?.Invoke();
			alreadyTriggerd = true;
		}
		public bool Update(float deltaTime)
		{
			if (timer > 0)
			{
				timer -= deltaTime;
				return true;
			}
			else
			{
				if (!alreadyTriggerd)
					Deactivate();
				return false;
			}
		}
	}
}