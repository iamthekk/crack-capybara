using System;
using UnityEngine;

namespace Framework.Logic.UI
{
	[RequireComponent(typeof(Canvas))]
	public class SortingOrderTool4Canvas : SortingOrderToolBase
	{
		protected override void InitData()
		{
			base.InitData();
			if (this.canvas == null)
			{
				this.canvas = base.GetComponent<Canvas>();
			}
		}

		public override void SetSortingOrder(int baseSortingOrder)
		{
			this.InitData();
			if (this.canvas != null)
			{
				this.canvas.sortingOrder = baseSortingOrder + this.offsetSortingOrder;
			}
		}

		public Canvas canvas;
	}
}
