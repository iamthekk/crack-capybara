using System;
using Framework.EventSystem;
using Proto.SevenDayTask;

namespace HotFix
{
	public class EventArgsActivitySlotTrainData : BaseEventArgs
	{
		public void SetData(TurnTableGetInfoResponse response)
		{
			this.Response = response;
		}

		public override void Clear()
		{
			this.Response = null;
		}

		public TurnTableGetInfoResponse Response;
	}
}
