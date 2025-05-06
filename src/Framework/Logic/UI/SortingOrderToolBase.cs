using System;
using UnityEngine;

namespace Framework.Logic.UI
{
	public abstract class SortingOrderToolBase : MonoBehaviour
	{
		protected virtual void InitData()
		{
		}

		public abstract void SetSortingOrder(int baseSortingOrder);

		[Tooltip("层级排序基于基础偏移值")]
		public int offsetSortingOrder = 1;
	}
}
