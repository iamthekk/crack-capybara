using System;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class GuildShopViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			base.OnViewCreate();
			this.shopPanel.isGuildShop = true;
			this.shopPanel.SetLoadObjects((base.Loader as GuildShopViewModuleLoader).m_loadPageObjects);
			this.shopPanel.Init();
			this.leaveButton.onClick.AddListener(new UnityAction(this.OnLeaveButtonClick));
		}

		private void OnLeaveButtonClick()
		{
			GameApp.View.CloseView(ViewName.GuildShopViewModule, null);
		}

		protected override void OnViewDelete()
		{
			base.OnViewDelete();
			this.shopPanel.DeInit();
			this.leaveButton.onClick.RemoveListener(new UnityAction(this.OnLeaveButtonClick));
		}

		protected override void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnViewUpdate(deltaTime, unscaledDeltaTime);
			this.shopPanel.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnViewOpen(object data)
		{
			base.OnViewOpen(data);
			this.shopPanel.Show(data);
			if (!GuildSDKManager.Instance.IsDataInitOver)
			{
				GuildNetUtil.Guild.DoRequest_GuildLoginRequest(null);
			}
		}

		protected override void OnViewClose()
		{
			base.OnViewClose();
			this.shopPanel.Hide();
		}

		public CustomButton leaveButton;

		public UIBlackMarketShop shopPanel;
	}
}
