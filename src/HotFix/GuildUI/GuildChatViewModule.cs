using System;
using Dxx.Chat;
using Framework.EventSystem;
using Framework.Logic.UI;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	internal class GuildChatViewModule : GuildProxy.GuildProxy_BaseView
	{
		protected override void OnViewCreate()
		{
			this.chatCtrl.Init();
			this.buttonClose.onClick.AddListener(new UnityAction(this.CloseThisView));
		}

		protected override void OnViewOpen(object data)
		{
			GuildProxy.GameEvent.AddReceiver(LocalMessageName.CC_Guild_Leave, new HandlerEvent(this.OnEvent));
			this.chatCtrl.ShowAni();
			this.chatCtrl.Show();
		}

		protected override void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
		{
			GuildChatViewChatCtrl guildChatViewChatCtrl = this.chatCtrl;
			if (guildChatViewChatCtrl == null)
			{
				return;
			}
			guildChatViewChatCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void OnViewClose()
		{
			CustomButton customButton = this.buttonClose;
			if (customButton != null)
			{
				customButton.onClick.AddListener(new UnityAction(this.CloseThisView));
			}
			GuildProxy.GameEvent.RemoveReceiver(LocalMessageName.CC_Guild_Leave, new HandlerEvent(this.OnEvent));
			this.chatCtrl.Close();
		}

		protected override void OnViewDelete()
		{
			if (this.chatCtrl != null)
			{
				this.chatCtrl.DeInit();
			}
		}

		public void CloseThisView()
		{
			ChatProxy.UI.CloseChatView();
		}

		private void OnEvent(object sender, int type, BaseEventArgs eventArgs)
		{
		}

		public CustomButton buttonClose;

		public GuildChatViewChatCtrl chatCtrl;
	}
}
