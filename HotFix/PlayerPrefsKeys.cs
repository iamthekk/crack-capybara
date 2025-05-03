using System;
using System.Collections.Generic;
using System.Text;
using Framework;
using Framework.Logic;
using UnityEngine;

namespace HotFix
{
	public class PlayerPrefsKeys
	{
		public static int GetUserSupplyGift_Record(int itemId)
		{
			return Utility.PlayerPrefs.GetUserInt(string.Format("{0}_SupplyGiftRecord", itemId), 0);
		}

		public static void SetPlayerSupplyGift_Record(int itemId, int progress)
		{
			Utility.PlayerPrefs.SetUserInt(string.Format("{0}_SupplyGiftRecord", itemId), progress);
		}

		public static void SetPlayerExitTime(long timeStamp)
		{
			Utility.PlayerPrefs.SetUserString("PlayerQuitTimeStamp", timeStamp.ToString());
		}

		public static long GetPlayerLastExitTime()
		{
			return long.Parse(Utility.PlayerPrefs.GetUserString("PlayerQuitTimeStamp", "0"));
		}

		public static string ChapterFightTime(int chapterId)
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			return string.Format("{0}_ChapterFightTime_{1}", dataModule.userId, chapterId);
		}

		public static int GetChapterFightTime(int chapterId)
		{
			return Utility.PlayerPrefs.GetInt(PlayerPrefsKeys.ChapterFightTime(chapterId), 0);
		}

		public static void SaveChapterFightTime(int chapterId, int time)
		{
			Utility.PlayerPrefs.SetInt(PlayerPrefsKeys.ChapterFightTime(chapterId), time);
		}

		public static string BattleRecordEventKey()
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			return string.Format("{0}_BattleRecordEvent", dataModule.userId);
		}

		public static void SaveBattleRecordEvent(string value)
		{
			Utility.PlayerPrefs.SetString(PlayerPrefsKeys.BattleRecordEventKey(), value);
		}

		public static string GetBattleRecordEvent()
		{
			return Utility.PlayerPrefs.GetString(PlayerPrefsKeys.BattleRecordEventKey(), "");
		}

		public static string BattleRecordPlayerKey()
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			return string.Format("{0}_BattleRecordPlayer", dataModule.userId);
		}

		public static void SaveBattleRecordPlayer(string value)
		{
			Utility.PlayerPrefs.SetString(PlayerPrefsKeys.BattleRecordPlayerKey(), value);
		}

		public static string GetBattleRecordPlayer()
		{
			return Utility.PlayerPrefs.GetString(PlayerPrefsKeys.BattleRecordPlayerKey(), "");
		}

		public static string BattleRecordUIKey(int stage, int index)
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			return string.Format("{0}_BattleRecordUI_{1}_{2}", dataModule.userId, stage, index);
		}

		public static void SaveBattleRecordUI(int stage, int index, string value)
		{
			Utility.PlayerPrefs.SetString(PlayerPrefsKeys.BattleRecordUIKey(stage, index), value);
		}

		public static string GetBattleRecordUI(int stage, int index)
		{
			return Utility.PlayerPrefs.GetString(PlayerPrefsKeys.BattleRecordUIKey(stage, index), "");
		}

		public static string BattleRecordUIStageKey(int stage)
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			return string.Format("{0}_BattleRecordUIStage_{1}", dataModule.userId, stage);
		}

		public static void SaveBattleRecordUIStage(int stage, int value)
		{
			Utility.PlayerPrefs.SetInt(PlayerPrefsKeys.BattleRecordUIStageKey(stage), value);
		}

		public static int GetBattleRecordUIStage(int stage)
		{
			return Utility.PlayerPrefs.GetInt(PlayerPrefsKeys.BattleRecordUIStageKey(stage), 0);
		}

		public static string SweepRecordKey()
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			return string.Format("{0}_SweepRecord", dataModule.userId);
		}

		public static void SaveSweepRecord(string value)
		{
			Utility.PlayerPrefs.SetString(PlayerPrefsKeys.SweepRecordKey(), value);
		}

		public static string GetSweepRecord()
		{
			return Utility.PlayerPrefs.GetString(PlayerPrefsKeys.SweepRecordKey(), "");
		}

		public static bool GetIsRateShow()
		{
			return PlayerPrefs.GetInt("IsRateShow", 0) == 1;
		}

		public static void SaveIsRateShow(bool value)
		{
			PlayerPrefs.SetInt("IsRateShow", value ? 1 : 0);
		}

		public static void SetIsSelectJoinGuild(string value)
		{
			Utility.PlayerPrefs.SetString("IsSelectJoinGuild", value);
		}

		public static string GetIsSelectJoinGuild()
		{
			return Utility.PlayerPrefs.GetString("IsSelectJoinGuild");
		}

		public static void SetGuildBossSeason(string value)
		{
			Utility.PlayerPrefs.SetString("GuildBossSeason", value);
		}

		public static string GetGuildBossSeason()
		{
			return Utility.PlayerPrefs.GetString("GuildBossSeason");
		}

		public static void SetGuildBossDan(string value)
		{
			Utility.PlayerPrefs.SetString("GuildBossDan", value);
		}

		public static string GetGuildBossDan()
		{
			return Utility.PlayerPrefs.GetString("GuildBossDan");
		}

		public static void SetTalentLegacyNode(string value)
		{
			Utility.PlayerPrefs.SetUserString("TalentLegacyNode", value);
		}

		public static string GetTalentLegacyNode()
		{
			return Utility.PlayerPrefs.GetUserString("TalentLegacyNode", "");
		}

		public static void SetTalentLegacyNodeFinish(string value)
		{
			Utility.PlayerPrefs.SetUserString("TalentLegacyNodeFinish", value);
		}

		public static string GetTalentLegacyNodeFinish()
		{
			return Utility.PlayerPrefs.GetUserString("TalentLegacyNodeFinish", "");
		}

		public static void SetMountRideRed(string value)
		{
			Utility.PlayerPrefs.SetUserString("MountRideRed", value);
		}

		public static string GetMountRideRed()
		{
			return Utility.PlayerPrefs.GetUserString("MountRideRed", "");
		}

		public static void CacheMiningPosition(int stage, List<int> posList)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < posList.Count; i++)
			{
				stringBuilder.Append(posList[i]);
				if (i != posList.Count - 1)
				{
					stringBuilder.Append("|");
				}
			}
			LoginDataModule dataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			Utility.PlayerPrefs.SetString(string.Format("{0}_CacheMiningPos_{1}", dataModule.userId, stage), stringBuilder.ToString());
		}

		public static void ClearCacheMiningPos(int stage)
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			Utility.PlayerPrefs.DeleteKey(string.Format("{0}_CacheMiningPos_{1}", dataModule.userId, stage));
		}

		public static string GetCacheMiningPosition(int stage)
		{
			LoginDataModule dataModule = GameApp.Data.GetDataModule<LoginDataModule>(105);
			return Utility.PlayerPrefs.GetString(string.Format("{0}_CacheMiningPos_{1}", dataModule.userId, stage), "");
		}

		public static void SetChainPacksPop(string value, int itemId)
		{
			Utility.PlayerPrefs.SetUserString(HLog.StringBuilderFormat("ChainPacksPop_{0}", new object[] { itemId }), value);
		}

		public static string GetChainPacksPop(int itemId)
		{
			return Utility.PlayerPrefs.GetUserString(HLog.StringBuilderFormat("ChainPacksPop_{0}", new object[] { itemId }), "");
		}

		private const string Chapter_Fight_Time = "{0}_ChapterFightTime_{1}";

		private const string Battle_Record_Event = "{0}_BattleRecordEvent";

		private const string Battle_Record_Player = "{0}_BattleRecordPlayer";

		private const string Battle_Record_UI = "{0}_BattleRecordUI_{1}_{2}";

		private const string Battle_Record_UI_Stage = "{0}_BattleRecordUIStage_{1}";

		private const string Sweep_Record = "{0}_SweepRecord";

		private const string SupplyGift_Record = "{0}_SupplyGiftRecord";

		private const string PlayerQuitTimeStamp = "PlayerQuitTimeStamp";

		private const string IsRateShow = "IsRateShow";

		private const string IsSelectJoinGuild = "IsSelectJoinGuild";

		private const string GuildBossSeason = "GuildBossSeason";

		private const string GuildBossDan = "GuildBossDan";

		private const string StudyTalentLegacyNode = "TalentLegacyNode";

		private const string StudyTalentLegacyNodeFinsih = "TalentLegacyNodeFinish";

		private const string CacheMiningPos = "{0}_CacheMiningPos_{1}";

		private const string MountRideRed = "MountRideRed";

		private const string ChainPacksPop = "ChainPacksPop_{0}";
	}
}
