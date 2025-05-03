using System;
using UnityEngine;

namespace Framework.Logic.Component
{
	public class MeshOrder : MonoBehaviour
	{
		private void Start()
		{
			MeshRenderer component = base.GetComponent<MeshRenderer>();
			if (component)
			{
				switch (this.sortlayer)
				{
				case MeshOrder.SortingLayers.BackGround:
					component.sortingLayerName = "BackGround";
					break;
				case MeshOrder.SortingLayers.EffectFar:
					component.sortingLayerName = "EffectFar";
					break;
				case MeshOrder.SortingLayers.Member:
					component.sortingLayerName = "Member";
					break;
				case MeshOrder.SortingLayers.EffectNear:
					component.sortingLayerName = "EffectNear";
					break;
				}
				component.sortingOrder = this.order;
			}
		}

		public int order;

		public MeshOrder.SortingLayers sortlayer;

		public enum SortingLayers
		{
			None,
			BackGround,
			EffectFar,
			Member,
			EffectNear
		}
	}
}
