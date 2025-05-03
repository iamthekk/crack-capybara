using System;
using System.Collections.Generic;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildBossKillBoxDtoEx
	{
		public static GuildBossKillBox ToGuildBossKillData(this GuildBossKillBoxDto infoDto)
		{
			return new GuildBossKillBox
			{
				BoxID = (int)infoDto.BoxId,
				Progress = (int)infoDto.Progress,
				Need = (int)infoDto.Need,
				IsFinish = infoDto.IsFinish,
				IsReceive = infoDto.IsReceive,
				Rewards = infoDto.Rewards.ToGuildItemList()
			};
		}

		public static List<GuildBossKillBox> ToGuildBossKillList(this IList<GuildBossKillBoxDto> infoDtos)
		{
			List<GuildBossKillBox> list = new List<GuildBossKillBox>();
			for (int i = 0; i < infoDtos.Count; i++)
			{
				list.Add(infoDtos[i].ToGuildBossKillData());
			}
			return list;
		}
	}
}
