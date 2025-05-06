using System;
using Framework.Logic.Component;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix
{
	public class ProgressbarCtrl : CustomBehaviour
	{
		public float CurrentValue
		{
			get
			{
				return this._currentValue;
			}
			set
			{
				this._currentValue = value;
				this.UpdateSliderFill();
			}
		}

		public int MaxValue
		{
			get
			{
				return this._maxValue;
			}
			set
			{
				this._maxValue = value;
				this.UpdateSliderFill();
			}
		}

		public float CurrentPercent
		{
			get
			{
				if (this._maxValue > 0)
				{
					return Mathf.Clamp(this._currentValue / (float)this._maxValue, 0f, 1f);
				}
				return 1f;
			}
			set
			{
				this._currentValue = value * (float)this._maxValue;
				this.UpdateSliderFill();
			}
		}

		public void UpdateSliderFill()
		{
			float currentPercent = this.CurrentPercent;
			if (this.FillImage != null)
			{
				foreach (Image image in this.FillImage)
				{
					if (image != null)
					{
						image.fillAmount = currentPercent;
					}
				}
			}
			if (this.FillImageSlicedFilled != null)
			{
				this.FillImageSlicedFilled.fillAmount = currentPercent;
			}
			if (this.MaskSliderCoverRect != null)
			{
				this.MaskSliderCoverRect.localPosition = Vector3.Lerp(this._maskCoverStartPos, this._maskCoverEndPos, currentPercent);
			}
		}

		protected override void OnInit()
		{
			if (this.MaskSliderCoverRect != null)
			{
				this._maskCoverStartPos = this.MaskSliderCoverStartRect.localPosition;
				this._maskCoverEndPos = this.MaskSliderCoverEndRect.localPosition;
			}
		}

		protected override void OnDeInit()
		{
		}

		[SerializeField]
		private SlicedFilledImage FillImageSlicedFilled;

		[SerializeField]
		private Image[] FillImage;

		[SerializeField]
		private RectTransform MaskSliderCoverRect;

		[SerializeField]
		private RectTransform MaskSliderCoverStartRect;

		[SerializeField]
		private RectTransform MaskSliderCoverEndRect;

		private Vector3 _maskCoverStartPos;

		private Vector3 _maskCoverEndPos;

		private float _currentValue;

		private int _maxValue = 1;
	}
}
