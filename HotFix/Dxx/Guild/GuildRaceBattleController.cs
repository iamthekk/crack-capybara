using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.GameTestTools;
using HotFix;
using LocalModels.Bean;
using Proto.GuildRace;

namespace Dxx.Guild
{
	public class GuildRaceBattleController
	{
		public static GuildRaceBattleController Instance
		{
			get
			{
				return GuildRaceBattleController.mIns;
			}
		}

		public static void Create()
		{
			GuildRaceBattleController.mIns = new GuildRaceBattleController();
			GuildRaceBattleController.mIns.InitCtrl();
		}

		public static void Destroy()
		{
			GuildRaceBattleController.mIns = null;
		}

		public GuildSDKManager SDK
		{
			get
			{
				return this.mGuildSDK;
			}
		}

		public GuildActivityRace Race
		{
			get
			{
				return this.SDK.GuildActivity.GuildRace;
			}
		}

		public int SessionID
		{
			get
			{
				GuildActivityRace race = this.Race;
				if (race == null)
				{
					return 0;
				}
				return race.SeasonID;
			}
		}

		public ulong SessionStartTime
		{
			get
			{
				GuildActivityRace race = this.Race;
				if (race == null)
				{
					return 0UL;
				}
				return race.SeasonStartTime;
			}
		}

		public ulong SessionEndTime
		{
			get
			{
				GuildActivityRace race = this.Race;
				if (race == null)
				{
					return 0UL;
				}
				return race.SeasonEndTime;
			}
		}

		public bool IsSeasonAllOver { get; private set; }

		public List<GuildRaceGuildVSRecord> AllRecords
		{
			get
			{
				return this.mAllRecords;
			}
		}

		public string GuildName1
		{
			get
			{
				if (this.mRecord == null || this.mRecord.Guild1 == null || this.mRecord.Guild1.ShareData == null)
				{
					return "";
				}
				return this.mRecord.Guild1.ShareData.GuildShowName;
			}
		}

		public string GuildName2
		{
			get
			{
				if (this.mRecord == null || this.mRecord.Guild2 == null || this.mRecord.Guild2.ShareData == null)
				{
					return "";
				}
				return this.mRecord.Guild2.ShareData.GuildShowName;
			}
		}

		public GuildRaceStagePart CurrentRaceStage
		{
			get
			{
				return this.mCurrentRaceStage;
			}
		}

		public GuildRaceStageKind CurrentRaceKind
		{
			get
			{
				if (this.mCurrentRaceStage == null)
				{
					return GuildRaceStageKind.SeasonOver;
				}
				return this.mCurrentRaceStage.StageKind;
			}
		}

		public double CurStageLeftSec
		{
			get
			{
				return this.mCurStageLeftSec;
			}
		}

		public int MyGuildCurScore
		{
			get
			{
				return this.GetCurrentScore();
			}
		}

		public bool HasGetBattleRecord
		{
			get
			{
				return this.mRecord != null;
			}
		}

		public GuildRaceBattleProcess BattleProcess
		{
			get
			{
				return this.mBattleProcess;
			}
		}

		public void InitCtrl()
		{
			this.mBattleProcess = null;
			this.mRecord = null;
			this.mGuildSDK = GuildSDKManager.Instance;
			this.mSessionTab = GuildProxy.Table.GetRaceOpenTimeTable(this.SessionID);
			this.BuildSessionPart();
			this.CheckCurrentRaceStage();
		}

		public void SetGuildVSRecord(int day, GuildRaceGuildVSRecord record)
		{
			this.mRecordDay = day;
			this.mRecord = record;
			this.mBattleProcess = null;
			this.CheckCurrentRaceStage();
		}

		private void BuildSessionPart()
		{
			if (this.mSessionTab == null)
			{
				return;
			}
			this.mRaceStageList.Clear();
			ulong num = this.SessionStartTime;
			for (int i = 0; i < 14; i++)
			{
				GuildRaceStagePart guildRaceStagePart = this.MakeStage(i + 1);
				guildRaceStagePart.StartTime = num;
				num = guildRaceStagePart.EndTime;
				this.mRaceStageList.Add(guildRaceStagePart);
			}
		}

		public void CheckCurrentRaceStage()
		{
			ulong num = (ulong)GuildProxy.Net.ServerTime();
			for (int i = 0; i < this.mRaceStageList.Count; i++)
			{
				GuildRaceStagePart guildRaceStagePart = this.mRaceStageList[i];
				if (guildRaceStagePart.StartTime <= num && guildRaceStagePart.EndTime > num)
				{
					this.mCurrentRaceStage = guildRaceStagePart;
					break;
				}
			}
			if (num >= this.SessionEndTime)
			{
				this.IsSeasonAllOver = true;
				return;
			}
			this.IsSeasonAllOver = false;
			if (this.mCurrentRaceStage == null)
			{
				HLog.LogError("CheckCurrentRaceStage current staget is null !!!!");
				return;
			}
			if (this.mCurrentRaceStage != null)
			{
				this.mCurStageLeftSec = this.mCurrentRaceStage.EndTime - num;
			}
			if (this.mCurrentRaceStage.StageKind >= GuildRaceStageKind.Battle)
			{
				if (this.mBattleProcess == null)
				{
					GuildRaceStagePart lastBattleStage = this.mCurrentRaceStage;
					if (this.mCurrentRaceStage.StageKind != GuildRaceStageKind.Battle)
					{
						lastBattleStage = this.GetLastBattleStage();
					}
					if (lastBattleStage != null)
					{
						this.mBattleProcess = new GuildRaceBattleProcess();
						this.mBattleProcess.AllBattleStartTimeTick = lastBattleStage.StartTime;
						this.mBattleProcess.BuildAllProcess(this.mRecord);
						this.mBattleProcess.RecalcNow();
						return;
					}
				}
			}
			else if (this.mBattleProcess != null)
			{
				this.mBattleProcess = null;
			}
		}

		private GuildRaceStagePart GetLastBattleStage()
		{
			GuildRaceStagePart guildRaceStagePart = this.mCurrentRaceStage;
			if (guildRaceStagePart != null && guildRaceStagePart.StageKind == GuildRaceStageKind.Battle)
			{
				return guildRaceStagePart;
			}
			for (int i = this.mRaceStageList.Count - 1; i >= 0; i--)
			{
				GuildRaceStagePart guildRaceStagePart2 = this.mRaceStageList[i];
				if (guildRaceStagePart2 == guildRaceStagePart)
				{
					guildRaceStagePart = null;
				}
				if (guildRaceStagePart2.StageKind == GuildRaceStageKind.Battle && guildRaceStagePart == null)
				{
					return guildRaceStagePart2;
				}
			}
			return null;
		}

		private GuildRaceStagePart MakeStage(int stage)
		{
			GuildRaceStagePart guildRaceStagePart = new GuildRaceStagePart();
			guildRaceStagePart.Stage = stage;
			switch (stage)
			{
			case 1:
				guildRaceStagePart.StageKind = GuildRaceStageKind.GuildApply;
				guildRaceStagePart.BattleDay = 0;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage1;
				break;
			case 2:
				guildRaceStagePart.StageKind = GuildRaceStageKind.GuildMate;
				guildRaceStagePart.BattleDay = 0;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage2;
				break;
			case 3:
				guildRaceStagePart.StageKind = GuildRaceStageKind.UserApply;
				guildRaceStagePart.BattleDay = 1;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage3;
				break;
			case 4:
				guildRaceStagePart.StageKind = GuildRaceStageKind.BattlePrepare;
				guildRaceStagePart.BattleDay = 1;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage4;
				break;
			case 5:
				guildRaceStagePart.StageKind = GuildRaceStageKind.Battle;
				guildRaceStagePart.BattleDay = 1;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage5;
				break;
			case 6:
				guildRaceStagePart.StageKind = GuildRaceStageKind.BattleOver;
				guildRaceStagePart.BattleDay = 1;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage6;
				break;
			case 7:
				guildRaceStagePart.StageKind = GuildRaceStageKind.UserApply;
				guildRaceStagePart.BattleDay = 2;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage7;
				break;
			case 8:
				guildRaceStagePart.StageKind = GuildRaceStageKind.BattlePrepare;
				guildRaceStagePart.BattleDay = 2;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage8;
				break;
			case 9:
				guildRaceStagePart.StageKind = GuildRaceStageKind.Battle;
				guildRaceStagePart.BattleDay = 2;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage9;
				break;
			case 10:
				guildRaceStagePart.StageKind = GuildRaceStageKind.BattleOver;
				guildRaceStagePart.BattleDay = 2;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage10;
				break;
			case 11:
				guildRaceStagePart.StageKind = GuildRaceStageKind.UserApply;
				guildRaceStagePart.BattleDay = 3;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage11;
				break;
			case 12:
				guildRaceStagePart.StageKind = GuildRaceStageKind.BattlePrepare;
				guildRaceStagePart.BattleDay = 3;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage12;
				break;
			case 13:
				guildRaceStagePart.StageKind = GuildRaceStageKind.Battle;
				guildRaceStagePart.BattleDay = 3;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage13;
				break;
			case 14:
				guildRaceStagePart.StageKind = GuildRaceStageKind.SeasonOver;
				guildRaceStagePart.BattleDay = 3;
				guildRaceStagePart.LastTimeMinute = this.mSessionTab.stage14;
				break;
			}
			return guildRaceStagePart;
		}

		public GuildRaceStageKind GetStageKind(int stage)
		{
			switch (stage)
			{
			case 1:
				return GuildRaceStageKind.GuildApply;
			case 2:
				return GuildRaceStageKind.GuildMate;
			case 3:
				return GuildRaceStageKind.UserApply;
			case 4:
				return GuildRaceStageKind.BattlePrepare;
			case 5:
				return GuildRaceStageKind.Battle;
			case 6:
				return GuildRaceStageKind.BattleOver;
			case 7:
				return GuildRaceStageKind.UserApply;
			case 8:
				return GuildRaceStageKind.BattlePrepare;
			case 9:
				return GuildRaceStageKind.Battle;
			case 10:
				return GuildRaceStageKind.BattleOver;
			case 11:
				return GuildRaceStageKind.UserApply;
			case 12:
				return GuildRaceStageKind.BattlePrepare;
			case 13:
				return GuildRaceStageKind.Battle;
			case 14:
				return GuildRaceStageKind.SeasonOver;
			default:
				return GuildRaceStageKind.SeasonOver;
			}
		}

		public int GetStageDay(int stage)
		{
			switch (stage)
			{
			case 3:
			case 4:
			case 5:
			case 6:
				return 1;
			case 7:
			case 8:
			case 9:
			case 10:
				return 2;
			case 11:
			case 12:
			case 13:
			case 14:
				return 3;
			default:
				return 0;
			}
		}

		public int GetStageLastTimeMinute(int stage)
		{
			if (this.mSessionTab == null)
			{
				return 0;
			}
			switch (stage)
			{
			case 1:
				return this.mSessionTab.stage1;
			case 2:
				return this.mSessionTab.stage2;
			case 3:
				return this.mSessionTab.stage3;
			case 4:
				return this.mSessionTab.stage4;
			case 5:
				return this.mSessionTab.stage5;
			case 6:
				return this.mSessionTab.stage6;
			case 7:
				return this.mSessionTab.stage7;
			case 8:
				return this.mSessionTab.stage8;
			case 9:
				return this.mSessionTab.stage9;
			case 10:
				return this.mSessionTab.stage10;
			case 11:
				return this.mSessionTab.stage11;
			case 12:
				return this.mSessionTab.stage12;
			case 13:
				return this.mSessionTab.stage13;
			case 14:
				return this.mSessionTab.stage14;
			default:
				return 0;
			}
		}

		public GuildRaceStagePart GetStagePart(int stage)
		{
			for (int i = 0; i < this.mRaceStageList.Count; i++)
			{
				GuildRaceStagePart guildRaceStagePart = this.mRaceStageList[i];
				if (guildRaceStagePart.Stage == stage)
				{
					return guildRaceStagePart;
				}
			}
			return null;
		}

		public GuildRaceStagePart GetStagePartByTime(ulong time)
		{
			for (int i = 0; i < this.mRaceStageList.Count; i++)
			{
				GuildRaceStagePart guildRaceStagePart = this.mRaceStageList[i];
				if (guildRaceStagePart.StartTime <= time && guildRaceStagePart.EndTime > time)
				{
					return guildRaceStagePart;
				}
			}
			return null;
		}

		public void SetRaceStageList(List<GuildRaceStagePart> list)
		{
			this.mRaceStageList.Clear();
			this.mRaceStageList.AddRange(list);
		}

		public bool IsMyGuildJoinRace()
		{
			return this.SDK.GuildActivity.GuildRace != null && this.SDK.GuildActivity.GuildRace.IsGuildReg;
		}

		public bool IsMeJoinRace()
		{
			return this.SDK.GuildActivity.GuildRace != null && this.SDK.GuildActivity.GuildRace.IsMemberReg;
		}

		public bool IsCanGetMemberList()
		{
			return this.CurrentRaceStage != null && this.CurrentRaceStage.StageKind >= GuildRaceStageKind.UserApply && this.Race.HasRaceGroup && this.IsMyGuildJoinRace();
		}

		public bool IsCanGetOpponentMemberList()
		{
			if (!this.IsMyGuildJoinRace())
			{
				return false;
			}
			if (!this.Race.HasRaceGroup)
			{
				return false;
			}
			if (this.CurrentRaceStage != null)
			{
				GuildRaceStageKind stageKind = this.CurrentRaceStage.StageKind;
				if (stageKind - GuildRaceStageKind.UserApply <= 4)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsCanGetCurDayBattleRecord()
		{
			if (!this.IsMyGuildJoinRace())
			{
				return false;
			}
			if (!this.Race.HasRaceGroup)
			{
				return false;
			}
			if (this.CurrentRaceStage != null)
			{
				GuildRaceStageKind stageKind = this.CurrentRaceStage.StageKind;
				if (stageKind - GuildRaceStageKind.Battle <= 2)
				{
					return true;
				}
			}
			return false;
		}

		public bool HasGetCurDayBattleRecord()
		{
			return this.CurrentRaceStage != null && this.CurrentRaceStage.BattleDay == this.mRecordDay && this.mRecord != null;
		}

		public int GetCurBattleDay()
		{
			if (this.CurrentRaceStage != null)
			{
				return this.CurrentRaceStage.BattleDay;
			}
			return 0;
		}

		public void OnUpdate(float realtime)
		{
			this.mCurStageLeftSec -= (double)realtime;
			if (this.mCurStageLeftSec < 3.0)
			{
				this.CheckCurrentRaceStage();
			}
			if (this.mBattleProcess != null)
			{
				this.mBattleProcess.OnUpdate(realtime);
			}
		}

		public List<GuildRaceUserVSRecord> MakeBattlePlayRecord()
		{
			List<GuildRaceUserVSRecord> list = new List<GuildRaceUserVSRecord>();
			if (this.mRecord == null)
			{
				return list;
			}
			List<GuildRaceUserVSRecord> resultList = this.mRecord.ResultList;
			for (int i = 0; i < resultList.Count; i++)
			{
				list.Add(resultList[i]);
			}
			return list;
		}

		public void LoadMyGuildCurDayBattle(Action<int> callback)
		{
			if (!this.IsCanGetCurDayBattleRecord())
			{
				Action<int> callback2 = callback;
				if (callback2 == null)
				{
					return;
				}
				callback2(-1);
				return;
			}
			else
			{
				int day = this.GetCurBattleDay();
				if (day <= 0)
				{
					Action<int> callback3 = callback;
					if (callback3 == null)
					{
						return;
					}
					callback3(-2);
					return;
				}
				else
				{
					if (!this.HasGetCurDayBattleRecord())
					{
						GuildNetUtil.Guild.DoRequest_GuildRaceGuildRecordRequest(day, delegate(bool result, GuildRaceGuildRecordResponse resp)
						{
							if (result)
							{
								this.mAllRecords = resp.ToGuildVSRecordList();
								if (this.mAllRecords == null)
								{
									this.mAllRecords = new List<GuildRaceGuildVSRecord>();
								}
								this.SetGuildVSRecord(day, this.FindMyGuildRecord());
							}
							else
							{
								HLog.LogError("获取战斗列表失败");
							}
							Action<int> callback5 = callback;
							if (callback5 == null)
							{
								return;
							}
							callback5(result ? 1 : 0);
						});
						return;
					}
					Action<int> callback4 = callback;
					if (callback4 == null)
					{
						return;
					}
					callback4(1);
					return;
				}
			}
		}

		private GuildRaceGuildVSRecord FindMyGuildRecord()
		{
			List<GuildRaceGuildVSRecord> list = this.mAllRecords;
			if (list == null)
			{
				return null;
			}
			GuildRaceGuildVSRecord guildRaceGuildVSRecord = null;
			string guildID = this.SDK.GuildInfo.GuildID;
			for (int i = 0; i < list.Count; i++)
			{
				GuildRaceGuildVSRecord guildRaceGuildVSRecord2 = list[i];
				if (guildRaceGuildVSRecord2 != null && (guildRaceGuildVSRecord2.GuildID1 == guildID || guildRaceGuildVSRecord2.GuildID2 == guildID))
				{
					return guildRaceGuildVSRecord2;
				}
			}
			return guildRaceGuildVSRecord;
		}

		public int GetCurrentScore()
		{
			int num = 0;
			if (this.Race != null && this.Race.GuildRaceInfo != null)
			{
				num = this.Race.GuildRaceInfo.RaceScore;
			}
			if (this.CurrentRaceKind == GuildRaceStageKind.Battle && this.mBattleProcess != null && this.mBattleProcess.CurUserPKIndex >= 0)
			{
				num -= this.mBattleProcess.NotArriveScore;
				if (num < 0)
				{
					num = 0;
				}
			}
			return num;
		}

		[GameTestMethod("排位赛", "调试排位赛战斗", "", 0)]
		private static void DebugBattle()
		{
			if (GuildRaceBattleController.mIns == null)
			{
				return;
			}
			GuildProxy.Net.mDebugServerTime = 0L;
			List<GuildRaceStagePart> list = GuildRaceBattleController.mIns.mRaceStageList;
			for (int i = 0; i < list.Count; i++)
			{
				GuildRaceStagePart guildRaceStagePart = list[i];
				if (guildRaceStagePart.BattleDay == 1 && guildRaceStagePart.StageKind == GuildRaceStageKind.Battle)
				{
					GuildProxy.Net.mDebugServerTime = (long)(guildRaceStagePart.StartTime - (ulong)GameApp.Data.GetDataModule(DataName.LoginDataModule).ServerUTC);
					GuildProxy.Net.mDebugServerTime += 660L;
					GuildProxy.Net.mDebugServerTime -= 30L;
					return;
				}
			}
		}

		[GameTestMethod("排位赛", "测试公会报名红点", "", 0)]
		private static void TestGuildApplyRed()
		{
			if (GuildRaceBattleController.mIns == null)
			{
				return;
			}
			GuildProxy.Net.mDebugServerTime = 0L;
			List<GuildRaceStagePart> list = GuildRaceBattleController.mIns.mRaceStageList;
			for (int i = 0; i < list.Count; i++)
			{
				GuildRaceStagePart guildRaceStagePart = list[i];
				if (guildRaceStagePart.StageKind == GuildRaceStageKind.GuildApply)
				{
					GuildProxy.Net.mDebugServerTime = (long)(guildRaceStagePart.StartTime - (ulong)GameApp.Data.GetDataModule(DataName.LoginDataModule).ServerUTC);
					GuildProxy.Net.mDebugServerTime += 5L;
					break;
				}
			}
			GuildProxy.RedPoint.CalcRedPoint("Guild.Activity.Race", true);
		}

		[GameTestMethod("排位赛", "测试个人报名红点", "", 0)]
		private static void TestUserApplyRed()
		{
			if (GuildRaceBattleController.mIns == null)
			{
				return;
			}
			GuildProxy.Net.mDebugServerTime = 0L;
			List<GuildRaceStagePart> list = GuildRaceBattleController.mIns.mRaceStageList;
			for (int i = 0; i < list.Count; i++)
			{
				GuildRaceStagePart guildRaceStagePart = list[i];
				if (guildRaceStagePart.BattleDay == 1 && guildRaceStagePart.StageKind == GuildRaceStageKind.UserApply)
				{
					GuildProxy.Net.mDebugServerTime = (long)(guildRaceStagePart.StartTime - (ulong)GameApp.Data.GetDataModule(DataName.LoginDataModule).ServerUTC);
					GuildProxy.Net.mDebugServerTime += 5L;
					break;
				}
			}
			GuildSDKManager.Instance.GuildActivity.GuildRace.GuildRaceInfo.IsGuildReg = true;
			GuildProxy.RedPoint.CalcRedPoint("Guild.Activity.Race", true);
		}

		[GameTestMethod("排位赛", "测试赛季结束", "", 0)]
		private static void TestSeaSonOver()
		{
			if (GuildRaceBattleController.mIns == null)
			{
				return;
			}
			GuildProxy.Net.mDebugServerTime = 0L;
			List<GuildRaceStagePart> list = GuildRaceBattleController.mIns.mRaceStageList;
			long serverUTC = GameApp.Data.GetDataModule(DataName.LoginDataModule).ServerUTC;
			for (int i = 0; i < list.Count; i++)
			{
				GuildRaceStagePart guildRaceStagePart = list[i];
				if (guildRaceStagePart.StageKind == GuildRaceStageKind.SeasonOver)
				{
					GuildProxy.Net.mDebugServerTime = (long)(guildRaceStagePart.EndTime - (ulong)serverUTC);
					GuildProxy.Net.mDebugServerTime -= 10L;
					break;
				}
			}
			GuildRaceBattleController.mIns.CheckCurrentRaceStage();
		}

		public const int RaceStageCount = 14;

		private static GuildRaceBattleController mIns;

		private GuildSDKManager mGuildSDK;

		private GuildRace_opentime mSessionTab;

		private GuildRaceGuildVSRecord mRecord;

		private int mRecordDay;

		private List<GuildRaceGuildVSRecord> mAllRecords = new List<GuildRaceGuildVSRecord>();

		private List<GuildRaceStagePart> mRaceStageList = new List<GuildRaceStagePart>();

		private GuildRaceStagePart mCurrentRaceStage;

		private double mCurStageLeftSec;

		private GuildRaceBattleProcess mBattleProcess;
	}
}
