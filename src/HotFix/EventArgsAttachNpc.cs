using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsAttachNpc : BaseEventArgs
	{
		public int npcId { get; private set; }

		public Action onFinish { get; private set; }

		public void SetData(int npcId, Action onComplete)
		{
			this.npcId = npcId;
			this.onFinish = onComplete;
		}

		public override void Clear()
		{
		}
	}
}
