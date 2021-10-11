using System;
using UnityEngine;

namespace HuntroxGames.LD49
{
	[System.Serializable]
	public class Item : MercenaryDemand , IMercenaryDemand
	{
		public float health;
		public float ms;
		public string DemandID { get => demandID; }
		public bool IsMyDemand(string demandID)
		{
			return this.demandID == demandID;
		}
	}

	public abstract class MercenaryDemand
	{
		public string itemName;
		[TextArea(1,4)]public string itemDescription;
		public int itemValue;
		/*[NonSerialized]*/public Sprite icon;
		public string demandID;
		[Range(0, 1)] public float demandFrequency;
	}

}