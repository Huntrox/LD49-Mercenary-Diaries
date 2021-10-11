using System;
using System.Collections.Generic;
using UnityEngine;

namespace HuntroxGames.LD49
{
    [System.Serializable]
    public class Inventory<T>
    {
        public List<T> items;
        public int cash;
		public int invMaxItems = -1;
		public Action OnRefresh;
		#region Constructor
		public Inventory()
		{
			items = new List<T>();
		}

		public Inventory(int cash)
		{
			items = new List<T>();
			this.cash = cash;
		}
		public Inventory(List<T> items, int cash,int invMaxItems = -1)
		{
			this.items = items;
			this.cash = cash;
			this.invMaxItems = invMaxItems;
		}

		public Inventory(List<T> items, int cash, int invMaxItems, Action onRefresh)
		{
			this.items = items;
			this.cash = cash;
			this.invMaxItems = invMaxItems;
			OnRefresh = onRefresh;
		}
		#endregion

		public bool AddItem(T item)
		{
			if (invMaxItems == -1)
			{
				items.Add(item);
				OnRefresh?.Invoke();
				return true;
			}
			else
			{
				if (items.Count >= invMaxItems)
				{
					OnRefresh?.Invoke();
					return false;
				}
				else
				{
					items.Add(item);
					OnRefresh?.Invoke();
					return true;
				}
			}
		}
	}
}