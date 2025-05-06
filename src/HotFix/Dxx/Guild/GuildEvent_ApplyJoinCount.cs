using System;

namespace Dxx.Guild
{
	public class GuildEvent_ApplyJoinCount : GuildBaseEvent
	{
		public override void Clear()
		{
			this.ApplyJoinCount = -1;
		}

		public int ApplyJoinCount = -1;
	}
}
