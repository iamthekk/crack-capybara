using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgUserSelected : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public GameEventUIButtonData buttonData;
	}
}
