using System;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIScrollDrag : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
	private void Awake()
	{
		this.thisScrollRect = base.GetComponent<CustomScrollRect>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (this.anotherScrollRect == null)
		{
			return;
		}
		this.anotherScrollRect.enabled = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (eventData == null)
		{
			return;
		}
		if (this.anotherScrollRect == null)
		{
			return;
		}
		if (this.bFirstDrag)
		{
			float num = Vector2.Angle(eventData.delta, Vector2.up);
			if (num > 45f && num < 135f)
			{
				this.thisScrollRect.enabled = !this.thisIsUpAndDown;
				this.anotherScrollRect.enabled = this.thisIsUpAndDown;
			}
			else
			{
				this.anotherScrollRect.enabled = !this.thisIsUpAndDown;
				this.thisScrollRect.enabled = this.thisIsUpAndDown;
			}
		}
		if (this.thisScrollRect.enabled)
		{
			if (this.bFirstDrag)
			{
				this.thisScrollRect.OnBeginDrag(eventData);
				this.bFirstDrag = false;
			}
			this.thisScrollRect.OnDrag(eventData);
			return;
		}
		if (this.bFirstDrag)
		{
			this.anotherScrollRect.OnBeginDrag(eventData);
			this.bFirstDrag = false;
		}
		this.anotherScrollRect.OnDrag(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (this.anotherScrollRect == null)
		{
			return;
		}
		if (this.thisScrollRect.enabled)
		{
			this.thisScrollRect.OnEndDrag(eventData);
		}
		else
		{
			this.anotherScrollRect.OnEndDrag(eventData);
		}
		this.anotherScrollRect.enabled = true;
		this.thisScrollRect.enabled = true;
		this.bFirstDrag = true;
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (this.Event_OnClick != null)
		{
			this.Event_OnClick();
		}
	}

	public CustomScrollRect anotherScrollRect;

	public bool thisIsUpAndDown = true;

	public Action Event_OnClick;

	private CustomScrollRect thisScrollRect;

	private bool bFirstDrag = true;
}
