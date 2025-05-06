using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Dxx.Guild
{
	public static class GuildBossInfoEx
	{
		public static GuildBossData GetCurrentBoss(this GuildBossInfo info)
		{
			return info.BossData;
		}

		public static List<GuildBossChallengeRecord> GetChallengeRecords(this GuildBossInfo info, int count)
		{
			List<GuildBossChallengeRecord> list = new List<GuildBossChallengeRecord>();
			int num = info.ChallengeRecords.Count - 1;
			while (num >= 0 && list.Count < count)
			{
				list.Insert(0, info.ChallengeRecords[num]);
				num--;
			}
			return list;
		}

		public static GuildBossKillBox GetCurrentBox(this GuildBossInfo info)
		{
			GuildBossKillBox guildBossKillBox = null;
			if (info.KillBoxList.Count == 0)
			{
				return guildBossKillBox;
			}
			for (int i = 0; i < info.KillBoxList.Count; i++)
			{
				if (info.KillBoxList[i].boxState == GuildBossKillBox.GuildBossKillBoxState.CanGetReward)
				{
					guildBossKillBox = info.KillBoxList[i];
					break;
				}
			}
			if (guildBossKillBox == null)
			{
				for (int j = 0; j < info.KillBoxList.Count; j++)
				{
					if (info.KillBoxList[j].boxState == GuildBossKillBox.GuildBossKillBoxState.Undone)
					{
						guildBossKillBox = info.KillBoxList[j];
						break;
					}
				}
			}
			if (guildBossKillBox == null)
			{
				guildBossKillBox = info.KillBoxList[info.KillBoxList.Count - 1];
			}
			return guildBossKillBox;
		}

		public static List<GuildBossKillBox> GetShowBoxList(this GuildBossInfo info)
		{
			List<GuildBossKillBox> list = new List<GuildBossKillBox>();
			GuildBossKillBox currentBox = info.GetCurrentBox();
			int num = info.KillBoxList.IndexOf(currentBox);
			list.Add(currentBox);
			for (int i = 0; i < 3; i++)
			{
				int num2 = num - i - 1;
				int num3 = num + i + 1;
				if (num2 >= 0)
				{
					list.Insert(0, info.KillBoxList[num2]);
				}
				if (num3 < info.KillBoxList.Count)
				{
					list.Add(info.KillBoxList[num3]);
				}
			}
			return list;
		}

		public static bool IsFirstBox(this GuildBossInfo info, GuildBossKillBox box)
		{
			return info.KillBoxList.IndexOf(box) == 0;
		}

		public static bool IsLastBox(this GuildBossInfo info, GuildBossKillBox box)
		{
			return info.KillBoxList.IndexOf(box) == info.KillBoxList.Count - 1;
		}

		public static int GetBoxIndex(this GuildBossInfo info, GuildBossKillBox box)
		{
			return info.KillBoxList.IndexOf(box);
		}

		public static GuildBOSS_guildBoss GetCurrentBossShowModel(this GuildBossInfo info)
		{
			if (info == null)
			{
				return null;
			}
			if (info.BossData == null)
			{
				return null;
			}
			GuildBOSS_guildBossStep guildBossStepTable = GuildProxy.Table.GetGuildBossStepTable(info.BossData.BossStep);
			if (guildBossStepTable == null)
			{
				return null;
			}
			GuildBOSS_guildBoss guildBossTable = GuildProxy.Table.GetGuildBossTable(guildBossStepTable.BossId);
			if (guildBossTable == null)
			{
				return null;
			}
			return guildBossTable;
		}
	}
}
