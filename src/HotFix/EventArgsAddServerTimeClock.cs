using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsAddServerTimeClock : BaseEventArgs
	{
		public override void Clear()
		{
			this.ClockCall = null;
		}

		public ServerTimeClockCall ClockCall;
	}
}
