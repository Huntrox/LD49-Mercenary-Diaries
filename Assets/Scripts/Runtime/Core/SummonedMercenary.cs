using DG.Tweening;
using HuntroxGames.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HuntroxGames.LD49
{
	public class SummonedMercenary : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
    {


		private Mercenary mercenary;
		private Image portrait;
		[SerializeField] private Image healthFill;
		[SerializeField] private Image smFill;
		[SerializeField] private TextMeshProUGUI healthText;
		[SerializeField] private TextMeshProUGUI smText;
		[SerializeField] private float fillDuration =0.3f;


		private bool dismissing = false;
		private SquadManager squadManager;
		private UIManager uimanager;
		public void Init(Mercenary merc)
		{
			portrait = transform.Find("portrait").GetComponent<Image>();
			mercenary = merc;
			portrait.sprite = merc.portrait;
			squadManager = FindObjectOfType<SquadManager>();
			uimanager = FindObjectOfType<UIManager>();

			healthText.text = mercenary.currentHealth + "/" + mercenary.MaxHealth;
			smText.text = mercenary.currMentalStability + "/" + mercenary.mentalStability;
			healthFill.DOFill(mercenary.CurrentHealthPerc, fillDuration).SetEase(Ease.InOutSine);
			smFill.DOFill(mercenary.CurrentMentalStabilityPerc, fillDuration).SetEase(Ease.InOutSine);
		}

		public void DismissMerc()
		{
			uimanager.DismissMercenary();
		}
		public void Dismiss()
		{
			dismissing = true;
			GetComponent<CanvasGroup>().DOFade(0, 0.3f).SetEase(Ease.InOutSine).OnComplete(() =>
			 {
				 Destroy(gameObject);
			 });
		}

		public bool TryGiveItem(Item item)
		{

			mercenary.CurrentHealthPerc = mercenary.currentHealth + item.health;
			mercenary.CurrentMentalStabilityPerc = mercenary.currMentalStability + item.ms;
			squadManager.UpdateCurrency(-item.itemValue);
			squadManager.ReleaseItem();
			healthText.text = mercenary.currentHealth + "/" + mercenary.MaxHealth;
			smText.text = mercenary.currMentalStability + "/" + mercenary.mentalStability;
			healthFill.DOFill(mercenary.CurrentHealthPerc, fillDuration).SetEase(Ease.InOutSine);
			smFill.DOFill(mercenary.CurrentMentalStabilityPerc, fillDuration).SetEase(Ease.InOutSine);
			return false;
		}
		public void OnPointerClick(PointerEventData eventData)
		{
			if (squadManager.HasItem && eventData != null && squadManager.HoldItem != null)
				TryGiveItem(squadManager.HoldItem);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		public void OnPointerExit(PointerEventData eventData)
		{

		}


	}
}
