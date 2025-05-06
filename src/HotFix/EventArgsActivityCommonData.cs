using System;
using Framework.EventSystem;
using Proto.Activity;

namespace HotFix
{
	public class EventArgsActivityCommonData : BaseEventArgs
	{
		public void SetData(ActivityGetListResponse response)
		{
			this.Response = response;
		}

		public override void Clear()
		{
			this.Response = null;
		}

		public ActivityGetListResponse Response;
	}
}
