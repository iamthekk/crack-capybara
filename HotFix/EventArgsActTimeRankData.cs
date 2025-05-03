using System;
using Framework.EventSystem;
using Proto.ActTime;

namespace HotFix
{
	public class EventArgsActTimeRankData : BaseEventArgs
	{
		public void SetData(ActTimeRankResponse response)
		{
			this.Response = response;
		}

		public override void Clear()
		{
			this.Response = null;
		}

		public ActTimeRankResponse Response;
	}
}
