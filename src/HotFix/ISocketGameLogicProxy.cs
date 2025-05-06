using System;
using Framework.SocketNet;
using Proto.Chat;
using Socket.Guild;

namespace HotFix
{
	public interface ISocketGameLogicProxy : ISocketGameProxyBase
	{
		void AddJoinGroupHandler(Action<SocketGroupType, string> handler);

		void RemoveJoinGroupHandler(Action<SocketGroupType, string> handler);

		void AddQuitGroupHandler(Action<SocketGroupType, string> handler);

		void RemoveQuitGroupHandler(Action<SocketGroupType, string> handler);

		void AddPushMessageHandler(Action<SocketPushMessage> handler);

		void RemovePushMessageHandler(Action<SocketPushMessage> handler);

		void SendChat(ChatCommonRequest req, Action<bool, ChatCommonResponse> callback);

		void SendChat(ChatGuildRequest req, Action<bool, ChatGuildResponse> callback);

		void GetMessageHistory(long msgSeq, SocketGroupType groupType, string groupId, Action<bool, ChatGetMessageRecordsResponse> callback);

		void GetUnreadCount(long msgSeq, SocketGroupType groupType, string groupId, Action<bool, long, int> callback);

		void TranslateChat(long msgSeq, SocketGroupType groupType, string groupId, int fromLanguage, int toLanguage, string content, Action<bool, ChatTranslateResponse> callback);
	}
}
