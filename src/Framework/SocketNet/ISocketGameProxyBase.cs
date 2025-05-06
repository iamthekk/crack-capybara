using System;

namespace Framework.SocketNet
{
	public interface ISocketGameProxyBase
	{
		void OnInit();

		void OnUnInit();

		void SetSocketGroup(int kind, string groupName);
	}
}
