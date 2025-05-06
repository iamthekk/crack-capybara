using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_GetGuildList : GuildBaseEvent
	{
		public override void Clear()
		{
			this.GuildList.Clear();
		}

		public int GroupID;

		public List<GuildShareData> GuildList = new List<GuildShareData>();
	}
}
