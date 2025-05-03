using System;
using System.Collections.Generic;
using Dxx.Guild;

namespace HotFix.RedPoint.Calculators
{
	public class RPGuild
	{
		public class Guild_Sign : IRedPointRecordCalculator
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
				GuildSignData guildSignData = instance.GuildInfo.GuildSignData;
				if (guildSignData == null)
				{
					return 0;
				}
				if (guildSignData.SignCount < 1)
				{
					return 1;
				}
				return 0;
			}
		}

		public class Guild_ApplyJoin : IRedPointRecordCalculator
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
				if (!instance.Permission.HasPermission(GuildPermissionKind.ApplyJoin, null))
				{
					return 0;
				}
				if (instance.GuildInfo.ApplyJoinCount <= 0)
				{
					return 0;
				}
				return 1;
			}
		}

		public class Guild_Boss : IRedPointRecordCalculator
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
				if (instance.GuildActivity.GuildBoss == null)
				{
					return 0;
				}
				if (instance.GuildActivity.GuildBoss.GetCurrentBoss() == null)
				{
					return 0;
				}
				if (instance.GuildActivity.GuildBoss.ChallengeCount > 0)
				{
					return 1;
				}
				return 0;
			}
		}

		public class Guild_BossBoxReward : IRedPointRecordCalculator
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
				if (instance.GuildActivity.GuildBoss == null)
				{
					return 0;
				}
				if (instance.GuildActivity.GuildBoss.KillRewardList.Count > 0)
				{
					return 1;
				}
				return 0;
			}
		}

		public class Guild_BossTask : IRedPointRecordCalculator
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
				if (instance.GuildActivity.GuildBoss == null)
				{
					return 0;
				}
				int num = 0;
				List<GuildBossTask> taskBossList = instance.GuildActivity.GuildBoss.TaskBossList;
				for (int i = 0; i < taskBossList.Count; i++)
				{
					if (taskBossList[i].taskState == GuildBossTask.GuildBossTaskState.CanGetReward)
					{
						num = 1;
						break;
					}
				}
				return num;
			}
		}

		public class Guild_Race : IRedPointRecordCalculator
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
				if (instance.GuildActivity.GuildRace == null)
				{
					return 0;
				}
				if (instance.GuildActivity.GuildRace.SeasonID == 0)
				{
					return 0;
				}
				GuildRaceBattleController guildRaceBattleController = new GuildRaceBattleController();
				guildRaceBattleController.InitCtrl();
				GuildRaceStageKind currentRaceKind = guildRaceBattleController.CurrentRaceKind;
				if (currentRaceKind != GuildRaceStageKind.GuildApply)
				{
					if (currentRaceKind != GuildRaceStageKind.UserApply)
					{
						return 0;
					}
					if (!instance.GuildActivity.GuildRace.IsGuildReg)
					{
						return 0;
					}
					if (instance.GuildActivity.GuildRace.IsMemberReg)
					{
						return 0;
					}
					return 1;
				}
				else
				{
					if (!instance.Permission.HasPermission(GuildPermissionKind.GuildActivities, null))
					{
						return 0;
					}
					if (instance.GuildActivity.GuildRace.IsGuildReg)
					{
						return 0;
					}
					return 1;
				}
			}
		}
	}
}
