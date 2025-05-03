using System;

namespace HotFix
{
	public abstract class GuideActionBase
	{
		public GuideData Guide { get; protected set; }

		public abstract void DoAction(GuideData gd);

		public virtual void DoActionAfterGuide(GuideData gd)
		{
		}

		protected void NormalError(string info)
		{
			if (!GuideController.ShowGuideLog)
			{
				return;
			}
			HLog.LogError("[新手引导][" + base.GetType().Name + "]自定义引导动作错误" + info);
		}

		protected virtual void OverThisGuide()
		{
		}
	}
}
