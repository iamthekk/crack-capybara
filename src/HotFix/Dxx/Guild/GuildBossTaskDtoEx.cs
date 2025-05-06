using System;
using System.Collections.Generic;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildBossTaskDtoEx
	{
		public static GuildBossTask ToGuildBossTask(this GuildBossTaskDto infoDto)
		{
			return new GuildBossTask
			{
				TaskID = (int)infoDto.TaskId,
				Progress = (long)((int)infoDto.Progress),
				Need = (long)((int)infoDto.Need),
				IsFinish = infoDto.IsFinish,
				IsReceive = infoDto.IsReceive,
				Rewards = infoDto.Rewards.ToGuildItemList(),
				ContentLanguageID = (int)infoDto.LanguageId
			};
		}

		public static List<GuildBossTask> ToGuildBossTaskList(this IList<GuildBossTaskDto> infoDtos)
		{
			List<GuildBossTask> list = new List<GuildBossTask>();
			for (int i = 0; i < infoDtos.Count; i++)
			{
				list.Add(infoDtos[i].ToGuildBossTask());
			}
			return list;
		}
	}
}
