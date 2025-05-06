using System;
using DG.Tweening;
using Framework.Logic;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class NumAni
	{
		public void Init(Transform tran, CustomText ct)
		{
			this.trTarget = tran;
			this.text = ct;
			this.m_curValue = 0;
		}

		public void Init(Transform tran, CustomText ct, float duration, float scale)
		{
			this.Init(tran, ct);
			this.PlayDuration = duration;
			this.Scale = scale;
		}

		public void OnUpdate(float deltaTime)
		{
			if (!this.m_isPlaying)
			{
				return;
			}
			this.m_time += deltaTime;
			float num = Utility.Math.Clamp01(this.m_time / this.PlayDuration);
			int num2 = this.m_newCurValue;
			if (this.m_curValue != this.m_newCurValue)
			{
				num2 = this.m_curValue + (int)((float)(this.m_newCurValue - this.m_curValue) * num);
			}
			this.SetText(num2);
			if (Utility.Math.Abs(num - 1f) < 0.0001f)
			{
				this.m_isPlaying = false;
				this.m_curValue = this.m_newCurValue;
				this.SetText(this.m_newCurValue);
			}
		}

		public void SetValue(int current)
		{
			if (current == this.m_curValue)
			{
				return;
			}
			this.m_newCurValue = current;
			if (this.m_curValue != this.m_newCurValue)
			{
				this.m_isPlaying = true;
				this.m_time = 0f;
				this.SetText(this.m_curValue);
			}
			this.DoAnimation();
		}

		private void SetText(int current)
		{
			if (this.text == null)
			{
				return;
			}
			this.text.text = current.ToString();
		}

		private void DoAnimation()
		{
			float t = this.PlayDuration / 2f;
			Vector3 vector;
			vector..ctor(this.Scale, this.Scale, 1f);
			TweenSettingsExtensions.OnComplete<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.trTarget, vector, t), 6), delegate
			{
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.trTarget, Vector3.one, t), 7);
			});
		}

		private float PlayDuration = 1f;

		private float Scale = 1.5f;

		public CustomText text;

		public Transform trTarget;

		protected bool m_isPlaying;

		protected int m_curValue;

		protected int m_newCurValue;

		private float m_time;
	}
}
