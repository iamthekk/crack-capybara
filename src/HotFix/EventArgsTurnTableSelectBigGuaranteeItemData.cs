using System;
using Framework.EventSystem;
using Proto.SevenDayTask;

namespace HotFix
{
	public class EventArgsTurnTableSelectBigGuaranteeItemData : BaseEventArgs
	{
		public void SetData(TurnTableSelectBigGuaranteeItemResponse response)
		{
			this.Response = response;
		}

		public override void Clear()
		{
			this.Response = null;
		}

		public TurnTableSelectBigGuaranteeItemResponse Response;
	}
}
