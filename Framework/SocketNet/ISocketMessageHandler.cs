using System;
using Google.Protobuf;

namespace Framework.SocketNet
{
	public interface ISocketMessageHandler
	{
		bool HandleByteMessage(byte[] bytes, int startindex);

		byte[] ConvertMessageToByteArray(IMessage msg);

		int GetMsgID(IMessage msg);
	}
}
