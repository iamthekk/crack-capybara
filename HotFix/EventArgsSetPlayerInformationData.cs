using System;
using Framework.EventSystem;
using Proto.User;

namespace HotFix
{
	public class EventArgsSetPlayerInformationData : BaseEventArgs
	{
		public void SetData(long targetUserID, PlayerInfoDto response)
		{
			this.m_targetUserID = targetUserID;
			this.m_response = response;
		}

		public override void Clear()
		{
		}

		public long m_targetUserID;

		public PlayerInfoDto m_response;
	}
}
