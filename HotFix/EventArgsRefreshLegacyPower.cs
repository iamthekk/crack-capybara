using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRefreshLegacyPower : BaseEventArgs
	{
		public void SetData(int memberInstanceId, int skillId, long current, long max)
		{
			this.memberInstanceId = memberInstanceId;
			this.skillId = skillId;
			this.current = current;
			this.max = max;
		}

		public override void Clear()
		{
		}

		public int memberInstanceId;

		public int skillId;

		public long current;

		public long max;
	}
}
