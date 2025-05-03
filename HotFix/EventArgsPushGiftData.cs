using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsPushGiftData : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public PushGiftData PushGiftData;
	}
}
