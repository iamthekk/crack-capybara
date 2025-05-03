using System;
using System.Collections.Generic;
using Proto.Common;

namespace Dxx.Guild
{
	public static class RewardDtoEx
	{
		public static GuildItemData ToGuildItemData(this RewardDto reward)
		{
			return new GuildItemData
			{
				rowId = 0L,
				id = (int)reward.ConfigId,
				count = (int)reward.Count
			};
		}

		public static List<GuildItemData> ToGuildItemList(this IList<RewardDto> rewardlist)
		{
			List<GuildItemData> list = new List<GuildItemData>();
			for (int i = 0; i < rewardlist.Count; i++)
			{
				list.Add(rewardlist[i].ToGuildItemData());
			}
			return list;
		}
	}
}
