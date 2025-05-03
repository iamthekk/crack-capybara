using System;

namespace Dxx.Guild
{
	public class GuildEvent_MyPositionChange : GuildBaseEvent
	{
		public override void Clear()
		{
		}

		public void SetData(GuildPositionType oldpos, GuildPositionType newpos)
		{
			this.OldPosition = oldpos;
			this.NewPosition = newpos;
		}

		public GuildPositionType OldPosition;

		public GuildPositionType NewPosition;
	}
}
