using System;
using DG.Tweening;
using Spine.Unity;

namespace HotFix
{
	public class EventPointActionArena : EventPointActionNormal
	{
		protected override void InitPoint()
		{
			base.InitPoint();
			if (this.animatorA == null)
			{
				this.animatorA = new SpineAnimator(this.spineAniA);
			}
			if (this.animatorB == null)
			{
				this.animatorB = new SpineAnimator(this.spineAniB);
			}
		}

		protected override void DeInitPoint()
		{
			base.DeInitPoint();
			this.sequencePool.Clear(false);
		}

		protected override void DoAction(int actionId)
		{
			if (this.animatorA == null || this.animatorB == null)
			{
				return;
			}
			this.sequencePool.Clear(false);
			switch (actionId)
			{
			case 1:
			{
				this.animatorA.PlayAni("Attack", true);
				Sequence sequence = this.sequencePool.Get();
				TweenSettingsExtensions.AppendInterval(sequence, 0.1f);
				TweenSettingsExtensions.AppendCallback(sequence, delegate
				{
					this.animatorB.PlayAni("Attack", true);
				});
				return;
			}
			case 2:
			{
				this.animatorA.PlayAni("Skill", false);
				this.animatorA.AddAni("Idle", true, 0f);
				this.animatorB.PlayAni("Hit", false);
				this.animatorB.AddAni("Death", false, 0f);
				float animationDuration = this.animatorB.GetAnimationDuration("Hit");
				float animationDuration2 = this.animatorB.GetAnimationDuration("Death");
				Sequence sequence2 = this.sequencePool.Get();
				TweenSettingsExtensions.AppendInterval(sequence2, animationDuration + animationDuration2);
				TweenSettingsExtensions.AppendCallback(sequence2, delegate
				{
					ColorRenderCtrl component = this.spineAniB.GetComponent<ColorRenderCtrl>();
					if (component)
					{
						component.HideAni(delegate
						{
							this.spineAniB.gameObject.SetActiveSafe(false);
						});
					}
				});
				return;
			}
			case 3:
			{
				this.animatorB.PlayAni("Skill", false);
				this.animatorB.AddAni("Idle", true, 0f);
				this.animatorA.PlayAni("Hit", false);
				this.animatorA.AddAni("Death", false, 0f);
				float animationDuration3 = this.animatorA.GetAnimationDuration("Hit");
				float animationDuration4 = this.animatorA.GetAnimationDuration("Death");
				Sequence sequence3 = this.sequencePool.Get();
				TweenSettingsExtensions.AppendInterval(sequence3, animationDuration3 + animationDuration4);
				TweenSettingsExtensions.AppendCallback(sequence3, delegate
				{
					ColorRenderCtrl component2 = this.spineAniA.GetComponent<ColorRenderCtrl>();
					if (component2)
					{
						component2.HideAni(delegate
						{
							this.spineAniA.gameObject.SetActiveSafe(false);
						});
					}
				});
				return;
			}
			default:
				return;
			}
		}

		public SkeletonAnimation spineAniA;

		public SkeletonAnimation spineAniB;

		private SpineAnimator animatorA;

		private SpineAnimator animatorB;

		private readonly SequencePool sequencePool = new SequencePool();
	}
}
