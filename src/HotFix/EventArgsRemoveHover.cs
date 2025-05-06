using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsRemoveHover : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public BaseHover hover;
	}
}
