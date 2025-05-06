using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using Proto.Guild;

namespace Dxx.Guild
{
	public static class GuildBossInfoDtoEx
	{
		public static GuildBossInfo ToGuildBossInfo(this GuildBossInfoDto infoDto)
		{
			GuildBossInfo guildBossInfo = new GuildBossInfo();
			guildBossInfo.ChallengeCount = (int)infoDto.ChallengeCnt;
			guildBossInfo.MaxRecoveryCount = (int)infoDto.MaxRecoveryCnt;
			guildBossInfo.NextChallengeRecoverTime = (long)((int)infoDto.NextChallengeCntRecoveryTime);
			guildBossInfo.ChallengeCountByRecoverSeconds = (int)infoDto.ChallengeCntRecoverySeconds;
			guildBossInfo.ChallengeCountRecoverCountPerTime = (int)infoDto.ChallengeCntRecoveryPerTime;
			guildBossInfo.SetBuyCountData(GuildBossInfoDtoEx.CreateBuyCountData(GuildBossBuyKind.Gold, 1, (int)infoDto.BuyCntByCoins, (int)infoDto.BuyCntCostByCoins, (int)infoDto.MaxBuyCntByCoins));
			guildBossInfo.SetBuyCountData(GuildBossInfoDtoEx.CreateBuyCountData(GuildBossBuyKind.Diamonds, 2, (int)infoDto.BuyCntByDiamonds, (int)infoDto.BuyCntCostByDiamonds, (int)infoDto.MaxBuyCntByDiamonds));
			guildBossInfo.TotalPersonalDamage = (long)infoDto.TotalPersonalDamage;
			guildBossInfo.TotalGuildDamage = (long)infoDto.TotalGuildDamage;
			guildBossInfo.KillBossCount = (int)infoDto.KillBossCnt;
			guildBossInfo.BossRefreshTimestamp = (long)infoDto.BossRefreshTimestamp;
			guildBossInfo.BossData = infoDto.BossConfig.ToGuildBossData();
			guildBossInfo.TaskBossList = infoDto.BossTask.ToGuildBossTaskList();
			guildBossInfo.KillBoxList = infoDto.KillBox.ToGuildBossKillList();
			guildBossInfo.ChallengeRecords = new List<GuildBossChallengeRecord>();
			RepeatedField<string> challengeRecords = infoDto.ChallengeRecords;
			for (int i = 0; i < challengeRecords.Count; i++)
			{
				if (!string.IsNullOrEmpty(challengeRecords[i]))
				{
					GuildBossChallengeRecord guildBossChallengeRecord = GuildProxy.JSON.ToObject<GuildBossChallengeRecord>(challengeRecords[i]);
					guildBossInfo.ChallengeRecords.Add(guildBossChallengeRecord);
				}
			}
			guildBossInfo.DayPartCount = (int)infoDto.DayPartCount;
			guildBossInfo.GuildDan = (int)infoDto.GuildDan;
			guildBossInfo.GuildSeason = (int)infoDto.GuildSeason;
			guildBossInfo.GuildRank = infoDto.GuildRank;
			guildBossInfo.PersonRank = infoDto.PersonRank;
			guildBossInfo.LastPersonRank = infoDto.LastPersonRank;
			guildBossInfo.LastGuildRank = infoDto.LastGuildRank;
			guildBossInfo.KillRewardList = new List<int>();
			guildBossInfo.KillRewardList.AddRange(infoDto.KilledBossList);
			guildBossInfo.ServerSeasonEndTime = (long)infoDto.SeasonEndTime;
			return guildBossInfo;
		}

		private static GuildBossBuyCountData CreateBuyCountData(GuildBossBuyKind buykind, int costitemid, int buycount, int buycost, int maxbuycount)
		{
			return new GuildBossBuyCountData
			{
				BuyKind = buykind,
				BuyCount = buycount,
				BuyCost = new GuildItemData
				{
					id = costitemid,
					count = buycost
				},
				MaxBuyCount = maxbuycount
			};
		}
	}
}
