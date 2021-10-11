﻿using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[AddComponentMenu("UI/ButtonExtension", 31)]
public class ExtendedButton : Button, IPointerEnterHandler, IPointerExitHandler
{

    [FormerlySerializedAs("enterAnimation")]
    [SerializeField]
    private bool m_EnterAnimation = false;

    [FormerlySerializedAs("exitAnimation")]
    [SerializeField]
    private bool m_ExitAnimationn = false;

    [FormerlySerializedAs("clickAnimation")]
    [SerializeField]
    private bool m_ClickAnimation = false;

    public bool enterAnimation
    {

        get { return m_EnterAnimation; }
        set { m_EnterAnimation = value; }

    }
    public bool exitAnimation
    {

        get { return m_ExitAnimationn; }
        set { m_ExitAnimationn = value; }

    }
    public bool clickAnimation
    {

        get { return m_ClickAnimation; }
        set { m_ClickAnimation = value; }

    }

    /*	[FormerlySerializedAs("onEnterAnimation")]
		[SerializeField]
		private ButtonExtensionAnimation m_OnEnterAnimation = new ButtonExtensionAnimation();

		[FormerlySerializedAs("onExitAnimation")]
		[SerializeField]
		private ButtonExtensionAnimation m_OnExitAnimationn = new ButtonExtensionAnimation();

		[FormerlySerializedAs("onClickAnimation")]
		[SerializeField]
		private ButtonExtensionAnimation m_OnClickAnimation = new ButtonExtensionAnimation();

		public ButtonExtensionAnimation onEnterAnimation
		{

			get { return m_OnEnterAnimation; }
			set { m_OnEnterAnimation = value; }

		}
		public ButtonExtensionAnimation onExitAnimation
		{

			get { return m_OnExitAnimationn; }
			set { m_OnExitAnimationn = value; }

		}
		public ButtonExtensionAnimation onClickAnimation
		{

			get { return m_OnClickAnimation; }
			set { m_OnClickAnimation = value; }

		}
    */
    [FormerlySerializedAs("onEnter")]
    [SerializeField]
    private UnityEvent m_OnEnter = new UnityEvent();

    [FormerlySerializedAs("onExit")]
    [SerializeField]
    private UnityEvent m_OnExit = new UnityEvent();

    public UnityEvent onEnter
    {
        get { return m_OnEnter; }
        set { m_OnEnter = value; }
    }
    public UnityEvent onExit
    {
        get { return m_OnExit; }
        set { m_OnExit = value; }
    }



    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsActive() || !IsInteractable())
            return;

        base.OnPointerEnter(eventData);

        UISystemProfilerApi.AddMarker("Button.onEnter", this);
        m_OnEnter.Invoke();
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!IsActive() || !IsInteractable())
            return;


        base.OnPointerExit(eventData);


        UISystemProfilerApi.AddMarker("Button.onExit", this);
        m_OnExit.Invoke();
    }
	public override void OnPointerClick(PointerEventData eventData)
	{
		base.OnPointerClick(eventData);

	}

	public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        OnPointerExit(null);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        OnPointerEnter(null);
    }

    public override void OnSubmit(BaseEventData eventData)
    {
        base.OnSubmit(eventData);
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        OnPointerClick(pointer);
    }
}
 