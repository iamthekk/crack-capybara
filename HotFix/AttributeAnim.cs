using System;
using DG.Tweening;
using Framework.Logic;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class AttributeAnim
	{
		public void Init(Transform tran, CustomText ct, bool isPercent = false)
		{
			this.trTarget = tran;
			this.text = ct;
			this.m_curValue = 0L;
			this.m_isPercent = isPercent;
			this.SetText(this.m_curValue);
			this.PlayDuration = 0.4f;
		}

		public void SetDuration(float duration)
		{
			this.PlayDuration = duration;
		}

		public void OnUpdate(float deltaTime)
		{
			if (!this.m_isPlaying)
			{
				return;
			}
			this.m_time += deltaTime;
			float num = Utility.Math.Clamp01(this.m_time / this.PlayDuration);
			long num2 = this.m_newCurValue;
			if (this.m_curValue != this.m_newCurValue)
			{
				num2 = this.m_curValue + (long)((float)(this.m_newCurValue - this.m_curValue) * num);
			}
			this.SetText(num2);
			if (Utility.Math.Abs(num - 1f) < 0.0001f)
			{
				this.m_isPlaying = false;
				this.m_curValue = this.m_newCurValue;
				this.SetText(this.m_newCurValue);
				Action action = this.aniFinish;
				if (action == null)
				{
					return;
				}
				action();
			}
		}

		public void SetValue(long current)
		{
			if (current == this.m_curValue)
			{
				return;
			}
			this.m_newCurValue = current;
			if (this.m_isPlaying)
			{
				return;
			}
			if (!this.m_curValue.Equals(0L))
			{
				if (this.m_curValue != this.m_newCurValue)
				{
					this.m_isPlaying = true;
					this.m_time = 0f;
					this.SetText(this.m_curValue);
				}
				this.DoAnimation();
				return;
			}
			this.m_curValue = this.m_newCurValue;
			this.SetText(this.m_curValue);
		}

		public void SetValue(long current, Action onFinish)
		{
			if (current == this.m_curValue)
			{
				return;
			}
			this.aniFinish = onFinish;
			this.m_newCurValue = current;
			if (this.m_isPlaying)
			{
				return;
			}
			if (this.m_curValue != this.m_newCurValue)
			{
				this.m_isPlaying = true;
				this.m_time = 0f;
				this.SetText(this.m_curValue);
			}
			this.DoAnimation();
		}

		private void SetText(long current)
		{
			if (this.text == null)
			{
				return;
			}
			this.text.text = (this.m_isPercent ? (DxxTools.FormatNumber(current) + "%") : DxxTools.FormatNumber(current).ToString());
		}

		private void DoAnimation()
		{
			float t = this.PlayDuration / 2f;
			Vector3 vector;
			vector..ctor(1.2f, 1.2f, 1f);
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.trTarget, vector, t), 6), delegate
			{
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.trTarget, Vector3.one, t), 7);
			});
		}

		public void SetTextColor(Color color)
		{
			this.text.color = color;
		}

		private float PlayDuration = 0.4f;

		private const float Scale = 1.2f;

		public CustomText text;

		public Transform trTarget;

		protected bool m_isPlaying;

		protected long m_curValue;

		protected long m_newCurValue;

		protected bool m_isPercent;

		private Action aniFinish;

		private float m_time;
	}
}
