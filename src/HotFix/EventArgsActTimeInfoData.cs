using System;
using Framework.EventSystem;
using Proto.ActTime;

namespace HotFix
{
	public class EventArgsActTimeInfoData : BaseEventArgs
	{
		public void SetData(ActTimeInfoResponse response)
		{
			this.Response = response;
		}

		public override void Clear()
		{
			this.Response = null;
		}

		public ActTimeInfoResponse Response;
	}
}
