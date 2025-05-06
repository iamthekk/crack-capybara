using System;
using System.Collections.Generic;
using Dxx.Guild;
using LocalModels.Bean;

namespace HotFix.RedPoint.Calculators
{
	public class Guild_Donation : IRedPointRecordCalculator
	{
		public int CalcRedPoint(RedPointDataRecord record)
		{
			GuildSDKManager instance = GuildSDKManager.Instance;
			if (instance == null)
			{
				return 0;
			}
			if (!instance.GuildInfo.HasGuild)
			{
				return 0;
			}
			int dayContributeTimes = instance.GuildInfo.DayContributeTimes;
			List<Guild_guildcontribute> guildContributeConfigs = instance.GuildInfo.GuildContributeConfigs;
			if (dayContributeTimes < guildContributeConfigs.Count)
			{
				int num = 0;
				Guild_guildLevel guildLevelTable = GuildProxy.Table.GetGuildLevelTable(instance.GuildInfo.GuildDetailData.ShareData.GuildLevel);
				if (guildLevelTable != null)
				{
					num = guildLevelTable.MaxContribute - instance.GuildInfo.DayAllContributeTimes;
				}
				if (guildContributeConfigs[dayContributeTimes].CostItem.Length == 0 && num > 0)
				{
					return 1;
				}
			}
			return 0;
		}
	}
}
