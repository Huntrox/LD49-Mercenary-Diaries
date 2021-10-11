using System;
using UnityEngine;

namespace HuntroxGames.LD49
{
    public static class GlobalSettings
    {

        public const float MAX_MECENARY_EFFECIENCE = 10f;
        public const float EXP_MULTIPLY_AMOUNT = 1.7f;
        public const float START_EXP_AMOUNT = 120f;

        public const int MAX_HIRED_MERCENARIES = 10;
        public const int MAX_MERCENARIES_PER_CONTRACT = 5;

        public const float HEALTH_LOSE_PER_LEVEL_MODIFIER =0.2f;
        public const float MS_LOSE_PER_LEVEL_MODIFIER =0.2f;

		public const float HEALTH_PER_LEVEL_MODIFIER = 0.5f;
		public const float MS_PER_LEVEL_MODIFIER = 0.5f;
		public const float EFECIENCY_PER_LEVEL_MODIFIER = 0.2f;



        public const float NORMAL_MODIFIER =1f;
        public const float HARD_MODIFIER =1.5f;
        public const float MISSIONIMPOSSIBLE_MODIFIER =2f;




        public static float GetDiffModifier(ContractDifficultyType diffic)
		{
			switch (diffic)
			{
				case ContractDifficultyType.Normal:
					return NORMAL_MODIFIER;
				case ContractDifficultyType.Hard:
					return HARD_MODIFIER;
				case ContractDifficultyType.MissionImpossible:
					return MISSIONIMPOSSIBLE_MODIFIER;
				default:
					return NORMAL_MODIFIER;
			}
		}

		public static float GetInstabilityRiskModif(InstabilityRiskActions instabilityRisk)
		{
			switch (instabilityRisk)
			{
				case InstabilityRiskActions.Low:
					return 0.1f;
				case InstabilityRiskActions.Medium:
					return 0.15f;
				case InstabilityRiskActions.High:
					return 0.25f;
				default:
					return 0.1f;
			}
		}

		public static float AttackModifier(InstabilityRiskActions instabilityRisk)
		{
			switch (instabilityRisk)
			{
				case InstabilityRiskActions.Low:
					return 0.5f;
				case InstabilityRiskActions.Medium:
					return 1f;
				case InstabilityRiskActions.High:
					return 1.5f;
				default:
					return .5f;
			}
		}
	}
}