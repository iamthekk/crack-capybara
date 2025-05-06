using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildEvent_GuildMemberChange : GuildBaseEvent
	{
		public override void Clear()
		{
			this.UserList.Clear();
			this.DeleteUser.Clear();
		}

		public string GuildID;

		public List<GuildUserShareData> UserList = new List<GuildUserShareData>();

		public List<long> DeleteUser = new List<long>();

		public int MemberCount = -1;
	}
}
