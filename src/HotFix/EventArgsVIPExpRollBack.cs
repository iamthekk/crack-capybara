using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgsVIPExpRollBack : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public int RollBackVIPExp;
	}
}
