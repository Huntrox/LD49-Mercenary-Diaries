using UnityEngine;

namespace HuntroxGames.LD49
{
	public class Contract
	{
		public string title;
		[TextArea(1,4)]public string description;
		public int cashReward;
		public int expReward;
		public int recomendedLevel;
		public float time;
		public ContractDifficultyType contractDifficultyType;
	}

	public enum ContractDifficultyType
	{
		Normal,
		Hard,
		MissionImpossible
	}
	[System.Serializable]
	public struct ContractDifficulty
	{
		public float mentalStabilityModifier;
		public float healthModifier;
	}

}