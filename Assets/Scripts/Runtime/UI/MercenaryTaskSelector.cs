using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HuntroxGames.LD49
{
    public class MercenaryTaskSelector : MonoBehaviour
    {
		[SerializeField] private Color nonSelectedColor;
		[SerializeField] private GameObject mercPrefab;
		[SerializeField] private Transform mercContainer;
		[SerializeField] private TextMeshProUGUI mercsListText;
		[SerializeField] private TextMeshProUGUI estmTimeListText;


		private SquadManager squadManager;
		private List<int> assignedMercs;
		private List<GameObject> assignedMercsGOs;
		private ActiveContract currentContract;
		void Start()
        {
            squadManager = GetComponent<SquadManager>();
        }




		public void SelectMercenaries(ActiveContract signedContract)
		{
			currentContract = signedContract;
			assignedMercs = new List<int>();
			assignedMercsGOs = new List<GameObject>();
			for (int i = 0; i < squadManager.AvailableMercenaries.Count; i++)
			{
				int index = i;
				var merc = squadManager.AvailableMercenaries[index];
				var go = Instantiate(mercPrefab, mercContainer);
				SetupMercUI(go, index, merc);
				assignedMercsGOs.Add(go);

			}
			RefreshList();
		}

		private void SetupMercUI(GameObject go, int index, Mercenary merc)
		{
			go.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
			var sr = go.transform.Find("portrait").GetComponent<Image>();
			sr.sprite = merc.portrait;
			var checkbox =go.transform.Find("checkbox").GetComponent<Toggle>();
			checkbox.onValueChanged.AddListener((value) =>
			{
				if(value )
				{
					if (assignedMercs.Count < GlobalSettings.MAX_MERCENARIES_PER_CONTRACT)
					{
						assignedMercs.Add(index);
						sr.color = Color.white;
						sr.GetComponent<RectTransform>().DOAnchorPosY(-20, 0.2f).SetEase(Ease.InOutSine);
						RefreshList();
					}
					else
					{
						UIManager.GlobalPopup("You can assign only up to 5 per mission");
						checkbox.SetIsOnWithoutNotify(false);
					}
				}
				else
				{
					assignedMercs.Remove(index);
					sr.color = nonSelectedColor;
					sr.GetComponent<RectTransform>().DOAnchorPosY(0, 0.2f).SetEase(Ease.InOutSine);
					RefreshList();
				}

			});
		}

		private void RefreshList()
		{
			mercsListText.text = "";
			float effeciencyFactor = 0;
			float estEstimatedTime = currentContract.remainingTime;
			for (int i = 0; i < assignedMercs.Count; i++)
			{
				mercsListText.text +=(i+1)+"-"+ squadManager.GetMercenary(assignedMercs[i]).mercenaryName+"\n";
				effeciencyFactor += squadManager.GetMercenary(assignedMercs[i]).efficiency;
			}
			var offPers = (estEstimatedTime * effeciencyFactor) / 100;
			estEstimatedTime -= offPers;
			var ts = TimeSpan.FromSeconds(estEstimatedTime);
			estmTimeListText.text = string.Format("Estimated time: {0:00}:{1:00}", ts.Minutes, ts.Seconds);
		}

		public void OnCancel()
		{
			if (!assignedMercsGOs.IsNullOrEmpty())
				assignedMercsGOs.ForEach(x =>
				{
					x.GetComponent<CanvasGroup>().DOFade(0, 0.3f).OnComplete(() => Destroy(x));
				});
			assignedMercsGOs = null;
			assignedMercs = new List<int>();
			currentContract = null;
		}
		public void OnFinishAssigning(Action<ActiveContract> callback)
		{
			if (assignedMercs.IsNullOrEmpty())
			{

				UIManager.GlobalPopup("You need At least one mercenary selected!");
				return;
			}

			currentContract.mercenaries = new List<Mercenary>();
			for (int i = 0; i < assignedMercs.Count; i++)
			{
				var merc = squadManager.GetMercenary(assignedMercs[i]);
				currentContract.mercenaries.Add(merc);
			}
			callback?.Invoke(currentContract);
			OnCancel();
		}
	}
}
