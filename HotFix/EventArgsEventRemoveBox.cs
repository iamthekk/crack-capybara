using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsEventRemoveBox : BaseEventArgs
	{
		public int boxId { get; private set; }

		public int memberId { get; private set; }

		public void SetData(int boxId, int memberId)
		{
			this.boxId = boxId;
			this.memberId = memberId;
		}

		public override void Clear()
		{
		}
	}
}
