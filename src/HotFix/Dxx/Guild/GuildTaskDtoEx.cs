using System;
using System.Collections.Generic;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildTaskDtoEx
	{
		public static GuildTaskData ToGuidTaskData(this GuildTaskDto taskdto)
		{
			return new GuildTaskData
			{
				TaskId = (int)taskdto.TaskId,
				CurrentRate = (int)taskdto.Progress,
				NeedRate = (int)taskdto.Need,
				IsFinish = taskdto.IsFinish,
				HasGetRewards = taskdto.IsReceive,
				ContentLanguageID = taskdto.LanguageId,
				Rewards = taskdto.Rewards.ToGuildItemList()
			};
		}

		public static List<GuildTaskData> ToGuidTaskList(this IList<GuildTaskDto> tasklist)
		{
			List<GuildTaskData> list = new List<GuildTaskData>();
			for (int i = 0; i < tasklist.Count; i++)
			{
				list.Add(tasklist[i].ToGuidTaskData());
			}
			return list;
		}
	}
}
