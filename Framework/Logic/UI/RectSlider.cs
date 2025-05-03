using System;
using UnityEngine;
using UnityEngine.UI;

namespace Framework.Logic.UI
{
	[RequireComponent(typeof(RectTransform))]
	[ExecuteInEditMode]
	public class RectSlider : MonoBehaviour
	{
		private void UpdateFill()
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = base.GetComponent<RectTransform>();
			}
			if (this.rectMask == null)
			{
				this.rectMask = base.GetComponentInChildren<RectMask2D>();
			}
			if (this.rectMask == null)
			{
				return;
			}
			this.padding = Vector4.zero;
			switch (this.fillMode)
			{
			case RectSlider.FillMode.LeftToRight:
				this.padding.z = this.rectTransform.rect.width * (1f - this.value);
				break;
			case RectSlider.FillMode.RightToLeft:
				this.padding.x = this.rectTransform.rect.width * (1f - this.value);
				break;
			case RectSlider.FillMode.UpToDown:
				this.padding.y = this.rectTransform.rect.height * (1f - this.value);
				break;
			case RectSlider.FillMode.DownToUp:
				this.padding.w = this.rectTransform.rect.height * (1f - this.value);
				break;
			}
			this.rectMask.padding = this.padding;
		}

		private void Update()
		{
			this.UpdateFill();
		}

		public RectMask2D rectMask;

		public RectSlider.FillMode fillMode;

		[Range(0f, 1f)]
		public float value;

		private RectTransform rectTransform;

		private Vector4 padding;

		public enum FillMode
		{
			LeftToRight,
			RightToLeft,
			UpToDown,
			DownToUp
		}
	}
}
