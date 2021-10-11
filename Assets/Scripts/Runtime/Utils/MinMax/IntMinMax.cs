﻿using UnityEngine;
namespace HuntroxGames.Utils
{
	[System.Serializable]
	public struct IntMinMax
	{

		public int _MinValue;
		public int _MaxValue;

		public IntMinMax(int minValue, int maxValue)
		{
			this._MinValue = minValue;
			this._MaxValue = maxValue;
		}

		/// <summary>
		/// returns a Random value between Min and Max Value
		/// </summary>
		public int Random => UnityEngine.Random.Range(MinValue, MaxValue);

		public int MinValue
		{
			get => _MinValue;
			set => _MinValue = Mathf.Clamp(value, int.MinValue, MaxValue);
		}
		public int MaxValue
		{
			get => _MaxValue;

			set => _MaxValue = Mathf.Clamp(value, _MinValue, int.MaxValue);
		}

		public static IntMinMax ZeroOne => new IntMinMax(0, 1);
		public static IntMinMax ZeroTen => new IntMinMax(0, 10);
	}
}