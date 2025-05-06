using System;
using Framework.EventSystem;
using Proto.SignIn;

namespace HotFix
{
	public class EventSignDataList : BaseEventArgs
	{
		public void SetData(SignInData signInData)
		{
			this.signInData = signInData;
		}

		public override void Clear()
		{
			this.signInData = null;
		}

		public SignInData signInData;
	}
}
