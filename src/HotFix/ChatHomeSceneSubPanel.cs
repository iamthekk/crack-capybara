using System;
using Dxx.Chat;
using HotFix.ChatUI;
using Proto.Chat;
using UnityEngine;

namespace HotFix
{
	public class ChatHomeSceneSubPanel : ChatHomeSubPanel
	{
		public override ChatHomeSubPanelType PanelType
		{
			get
			{
				return ChatHomeSubPanelType.Scene;
			}
		}

		protected override void OnPreInit()
		{
			this.chatPanel.Init();
			this.chatPanel.OutputUI.HistoryRequire = new ChatHomeSceneSubPanel.SceneHistoryRequire();
			this.chatPanel.InputUI.SendInterval = (float)Singleton<GameConfig>.Instance.SceneChatInterval;
			this.chatPanel.InputUI.MaxSendCharCount = Singleton<GameConfig>.Instance.ChatCharCount;
		}

		protected override void OnPreDeInit()
		{
			this.chatPanel.DeInit();
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

		public override void OnLanguageChange()
		{
			if (this.chatPanel != null && this.chatPanel.isActiveAndEnabled)
			{
				this.chatPanel.OnLanguageChange();
			}
		}

		protected override void OnSelect()
		{
			base.OnSelect();
			this.chatPanel.OnShow();
			this.chatPanel.SetChatGroup(SocketGroupType.SCENE, ChatProxy.SceneChat.GetGroupID());
		}

		protected override void OnUnSelect()
		{
			base.OnUnSelect();
			this.chatPanel.OnClose();
		}

		[SerializeField]
		private ChatCustomPanel chatPanel;

		private class SceneHistoryRequire : ChatHistoryMessageRequire
		{
			public override void RequireHistory(ChatData chat, Action<bool> callback)
			{
				long num = ((chat != null) ? ((chat.msgId != 0L) ? chat.msgId : chat.UIBindMsgID) : 0L);
				ISocketGameLogicProxy socketNetProxy = ChatProxy.Net.SocketNetProxy;
				if (socketNetProxy == null)
				{
					return;
				}
				socketNetProxy.GetMessageHistory(num, SocketGroupType.SCENE, ChatProxy.SceneChat.GetGroupID(), delegate(bool result, ChatGetMessageRecordsResponse resp)
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
