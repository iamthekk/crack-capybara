using System;
using Framework;
using Framework.EventSystem;
using Framework.ViewModule;
using HotFix.VIPUI;

namespace HotFix
{
	public class VIPViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.VIPInfoUI.Init();
			this.ProgressUI.Init();
			this.PopCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnPopClick);
		}

		public override void OnOpen(object data)
		{
			VIPViewModule.OpenData openData = data as VIPViewModule.OpenData;
			if (openData != null)
			{
				this.mOpenData = openData;
			}
			if (this.mOpenData == null)
			{
				this.mOpenData = new VIPViewModule.OpenData();
				this.mOpenData.VIPLevel = GameApp.Data.GetDataModule(DataName.VIPDataModule).VipLevel;
			}
			this.VIPInfoUI.OnViewOpen(this.mOpenData.VIPLevel);
			this.RefreshUI();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void OnClose()
		{
			this.VIPInfoUI.OnViewClose();
		}

		public override void OnDelete()
		{
			VIPInfoUI vipinfoUI = this.VIPInfoUI;
			if (vipinfoUI != null)
			{
				vipinfoUI.DeInit();
			}
			VIPProgressUI progressUI = this.ProgressUI;
			if (progressUI == null)
			{
				return;
			}
			progressUI.DeInit();
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void RefreshUI()
		{
			VIPInfoUI vipinfoUI = this.VIPInfoUI;
			if (vipinfoUI != null)
			{
				vipinfoUI.RefreshUI();
			}
			VIPProgressUI progressUI = this.ProgressUI;
			if (progressUI == null)
			{
				return;
			}
			progressUI.RefreshUI();
		}

		private void OnClickCloseThis()
		{
			GameApp.View.CloseView(ViewName.VIPViewModule, null);
		}

		private void OnPopClick(UIPopCommon.UIPopCommonClickType type)
		{
			this.OnClickCloseThis();
		}

		public VIPInfoUI VIPInfoUI;

		public VIPProgressUI ProgressUI;

		public UIPopCommon PopCommon;

		private VIPViewModule.OpenData mOpenData;

		public class OpenData
		{
			public int VIPLevel;
		}
	}
}
