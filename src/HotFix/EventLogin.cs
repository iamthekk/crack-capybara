using System;
using Framework.EventSystem;
using Proto.User;

namespace HotFix
{
	public class EventLogin : BaseEventArgs
	{
		public void SetData(UserLoginResponse UserLoginResponse)
		{
			this.userLoginResponse = UserLoginResponse;
		}

		public override void Clear()
		{
			this.userLoginResponse = null;
		}

		public UserLoginResponse userLoginResponse;
	}
}
