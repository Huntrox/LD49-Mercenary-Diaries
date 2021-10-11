using HuntroxGames.LD49;
using HuntroxGames.Utils;
using UnityEngine;

public class TooltipTriggerEvent : EventsHandler
{
	[SerializeField] private string header;
	[SerializeField,TextArea(3,7)] private string Content;
	[SerializeField] private float delay =0.1f;
	[SerializeField] private string price;
	private UIManager uIManager;

	public void Init(string header="",string description="",string price= "", float delay=0.3f)
	{
		this.header = header;
		this.Content = description;
		this.price = price;
		this.delay = delay;
	}

 
    protected override void EventHandler(TriggerEvent t_event)
	{
		if (t_event == TriggerEvent.OnStart)
			uIManager = FindObjectOfType<UIManager>();

		if (t_event == triggerEvent && t_event != TriggerEvent.OnPointerExit)
        {
			uIManager.ShowTooltip(header, Content, price, delay);
		}
		if(t_event == TriggerEvent.OnPointerExit)
        {
			uIManager.HideTooltip();
		}
	}
}
