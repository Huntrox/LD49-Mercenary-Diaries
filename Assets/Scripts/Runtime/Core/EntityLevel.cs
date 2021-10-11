using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HuntroxGames.LD49
{
	[System.Serializable]
	public class EntityLevel
	{
		public int currentLevel = 1;
		private double currentExp;
		public double maxExp = 100;
		public Action onLevelUp;
		public EntityLevel(int level, Action _onLevelUp)
		{
			this.onLevelUp = _onLevelUp;
			SetLevl(level);
		}
		public void AddExp(double amount)
		{
			currentExp += amount;
			CheckLevelUp();
		}

		private void CheckLevelUp()
		{
			for (int i = 0; i < 100; i++)
			{
				if (currentExp >= maxExp)
					CalculateXp();
				else
					break;
			}

		}
		private void CalculateXp()
		{
			currentLevel++;
			currentExp -= maxExp;
			maxExp *= GlobalSettings.EXP_MULTIPLY_AMOUNT;
			onLevelUp?.Invoke();

		}

		public void SetLevl(int recLevel)
		{
			maxExp = GlobalSettings.START_EXP_AMOUNT;
			currentLevel = recLevel;
			currentExp = 0;
			recLevel = recLevel - 1;
			for (int i = 0; i < recLevel; i++)
				maxExp *= GlobalSettings.EXP_MULTIPLY_AMOUNT;

		}
	}
}