using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class CustomScrollRect : ScrollRect
	{
		protected override void Awake()
		{
			base.Awake();
			if (this.m_parentScrollRect == null)
			{
				Transform parent = base.transform.parent;
				if (parent)
				{
					this.m_parentScrollRect = parent.GetComponentInParent<CustomScrollRect>();
				}
			}
			this.m_direction = (base.horizontal ? CustomScrollRect.Direction.Horizontal : CustomScrollRect.Direction.Vertical);
		}

		public override void OnBeginDrag(PointerEventData eventData)
		{
			if (this.m_parentScrollRect)
			{
				this.m_beginDragDirection = ((Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y)) ? CustomScrollRect.Direction.Horizontal : CustomScrollRect.Direction.Vertical);
				if (this.m_beginDragDirection != this.m_direction)
				{
					ExecuteEvents.Execute<IBeginDragHandler>(this.m_parentScrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
					return;
				}
			}
			base.OnBeginDrag(eventData);
		}

		public override void OnDrag(PointerEventData eventData)
		{
			if (this.m_parentScrollRect && this.m_beginDragDirection != this.m_direction)
			{
				ExecuteEvents.Execute<IDragHandler>(this.m_parentScrollRect.gameObject, eventData, ExecuteEvents.dragHandler);
				return;
			}
			base.OnDrag(eventData);
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			if (this.m_parentScrollRect && this.m_beginDragDirection != this.m_direction)
			{
				ExecuteEvents.Execute<IEndDragHandler>(this.m_parentScrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
				return;
			}
			base.OnEndDrag(eventData);
		}

		public override void OnScroll(PointerEventData data)
		{
			if (this.m_parentScrollRect && this.m_beginDragDirection != this.m_direction)
			{
				ExecuteEvents.Execute<IScrollHandler>(this.m_parentScrollRect.gameObject, data, ExecuteEvents.scrollHandler);
				return;
			}
			base.OnScroll(data);
		}

		public CustomScrollRect m_parentScrollRect;

		private CustomScrollRect.Direction m_direction;

		private CustomScrollRect.Direction m_beginDragDirection;

		public enum Direction
		{
			Horizontal,
			Vertical
		}
	}
}
