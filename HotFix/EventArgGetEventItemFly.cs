using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgGetEventItemFly : BaseEventArgs
	{
		public GameEventItemData item { get; private set; }

		public void SetData(GameEventItemData data)
		{
			this.item = data;
		}

		public override void Clear()
		{
		}
	}
}
