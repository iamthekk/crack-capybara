using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsMoveToNpc : BaseEventArgs
	{
		public NpcFunction function { get; private set; }

		public int memberId { get; private set; }

		public void SetData(NpcFunction func, int npcId)
		{
			this.function = func;
			this.memberId = npcId;
		}

		public override void Clear()
		{
		}
	}
}
