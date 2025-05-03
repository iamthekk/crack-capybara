using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRemoveServerTimeClock : BaseEventArgs
	{
		public override void Clear()
		{
			this.ClockCallUnionKey = "";
		}

		public string ClockCallUnionKey;
	}
}
