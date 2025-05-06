using System;
using Dxx.Chat;
using Framework.SocketNet;
using Google.Protobuf;
using HotFix;
using Proto.Chat;

public static class ChatNetwork
{
	public static class Chat
	{
		public static void DoRequest_ChatTranslate(SocketGroupType groupType, string chatgroupid, ChatData chat, Action<bool, ChatTranslateResponse> callback)
		{
			if (chat == null)
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
				int tolan = ChatProxy.Language.GetCurrentLanguage();
				ISocketGameLogicProxy socketNetProxy = ChatProxy.Net.SocketNetProxy;
				if (socketNetProxy == null)
				{
					return;
				}
				socketNetProxy.TranslateChat(chat.msgSeq, groupType, chatgroupid, chat.languageId, tolan, chat.chatContent, delegate(bool result, ChatTranslateResponse resp)
				{
					if (result)
					{
						chat.TranslateContent = resp.Context;
						chat.TranslateLanguageKind = tolan;
						ChatGroupData group = Singleton<ChatManager>.Instance.GetGroup(groupType, chatgroupid);
						if (group != null)
						{
							group.SetChatTranslate(chat, tolan, resp.Context);
						}
					}
					Action<bool, ChatTranslateResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(result, resp);
				});
				return;
			}
		}

		public static void DoRequest_SendChat(SocketGroupType groupType, string groupId, string content, int languageId, Action<bool, ChatCommonResponse> callback)
		{
			ISocketNet socketNet = ChatProxy.Net.SocketNet;
			if (!socketNet.Connected)
			{
				socketNet.CheckReconnect("Scene chat");
				if (callback != null)
				{
					callback(false, null);
				}
				return;
			}
			ChatCommonRequest chatCommonRequest = new ChatCommonRequest
			{
				CommonParams = ChatProxy.Net.GetCommonParams(),
				LanguageId = (uint)languageId,
				Context = content,
				GroupType = (uint)groupType,
				GroupId = groupId
			};
			ISocketGameLogicProxy socketNetProxy = ChatProxy.Net.SocketNetProxy;
			if (socketNetProxy == null)
			{
				return;
			}
			socketNetProxy.SendChat(chatCommonRequest, callback);
		}

		public static void DoRequest_SendChatGuild(string content, int languageid, Action<bool, ChatGuildResponse> callback)
		{
			ISocketNet socketNet = ChatProxy.Net.SocketNet;
			if (!socketNet.Connected)
			{
				socketNet.CheckReconnect("Guild chat");
				if (callback != null)
				{
					callback(false, null);
				}
				return;
			}
			ChatGuildRequest chatGuildRequest = new ChatGuildRequest();
			chatGuildRequest.CommonParams = ChatProxy.Net.GetCommonParams();
			chatGuildRequest.Context = content;
			chatGuildRequest.LanguageId = (uint)languageid;
			ISocketGameLogicProxy socketNetProxy = ChatProxy.Net.SocketNetProxy;
			if (socketNetProxy == null)
			{
				return;
			}
			socketNetProxy.SendChat(chatGuildRequest, callback);
		}

		public static void DoRequest_ShowItem(uint type, ulong id, Action<bool, ChatGuildShowItemResponse> callback)
		{
			ChatProxy.Net.TrySendMessage(new ChatGuildShowItemRequest
			{
				CommonParams = ChatProxy.Net.GetCommonParams(),
				ItemType = type,
				RowId = id
			}, delegate(IMessage response)
			{
				ChatGuildShowItemResponse chatGuildShowItemResponse = response as ChatGuildShowItemResponse;
				if (chatGuildShowItemResponse != null && chatGuildShowItemResponse.Code == 0)
				{
					Action<bool, ChatGuildShowItemResponse> callback2 = callback;
					if (callback2 == null)
					{
						return;
					}
					callback2(true, chatGuildShowItemResponse);
					return;
				}
				else
				{
					Action<bool, ChatGuildShowItemResponse> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(false, chatGuildShowItemResponse);
					return;
				}
			}, false, false);
		}
	}
}
