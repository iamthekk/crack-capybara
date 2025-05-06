using System;
using Framework.EventSystem;
using Proto.SevenDayTask;

namespace HotFix
{
	public class EventArgsTurnTableExtractData : BaseEventArgs
	{
		public void SetData(TurnTableExtractResponse response)
		{
			this.Response = response;
		}

		public override void Clear()
		{
			this.Response = null;
		}

		public TurnTableExtractResponse Response;
	}
}
