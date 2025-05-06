using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRemoveHoverByID : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public int instanceId;
	}
}
