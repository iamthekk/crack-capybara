using System;
using Google.Protobuf;

namespace Framework.NetWork
{
	public class NetWorkSendData
	{
		public void Clear()
		{
			this.m_message = null;
			this.m_callBack = null;
			this.m_isShowMask = false;
			this.m_isCache = false;
			this.m_isShowError = true;
			this.m_key = string.Empty;
		}

		public IMessage m_message;

		public Action<IMessage> m_callBack;

		public bool m_isShowMask;

		public bool m_isCache;

		public bool m_isShowError = true;

		public string m_key;
	}
}
