using System;
using DG.Tweening;
using Framework.Logic;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class AttributeHpAnim
	{
		public void Init(Transform tran, CustomText ct)
		{
			this.tranHp = tran;
			this.textHp = ct;
			this.m_currentHp = 0L;
			this.m_maxHp = 0L;
		}

		public void OnUpdate(float deltaTime)
		{
			if (!this.m_isPlaying)
			{
				return;
			}
			this.m_time += deltaTime;
			float num = Utility.Math.Clamp01(this.m_time / 0.4f);
			long num2 = this.m_newCurrentHp;
			if (this.m_currentHp != this.m_newCurrentHp)
			{
				num2 = this.m_currentHp + (long)((float)(this.m_newCurrentHp - this.m_currentHp) * num);
			}
			long num3 = this.m_newMaxHp;
			if (this.m_maxHp != this.m_newMaxHp)
			{
				num3 = this.m_maxHp + (long)((float)(this.m_newMaxHp - this.m_maxHp) * num);
			}
			this.SetText(num2, num3);
			if (Utility.Math.Abs(num - 1f) < 0.0001f)
			{
				this.m_isPlaying = false;
				this.m_currentHp = this.m_newCurrentHp;
				this.m_maxHp = this.m_newMaxHp;
				this.SetText(this.m_newCurrentHp, this.m_newMaxHp);
			}
		}

		public void SetHp(long current, long max, bool useAni = true)
		{
			if (current == this.m_currentHp && max == this.m_maxHp)
			{
				return;
			}
			this.m_newCurrentHp = current;
			this.m_newMaxHp = max;
			if (this.m_maxHp > 0L)
			{
				if (this.m_currentHp != this.m_newCurrentHp || this.m_maxHp != this.m_newMaxHp)
				{
					this.m_isPlaying = useAni;
					this.m_time = 0f;
					this.SetText(this.m_currentHp, this.m_maxHp);
				}
			}
			else
			{
				this.m_currentHp = this.m_newCurrentHp;
				this.m_maxHp = this.m_newMaxHp;
				this.SetText(this.m_currentHp, this.m_maxHp);
			}
			if (this.m_isPlaying)
			{
				this.TextAnimation();
			}
		}

		private void SetText(long current, long max)
		{
			if (this.textHp == null)
			{
				return;
			}
			this.textHp.text = string.Format("{0}/{1}", DxxTools.FormatNumber(current), DxxTools.FormatNumber(max));
		}

		private void TextAnimation()
		{
			float t = 0.2f;
			Vector3 vector;
			vector..ctor(1.3f, 1.3f, 1f);
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.tranHp, vector, t), 6), delegate
			{
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.tranHp, Vector3.one, t), 7);
			});
		}

		public void SetTextColor(Color color)
		{
			this.textHp.color = color;
		}

		public const float PlayDuration = 0.4f;

		private const float Scale = 1.3f;

		private CustomText textHp;

		private Transform tranHp;

		private bool m_isPlaying;

		private long m_currentHp;

		private long m_maxHp;

		private long m_newCurrentHp;

		private long m_newMaxHp;

		private float m_time;
	}
}
