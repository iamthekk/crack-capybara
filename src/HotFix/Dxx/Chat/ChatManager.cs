using System;
using System.Collections.Generic;
using Framework.SocketNet;
using Google.Protobuf;
using HotFix;
using NetWork;
using Socket.Guild;

namespace Dxx.Chat
{
	public class ChatManager : Singleton<ChatManager>
	{
		public event Action<SocketGroupType, string> OnJoinGroup;

		public event Action<SocketGroupType, string> OnQuitGroup;

		public bool Init()
		{
			ISocketNet socketNet = ChatProxy.Net.SocketNet;
			ISocketGameLogicProxy socketNetProxy = ChatProxy.Net.SocketNetProxy;
			if (socketNetProxy != null)
			{
				socketNetProxy.AddJoinGroupHandler(new Action<SocketGroupType, string>(this.OnJoinSocketGroup));
				socketNetProxy.AddQuitGroupHandler(new Action<SocketGroupType, string>(this.OnQuitSocketGroup));
				socketNetProxy.AddPushMessageHandler(new Action<SocketPushMessage>(this.OnServerSocketPushMessage));
			}
			return true;
		}

		public void Clear()
		{
			this.mGroupDic.Clear();
		}

		public void Destroy()
		{
			ISocketNet socketNet = ChatProxy.Net.SocketNet;
			ISocketGameLogicProxy socketNetProxy = ChatProxy.Net.SocketNetProxy;
			if (socketNetProxy != null)
			{
				socketNetProxy.RemoveJoinGroupHandler(new Action<SocketGroupType, string>(this.OnJoinSocketGroup));
				socketNetProxy.RemoveQuitGroupHandler(new Action<SocketGroupType, string>(this.OnQuitSocketGroup));
				socketNetProxy.RemovePushMessageHandler(new Action<SocketPushMessage>(this.OnServerSocketPushMessage));
			}
		}

		private int _MsgID(Type msgtype)
		{
			return (int)PackageFactory.GetMessageId(Activator.CreateInstance(msgtype) as IMessage);
		}

		public ChatGroupData GetGroup(SocketGroupType groupType, string groupId)
		{
			if (string.IsNullOrEmpty(groupId))
			{
				return null;
			}
			Dictionary<string, ChatGroupData> dictionary;
			if (!this.mGroupDic.TryGetValue(groupType, out dictionary))
			{
				return null;
			}
			ChatGroupData chatGroupData;
			if (dictionary.TryGetValue(groupId, out chatGroupData))
			{
				return chatGroupData;
			}
			return null;
		}

		public void AddGroup(SocketGroupType groupType, string groupId)
		{
			if (string.IsNullOrEmpty(groupId))
			{
				return;
			}
			Dictionary<string, ChatGroupData> dictionary;
			if (!this.mGroupDic.TryGetValue(groupType, out dictionary))
			{
				dictionary = new Dictionary<string, ChatGroupData>();
				this.mGroupDic[groupType] = dictionary;
			}
			if (dictionary.ContainsKey(groupId))
			{
				return;
			}
			dictionary[groupId] = new ChatGroupData(groupType, groupId);
		}

		public void ClearGroup(SocketGroupType groupType, string groupId)
		{
			if (string.IsNullOrEmpty(groupId))
			{
				return;
			}
			Dictionary<string, ChatGroupData> dictionary;
			if (!this.mGroupDic.TryGetValue(groupType, out dictionary))
			{
				return;
			}
			dictionary.Remove(groupId);
		}

		public static ChatData JsonStringToChatData(string content)
		{
			return ChatProxy.JSON.ToObject<ChatData>(content);
		}

		private void OnJoinSocketGroup(SocketGroupType type, string groupid)
		{
			if (string.IsNullOrEmpty(groupid))
			{
				return;
			}
			this.AddGroup(type, groupid);
			Action<SocketGroupType, string> onJoinGroup = this.OnJoinGroup;
			if (onJoinGroup == null)
			{
				return;
			}
			onJoinGroup(type, groupid);
		}

		private void OnQuitSocketGroup(SocketGroupType type, string groupid)
		{
			if (string.IsNullOrEmpty(groupid))
			{
				return;
			}
			this.ClearGroup(type, groupid);
			Action<SocketGroupType, string> onQuitGroup = this.OnQuitGroup;
			if (onQuitGroup == null)
			{
				return;
			}
			onQuitGroup(type, groupid);
		}

		private void OnServerSocketPushMessage(IMessage msg)
		{
			SocketPushMessage socketPushMessage = msg as SocketPushMessage;
			if (socketPushMessage != null)
			{
				switch (socketPushMessage.MessageType)
				{
				case 201U:
				case 202U:
					this.HandleMessage(SocketGroupType.GUILD, ChatProxy.GuildChat.GetGuildChatGroupId(), socketPushMessage);
					return;
				case 203U:
					this.HandleMessage(SocketGroupType.SCENE, ChatProxy.SceneChat.GetGroupID(), socketPushMessage);
					return;
				case 204U:
					this.HandleMessage(SocketGroupType.Server, ChatProxy.ServerChat.GetGroupID(), socketPushMessage);
					break;
				default:
					return;
				}
			}
		}

		private void HandleMessage(SocketGroupType groupType, string groupId, SocketPushMessage msg)
		{
			ChatData chatData = ChatManager.JsonStringToChatData(msg.MessageContent);
			chatData.msgSeq = (long)msg.MessageSeq;
			if (chatData != null && !string.IsNullOrEmpty(groupId))
			{
				ChatGroupData group = this.GetGroup(groupType, groupId);
				if (group != null)
				{
					group.AddNewChat(chatData);
				}
			}
		}

		private readonly Dictionary<SocketGroupType, Dictionary<string, ChatGroupData>> mGroupDic = new Dictionary<SocketGroupType, Dictionary<string, ChatGroupData>>();
	}
}
