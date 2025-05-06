using System;
using Framework.Logic;
using UnityEngine;

namespace HotFix
{
	public class TipsProgressCtrl : MonoBehaviour
	{
		public float Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				value = Utility.Math.Clamp01(value);
				this._Value = value;
				this.UpdateFill();
			}
		}

		private void Awake()
		{
			this.InitFill();
			this.tran = base.transform as RectTransform;
			this.OnAwake();
		}

		public void OutInit()
		{
			this.width = 0f;
			this.height = 0f;
			this.Awake();
		}

		protected virtual void OnAwake()
		{
		}

		private void InitFill()
		{
			if (!this.fill)
			{
				this.fill = base.transform.Find("Slider/Fill") as RectTransform;
				if (this.fill)
				{
					this.width = this.fill.sizeDelta.x;
					this.height = this.fill.sizeDelta.y;
				}
			}
		}

		private void RefreshSize()
		{
			if (this.width == 0f && this.fill)
			{
				RectTransform rectTransform = this.fill;
				this.width = rectTransform.sizeDelta.x;
				this.height = rectTransform.sizeDelta.y;
			}
		}

		public void SetFillX(float w)
		{
			if (this.fill)
			{
				RectTransform rectTransform = base.transform as RectTransform;
				RectTransform rectTransform2 = this.fill;
				RectTransform rectTransform3 = this.fill.parent as RectTransform;
				float num = (rectTransform.sizeDelta.x - rectTransform2.sizeDelta.x) / 2f;
				rectTransform.sizeDelta = new Vector2(w, rectTransform.sizeDelta.y);
				rectTransform3.sizeDelta = rectTransform.sizeDelta;
				rectTransform2.sizeDelta = new Vector2(w - num * 2f, rectTransform2.sizeDelta.y);
				rectTransform2.anchoredPosition = new Vector2(-rectTransform2.sizeDelta.x / 2f, rectTransform2.anchoredPosition.y);
				this.width = rectTransform2.sizeDelta.x;
				this.height = rectTransform2.sizeDelta.y;
			}
		}

		protected void UpdateFill()
		{
			this.InitFill();
			this.RefreshSize();
			if (this.fill)
			{
				this.fill.sizeDelta = new Vector2(this.Value * this.width, this.height);
			}
		}

		public TipsProgressCtrl.ProgressDirection direction;

		private RectTransform fill;

		private RectTransform tran;

		private float width;

		private float height;

		private float _Value;

		public enum ProgressDirection
		{
			LeftToRight
		}
	}
}
