using System;
using System.Collections;
using Google.Protobuf;

namespace Framework.NetWork
{
	public interface INetWorkManager
	{
		void SetData(string url, int version, NetWorkUsingType usingType);

		bool IsNetConnect();

		IEnumerator SendWebRequest(NetWorkSendData data);

		void HandleCommonData(IMessage msg);
	}
}
