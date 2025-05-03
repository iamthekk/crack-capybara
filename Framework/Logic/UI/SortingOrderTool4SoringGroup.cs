using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Framework.Logic.UI
{
	[RequireComponent(typeof(SortingGroup))]
	public class SortingOrderTool4SoringGroup : SortingOrderToolBase
	{
		protected override void InitData()
		{
			base.InitData();
			if (this.sortingGroup == null)
			{
				this.sortingGroup = base.GetComponent<SortingGroup>();
			}
		}

		public override void SetSortingOrder(int baseSortingOrder)
		{
			this.InitData();
			if (this.sortingGroup != null)
			{
				this.sortingGroup.sortingOrder = baseSortingOrder + this.offsetSortingOrder;
			}
		}

		public SortingGroup sortingGroup;
	}
}
