using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildBossTask
	{
		public GuildBossTask.GuildBossTaskState taskState
		{
			get
			{
				if (this.IsFinish && this.IsReceive)
				{
					return GuildBossTask.GuildBossTaskState.AllFinish;
				}
				if (this.IsFinish && !this.IsReceive)
				{
					return GuildBossTask.GuildBossTaskState.CanGetReward;
				}
				return GuildBossTask.GuildBossTaskState.Undone;
			}
		}

		public int TaskID;

		public long Progress;

		public long Need;

		public bool IsFinish;

		public bool IsReceive;

		public List<GuildItemData> Rewards;

		public int ContentLanguageID;

		public enum GuildBossTaskState
		{
			Undone,
			CanGetReward,
			AllFinish
		}
	}
}
