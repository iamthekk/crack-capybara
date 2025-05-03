using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildBossInfo
	{
		public GuildBossBuyCountData GetBuyCountData(GuildBossBuyKind kind)
		{
			GuildBossBuyCountData guildBossBuyCountData;
			if (this.mBuyCountDic.TryGetValue(kind, out guildBossBuyCountData))
			{
				return guildBossBuyCountData;
			}
			return null;
		}

		public void SetBuyCountData(GuildBossBuyCountData data)
		{
			if (data == null)
			{
				return;
			}
			GuildBossBuyCountData buyCountData = this.GetBuyCountData(data.BuyKind);
			if (buyCountData != null)
			{
				if (data.MaxBuyCount <= 0)
				{
					data.MaxBuyCount = buyCountData.MaxBuyCount;
				}
				this.mBuyCountDic[data.BuyKind] = data;
				return;
			}
			this.mBuyCountDic[data.BuyKind] = data;
		}

		public void CombineRecords(List<GuildBossChallengeRecord> records)
		{
			if (this.ChallengeRecords == null)
			{
				this.ChallengeRecords = new List<GuildBossChallengeRecord>();
			}
			if (records == null || records.Count <= 0)
			{
				return;
			}
			this.ChallengeRecords.InsertRange(0, records);
		}

		[Obsolete("不再使用恢复次数机制")]
		public int MaxRecoveryCount;

		[Obsolete("不再使用恢复次数机制")]
		public long NextChallengeRecoverTime;

		[Obsolete("不再使用恢复次数机制")]
		public int ChallengeCountByRecoverSeconds;

		[Obsolete("不再使用恢复次数机制")]
		public int ChallengeCountRecoverCountPerTime;

		public int ChallengeCount;

		public int BuyDiamondsCount;

		public int MaxDiamondsBuyCount;

		public int BuyDiamondsCost;

		public int BuyCoinCount;

		public int MaxCoinBuyCount;

		public int BuyCoinCost;

		public long TotalPersonalDamage;

		public long TotalGuildDamage;

		public int KillBossCount;

		public long BossRefreshTimestamp;

		private Dictionary<GuildBossBuyKind, GuildBossBuyCountData> mBuyCountDic = new Dictionary<GuildBossBuyKind, GuildBossBuyCountData>();

		public GuildBossData BossData;

		public List<GuildBossTask> TaskBossList;

		public List<GuildBossKillBox> KillBoxList;

		public bool IsFullChallengeRecords = true;

		public List<GuildBossChallengeRecord> ChallengeRecords;

		public int DayPartCount;

		public int GuildDan;

		public int GuildSeason;

		public int GuildRank;

		public int PersonRank;

		public int LastPersonRank;

		public int LastGuildRank;

		public List<int> KillRewardList;

		public long ServerSeasonEndTime;
	}
}
