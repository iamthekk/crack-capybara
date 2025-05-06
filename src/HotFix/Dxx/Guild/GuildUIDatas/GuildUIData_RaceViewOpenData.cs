using System;

namespace Dxx.Guild.GuildUIDatas
{
	public class GuildUIData_RaceViewOpenData
	{
		public void Init()
		{
		}

		public void DeInit()
		{
		}

		public object GuildRaceRecordViewCacheData
		{
			get
			{
				return this.mGuildRaceRecordViewCacheData;
			}
		}

		public object GuildRaceRecordBattleViewCacheData
		{
			get
			{
				return this.mGuildRaceRecordBattleViewCacheData;
			}
		}

		public void SetGuildRaceRecordViewCacheData(object data)
		{
			this.mGuildRaceRecordViewCacheData = data;
		}

		public void SetGuildRaceRecordBattleViewCacheData(object data)
		{
			this.mGuildRaceRecordBattleViewCacheData = data;
		}

		private object mGuildRaceRecordViewCacheData;

		private object mGuildRaceRecordBattleViewCacheData;
	}
}
