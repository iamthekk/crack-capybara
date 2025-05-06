using System;
using Framework.EventSystem;
using Proto.User;

namespace HotFix
{
	public class EventArgsSetOtherMainCityData : BaseEventArgs
	{
		public void SetData(long targetUserID, UserGetCityInfoResponse response)
		{
			this.m_targetUserID = targetUserID;
			this.m_response = response;
		}

		public override void Clear()
		{
		}

		public long m_targetUserID;

		public UserGetCityInfoResponse m_response;
	}
}
