using System;

namespace Framework.SocketNet
{
	public interface ISocketNet
	{
		bool Init();

		void DeInit();

		ISocketGameProxyBase GameProxy { get; }

		bool Connected { get; }

		void CheckReconnect(string whyneed);

		bool LoginSuccess { get; }

		void SetSocketGroup(int kind, string groupName);

		void SetSocketGameHandler(ISocketGameProxyBase proxy);

		void SetTokenAfterLogin(long userid, string token);
	}
}
