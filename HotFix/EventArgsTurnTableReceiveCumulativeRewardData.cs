using System;
using Framework.EventSystem;
using Proto.SevenDayTask;

namespace HotFix
{
	public class EventArgsTurnTableReceiveCumulativeRewardData : BaseEventArgs
	{
		public void SetData(TurnTableReceiveCumulativeRewardResponse response)
		{
			this.Response = response;
		}

		public override void Clear()
		{
			this.Response = null;
		}

		public TurnTableReceiveCumulativeRewardResponse Response;
	}
}
