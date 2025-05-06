using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildShareDetailData
	{
		public GuildUserShareData GetGuildPresident()
		{
			if (this.Members == null || this.Members.Count <= 0)
			{
				return null;
			}
			for (int i = 0; i < this.Members.Count; i++)
			{
				GuildUserShareData guildUserShareData = this.Members[i];
				if (guildUserShareData.GuildPosition == GuildPositionType.President)
				{
					return guildUserShareData;
				}
			}
			return null;
		}

		public GuildUserShareData GetMemberData(long id)
		{
			if (this.Members == null || this.Members.Count <= 0)
			{
				return null;
			}
			for (int i = 0; i < this.Members.Count; i++)
			{
				GuildUserShareData guildUserShareData = this.Members[i];
				if (guildUserShareData.UserID == id)
				{
					return guildUserShareData;
				}
			}
			return null;
		}

		public string GuildID;

		public string IMGroupID;

		public GuildShareData ShareData;

		public List<GuildUserShareData> Members = new List<GuildUserShareData>();
	}
}
