using UnityEngine;

namespace HuntroxGames.LD49
{
	[System.Serializable]
	public class Structure : MercenaryDemand, IMercenaryDemand
	{

		public string DemandID { get => demandID;}
		public bool IsMyDemand(string demandID)
		{
			throw new System.NotImplementedException();
		}
	}
}