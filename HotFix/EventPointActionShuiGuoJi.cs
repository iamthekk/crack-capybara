using System;
using DG.Tweening;
using Spine.Unity;

namespace HotFix
{
	public class EventPointActionShuiGuoJi : EventPointActionNormal
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
			if (this.animator == null)
			{
				return;
			}
			this.sequencePool.Clear(false);
			if (actionId == 1)
			{
				this.animator.PlayAni("Idle", true);
				return;
			}
			if (actionId != 2)
			{
				return;
			}
			Sequence sequence = this.sequencePool.Get();
			TweenSettingsExtensions.AppendInterval(sequence, 0.5f);
			TweenSettingsExtensions.AppendCallback(sequence, delegate
			{
				Singleton<GameEventController>.Instance.PushEvent(GameEventPushType.SurpriseNpcActionFinish, null);
			});
		}

		public SkeletonAnimation spineAni;

		private SpineAnimator animator;

		private readonly SequencePool sequencePool = new SequencePool();
	}
}
