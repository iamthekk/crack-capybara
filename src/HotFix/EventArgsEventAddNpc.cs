using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsEventAddNpc : BaseEventArgs
	{
		public NpcFunction function { get; private set; }

		public int npcId { get; private set; }

		public int stage { get; private set; }

		public Action onComplete { get; private set; }

		public void SetData(NpcFunction func, int npcId, int stage, Action onFinish = null)
		{
			this.function = func;
			this.npcId = npcId;
			this.stage = stage;
			this.onComplete = onFinish;
		}

		public override void Clear()
		{
		}
	}
}
