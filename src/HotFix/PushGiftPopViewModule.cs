using System;
using Framework;
using Framework.EventSystem;
using Framework.ViewModule;
using UnityEngine;

namespace HotFix
{
	public class PushGiftPopViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.instantiatedCtrl.Init();
		}

		public override void OnOpen(object data)
		{
			GameApp.Sound.PlayClip(665, 1f);
			this._pushGiftData = data as PushGiftData;
			if (this._pushGiftData == null)
			{
				this.CloseSelf();
				return;
			}
			this.instantiatedCtrl.SetData(this._pushGiftData, new Action(this.CloseSelf));
		}

		public override void OnClose()
		{
			if (this.instantiatedCtrl != null)
			{
				this.instantiatedCtrl.OnHide();
			}
		}

		public override void OnDelete()
		{
			this.instantiatedCtrl.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void CloseSelf()
		{
			GameApp.View.CloseView(ViewName.PushGiftPopViewModule, null);
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			this.instantiatedCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		[SerializeField]
		private PushGiftPopCtrlBase instantiatedCtrl;

		private PushGiftData _pushGiftData;
	}
}
