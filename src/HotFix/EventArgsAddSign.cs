using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsAddSign : BaseEventArgs
	{
		public void SetData(int instanceId, int buffId, int buffLayer, int buffRound)
		{
			this.instanceId = instanceId;
			this.buffId = buffId;
			this.buffLayer = buffLayer;
			this.buffRound = buffRound;
		}

		public override void Clear()
		{
		}

		public int instanceId;

		public int buffId;

		public int buffLayer = 1;

		public int buffRound;
	}
}
