using System;
using Spine.Unity;

namespace HotFix
{
	public class EventPointActionNpcFadeIn : EventPointActionNormal
	{
		protected override void InitPoint()
		{
			base.InitPoint();
			if (this.animator == null)
			{
				this.animator = new SpineAnimator(this.spineAni);
			}
			if (this.spineAni)
			{
				this.renderCtrl = this.spineAni.GetComponent<ColorRenderCtrl>();
			}
		}

		protected override void DoAction(int actionId)
		{
			if (this.animator == null)
			{
				return;
			}
			if (actionId == 1)
			{
				if (this.renderCtrl)
				{
					this.renderCtrl.SetAlpha(0f);
				}
				this.animator.PlayAni("Idle", true);
				return;
			}
			if (actionId != 2)
			{
				return;
			}
			if (this.renderCtrl)
			{
				this.renderCtrl.ShowAni(null);
			}
		}

		public SkeletonAnimation spineAni;

		private SpineAnimator animator;

		private ColorRenderCtrl renderCtrl;
	}
}
