using System;
using Spine.Unity;

namespace HotFix
{
	public class EventPointActionNiaoChao : EventPointActionNormal
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
			if (actionId == 1)
			{
				this.animator.PlayAni("Idle", true);
				return;
			}
			if (actionId != 2)
			{
				return;
			}
			this.animator.PlayAni("Appear", false);
			this.animator.AddAni("Appear_Idle", true, 0f);
		}

		public SkeletonAnimation spineAni;

		private SpineAnimator animator;

		private readonly SequencePool sequencePool = new SequencePool();
	}
}
