using System;
using System.Collections.Generic;
using Dxx.Guild;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Component;
using Framework.SocketNet;
using Framework.ViewModule;
using Google.Protobuf;
using HotFix;
using LocalModels;
using LocalModels.Bean;
using Newtonsoft.Json;
using Proto.Chat;
using Proto.Common;
using Proto.Guild;
using UnityEngine;

namespace Dxx.Chat
{
	public class ChatProxy
	{
		public abstract class ChatProxy_BaseBehaviour : CustomBehaviour
		{
			protected sealed override void OnInit()
			{
				this.ChatUI_OnInit();
			}

			protected sealed override void OnDeInit()
			{
				this.ChatUI_OnUnInit();
			}

			protected abstract void ChatUI_OnInit();

			protected abstract void ChatUI_OnUnInit();

			public bool IsActive()
			{
				return base.gameObject != null && base.gameObject.activeInHierarchy;
			}
		}

		internal class ChatProxy_BaseView : BaseViewModule
		{
			public sealed override void OnCreate(object data)
			{
				this.OnViewCreate();
			}

			public sealed override void OnOpen(object data)
			{
				this.OnViewOpen(data);
			}

			public sealed override void OnUpdate(float deltaTime, float unscaledDeltaTime)
			{
			}

			public sealed override void OnClose()
			{
				this.OnViewClose();
			}

			public sealed override void OnDelete()
			{
				this.OnViewDelete();
			}

			public sealed override void RegisterEvents(EventSystemManager manager)
			{
			}

			public sealed override void UnRegisterEvents(EventSystemManager manager)
			{
			}

			protected virtual void OnViewCreate()
			{
			}

			protected virtual void OnViewOpen(object data)
			{
			}

			protected virtual void OnViewUpdate(float deltaTime, float unscaledDeltaTime)
			{
			}

			protected virtual void OnViewClose()
			{
			}

			protected virtual void OnViewDelete()
			{
			}
		}

		public class Common
		{
			public static void OnRecvGuildChatRecords(ChatGroupData group, ChatGetMessageRecordsResponse resp)
			{
				if (group != null)
				{
					List<PushMessageDto> list = new List<PushMessageDto>();
					if (resp.MessageRecords.Count > 0)
					{
						list.AddRange(resp.MessageRecords);
					}
					List<ChatData> list2 = new List<ChatData>();
					foreach (PushMessageDto pushMessageDto in list)
					{
						switch (pushMessageDto.MessageType)
						{
						case 201U:
						case 203U:
						{
							ChatData chatData = ChatManager.JsonStringToChatData(pushMessageDto.MessageContent);
							if (chatData != null)
							{
								chatData.msgSeq = (long)pushMessageDto.MessageSeq;
								list2.Add(chatData);
							}
							break;
						}
						case 204U:
						{
							ChatData chatData2 = ChatManager.JsonStringToChatData(pushMessageDto.MessageContent);
							if (chatData2 != null)
							{
								chatData2.msgSeq = (long)pushMessageDto.MessageSeq;
								list2.Add(chatData2);
							}
							break;
						}
						}
					}
					if (list2.Count > 0)
					{
						group.AddPage();
						group.AddNewChatList(list2);
					}
				}
			}
		}

		public class GuildChat
		{
			public static GuildSDKManager GuildSDK()
			{
				return GuildSDKManager.Instance;
			}

			public static string GetGuildID()
			{
				GuildSDKManager guildSDKManager = ChatProxy.GuildChat.GuildSDK();
				if (guildSDKManager.GuildInfo.HasGuild)
				{
					return guildSDKManager.GuildInfo.GuildID;
				}
				return "";
			}

			public static string GetGuildChatGroupId()
			{
				GuildSDKManager guildSDKManager = ChatProxy.GuildChat.GuildSDK();
				if (guildSDKManager.GuildInfo.HasGuild)
				{
					return guildSDKManager.GuildInfo.IMGroupId;
				}
				return "";
			}

			public static ChatData GetMyDonationMessage()
			{
				GuildSDKManager guildSDKManager = ChatProxy.GuildChat.GuildSDK();
				if (!guildSDKManager.GuildInfo.HasGuild)
				{
					return null;
				}
				string guildID = guildSDKManager.GuildInfo.GuildID;
				ChatDataCtrlRule chatDataCtrlRule = Singleton<ChatManager>.Instance.GetGroup(SocketGroupType.GUILD, guildID).GetChatDataCtrlRule(ChatDataRuleKind.GuildDonation);
				if (chatDataCtrlRule == null)
				{
					return null;
				}
				ulong myUserID = ChatProxy.User.GetMyUserID();
				return chatDataCtrlRule.GetChatByUserID(myUserID);
			}
		}

		public class JSON
		{
			public static T ToObject<T>(string json)
			{
				return JsonConvert.DeserializeObject<T>(json);
			}
		}

		public class Language
		{
			private static LanguageManager _Lan
			{
				get
				{
					return Singleton<LanguageManager>.Instance;
				}
			}

			private static void DispatchEvent(int id, object obj)
			{
				GameApp.Event.DispatchNow(null, id, obj as BaseEventArgs);
			}

			public static int GetCurrentLanguage()
			{
				return ChatProxy.Language._Lan.GameLanguage;
			}

			public static string GetInfoByID_LogError(int id)
			{
				return ChatProxy.Language._Lan.GetInfoByID_LogError(id);
			}

			public static string GetInfoByID1_LogError(int id, object obj)
			{
				return ChatProxy.Language._Lan.GetInfoByID_LogError(id, new object[] { obj });
			}

			public static string GetInfoByID2_LogError(int id, object obj1, object obj2)
			{
				return ChatProxy.Language._Lan.GetInfoByID_LogError(id, new object[] { obj1, obj2 });
			}

			public static string GetInfoByID3_LogError(int id, object obj1, object obj2, object obj3)
			{
				return ChatProxy.Language._Lan.GetInfoByID_LogError(id, new object[] { obj1, obj2, obj3 });
			}

			public static string GetInfoByID4_LogError(int id, object obj1, object obj2, object obj3, object obj4)
			{
				return ChatProxy.Language._Lan.GetInfoByID_LogError(id, new object[] { obj1, obj2, obj3, obj4 });
			}

			public static string GetInfoByID(string id)
			{
				return ChatProxy.Language._Lan.GetInfoByID(id);
			}

			public static string GetInfoByID1(string id, object obj)
			{
				return ChatProxy.Language._Lan.GetInfoByID(id, new object[] { obj });
			}

			public static string GetInfoByID2(string id, object obj1, object obj2)
			{
				return ChatProxy.Language._Lan.GetInfoByID(id, new object[] { obj1, obj2 });
			}

			public static string GetInfoByID3(string id, object obj1, object obj2, object obj3)
			{
				return ChatProxy.Language._Lan.GetInfoByID(id, new object[] { obj1, obj2, obj3 });
			}

			public static string GetInfoByID4(string id, object obj1, object obj2, object obj3, object obj4)
			{
				return ChatProxy.Language._Lan.GetInfoByID(id, new object[] { obj1, obj2, obj3, obj4 });
			}

			public static string GetNickName(string nick, ulong userid)
			{
				if (string.IsNullOrEmpty(nick))
				{
					return DxxTools.GetDefaultNick((long)userid);
				}
				return nick;
			}

			public static void TipsItemNotEnough()
			{
				GameApp.View.ShowStringTip(ChatProxy.Language.GetInfoByID_LogError(1505));
			}

			[Obsolete("多语言待配表:发送消息的频率过快")]
			public static void TipsChatSendTooFrequently()
			{
				GameApp.View.ShowStringTip(ChatProxy.Language.GetInfoByID("5103"));
			}
		}

		public class Net
		{
			public static ISocketNet SocketNet
			{
				get
				{
					return GameApp.SocketNet;
				}
			}

			public static ISocketGameLogicProxy SocketNetProxy
			{
				get
				{
					return GameApp.SocketNet.GameProxy as ISocketGameLogicProxy;
				}
			}

			public static void TrySendMessage(IMessage req, Action<IMessage> callBack, bool isShowMask, bool isCache)
			{
				GuildProxy.Net.TrySendMessage(req, callBack, isShowMask, isCache, true);
			}

			public static CommonParams GetCommonParams()
			{
				return NetworkUtils.GetCommonParams();
			}

			public static void ReqGuildDonationReqItem(int itemid, Action<bool, GuildDonationReqItemResponse> callback)
			{
				HLog.LogError("未完成：发送捐赠请求！！！");
			}

			public static void GetGuildDonationGetRecords(ulong msgid, uint page, Action<bool, GuildDonationGetRecordsResponse> callBack)
			{
				HLog.LogError("未完成：收取捐赠请求！！！");
			}

			public static void GetGuildDonationGetOperationRecords(Action<bool, GuildDonationGetOperationRecordsResponse> callBack)
			{
				HLog.LogError("未完成：捐赠记录！！！");
			}
		}

		public class Res
		{
		}

		public class SceneChat
		{
			public static string GetGroupID()
			{
				return GameApp.Data.GetDataModule(DataName.LoginDataModule).LocalIMGroupId;
			}

			public static void CheckGetChatHistoryWhenEmpty()
			{
				string groupID = ChatProxy.SceneChat.GetGroupID();
				if (string.IsNullOrEmpty(groupID))
				{
					return;
				}
				ChatGroupData group = Singleton<ChatManager>.Instance.GetGroup(SocketGroupType.SCENE, groupID);
				if (group == null || group.ChatMessageList.Count > 0)
				{
					return;
				}
				ISocketGameLogicProxy socketNetProxy = ChatProxy.Net.SocketNetProxy;
				if (socketNetProxy == null)
				{
					return;
				}
				socketNetProxy.GetMessageHistory(0L, SocketGroupType.SCENE, groupID, null);
			}
		}

		public class ServerChat
		{
			public static string GetGroupID()
			{
				return GameApp.Data.GetDataModule(DataName.LoginDataModule).ServerIMGroupId;
			}

			public static void CheckGetChatHistoryWhenEmpty()
			{
				string groupID = ChatProxy.ServerChat.GetGroupID();
				if (string.IsNullOrEmpty(groupID))
				{
					return;
				}
				ChatGroupData group = Singleton<ChatManager>.Instance.GetGroup(SocketGroupType.Server, groupID);
				if (group == null || group.ChatMessageList.Count > 0)
				{
					return;
				}
				ISocketGameLogicProxy socketNetProxy = ChatProxy.Net.SocketNetProxy;
				if (socketNetProxy == null)
				{
					return;
				}
				socketNetProxy.GetMessageHistory(0L, SocketGroupType.Server, groupID, null);
			}
		}

		public class Table
		{
			private static LocalModelManager _Table
			{
				get
				{
					return GameApp.Table.GetManager();
				}
			}

			public static Emoji_Emoji GetEmojiTab(int id)
			{
				return ChatProxy.Table._Table.GetEmoji_EmojiModelInstance().GetElementById(id);
			}

			public static List<Emoji_Emoji> GetAllEmojiTab()
			{
				List<Emoji_Emoji> list = new List<Emoji_Emoji>();
				IList<Emoji_Emoji> allElements = ChatProxy.Table._Table.GetEmoji_EmojiModelInstance().GetAllElements();
				list.AddRange(allElements);
				return list;
			}

			public static Equip_equip GetEquipTab(int id)
			{
				return ChatProxy.Table._Table.GetEquip_equipModelInstance().GetElementById(id);
			}

			public static Guild_guildDonation GetDonationTable(int id)
			{
				return ChatProxy.Table._Table.GetGuild_guildDonationModelInstance().GetElementById(id);
			}

			public static List<Guild_guildDonation> GetAllDonationTab()
			{
				List<Guild_guildDonation> list = new List<Guild_guildDonation>();
				IList<Guild_guildDonation> allElements = ChatProxy.Table._Table.GetGuild_guildDonationModelInstance().GetAllElements();
				list.AddRange(allElements);
				return list;
			}

			public static Item_Item GetItemTab(int id)
			{
				return ChatProxy.Table._Table.GetItem_ItemModelInstance().GetElementById(id);
			}
		}

		public class UI
		{
			private static ViewModuleManager _UI
			{
				get
				{
					return GameApp.View;
				}
			}

			public static void CloseChatView()
			{
				ChatProxy.UI._UI.CloseView(ViewName.GuildChatViewModule, null);
			}

			public static void OpenChatShareView(object data = null)
			{
				ChatProxy.UI._UI.OpenView(ViewName.ChatShareItem, data, 1, null, null);
			}

			public static void CloseChatShareView()
			{
				ChatProxy.UI._UI.CloseView(ViewName.ChatShareItem, null);
			}

			public static void OpenChatDonationItemView(object data = null)
			{
				ChatProxy.UI._UI.OpenView(ViewName.ChatDonationItem, data, 1, null, null);
			}

			public static void CloseChatDonationItemView()
			{
				ChatProxy.UI._UI.CloseView(ViewName.ChatDonationItem, null);
			}

			public static void OpenChatDonationView(object data = null)
			{
				ChatProxy.UI._UI.OpenView(ViewName.ChatDonation, data, 1, null, null);
			}

			public static void CloseChatDonationView()
			{
				ChatProxy.UI._UI.CloseView(ViewName.ChatDonation, null);
			}

			public static void OpenChatDonationRecordView(object data = null)
			{
				ChatProxy.UI._UI.OpenView(ViewName.ChatDonationRecord, data, 1, null, null);
			}

			public static void CloseChatDonationRecordView()
			{
				ChatProxy.UI._UI.CloseView(ViewName.ChatDonationRecord, null);
			}

			public static List<ChatShareItemData> BuildChatShareItemDataList()
			{
				List<ChatShareItemData> list = new List<ChatShareItemData>();
				list.Sort(new Comparison<ChatShareItemData>(ChatShareItemData.Sort));
				return list;
			}

			public static List<ChatShareItemData> BuildChatShareEquipDataList()
			{
				List<ChatShareItemData> list = new List<ChatShareItemData>();
				list.Sort(new Comparison<ChatShareItemData>(ChatShareItemData.Sort));
				return list;
			}

			public static List<ChatShareItemData> BuildChatSharePetDataList()
			{
				List<ChatShareItemData> list = new List<ChatShareItemData>();
				list.Sort(new Comparison<ChatShareItemData>(ChatShareItemData.Sort));
				return list;
			}

			public static List<ChatShareItemData> BuildChatShareHeroDataList()
			{
				List<ChatShareItemData> list = new List<ChatShareItemData>();
				list.Sort(new Comparison<ChatShareItemData>(ChatShareItemData.Sort));
				return list;
			}

			private static bool IsInCondition(Guild_guildDonation tab)
			{
				return true;
			}

			public static List<ChatDonationItemData> BuildChatDonationItemDataList()
			{
				List<ChatDonationItemData> list = new List<ChatDonationItemData>();
				List<Guild_guildDonation> allDonationTab = ChatProxy.Table.GetAllDonationTab();
				for (int i = 0; i < allDonationTab.Count; i++)
				{
					Guild_guildDonation guild_guildDonation = allDonationTab[i];
					if (ChatProxy.UI.IsInCondition(guild_guildDonation))
					{
						ChatDonationItemData chatDonationItemData = new ChatDonationItemData
						{
							ItemID = guild_guildDonation.ID,
							Count = guild_guildDonation.Count
						};
						list.Add(chatDonationItemData);
					}
				}
				return list;
			}
		}

		public abstract class ChatUIBase : BaseViewModule
		{
			public GameObject ViewGObj
			{
				get
				{
					return base.gameObject;
				}
			}

			public sealed override void OnCreate(object data)
			{
				this.OnViewCreate();
			}

			protected virtual void OnViewCreate()
			{
			}

			public sealed override void OnOpen(object data)
			{
				this.OnViewOpen(data);
			}

			protected virtual void OnViewOpen(object data)
			{
			}

			public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
			{
			}

			public sealed override void OnClose()
			{
				this.OnViewClose();
			}

			protected virtual void OnViewClose()
			{
			}

			public override void OnDelete()
			{
				this.OnViewDelete();
			}

			protected virtual void OnViewDelete()
			{
			}

			public override void RegisterEvents(EventSystemManager manager)
			{
			}

			public override void UnRegisterEvents(EventSystemManager manager)
			{
			}
		}

		public class User
		{
			public static ulong GetMyUserID()
			{
				return (ulong)GameApp.Data.GetDataModule(DataName.LoginDataModule).userId;
			}

			public static int GetBagItemCount(int itemid)
			{
				return 0;
			}
		}
	}
}
