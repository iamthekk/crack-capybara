using System;
using Framework.EventSystem;
using Framework.ViewModule;
using HotFix.CrossArenaRecordUI;

namespace HotFix
{
	public class CrossArenaRecordViewModule : BaseViewModule
	{
		public ViewName GetName()
		{
			return ViewName.CrossArenaRecordViewModule;
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
		}

		public override void OnDelete()
		{
			CrossArenaRecordViewCtrl crossArenaRecordViewCtrl = this.mCtrl;
			if (crossArenaRecordViewCtrl != null)
			{
				crossArenaRecordViewCtrl.DeInit();
			}
			this.mCtrl = null;
		}

		public CrossArenaRecordViewCtrl mCtrl;
	}
}
