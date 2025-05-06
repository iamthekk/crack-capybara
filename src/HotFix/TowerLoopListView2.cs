using System;
using SuperScrollView;
using UnityEngine.EventSystems;

namespace HotFix
{
	public class TowerLoopListView2 : LoopListView2
	{
		public bool CanDrag { get; set; }

		public override void OnBeginDrag(PointerEventData eventData)
		{
			if (!this.CanDrag)
			{
				return;
			}
			base.OnBeginDrag(eventData);
		}

		public override void OnEndDrag(PointerEventData eventData)
		{
			if (!this.CanDrag)
			{
				return;
			}
			base.OnEndDrag(eventData);
		}

		public override void OnDrag(PointerEventData eventData)
		{
			if (!this.CanDrag)
			{
				return;
			}
			base.OnDrag(eventData);
		}
	}
}
