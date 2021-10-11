using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HuntroxGames.Utils
{
    public class GlobalActionsTimerManager : Singleton<GlobalActionsTimerManager>
    {

        private List<ActionTimer> actions = new List<ActionTimer>();

        void Update()
        {
            for (int i = 0; i < actions.Count; i++)
                actions[i].Update(Time.deltaTime);
        }

        public void AddAction(params ActionTimer[] actions)
		{
			foreach (var action in actions)
                AddAction(action);
		}



        public void AddAction(ActionTimer action)
		{
			if (actions.Contains(action))
			{
                Debug.LogWarning("action already been added");
                return;
			}
            actions.Add(action);
		}
        public void RemoveAction(ActionTimer action)
        {
            if (actions.Contains(action))
            {
                actions.Remove(action);
                return;
            }else
                Debug.LogWarning("action does not exists on the action list");

        }
    }
}