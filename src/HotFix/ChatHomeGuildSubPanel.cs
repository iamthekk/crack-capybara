using System;
using Dxx.Chat;
using Dxx.Guild;
using HotFix.ChatUI;
using Proto.Chat;
using UnityEngine;

namespace HotFix
{
	public class ChatHomeGuildSubPanel : ChatHomeSubPanel
	{
		public override ChatHomeSubPanelType PanelType
		{
			get
			{
				return ChatHomeSubPanelType.Guide;
			}
		}

		protected override void OnPreInit()
		{
			this.chatPanel.Init();
			this.chatPanel.OutputUI.HistoryRequire = new ChatHomeGuildSubPanel.GuildHistoryRequire();
			this.chatPanel.InputUI.SendInterval = (float)Singleton<GameConfig>.Instance.GuildChatInterval;
			this.chatPanel.InputUI.MaxSendCharCount = Singleton<GameConfig>.Instance.ChatCharCount;
			this.chatPanel.gameObject.SetActive(false);
			Singleton<ChatManager>.Instance.OnJoinGroup += this.InstanceOnJoinGroup;
			Singleton<ChatManager>.Instance.OnQuitGroup += this.InstanceOnQuitGroup;
		}

		protected override void OnPreDeInit()
		{
			this.chatPanel.DeInit();
			Singleton<ChatManager>.Instance.OnJoinGroup -= this.InstanceOnJoinGroup;
			Singleton<ChatManager>.Instance.OnQuitGroup -= this.InstanceOnQuitGroup;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.chatPanel.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void OnResetListView()
		{
			base.OnResetListView();
			this.chatPanel.ResetListView();
		}

		protected override void OnSelect()
		{
			base.OnSelect();
			this.Refresh();
		}

		protected override void OnUnSelect()
		{
			base.OnUnSelect();
			this.chatPanel.OnClose();
		}

		public override void OnLanguageChange()
		{
			if (this.chatPanel != null && this.chatPanel.isActiveAndEnabled)
			{
				this.chatPanel.OnLanguageChange();
			}
		}

		private void InstanceOnJoinGroup(SocketGroupType arg1, string arg2)
		{
			this.Refresh();
		}

		private void InstanceOnQuitGroup(SocketGroupType arg1, string arg2)
		{
			this.Refresh();
		}

		private void Refresh()
		{
			this.chatPanel.OnClose();
			bool flag = GuildSDKManager.Instance.GuildInfo != null && GuildSDKManager.Instance.GuildInfo.HasGuild;
			this.guildEmpty.SetActive(!flag);
			this.chatPanel.gameObject.SetActive(flag);
			if (flag)
			{
				this.chatPanel.OnShow();
				this.chatPanel.SetChatGroup(SocketGroupType.GUILD, ChatProxy.GuildChat.GetGuildChatGroupId());
				return;
			}
			this.chatPanel.SetChatGroup(SocketGroupType.UNKNOW, string.Empty);
		}

		[SerializeField]
		private ChatCustomPanel chatPanel;

		[SerializeField]
		private GameObject guildEmpty;

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
