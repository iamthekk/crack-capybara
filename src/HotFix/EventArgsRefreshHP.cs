using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRefreshHP : BaseEventArgs
	{
		public void SetData(int memberInstanceId, long current, long max)
		{
			this.memberInstanceId = memberInstanceId;
			this.current = current;
			this.max = max;
		}

		public override void Clear()
		{
		}

		public int memberInstanceId;

		public long current;

		public long max;
	}
}
