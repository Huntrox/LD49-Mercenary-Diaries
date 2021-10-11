using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HuntroxGames.LD49
{
    public class HiringManager : MonoBehaviour
    {
		[SerializeField] private List<Mercenary> mercenaries = new List<Mercenary>();

		public int MercenariesCount { get=> mercenaries.Count;}
		private SquadManager squadManager;
		private float timer = 45;
		private bool started;
		private void Start()
		{
			squadManager = GetComponent<SquadManager>();
			mercenaries.Add(Utils.Utils.CreateNewMercenary(InstabilityRiskActions.Low, 1));
			mercenaries.Add(Utils.Utils.CreateNewMercenary(InstabilityRiskActions.Medium, 1));
			mercenaries.Add(Utils.Utils.CreateNewMercenary(InstabilityRiskActions.High, 1));
			mercenaries.ForEach(m => m.level.onLevelUp = m.OnLevelUp);
		}

		private void Update()
		{
			if (started)
			{
				if (timer > 0)
				{
					timer -= Time.deltaTime;
				}
				else
				{
					if (mercenaries.Count < 10)
					{
						mercenaries.Add(Utils.Utils.CreateNewMercenary((InstabilityRiskActions)UnityEngine.Random.Range(0, 3), squadManager.AvrageLevel));
						UIManager.GlobalPopup("New mercenary available to hire");
						timer = 45;
					}
				}
			}
		}
		public void TryHireMercenary(int index)
		{
			if (mercenaries.IsNullOrEmpty() && index >= mercenaries.Count)
				return;

			var merc = mercenaries[index];
			if (squadManager.CurrentCash >= merc.value)
			{
				if (squadManager.AddNewMember(merc))
				{
					mercenaries.Remove(merc);
					squadManager.CurrentCash -= merc.value;
					UIManager.NotifiyCurrencyChanged();
				}
				else
					UIManager.GlobalPopup("You can only have mercenaries up to 10");
			}
			else
				UIManager.GlobalPopup("You dont have enough currency!");
		}

		internal void StartHiring()
		{

			started = true;
		}

		public Mercenary GetMercenary(int index)
		{
			if (mercenaries.IsNullOrEmpty() && index >= mercenaries.Count)
				return null;
			return mercenaries[index];
		}

		public void AddaMercenary(int recLevel)
		{
			var instbRisk = (InstabilityRiskActions)UnityEngine.Random.Range(0, 3);
			mercenaries.Add(Utils.Utils.CreateNewMercenary(instbRisk, recLevel));
		}
	}
}
