using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsWaitRoundCount : BaseEventArgs
	{
		public EventArgsWaitRoundCount(int instanceId, int value)
		{
			this.instanceId = instanceId;
			this.value = (long)value;
		}

		public override void Clear()
		{
		}

		public int instanceId;

		public long value;
	}
}
