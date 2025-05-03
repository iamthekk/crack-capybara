﻿using System;
using Spine.Unity;

namespace HotFix
{
	public class EventPointActionShiLing : EventPointActionNormal
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
			switch (actionId)
			{
			case 1:
				this.animator.PlayAni("Idle", true);
				return;
			case 2:
				this.animator.PlayAni("Victory", false);
				this.animator.AddAni("Victory_Idle", true, 0f);
				return;
			case 3:
				this.animator.PlayAni("Fail", false);
				this.animator.AddAni("Fail_Idle", true, 0f);
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
