using System;
using Framework.EventSystem;
using Proto.User;

namespace HotFix
{
	public class EventArgsSetEquipData : BaseEventArgs
	{
		public void SetData(UserLoginResponse userLoginResponse)
		{
			this.m_userLoginResponse = userLoginResponse;
		}

		public override void Clear()
		{
			this.m_userLoginResponse = null;
		}

		public UserLoginResponse m_userLoginResponse;
	}
}
