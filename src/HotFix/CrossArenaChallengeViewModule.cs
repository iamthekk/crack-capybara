using System;
using Framework.EventSystem;
using Framework.ViewModule;
using HotFix.CrossArenaChallengeUI;

namespace HotFix
{
	public class CrossArenaChallengeViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.CrossArenaChallengeViewModule;
		}

		public override void OnCreate(object data)
		{
			this.mCtrl.SetView(this);
			this.mCtrl.Init();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.mCtrl.RegisterEvents(manager);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.mCtrl.UnRegisterEvents(manager);
		}

		public override void OnOpen(object data)
		{
			this.mCtrl.OnViewOpen(data);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.mCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnClose()
		{
			this.mCtrl.OnViewClose();
		}

		public override void OnDelete()
		{
			CrossArenaChallengeViewCtrl crossArenaChallengeViewCtrl = this.mCtrl;
			if (crossArenaChallengeViewCtrl != null)
			{
				crossArenaChallengeViewCtrl.DeInit();
			}
			this.mCtrl = null;
		}

		public CrossArenaChallengeViewCtrl mCtrl;
	}
}
