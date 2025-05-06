using System;
using DG.Tweening;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class TextJumpChangeCtrl : CustomBehaviour
	{
		public Vector3 ScaleOffset { get; set; } = new Vector3(-0.2f, 0.1f, 0f);

		public Vector3 PosOffset { get; set; } = new Vector3(0f, 20f, 0f);

		public float Duration { get; set; } = 0.5f;

		protected override void OnInit()
		{
			Transform transform = this.Text.transform;
			this.initialScale = transform.localScale;
			this.initiaLocalPos = transform.localPosition;
			this.Text.text = DxxTools.FormatNumber(this.currentValue);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetAnimData(Vector3 scaleOffsetVal, Vector3 posOffsetVal, float durationVal)
		{
			this.ScaleOffset = scaleOffsetVal;
			this.PosOffset = posOffsetVal;
			this.Duration = durationVal;
		}

		public void JumpToTargetValue(long targetValue)
		{
			this.JumpToTargetValue(this.currentValue, targetValue);
		}

		public void JumpToTargetValue(long startValue, long targetValue)
		{
			Sequence sequence = this.currencySequence;
			if (sequence != null)
			{
				TweenExtensions.Kill(sequence, false);
			}
			this.currencySequence = DOTween.Sequence();
			Transform transform = this.Text.transform;
			transform.localScale = this.initialScale;
			transform.localPosition = this.initiaLocalPos;
			this.currentValue = startValue;
			TweenSettingsExtensions.Append(this.currencySequence, TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnUpdate<Tweener>(DOTween.To(() => this.currentValue, delegate(long x)
			{
				this.currentValue = x;
			}, targetValue, this.Duration), delegate
			{
				this.Text.text = DxxTools.FormatNumber(this.currentValue);
			}), 6));
			float num = this.Duration / 2f;
			Vector3 vector = this.initialScale + this.ScaleOffset;
			TweenSettingsExtensions.Insert(this.currencySequence, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(transform, vector, num), 6));
			TweenSettingsExtensions.Insert(this.currencySequence, num, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(transform, this.initialScale, num), 24));
			Vector3 vector2 = this.initiaLocalPos + this.PosOffset;
			TweenSettingsExtensions.Insert(this.currencySequence, 0f, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(transform, vector2, num, false), 6));
			TweenSettingsExtensions.Insert(this.currencySequence, num, TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(transform, this.initiaLocalPos, num, false), 24));
		}

		public CustomText Text;

		private Vector3 initialScale;

		private Vector3 initiaLocalPos;

		private Sequence currencySequence;

		private long currentValue;
	}
}
