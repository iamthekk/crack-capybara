using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Framework.Logic;
using Framework.Logic.UI;

namespace HotFix
{
	public class ProgressTextCtrl : TipsProgressCtrl
	{
		public CustomText text { get; private set; }

		public int current
		{
			get
			{
				return this._cur;
			}
			set
			{
				this._cur = value;
				this.UpdateValue();
				if (this.OnValueChanged != null)
				{
					this.OnValueChanged(this.current, this.max);
				}
			}
		}

		public int currentcount
		{
			get
			{
				return this._cur;
			}
			set
			{
				if (value >= 0)
				{
					this._cur = value;
					if (this._max > 0 && this.text)
					{
						this.text.text = string.Format("{0}/{1}", this._cur, this._max);
					}
				}
			}
		}

		public int max
		{
			get
			{
				return this._max;
			}
			set
			{
				if (value > 0)
				{
					this._max = value;
					this.UpdateValue();
				}
			}
		}

		protected override void OnAwake()
		{
			this.text = base.transform.Find("Slider/Text").GetComponent<CustomText>();
		}

		private void UpdateValue()
		{
			if (this._max > 0)
			{
				base.Value = (float)Utility.Math.Clamp(this._cur, 0, this._cur) / ((float)this._max + 0f);
				if (this.text)
				{
					this.text.text = string.Format("{0}/{1}", this._cur, this._max);
				}
			}
		}

		public void SetText(string value)
		{
			if (this.text)
			{
				this.text.text = value;
			}
		}

		public void PlayTextScale(float scale, float time)
		{
			if (this.text)
			{
				Sequence sequence = DOTween.Sequence();
				TweenSettingsExtensions.SetUpdate<Sequence>(sequence, true);
				TweenSettingsExtensions.Append(sequence, ShortcutExtensions.DOScale(this.text.transform, scale, time));
			}
		}

		public Sequence PlayCount(int after, float alltime, Action onFinish)
		{
			int num = Utility.Math.Abs(after - this.current);
			float num2 = alltime;
			if (num != 0)
			{
				num2 = alltime / (float)num;
			}
			num2 = Utility.Math.Clamp(num2, 0f, 0.15f);
			this.PlayTextScale(1.4f, 0.15f);
			TweenSettingsExtensions.SetUpdate<Tweener>(DOTween.To(() => this.current, delegate(int x)
			{
				this.current = x;
			}, after, num2 * (float)num), true);
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.SetUpdate<Sequence>(TweenSettingsExtensions.AppendInterval(TweenSettingsExtensions.AppendCallback(TweenSettingsExtensions.AppendInterval(sequence, num2 * (float)(num - 1)), delegate
			{
				Action onFinish2 = onFinish;
				if (onFinish2 != null)
				{
					onFinish2();
				}
				this.PlayTextScale(1f, 0.15f);
			}), num2), true);
			return sequence;
		}

		public void PlayPercent(float after, float time)
		{
			TweenSettingsExtensions.SetUpdate<TweenerCore<float, float, FloatOptions>>(DOTween.To(() => base.Value, delegate(float x)
			{
				base.Value = x;
			}, after, time), true);
		}

		private int _cur;

		private int _max = 10;

		public Action<int, int> OnValueChanged;
	}
}
