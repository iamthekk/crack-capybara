using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgSick : BaseEventArgs
	{
		public GameEventItemData data { get; private set; }

		public void SetData(GameEventItemData itemData)
		{
			this.data = itemData;
		}

		public override void Clear()
		{
		}
	}
}
