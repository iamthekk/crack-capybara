using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsItemUpdate : BaseEventArgs
	{
		public EventArgsItemUpdate SetData(int itemId)
		{
			this.itemId = itemId;
			return this;
		}

		public override void Clear()
		{
		}

		public int itemId;
	}
}
