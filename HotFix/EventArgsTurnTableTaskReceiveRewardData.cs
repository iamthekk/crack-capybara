using System;
using Framework.EventSystem;
using Proto.SevenDayTask;

namespace HotFix
{
	public class EventArgsTurnTableTaskReceiveRewardData : BaseEventArgs
	{
		public void SetData(TurnTableTaskReceiveRewardResponse response)
		{
			this.Response = response;
		}

		public override void Clear()
		{
			this.Response = null;
		}

		public TurnTableTaskReceiveRewardResponse Response;
	}
}
