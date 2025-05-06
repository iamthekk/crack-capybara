using System;
using Dxx.Chat;
using Dxx.Guild;
using HotFix.ChatUI;
using Proto.Chat;

namespace HotFix.GuildUI
{
	public class GuildChatViewChatCtrl : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.ChatPanel.Init();
			this.ChatPanel.OutputUI.HistoryRequire = new GuildChatViewChatCtrl.GuildHistoryRequire();
			this.ChatPanel.InputUI.SendInterval = (float)Singleton<GameConfig>.Instance.GuildChatInterval;
			this.ChatPanel.InputUI.MaxSendCharCount = Singleton<GameConfig>.Instance.ChatCharCount;
		}

		protected override void GuildUI_OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			ChatCustomPanel chatPanel = this.ChatPanel;
			if (chatPanel == null)
			{
				return;
			}
			chatPanel.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		protected override void GuildUI_OnUnInit()
		{
			ChatCustomPanel chatPanel = this.ChatPanel;
			if (chatPanel == null)
			{
				return;
			}
			chatPanel.DeInit();
		}

		protected override void GuildUI_OnShow()
		{
			base.gameObject.SetActiveSafe(true);
			this.ChatPanel.OnShow();
			this.ChatPanel.SetChatGroup(SocketGroupType.GUILD, ChatProxy.GuildChat.GetGuildChatGroupId());
		}

		protected override void GuildUI_OnClose()
		{
			this.ChatPanel.OnClose();
			base.gameObject.SetActiveSafe(false);
		}

		public void ShowAni()
		{
		}

		public ChatCustomPanel ChatPanel;

		private class GuildHistoryRequire : ChatHistoryMessageRequire
		{
			public override void RequireHistory(ChatData chat, Action<bool> callback)
			{
				long num = ((chat != null) ? ((chat.msgId != 0L) ? chat.msgId : chat.UIBindMsgID) : 0L);
				ISocketGameLogicProxy socketNetProxy = ChatProxy.Net.SocketNetProxy;
				if (socketNetProxy == null)
				{
					return;
				}
				socketNetProxy.GetMessageHistory(num, SocketGroupType.GUILD, GuildSDKManager.Instance.GuildInfo.IMGroupId, delegate(bool result, ChatGetMessageRecordsResponse resp)
				{
					Action<bool> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(result);
				});
			}
		}
	}
}
