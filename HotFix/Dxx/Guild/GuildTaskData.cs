using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildTaskData
	{
		public GuildTaskData.GuildTaskState taskState
		{
			get
			{
				if (this.IsFinish && this.HasGetRewards)
				{
					return GuildTaskData.GuildTaskState.AllFinish;
				}
				if (this.IsFinish && !this.HasGetRewards)
				{
					return GuildTaskData.GuildTaskState.CanGetReward;
				}
				return GuildTaskData.GuildTaskState.Undone;
			}
		}

		public int TaskId;

		public int CurrentRate;

		public int NeedRate;

		public bool IsFinish;

		public bool HasGetRewards;

		public string ContentLanguageID;

		public List<GuildItemData> Rewards = new List<GuildItemData>();

		public enum GuildTaskState
		{
			Undone,
			CanGetReward,
			AllFinish
		}
	}
}
