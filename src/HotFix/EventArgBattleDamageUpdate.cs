using System;
using Framework.EventSystem;

namespace HotFix
{
	public class EventArgBattleDamageUpdate : BaseEventArgs
	{
		public override void Clear()
		{
		}

		public long CurDamage;

		public long CurHP;
	}
}
