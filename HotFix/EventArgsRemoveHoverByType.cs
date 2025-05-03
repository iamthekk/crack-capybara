using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRemoveHoverByType : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public HoverType type;
	}
}
