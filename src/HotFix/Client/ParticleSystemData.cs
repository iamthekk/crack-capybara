using System;
using UnityEngine;

namespace HotFix.Client
{
	public class ParticleSystemData
	{
		public void Init(ParticleSystem ps)
		{
			if (ps == null)
			{
				return;
			}
			this.particleSystem = ps;
			this.renderer = this.particleSystem.GetComponent<Renderer>();
			this.initSortingOrder = this.renderer.sortingOrder;
			this.initSortingLayerName = this.renderer.sortingLayerName;
		}

		public void SetOrderLayer(int value)
		{
			this.renderer.sortingOrder = value;
		}

		public int initSortingOrder;

		public string initSortingLayerName;

		public ParticleSystem particleSystem;

		public Renderer renderer;
	}
}
