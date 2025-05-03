using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsBuySupplyGiftSuccess : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public int ConfigId;
	}
}
