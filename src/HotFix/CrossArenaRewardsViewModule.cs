using System;
using Framework.EventSystem;
using Framework.ViewModule;
using HotFix.CrossArenaRewardsUI;

namespace HotFix
{
	public class CrossArenaRewardsViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.CrossArenaRewardsViewModule;
		}

		public override void OnCreate(object data)
		{
			this.mCtrl.Init();
			this.mCtrl.SetView(this);
		}

		public override void OnOpen(object data)
		{
			CrossArenaRewardsViewCtrl crossArenaRewardsViewCtrl = this.mCtrl;
			if (crossArenaRewardsViewCtrl == null)
			{
				return;
			}
			crossArenaRewardsViewCtrl.RefreshOnOpen();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			CrossArenaRewardsViewCtrl crossArenaRewardsViewCtrl = this.mCtrl;
			if (crossArenaRewardsViewCtrl == null)
			{
				return;
			}
			crossArenaRewardsViewCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
		}

		public override void OnDelete()
		{
			CrossArenaRewardsViewCtrl crossArenaRewardsViewCtrl = this.mCtrl;
			if (crossArenaRewardsViewCtrl != null)
			{
				crossArenaRewardsViewCtrl.DeInit();
			}
			this.mCtrl = null;
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public CrossArenaRewardsViewCtrl mCtrl;
	}
}
