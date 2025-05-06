using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsShowHpHUD : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public bool isShow;

		public int instanceId;
	}
}
