using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HuntroxGames.LD49
{
	[System.Serializable]
	public class Mercenary
	{
		[Header("Info")]
		public string mercenaryName;
		public string mercenaryDesc;
		public Sprite portrait;
		public int value;
		[Header("Level")]
		public EntityLevel level;
		[Header("Health")]
		public float MaxHealth;
		public float currentHealth;
		[Header("MentalStability")]
		public float mentalStability;
		public float currMentalStability;
		public float mentalStabilityModifier;
		public InstabilityRiskActions instabilityRisk;
		[Header("Efficiency")]
		public float efficiency;

		private bool isOnATask;
		private float contractRemainingTimer = 0;
		public bool IsDead => currentHealth <= 0;
		public bool IsOnATask { get => isOnATask; set => isOnATask = value; }
		public float ContractRemainingTimer { get => contractRemainingTimer; set => contractRemainingTimer = value; }

		public float CurrentHealthPerc
		{
			get => (currentHealth / MaxHealth) * 1;
			set
			{
				currentHealth = Mathf.Clamp(value, 0, MaxHealth);
			}
		}
		public float CurrentMentalStabilityPerc
		{
			get => (currMentalStability / mentalStability) * 1;
			set 
			{
				currMentalStability =Mathf.Clamp(value,0, mentalStability);
			}
		}
		public float EfficiencyPerc
		{
			get => (efficiency / GlobalSettings.MAX_MECENARY_EFFECIENCE) * 1;
			set => efficiency = value;
		}
		public int Level => level.currentLevel;

		public void OnCompletingAcontract(ContractDifficulty contractDifficulty)
		{
			CurrentHealthPerc = currentHealth - contractDifficulty.healthModifier;
			CurrentMentalStabilityPerc = currMentalStability - contractDifficulty.mentalStabilityModifier;

		}
		public void OnLevelUp()
		{
			MaxHealth += GlobalSettings.HEALTH_PER_LEVEL_MODIFIER;
			mentalStability += GlobalSettings.MS_PER_LEVEL_MODIFIER;
			efficiency += GlobalSettings.EFECIENCY_PER_LEVEL_MODIFIER;
		}
		public bool IsInsane()
		{
			float modf = (float)GlobalSettings.GetInstabilityRiskModif(instabilityRisk);

			return CurrentMentalStabilityPerc <= modf;
		}

		public InstabilityActions CheckMS(List<Mercenary> mercenaries)
		{
			List<InstabilityActions> posiableActions = new List<InstabilityActions>();
			if (!IsInsane())
				return InstabilityActions.None;

			float modf = (float)GlobalSettings.GetInstabilityRiskModif(instabilityRisk);

			switch (instabilityRisk)
			{
				case InstabilityRiskActions.Low:
					if(CurrentMentalStabilityPerc > 0 && CurrentMentalStabilityPerc <= modf)
						posiableActions.Add(InstabilityActions.None);
					posiableActions.Add(InstabilityActions.Attack);
					posiableActions.Add(InstabilityActions.RunAway);
					break;
				case InstabilityRiskActions.Medium:
					if (CurrentMentalStabilityPerc > 0 && CurrentMentalStabilityPerc <= modf)
							posiableActions.Add(InstabilityActions.None);
					if (mercenaries.Count > 1)
						posiableActions.Add(InstabilityActions.Attack);
					posiableActions.Add(InstabilityActions.Suicid);
					posiableActions.Add(InstabilityActions.RunAway);
					break;
				case InstabilityRiskActions.High:
					posiableActions.Add(InstabilityActions.Suicid);
					posiableActions.Add(InstabilityActions.RunAway);
					if (mercenaries.Count > 1)
					{
						posiableActions.Add(InstabilityActions.Attack);
						posiableActions.Add(InstabilityActions.Kill);
					}
					break;
				default:
					break;
			}
			return posiableActions.ToArray().RandomElement();
		}
	}
	public enum InstabilityRiskActions
	{
		Low,
		Medium,
		High,
	}
	public enum InstabilityActions
	{
		None,
		Suicid,
		Kill,
		Attack,
		RunAway
	}
}