using System;
using Dxx.Chat;
using Framework;
using Google.Protobuf;
using HotFix;
using NetWork;
using Socket.Guild;

namespace Dxx.Guild
{
	public class GuildNetworkProxy_Common
	{
		private static GuildSDKManager GuildSDK
		{
			get
			{
				return GuildSDKManager.Instance;
			}
		}

		public void StartProxy()
		{
			ISocketGameLogicProxy socketNetProxy = GuildProxy.Net.SocketNetProxy;
			if (socketNetProxy != null)
			{
				socketNetProxy.AddPushMessageHandler(new Action<SocketPushMessage>(this.OnServerSocketPushMessage));
			}
			GuildNetworkProxy_Common.GuildSDK.Event.RegisterEvent(10, new GuildHandlerEvent(this.OnGuildLogin));
		}

		public void StopProxy()
		{
			ISocketGameLogicProxy socketNetProxy = GuildProxy.Net.SocketNetProxy;
			if (socketNetProxy != null)
			{
				socketNetProxy.RemovePushMessageHandler(new Action<SocketPushMessage>(this.OnServerSocketPushMessage));
			}
			GuildNetworkProxy_Common.GuildSDK.Event.UnRegisterEvent(10, new GuildHandlerEvent(this.OnGuildLogin));
		}

		private int _MsgID(Type msgtype)
		{
			return (int)PackageFactory.GetMessageId(Activator.CreateInstance(msgtype) as IMessage);
		}

		private void OnServerSocketPushMessage(IMessage msg)
		{
			SocketPushMessage socketPushMessage = msg as SocketPushMessage;
			if (socketPushMessage != null)
			{
				GuildSocketPushJsonData.GuildBaseJsonData guildBaseJsonData = GuildSocketPushJsonData.TryGetData((int)socketPushMessage.MessageType, socketPushMessage.MessageContent);
				if (guildBaseJsonData != null)
				{
					guildBaseJsonData.HandleMessage();
				}
				if (GuildNetworkProxy_Common.GuildSDK.GuildInfo.HasGuild)
				{
					string imgroupId = GuildNetworkProxy_Common.GuildSDK.GuildInfo.IMGroupId;
					ChatData chatData = GuildProxy.Chat.TryGetChatData((int)socketPushMessage.MessageType, socketPushMessage.MessageContent);
					if (chatData != null)
					{
						ChatGroupData guildChatGroup = GuildProxy.Chat.GetGuildChatGroup(imgroupId);
						if (guildChatGroup != null)
						{
							guildChatGroup.AddNewChat(chatData);
						}
					}
				}
			}
		}

		private void OnServerSocketPushMessage(SocketPushMessage msg)
		{
			GuildSocketPushJsonData.GuildBaseJsonData guildBaseJsonData = GuildSocketPushJsonData.TryGetData((int)msg.MessageType, msg.MessageContent);
			if (guildBaseJsonData != null)
			{
				guildBaseJsonData.HandleMessage();
			}
			if (GuildNetworkProxy_Common.GuildSDK.GuildInfo.HasGuild)
			{
				string imgroupId = GuildNetworkProxy_Common.GuildSDK.GuildInfo.IMGroupId;
				ChatData chatData = GuildProxy.Chat.TryGetChatData((int)msg.MessageType, msg.MessageContent);
				if (chatData != null)
				{
					chatData.msgSeq = (long)msg.MessageSeq;
					ChatGroupData guildChatGroup = GuildProxy.Chat.GetGuildChatGroup(imgroupId);
					if (guildChatGroup != null)
					{
						guildChatGroup.AddNewChat(chatData);
					}
				}
			}
		}

		private void OnGuildLogin(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_LoginSuccess guildEvent_LoginSuccess = eventArgs as GuildEvent_LoginSuccess;
			if (guildEvent_LoginSuccess != null)
			{
				if (guildEvent_LoginSuccess.MyGuildShareDetail != null)
				{
					GameApp.SocketNet.SetSocketGroup(1, guildEvent_LoginSuccess.MyGuildShareDetail.IMGroupID);
					return;
				}
				GameApp.SocketNet.SetSocketGroup(1, "");
			}
		}
	}
}
