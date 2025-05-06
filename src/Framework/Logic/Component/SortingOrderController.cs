using System;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class SortingOrderController : MonoBehaviour
	{
		private void OnEnable()
		{
			if (this.m_render == null)
			{
				return;
			}
			this.m_render.sortingOrder = this.m_sortingOrder;
		}

		private void OnDisable()
		{
			if (this.m_render == null)
			{
				return;
			}
			this.m_render.sortingOrder = this.m_sortingOrder;
		}

		public int m_sortingOrder = 10000;

		public Renderer m_render;
	}
}
