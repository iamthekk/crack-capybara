using System;
using UnityEngine;

namespace Framework.Logic.UI
{
	[RequireComponent(typeof(Renderer))]
	public class SortingOrderTool4Renderer : SortingOrderToolBase
	{
		protected override void InitData()
		{
			base.InitData();
			if (this.renderer == null)
			{
				this.renderer = base.GetComponent<Renderer>();
			}
		}

		public override void SetSortingOrder(int baseSortingOrder)
		{
			this.InitData();
			if (this.renderer != null)
			{
				this.renderer.sortingOrder = baseSortingOrder + this.offsetSortingOrder;
			}
		}

		public Renderer renderer;
	}
}
