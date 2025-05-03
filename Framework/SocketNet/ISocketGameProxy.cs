using System;
using System.Threading.Tasks;
using Google.Protobuf;

namespace Framework.SocketNet
{
	public interface ISocketGameProxy : ISocketGameProxyBase
	{
		void OnInit();

		void OnUnInit();

		Task SendLogin();

		void SetSocketGroup(int kind, string groupName);

		void SendHeart();

		int GetSocketMsgID(IMessage msg);
	}
}
