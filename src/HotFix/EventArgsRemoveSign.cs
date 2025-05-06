using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRemoveSign : BaseEventArgs
	{
		public void SetData(int instanceId, int buffId)
		{
			this.instanceId = instanceId;
			this.buffId = buffId;
		}

		public override void Clear()
		{
		}

		public int instanceId;

		public int buffId;
	}
}
