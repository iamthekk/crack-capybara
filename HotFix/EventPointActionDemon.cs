using System;
using DG.Tweening;
using Spine.Unity;

namespace HotFix
{
	public class EventPointActionDemon : EventPointActionNormal
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
			{
				string text = "Appear";
				this.spineAni.AnimationState.SetAnimation(0, text, false);
				this.spineAni.AnimationState.Update(0f);
				this.spineAni.AnimationState.Apply(this.spineAni.Skeleton);
				this.spineAni.timeScale = 0f;
				return;
			}
			case 2:
			{
				string text2 = "Appear";
				this.spineAni.timeScale = 1f;
				this.animator.PlayAni(text2, false);
				this.animator.AddAni("Idle", true, 0f);
				float animationDuration = this.animator.GetAnimationDuration(text2);
				Sequence sequence = this.sequencePool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, animationDuration);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					Singleton<GameEventController>.Instance.PushEvent(GameEventPushType.SurpriseNpcActionFinish, null);
				});
				return;
			}
			case 3:
			{
				string text3 = "Disappear";
				float animationDuration2 = this.animator.GetAnimationDuration(text3);
				this.animator.PlayAni(text3, false);
				Sequence sequence2 = this.sequencePool.Get();
				TweenSettingsExtensions.AppendInterval(sequence2, animationDuration2);
				TweenSettingsExtensions.AppendCallback(sequence2, delegate
				{
					this.spineAni.gameObject.SetActiveSafe(false);
				});
				return;
			}
			case 4:
				this.animator.PlayAni("Choose", false);
				this.animator.AddAni("Idle", true, 0f);
				return;
			default:
				return;
			}
		}

		public SkeletonAnimation spineAni;

		private SpineAnimator animator;

		private readonly SequencePool sequencePool = new SequencePool();
	}
}
