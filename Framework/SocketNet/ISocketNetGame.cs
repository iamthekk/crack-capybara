using System;
using Google.Protobuf;

namespace Framework.SocketNet
{
	public interface ISocketNetGame : ISocketNet
	{
		void AddMessageHandler(int msgid, Action<IMessage> handler);

		void RemoveMessageHandler(int msgid, Action<IMessage> handler);

		void SetSocketMessageHandler(ISocketMessageHandler handler);

		void SetMessageToQueue(IMessage msg);

		void OnSetLoginSuccess();

		void OnLoginRepeat();

		void OnLoginReconnect();

		DateTime LastRecvHeartTime { get; set; }

		string UserToken { get; }

		void Send(IMessage msg);
	}
}
