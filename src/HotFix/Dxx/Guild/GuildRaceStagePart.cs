using System;

namespace Dxx.Guild
{
	public class GuildRaceStagePart
	{
		public ulong EndTime
		{
			get
			{
				return this.StartTime + (ulong)((long)(this.LastTimeMinute * 60));
			}
		}

		public int Stage;

		public int BattleDay;

		public GuildRaceStageKind StageKind;

		public ulong StartTime;

		public int LastTimeMinute;
	}
}
