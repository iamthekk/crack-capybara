using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	public class UISortOrder : MonoBehaviour
	{
		private int curOrder
		{
			get
			{
				if (this._cur_order < 0 && this.targetCanvas != null)
				{
					this._cur_order = this.targetCanvas.sortingOrder + this.orderOffset;
				}
				return this._cur_order;
			}
			set
			{
				this._cur_order = value;
			}
		}

		private Canvas targetCanvas
		{
			get
			{
				if (this.target != null)
				{
					return this.target.canvas;
				}
				if (this.parentCanvas != null)
				{
					return this.parentCanvas;
				}
				return null;
			}
		}

		private void Start()
		{
			if (this.target == null)
			{
				this.parentCanvas = base.transform.parent.GetComponentInParent<Canvas>();
			}
			this.UpdateSortOrder();
		}

		private void Update()
		{
			if (this.targetCanvas != null && this.curOrder != this.targetCanvas.sortingOrder + this.orderOffset)
			{
				this.curOrder = this.targetCanvas.sortingOrder + this.orderOffset;
				this.UpdateSortOrder();
			}
		}

		private void UpdateSortOrder()
		{
			if (this.target != null && this.particleParent != null)
			{
				Renderer[] componentsInChildren = this.particleParent.GetComponentsInChildren<Renderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].sortingLayerID = this.targetCanvas.sortingLayerID;
					componentsInChildren[i].sortingOrder = this.curOrder;
				}
			}
			if (this.canvas != null)
			{
				this.canvas.overrideSorting = true;
				this.canvas.sortingLayerID = this.targetCanvas.sortingLayerID;
				this.canvas.sortingOrder = this.curOrder;
			}
		}

		public Graphic target;

		public GameObject particleParent;

		public Canvas canvas;

		public int orderOffset;

		private int _cur_order = -1;

		private Canvas parentCanvas;
	}
}
