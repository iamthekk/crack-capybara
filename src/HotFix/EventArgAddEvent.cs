using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgAddEvent : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public GameEventUIData uiData;
	}
}
