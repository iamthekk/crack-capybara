using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRefreshShield : BaseEventArgs
	{
		public void SetData(int memberInstanceId, long current)
		{
			this.memberInstanceId = memberInstanceId;
			this.current = current;
		}

		public override void Clear()
		{
		}

		public int memberInstanceId;

		public long current;
	}
}
