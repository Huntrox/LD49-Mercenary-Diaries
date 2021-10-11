using UnityEngine;
using UnityEngine.UI;

namespace HuntroxGames.LD49
{
	public class ItemUI : MonoBehaviour
    {
        private ExtendedButton extendedButton;
        private Image sprite;
        private SquadManager squadManager;
        private UIManager uIManager;
        public void Setup(Item item, int index)
        {
            uIManager = FindObjectOfType<UIManager>();
            squadManager = FindObjectOfType<SquadManager>();
            extendedButton = GetComponent<ExtendedButton>();
            sprite = transform.Find("icon").GetComponent<Image>();
            sprite.sprite = item.icon;

            extendedButton.onClick.RemoveAllListeners();
            extendedButton.onEnter.RemoveAllListeners();
            extendedButton.onExit.RemoveAllListeners();

            extendedButton.onClick.AddListener(() => squadManager.TryBuyItem(index));
            extendedButton.onEnter.AddListener(() => uIManager.ShowTooltip(item.itemName, item.itemDescription, item.itemValue.ToString(), 0.1f));
            extendedButton.onExit.AddListener(() => uIManager.HideTooltip());


        }
    }
}