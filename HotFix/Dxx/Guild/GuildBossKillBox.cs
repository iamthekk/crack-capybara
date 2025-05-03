using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildBossKillBox
	{
		public GuildBossKillBox.GuildBossKillBoxState boxState
		{
			get
			{
				if (this.IsFinish && this.IsReceive)
				{
					return GuildBossKillBox.GuildBossKillBoxState.AllFinish;
				}
				if (this.IsFinish && !this.IsReceive)
				{
					return GuildBossKillBox.GuildBossKillBoxState.CanGetReward;
				}
				return GuildBossKillBox.GuildBossKillBoxState.Undone;
			}
		}

		public int BoxID;

		public int Progress;

		public int Need;

		public bool IsFinish;

		public bool IsReceive;

		public List<GuildItemData> Rewards;

		public enum GuildBossKillBoxState
		{
			Undone,
			CanGetReward,
			AllFinish
		}
	}
}
