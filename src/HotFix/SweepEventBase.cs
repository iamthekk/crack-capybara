using System;
using DG.Tweening;
using UnityEngine;

namespace HotFix
{
	public abstract class SweepEventBase : GameEventBase
	{
		protected void GoNext(float delayEx = 0f)
		{
			if (this.isHangUp)
			{
				return;
			}
			Sequence sequence = DOTween.Sequence();
			float num = (0.3f + delayEx) / Time.timeScale;
			if (num < 0.1f)
			{
				num = 0.1f;
			}
			TweenSettingsExtensions.AppendInterval(sequence, num);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				this.OnClickButton(0);
			});
		}

		protected void DelayShowUI()
		{
			Sequence sequence = DOTween.Sequence();
			float num = 0.3f / Time.timeScale;
			if (num < 0.1f)
			{
				num = 0.1f;
			}
			TweenSettingsExtensions.AppendInterval(sequence, num);
			TweenSettingsExtensions.AppendCallback(sequence, new TweenCallback(this.ShowUI));
		}

		public const float AUTO_SWEEP_DELAY = 0.3f;

		public const float AUTO_SWEEP_MIN_DELAY = 0.1f;
	}
}
