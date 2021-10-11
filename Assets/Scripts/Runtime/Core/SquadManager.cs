using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HuntroxGames.LD49
{
    public class SquadManager : MonoBehaviour
    {

        [SerializeField] private int starterCash;
        [SerializeField] private List<Mercenary> mercenaries = new List<Mercenary>();
        [SerializeField] private Inventory<Item> inventory;
        [SerializeField] private Image itemIcon;
		[SerializeField] private RectTransform canvas;
        private Item holdItem;
        private bool hasItem;
		private int squadAvrageLevel;
        public List<Mercenary> AvailableMercenaries => mercenaries.Where(m => m.IsOnATask == false).ToList();

        public int CurrentCash { get => inventory.cash; set => inventory.cash = value; }
		public int AvrageLevel { get => squadAvrageLevel;}

		public int CurrentSquadCount { get=> mercenaries.Count;}
        public List<Item> Items => inventory.items;

		public bool HasItem { get => hasItem != null; }
		public Item HoldItem { get => holdItem; }

		private void Awake()
		{
            var itemsDB = Resources.Load<ItemsDatabase>("Database");
            inventory = new Inventory<Item>(itemsDB.items, starterCash, 9);
            inventory.OnRefresh = OnInventoryRefrshed;
        }
		void Start()
        {



        }

        public void UpdateCurrency(int cash)
        {
            if (cash == 0)
                return;
            UIManager.GlobalPopup(cash > 0 ? "+" : "" + cash + "$");
            inventory.cash += cash;
            UIManager.NotifiyCurrencyChanged();
        }

        private void Update()
        {
            if (hasItem)
            {
                Vector2 anchPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, null, out anchPos);
                itemIcon.rectTransform.anchoredPosition = anchPos;
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    ReleaseItem();
                }
            }
			if (Input.GetMouseButtonDown(0))
			{
				List<RaycastResult> results = new List<RaycastResult>();
				var pointerEventData = new PointerEventData(EventSystem.current);
				pointerEventData.position = Input.mousePosition;
				EventSystem.current.RaycastAll(pointerEventData, results);
				if (results.Count > 0 && !results.Any(x=> x.gameObject.CompareTag("Mercenary")))
					ReleaseItem();
			}
		}

        public void ReleaseItem()
        {
            holdItem = null;
            hasItem = false;
            if (itemIcon)
                itemIcon.enabled = false;
        }

		internal void TryBuyItem(int index)
		{
            var  _holdItem = inventory.items[index];
            if(CurrentCash < _holdItem.itemValue)
			{
                ReleaseItem();
                UIManager.GlobalPopup("You don't have enough currnecy!");
                return;
			}
            hasItem = true;
            itemIcon.enabled = true;
            holdItem = _holdItem;
            itemIcon.sprite = holdItem.icon;
        }

		public bool TryAddItemToInventory(Item item)
		    => inventory.AddItem(item);
		

		private void OnInventoryRefrshed()
		{

		}

        public bool AddNewMember(Mercenary mercenary)
		{
            if (mercenaries.Count < GlobalSettings.MAX_HIRED_MERCENARIES)
            {
                mercenaries.Add(mercenary);
                CalcutaleAvrage();
                return true;
            }
            return false;
		}
        public void RemoveFromSquad(Mercenary mercenary)
        {
            if (mercenaries.Contains(mercenary))
            {
                mercenaries.Remove(mercenary);
                CalcutaleAvrage();
            }
        }

        public void RemoveFromSquad(int index)
		{
            var removedMerc = mercenaries[index];
            mercenaries.Remove(removedMerc);
            CalcutaleAvrage();

        }
        public void AddExpToSquad(int xp)
		{

			for (int i = 0; i < mercenaries.Count; i++)
                mercenaries[i].level.AddExp(xp);

            CalcutaleAvrage();
        }

        private void CalcutaleAvrage()
        {
            var levels = 0.0;
            if (!mercenaries.IsNullOrEmpty())
                levels = mercenaries.Average(s => s.level.currentLevel);
            squadAvrageLevel = Mathf.RoundToInt((float)levels);
        }
        public Mercenary GetMercenary(int index)
		{
            if (mercenaries.IsNullOrEmpty() && index >= mercenaries.Count)
                return null;
            return mercenaries[index];
		}
        public void AddCash(int cashReward)
        {
            if (cashReward == 0)
                return;
            inventory.cash += cashReward;
        }
        internal Mercenary SummonMercenary(int currentIndex)
        {

            return GetMercenary(currentIndex);

        }

#if UNITY_EDITOR
        [ContextMenu("AddExpToSquad")]
        private void squadExp() => AddExpToSquad(500);
        [ContextMenu("CalculateLevels")]
        private void calcutaleAvrage() => CalcutaleAvrage();




#endif
	}
}
