using System;
using System.Collections.Generic;
using System.Globalization;
using Dxx.Chat;
using Framework;
using Framework.EventSystem;
using Framework.Logic.Modules;
using Framework.SocketNet;
using HabbySDK.Llc;
using HabbySDK.Llc.Base;
using Newtonsoft.Json;
using Proto.Chat;
using Socket.Guild;

namespace HotFix
{
	public class HabbyIm_GameProxy : ISocketGameLogicProxy, ISocketGameProxyBase, ILlcActionListener
	{
		public void OnInit()
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameLogin_LoginData, new HandlerEvent(this.OnUserLogin));
			LlcMgr.Instance.SubscribeAction(new LlcNotificationType[] { 1, 2 }, this);
		}

		public void OnUnInit()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameLogin_LoginData, new HandlerEvent(this.OnUserLogin));
			LlcMgr.Instance.UnsubscribeAction(new LlcNotificationType[] { 1, 2 }, this);
		}

		public void SetSocketGroup(int kind, string groupName)
		{
			this.SetSocketGroup((SocketGroupType)kind, groupName);
		}

		public event Action<SocketGroupType, string> OnJoinGroup
		{
			add
			{
				if (value != null)
				{
					foreach (KeyValuePair<SocketGroupType, string> keyValuePair in this.mSocketGroupSet)
					{
						value(keyValuePair.Key, keyValuePair.Value);
					}
				}
				this.mOnJoinGroup.Add(value);
			}
			remove
			{
				this.mOnJoinGroup.Remove(value);
			}
		}

		public void AddJoinGroupHandler(Action<SocketGroupType, string> handler)
		{
			this.OnJoinGroup += handler;
		}

		public void RemoveJoinGroupHandler(Action<SocketGroupType, string> handler)
		{
			this.OnJoinGroup -= handler;
		}

		public event Action<SocketGroupType, string> OnQuitGroup
		{
			add
			{
				this.mOnQuitGroup.Add(value);
			}
			remove
			{
				this.mOnQuitGroup.Remove(value);
			}
		}

		public void AddQuitGroupHandler(Action<SocketGroupType, string> handler)
		{
			this.OnQuitGroup += handler;
		}

		public void RemoveQuitGroupHandler(Action<SocketGroupType, string> handler)
		{
			this.OnQuitGroup -= handler;
		}

		public event Action<SocketPushMessage> OnPushMessage
		{
			add
			{
				this.mOnPushMessage.Add(value);
			}
			remove
			{
				this.mOnPushMessage.Remove(value);
			}
		}

		public void AddPushMessageHandler(Action<SocketPushMessage> handler)
		{
			this.OnPushMessage += handler;
		}

		public void RemovePushMessageHandler(Action<SocketPushMessage> handler)
		{
			this.OnPushMessage -= handler;
		}

		public void SendChat(ChatCommonRequest req, Action<bool, ChatCommonResponse> callback)
		{
			string text = JsonConvert.SerializeObject(new ImChatCustomData
			{
				languageId = req.LanguageId.ToString(CultureInfo.InvariantCulture)
			});
			LlcMgr.Instance.SendChat(req.GroupId, req.Context, text, delegate(bool result, int code)
			{
				if (HabbyIm_GameProxy.IsSendChatError(code))
				{
					GuildProxy.UI.ShowTips(Singleton<LanguageManager>.Instance.GetInfoByID("server_err_" + code.ToString()));
				}
				ChatCommonResponse chatCommonResponse = new ChatCommonResponse();
				chatCommonResponse.Code = code;
				Action<bool, ChatCommonResponse> callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2(result, chatCommonResponse);
			});
		}

		public void SendChat(ChatGuildRequest req, Action<bool, ChatGuildResponse> callback)
		{
			ImChatCustomData imChatCustomData = new ImChatCustomData
			{
				languageId = req.LanguageId.ToString(CultureInfo.InvariantCulture)
			};
			ChatGuildResponse resp = new ChatGuildResponse();
			string text;
			if (this.mSocketGroupSet.TryGetValue(SocketGroupType.GUILD, out text))
			{
				string text2 = JsonConvert.SerializeObject(imChatCustomData);
				LlcMgr.Instance.SendChat(text, req.Context, text2, delegate(bool result, int code)
				{
					if (HabbyIm_GameProxy.IsSendChatError(code))
					{
						GuildProxy.UI.ShowTips(Singleton<LanguageManager>.Instance.GetInfoByID("server_err_" + code.ToString()));
					}
					resp.Code = code;
					Action<bool, ChatGuildResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(result, resp);
				});
				return;
			}
			resp.Code = -1;
			Action<bool, ChatGuildResponse> callback2 = callback;
			if (callback2 == null)
			{
				return;
			}
			callback2(false, resp);
		}

		public static bool IsSendChatError(int code)
		{
			return (code >= 310000 && code <= 319999) || (code >= 320000 && code <= 329999);
		}

		public void GetMessageHistory(long msgSeq, SocketGroupType groupType, string groupId, Action<bool, ChatGetMessageRecordsResponse> callback)
		{
			long num = -1L;
			ChatGroupData group = Singleton<ChatManager>.Instance.GetGroup(groupType, groupId);
			if (group != null && group.ChatMessageList.Count > 0)
			{
				num += group.OldestMessageID;
			}
			LlcMgr.Instance.GetMessageHistory(groupId, num, 20L, delegate(ImHttpGroupMessageHistoryResp resp, string err, int code)
			{
				if (code != 0 || resp == null || resp.data == null || resp.data.msgList == null || resp.data.msgList.Length == 0)
				{
					Action<bool, ChatGetMessageRecordsResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(false, null);
					return;
				}
				else
				{
					long num2 = long.MaxValue;
					ChatGetMessageRecordsResponse chatGetMessageRecordsResponse = new ChatGetMessageRecordsResponse();
					chatGetMessageRecordsResponse.Code = resp.code;
					for (int i = 0; i < resp.data.msgList.Length; i++)
					{
						ImHttpGroupMessageHistoryItem imHttpGroupMessageHistoryItem = resp.data.msgList[i];
						uint num3;
						if (uint.TryParse(imHttpGroupMessageHistoryItem.msgContent.customType, out num3))
						{
							PushMessageDto pushMessageDto = new PushMessageDto();
							pushMessageDto.MessageType = num3;
							pushMessageDto.MessageContent = imHttpGroupMessageHistoryItem.msgContent.data;
							pushMessageDto.MessageSeq = (ulong)imHttpGroupMessageHistoryItem.msgSeq;
							chatGetMessageRecordsResponse.MessageRecords.Add(pushMessageDto);
							if (num2 > imHttpGroupMessageHistoryItem.msgSeq)
							{
								num2 = imHttpGroupMessageHistoryItem.msgSeq;
							}
						}
					}
					ChatProxy.Common.OnRecvGuildChatRecords(group, chatGetMessageRecordsResponse);
					if (group != null && (group.OldestMessageID == 0L || group.OldestMessageID > num2))
					{
						group.OldestMessageID = num2;
					}
					Action<bool, ChatGetMessageRecordsResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, chatGetMessageRecordsResponse);
					return;
				}
			});
		}

		public void GetUnreadCount(long msgSeq, SocketGroupType groupType, string groupId, Action<bool, long, int> callback)
		{
			if (msgSeq == 0L)
			{
				msgSeq = -1L;
			}
			LlcMgr.Instance.GetUnreadCount(groupId, msgSeq, delegate(ImHttpGetGroupMaxSeqResp resp, string err, int code)
			{
				if (code != 0 || resp == null)
				{
					Action<bool, long, int> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(false, 0L, 0);
					return;
				}
				else
				{
					Action<bool, long, int> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, resp.ExtraInfo.maxSeq, (int)resp.ExtraInfo.unreadCount);
					return;
				}
			});
		}

		public void TranslateChat(long msgSeq, SocketGroupType groupType, string groupId, int fromLanguage, int toLanguage, string content, Action<bool, ChatTranslateResponse> callback)
		{
			LlcMgr.Instance.TranslateMessage(groupId, msgSeq, content, HabbyIm_GameProxy.LanguageDict[fromLanguage], HabbyIm_GameProxy.LanguageDict[toLanguage], delegate(ImHttpTranslateResp resp, string err, int code)
			{
				if (code != 0 || resp == null || resp.data == null)
				{
					Action<bool, ChatTranslateResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(false, null);
					return;
				}
				else
				{
					ChatTranslateResponse chatTranslateResponse = new ChatTranslateResponse();
					chatTranslateResponse.Code = code;
					chatTranslateResponse.Context = resp.data.translatedMsg;
					Action<bool, ChatTranslateResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(true, chatTranslateResponse);
					return;
				}
			});
		}

		private void OnUserLogin(object sender, int type, BaseEventArgs eventArgs)
		{
			EventLogin eventLogin = eventArgs as EventLogin;
			if (eventLogin != null)
			{
				GameApp.SocketNet.SetTokenAfterLogin(eventLogin.userLoginResponse.UserId, eventLogin.userLoginResponse.AccessToken);
			}
		}

		public void SetSocketGroup(SocketGroupType type, string key)
		{
			this.InternalSetGroup(type, key);
		}

		private void InternalSetGroup(SocketGroupType type, string key)
		{
			string text;
			if (this.mSocketGroupSet.TryGetValue(type, out text))
			{
				if (text == key)
				{
					return;
				}
			}
			else if (string.IsNullOrEmpty(key))
			{
				return;
			}
			if (string.IsNullOrEmpty(key))
			{
				this.Log(string.Format("退出Socket组 {0} = {1}", type, text));
				this.mSocketGroupSet.Remove(type);
				this.InternalOnQuitGroup(type, text);
			}
			else
			{
				this.Log(string.Format("加入Socket组 {0} = {1}", type, key));
				this.mSocketGroupSet[type] = key;
				if (!string.IsNullOrEmpty(text))
				{
					this.InternalOnQuitGroup(type, text);
				}
				this.InternalOnJoinGroup(type, key);
			}
			if (GameApp.SocketNet.LoginSuccess)
			{
				this.JoinOrQuitSocketGroup(type, text, key);
			}
		}

		public void Notify(LlcResponse response)
		{
			if (response.action == "connectionState" && response.subModule == "connectionState_authorised")
			{
				string text;
				if (this.mSocketGroupSet.TryGetValue(SocketGroupType.SCENE, out text))
				{
					this.JoinOrQuitSocketGroup(SocketGroupType.SCENE, null, text);
				}
				string text2;
				if (this.mSocketGroupSet.TryGetValue(SocketGroupType.Server, out text2))
				{
					this.JoinOrQuitSocketGroup(SocketGroupType.Server, null, text2);
					return;
				}
			}
			else if (response.action == "im" && response.subModule == "im_groupMessage_v2" && response.errorCode == 0)
			{
				LlcImRecvData llcImRecvData = (LlcImRecvData)response.responseData;
				uint num;
				if (llcImRecvData != null && llcImRecvData.msgContent != null && uint.TryParse(llcImRecvData.msgContent.customType, out num))
				{
					this.InternalOnPushMessage(new SocketPushMessage
					{
						MessageType = num,
						MessageContent = llcImRecvData.msgContent.data,
						MessageSeq = (ulong)llcImRecvData.msgSeq
					});
				}
			}
		}

		private void InternalOnQuitGroup(SocketGroupType type, string key)
		{
			for (int i = 0; i < this.mOnQuitGroup.Count; i++)
			{
				Action<SocketGroupType, string> action = this.mOnQuitGroup[i];
				if (action != null)
				{
					action(type, key);
				}
			}
		}

		private void InternalOnJoinGroup(SocketGroupType type, string key)
		{
			for (int i = 0; i < this.mOnJoinGroup.Count; i++)
			{
				Action<SocketGroupType, string> action = this.mOnJoinGroup[i];
				if (action != null)
				{
					action(type, key);
				}
			}
		}

		private void InternalOnPushMessage(SocketPushMessage message)
		{
			for (int i = 0; i < this.mOnPushMessage.Count; i++)
			{
				Action<SocketPushMessage> action = this.mOnPushMessage[i];
				if (action != null)
				{
					action(message);
				}
			}
		}

		private void JoinOrQuitSocketGroup(SocketGroupType type, string oldKey, string newKey)
		{
			this.Log(string.Format("JoinOrQuitSocketGroup : >> {0} = {1} => {2}", type, oldKey, newKey));
			if (type == SocketGroupType.GUILD)
			{
				return;
			}
			if (!string.IsNullOrEmpty(oldKey))
			{
				LlcMgr.Instance.QuitGroup(oldKey, delegate(bool result, int code)
				{
					if (!string.IsNullOrEmpty(newKey))
					{
						LlcMgr.Instance.JoinGroup(newKey, delegate(bool result, int code)
						{
						}, false);
					}
				});
				return;
			}
			if (!string.IsNullOrEmpty(newKey))
			{
				LlcMgr.Instance.JoinGroup(newKey, delegate(bool result, int code)
				{
				}, false);
			}
		}

		private void Log(string message)
		{
		}

		private void LogWarning(string message)
		{
		}

		private void LogError(string message)
		{
			HLog.LogError("[Socket][Proxy]" + message);
		}

		private Dictionary<SocketGroupType, string> mSocketGroupSet = new Dictionary<SocketGroupType, string>();

		private List<Action<SocketGroupType, string>> mOnQuitGroup = new List<Action<SocketGroupType, string>>();

		private List<Action<SocketGroupType, string>> mOnJoinGroup = new List<Action<SocketGroupType, string>>();

		private List<Action<SocketPushMessage>> mOnPushMessage = new List<Action<SocketPushMessage>>();

		public const int CHAT_ErrorCodeStart = 320000;

		public const int CHAT_ErrorCodeEnd = 329999;

		public const int SOCKET_ErrorCodeStart = 310000;

		public const int SOCKET_ErrorCodeEnd = 319999;

		public static Dictionary<LanguageType, string> LanguageDict = new Dictionary<LanguageType, string>
		{
			{ 0, "en" },
			{ 1, "es" },
			{ 2, "zh" },
			{ 3, "zh-TW" },
			{ 4, "ja" },
			{ 5, "fr" },
			{ 6, "de" },
			{ 7, "it" },
			{ 8, "nl" },
			{ 9, "ru" },
			{ 10, "ar" },
			{ 11, "ko" },
			{ 12, "vi" },
			{ 13, "th" },
			{ 15, "pt" },
			{ 14, "id" }
		};
	}
}
