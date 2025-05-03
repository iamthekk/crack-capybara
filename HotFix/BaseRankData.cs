using System;
using Dxx.Guild;
using Framework;
using Proto.ActTime;
using Proto.Common;
using Proto.Guild;
using Proto.LeaderBoard;

namespace HotFix
{
	public class BaseRankData
	{
		public RankUserDto rankUserDto { get; private set; }

		public bool VersionMatched { get; private set; }

		public virtual void SetMyRank(RankType rankType)
		{
			this.RankType = rankType;
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.UserId = dataModule.userId;
			this.NickName = dataModule.NickName;
			this.AvatarId = dataModule.Avatar;
			this.AvatarFrameId = dataModule.AvatarFrame;
			this.TitleId = dataModule.AvatarTitle;
			this.GuildName = dataModule.GetGuildName(false);
			switch (rankType)
			{
			case RankType.WorldBoss:
			{
				WorldBossDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.WorldBossDataModule);
				this.Rank = dataModule2.SelfRank;
				this.RankScore = dataModule2.TotalDamage;
				break;
			}
			case RankType.RogueDungeon:
			{
				RogueDungeonDataModule dataModule3 = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule);
				this.Rank = dataModule3.PlayerRank;
				this.RankScore = (long)((ulong)(dataModule3.CurrentFloorID - 1U));
				this.RankScore = ((this.RankScore < 0L) ? 0L : this.RankScore);
				break;
			}
			case RankType.GuildBossRank:
				this.Rank = GuildSDKManager.Instance.GuildActivity.GuildBoss.GuildRank;
				this.GuildId = GuildSDKManager.Instance.GuildInfo.GuildDetailData.ShareData.GuildID_ULong;
				this.RankScore = GuildSDKManager.Instance.GuildInfo.GuildDetailData.ShareData.GuildPower;
				this.GuildDamage = GuildSDKManager.Instance.GuildActivity.GuildBoss.TotalGuildDamage;
				this.GuildLevel = GuildSDKManager.Instance.GuildInfo.GuildDetailData.ShareData.GuildLevel;
				this.GuildIcon = GuildSDKManager.Instance.GuildInfo.GuildDetailData.ShareData.GuildIcon;
				this.GuildIconBg = GuildSDKManager.Instance.GuildInfo.GuildDetailData.ShareData.GuildIconBg;
				break;
			case RankType.GuildBossSelfRank:
				this.Rank = GuildSDKManager.Instance.GuildActivity.GuildBoss.PersonRank;
				this.GuildId = GuildSDKManager.Instance.GuildInfo.GuildDetailData.ShareData.GuildID_ULong;
				this.RankScore = GuildSDKManager.Instance.GuildInfo.GuildDetailData.ShareData.GuildPower;
				this.GuildDamage = GuildSDKManager.Instance.GuildActivity.GuildBoss.TotalPersonalDamage;
				this.GuildLevel = GuildSDKManager.Instance.GuildInfo.GuildDetailData.ShareData.GuildLevel;
				this.GuildIcon = GuildSDKManager.Instance.GuildInfo.GuildDetailData.ShareData.GuildIcon;
				this.GuildIconBg = GuildSDKManager.Instance.GuildInfo.GuildDetailData.ShareData.GuildIconBg;
				break;
			case RankType.NewWorld:
			{
				NewWorldDataModule dataModule4 = GameApp.Data.GetDataModule(DataName.NewWorldDataModule);
				RankUserDto selfRank = dataModule4.SelfRank;
				this.Rank = ((selfRank != null) ? selfRank.Rank : 0);
				AddAttributeDataModule dataModule5 = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
				this.Power = (ulong)dataModule5.Combat;
				break;
			}
			}
			this.VersionMatched = DxxTools.Game.TryVersionMatch(this);
		}

		public virtual void SetUserRank(RankType rankType, RankUserDto data)
		{
			this.rankUserDto = data;
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			if (data.UserInfo.UserId == dataModule.userId)
			{
				this.SetMyRank(rankType);
				return;
			}
			this.RankType = rankType;
			this.UserId = data.UserInfo.UserId;
			this.NickName = data.UserInfo.NickName;
			this.AvatarId = (int)data.UserInfo.Avatar;
			this.AvatarFrameId = (int)data.UserInfo.AvatarFrame;
			this.TitleId = (int)data.UserInfo.TitleId;
			this.GuildName = ((data.UserInfo.GuildDto != null) ? data.UserInfo.GuildDto.GuildName : "");
			this.Rank = data.Rank;
			this.RankScore = data.Score;
			this.Power = data.UserInfo.Power;
			this.VersionMatched = DxxTools.Game.TryVersionMatch(this);
		}

		public virtual void SetActRank(RankType rankType, ActTimeRankDto data)
		{
			this.VersionMatched = DxxTools.Game.TryVersionMatch(this);
		}

		public virtual void SetGuildRank(RankType rankType, GuildSimpleDto dto, int rank)
		{
			this.RankType = rankType;
			this.AvatarId = (int)dto.Avatar;
			this.AvatarFrameId = (int)dto.AvatarFrame;
			this.TitleId = (int)dto.TitleId;
			this.GuildId = dto.GuildId;
			this.GuildName = dto.GuildName;
			this.Rank = rank;
			this.RankScore = (long)dto.Power;
			this.GuildDamage = (long)dto.Damage;
			this.GuildLevel = (int)dto.Level;
			this.UserId = (long)dto.UserId;
			this.NickName = dto.NickName;
			this.GuildIcon = (int)dto.GuildIcon;
			this.GuildIconBg = (int)dto.GuildIconBg;
			this.VersionMatched = DxxTools.Game.TryVersionMatch(this);
		}

		public virtual void SetRogueRank(RankType rankType, HellRankDto dto, int rank)
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			if (dto.UserId == dataModule.userId)
			{
				this.SetMyRank(rankType);
			}
			else
			{
				this.RankType = rankType;
				this.UserId = dto.UserId;
				this.NickName = dto.NickName;
				this.AvatarId = dto.Avatar;
				this.AvatarFrameId = dto.AvatarFrame;
				this.TitleId = dto.TitleId;
				this.GuildName = dto.GuildName;
				this.VersionMatched = DxxTools.Game.TryVersionMatch(this);
			}
			this.Rank = rank;
			this.RankScore = (long)(dto.Tower / 1000 - 1);
			this.RankScore = ((this.RankScore < 0L) ? 0L : this.RankScore);
		}

		public RankType RankType;

		public long UserId;

		public int Rank;

		public long RankScore;

		public string NickName;

		public int AvatarId;

		public int AvatarFrameId;

		public int TitleId;

		public ulong GuildId;

		public string GuildName;

		public int GuildLevel;

		public long GuildDamage;

		public int GuildIcon;

		public int GuildIconBg;

		public ulong Power;
	}
}
