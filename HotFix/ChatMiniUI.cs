using System;
using System.Collections.Generic;
using Dxx.Chat;
using Dxx.Guild;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.Logic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix
{
	public class ChatMiniUI : CustomBehaviour
	{
		private static List<ChatShowKind> ChatMiniShowChatShowKind { get; } = new List<ChatShowKind>
		{
			ChatShowKind.CustomText,
			ChatShowKind.Emoji,
			ChatShowKind.SystemTips,
			ChatShowKind.MultipleSystemTips
		};

		private static ChatData GetEmptyChat()
		{
			return new ChatData
			{
				ShowKind = ChatShowKind.EmptyList
			};
		}

		public Action<ChatHomeSubPanelType> OnClickViewArea { get; set; }

		protected override void OnInit()
		{
			this.curPanelType = ChatHomeSubPanelType.Scene;
			this.chatButton.onClick.AddListener(new UnityAction(this.OnOpenChatUI));
			this.sceneChatGroup.Init();
			this.guildChatGroup.Init();
			this.sceneChatGroup.onChatChange = new Action(this.OnChatChange);
			this.guildChatGroup.onChatChange = new Action(this.OnChatChange);
			this.chatItem.Init();
			this.guildChatGroup.SetActive(SocketGroupType.GUILD, ChatProxy.GuildChat.GetGuildChatGroupId(), new Action(GuildProxy.Chat.CheckGetChatHistoryWhenEmpty));
			this.sceneChatGroup.SetActive(SocketGroupType.Server, ChatProxy.ServerChat.GetGroupID(), new Action(ChatProxy.ServerChat.CheckGetChatHistoryWhenEmpty));
			this.sceneChatGroup.SetActive(SocketGroupType.SCENE, ChatProxy.SceneChat.GetGroupID(), new Action(ChatProxy.SceneChat.CheckGetChatHistoryWhenEmpty));
			GuildSDKManager.Instance.Event.RegisterEvent(10, new GuildHandlerEvent(this.OnGuildLoginSuccess));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventChangeLanguage));
		}

		protected override void OnDeInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventChangeLanguage));
			this.chatButton.onClick.RemoveListener(new UnityAction(this.OnOpenChatUI));
			this.sceneChatGroup.SetUnActive();
			this.guildChatGroup.SetUnActive();
			this.sceneChatGroup.DeInit();
			this.guildChatGroup.DeInit();
			this.chatItem.DeInit();
			GuildSDKManager.Instance.Event.UnRegisterEvent(10, new GuildHandlerEvent(this.OnGuildLoginSuccess));
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public void OnShow()
		{
			this.RefreshAllUI();
		}

		public void OnHide()
		{
		}

		private void OnChatChange()
		{
			ChatData chatData = this.sceneChatGroup.CurShowChat;
			ChatData chatData2 = this.guildChatGroup.CurShowChat;
			if (chatData.timestamp >= chatData2.timestamp)
			{
				this.curShowChat = chatData;
				this.curGroupNameKey = "chat_local_title";
				this.curPanelType = ChatHomeSubPanelType.Scene;
			}
			else
			{
				this.curShowChat = chatData2;
				this.curGroupNameKey = "5002";
				this.curPanelType = ChatHomeSubPanelType.Guide;
			}
			this.chatItem.SetData(this.curGroupNameKey, this.curShowChat);
		}

		private void RefreshAllUI()
		{
			this.OnChatChange();
		}

		private void OnOpenChatUI()
		{
			Action<ChatHomeSubPanelType> onClickViewArea = this.OnClickViewArea;
			if (onClickViewArea == null)
			{
				return;
			}
			onClickViewArea(this.curPanelType);
		}

		private void OnGuildLoginSuccess(int type, GuildBaseEvent eventArgs)
		{
			this.guildChatGroup.SetActive(SocketGroupType.GUILD, ChatProxy.GuildChat.GetGuildChatGroupId(), new Action(GuildProxy.Chat.CheckGetChatHistoryWhenEmpty));
			this.RefreshAllUI();
		}

		private void OnEventChangeLanguage(object sender, int type, BaseEventArgs eventargs)
		{
			this.RefreshAllUI();
		}

		[SerializeField]
		private CustomButton chatButton;

		[SerializeField]
		private ChatMiniItem chatItem;

		private readonly ChatMiniUI.ChatMiniGroup sceneChatGroup = new ChatMiniUI.ChatMiniGroup();

		private readonly ChatMiniUI.ChatMiniGroup guildChatGroup = new ChatMiniUI.ChatMiniGroup();

		private ChatData curShowChat;

		private string curGroupNameKey;

		private ChatHomeSubPanelType curPanelType;

		private class ChatMiniGroup : IChatGroupDataChange
		{
			public ChatData CurShowChat
			{
				get
				{
					return this.selfCurShowChat;
				}
				private set
				{
					this.selfCurShowChat = value ?? ChatMiniUI.GetEmptyChat();
				}
			}

			public void Init()
			{
				this.CurShowChat = null;
			}

			public void DeInit()
			{
				this.SetUnActive();
				this.onChatChange = null;
			}

			public void SetActive(SocketGroupType groupType, string groupId, Action tryRequestHisMess)
			{
				this.SetUnActive();
				this.chatGroup = Singleton<ChatManager>.Instance.GetGroup(groupType, groupId);
				if (this.chatGroup != null)
				{
					this.chatGroup.AddDataListener(this);
					if (tryRequestHisMess != null)
					{
						tryRequestHisMess();
					}
				}
				this.TryChangeChat(false);
			}

			public void SetUnActive()
			{
				if (this.chatGroup != null)
				{
					this.chatGroup.RemoveDataListener(this);
				}
				this.CurShowChat = null;
			}

			public void OnAddChatData(List<ChatData> chatdata)
			{
				this.TryChangeChat(false);
			}

			public void OnChangeChatData(List<ChatData> chatdata)
			{
				this.TryChangeChat(true);
			}

			public void OnDelChatData(List<ChatData> chatdata)
			{
				this.TryChangeChat(false);
			}

			private void TryChangeChat(bool isForce)
			{
				if (this.chatGroup == null)
				{
					if (this.CurShowChat.ShowKind != ChatShowKind.EmptyList)
					{
						this.CurShowChat = null;
						Action action = this.onChatChange;
						if (action == null)
						{
							return;
						}
						action();
					}
					return;
				}
				ChatData chatData = null;
				for (int i = 0; i < this.chatGroup.ChatMessageList.Count; i++)
				{
					ChatData chatData2 = this.chatGroup.ChatMessageList[i];
					if (ChatMiniUI.ChatMiniShowChatShowKind.Contains(chatData2.ShowKind))
					{
						chatData = chatData2;
						break;
					}
				}
				if (!isForce)
				{
					if (chatData == null)
					{
						if (this.CurShowChat.ShowKind != ChatShowKind.EmptyList)
						{
							this.CurShowChat = null;
							Action action2 = this.onChatChange;
							if (action2 == null)
							{
								return;
							}
							action2();
							return;
						}
					}
					else if (chatData.msgId != this.CurShowChat.msgId || chatData.ShowKind != this.CurShowChat.ShowKind)
					{
						this.CurShowChat = chatData;
						Action action3 = this.onChatChange;
						if (action3 == null)
						{
							return;
						}
						action3();
					}
					return;
				}
				this.CurShowChat = chatData;
				Action action4 = this.onChatChange;
				if (action4 == null)
				{
					return;
				}
				action4();
			}

			private ChatGroupData chatGroup;

			private ChatData selfCurShowChat;

			public Action onChatChange;
		}
	}
}
