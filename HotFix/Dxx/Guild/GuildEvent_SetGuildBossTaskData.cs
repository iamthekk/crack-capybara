using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_SetGuildBossTaskData : GuildBaseEvent
	{
		public override void Clear()
		{
			this.TaskList = null;
		}

		public List<GuildBossTask> TaskList;
	}
}
