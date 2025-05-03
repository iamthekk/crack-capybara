using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsStartLogin : BaseEventArgs
	{
		public void SetData(string account, string deviceId, string account2)
		{
			this.m_account = account;
			this.m_deviceId = deviceId;
			this.m_account2 = account2;
		}

		public override void Clear()
		{
			this.m_account = string.Empty;
			this.m_deviceId = string.Empty;
			this.m_account2 = string.Empty;
		}

		public string m_account = string.Empty;

		public string m_deviceId = string.Empty;

		public string m_account2 = string.Empty;
	}
}
