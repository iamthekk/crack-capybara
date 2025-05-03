using System;
using DG.Tweening;
using Spine.Unity;

namespace HotFix
{
	public class EventPointActionShiLaiMu : EventPointActionNormal
	{
		protected override void InitPoint()
		{
			base.InitPoint();
			if (this.animator == null)
			{
				this.animator = new SpineAnimator(this.spineAni);
			}
		}

		protected override void DeInitPoint()
		{
			base.DeInitPoint();
			this.sequencePool.Clear(false);
		}

		protected override void DoAction(int actionId)
		{
			if (this.animator == null || this.spineAni == null)
			{
				return;
			}
			this.sequencePool.Clear(false);
			switch (actionId)
			{
			case 1:
				this.animator.PlayAni("Idle", true);
				return;
			case 2:
				this.animator.PlayAni("Hit", false);
				this.animator.AddAni("Idle", true, 0f);
				return;
			case 3:
				this.animator.PlayAni("Jump", true);
				return;
			case 4:
			{
				string text = "Death";
				float animationDuration = this.animator.GetAnimationDuration(text);
				this.animator.PlayAni(text, false);
				Sequence sequence = this.sequencePool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, animationDuration);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.spineAni.gameObject.SetActiveSafe(false);
				});
				return;
			}
			default:
				return;
			}
		}

		public SkeletonAnimation spineAni;

		private SpineAnimator animator;

		private readonly SequencePool sequencePool = new SequencePool();
	}
}
