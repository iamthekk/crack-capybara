using System;
using System.Collections.Generic;
using LocalModels.Bean;

namespace Dxx.Guild
{
	public class GuildRaceBattleProcess
	{
		public float TimeSec
		{
			get
			{
				return this.mTimeSec;
			}
		}

		public float TimeSecMax
		{
			get
			{
				return this.mTimeSecMax;
			}
		}

		public GuildRaceBattleProcess.UserPK CurUserPK
		{
			get
			{
				return this.mCurUserPK;
			}
		}

		public int CurUserPKIndex
		{
			get
			{
				return this.mCurUserPKIndex;
			}
		}

		public GuildRaceBattleProcess.UserBattle CurBattle
		{
			get
			{
				return this.mCurBattle;
			}
		}

		public int CurBattleIndex
		{
			get
			{
				return this.mCurBattleIndex;
			}
		}

		public void BuildAllProcess(GuildRaceGuildVSRecord record)
		{
			if (record == null)
			{
				return;
			}
			List<GuildRaceUserVSRecord> resultList = record.ResultList;
			this.PKList.Clear();
			this.BattleList.Clear();
			for (int i = 0; i < 5; i++)
			{
				GuildRaceUserVSRecord guildRaceUserVSRecord = null;
				if (i < resultList.Count)
				{
					guildRaceUserVSRecord = resultList[i];
				}
				if (guildRaceUserVSRecord == null)
				{
					guildRaceUserVSRecord = new GuildRaceUserVSRecord();
				}
				GuildRaceBattleProcess.UserPK userPK = new GuildRaceBattleProcess.UserPK();
				userPK.Index = i;
				userPK.Record = guildRaceUserVSRecord;
				userPK.User1 = guildRaceUserVSRecord.User1;
				userPK.User2 = guildRaceUserVSRecord.User2;
				float num = 0f;
				for (int j = 0; j < 3; j++)
				{
					GuildRaceBattleProcess.UserBattle userBattle = new GuildRaceBattleProcess.UserBattle();
					userBattle.WaitTimeSec = 60f;
					userBattle.BattleTimeSec = 60f;
					userBattle.TotalTimeSec = userBattle.WaitTimeSec + userBattle.BattleTimeSec;
					userBattle.Index = j;
					num += userBattle.TotalTimeSec;
					if (guildRaceUserVSRecord != null)
					{
						userBattle.User1 = guildRaceUserVSRecord.User1;
						userBattle.User2 = guildRaceUserVSRecord.User2;
						if (guildRaceUserVSRecord.ResultList != null && guildRaceUserVSRecord.ResultList.Count > j)
						{
							userBattle.Result = guildRaceUserVSRecord.ResultList[j];
						}
					}
					userPK.Battles.Add(userBattle);
					this.BattleList.Add(userBattle);
				}
				userPK.TotalTimeSec = num;
				userPK.CalcResult();
				this.PKList.Add(userPK);
			}
		}

		public void OnUpdate(float realtime)
		{
			this.mTimeSec -= realtime;
			if (this.mTimeSec <= 0f)
			{
				this.RecalcNow();
			}
		}

		public void RecalcNow()
		{
			this.ReCalcCurrentBattle();
			this.ReCalcNotArriveScore();
		}

		private void ReCalcCurrentBattle()
		{
			this.mCurUserPK = null;
			this.mCurBattle = null;
			this.IsWaittingTime = false;
			this.AllBattleOver = false;
			this.mCurUserPKIndex = -1;
			this.mCurBattleIndex = -1;
			this.mTimeSecMax = 1f;
			ulong num = (ulong)GuildProxy.Net.ServerTime();
			GuildRaceBattleProcess.UserPK userPK = null;
			float num2 = num - this.AllBattleStartTimeTick;
			float num3 = 0f;
			bool flag = false;
			for (int i = 0; i < this.PKList.Count; i++)
			{
				GuildRaceBattleProcess.UserPK userPK2 = this.PKList[i];
				if (num2 - userPK2.TotalTimeSec <= 0f)
				{
					userPK = userPK2;
					this.mCurUserPKIndex = i;
					break;
				}
				num2 -= userPK2.TotalTimeSec;
				if (num2 >= 0f && i + 1 >= this.PKList.Count)
				{
					this.mCurUserPKIndex = this.PKList.Count;
					flag = true;
				}
			}
			this.AllBattleOver = flag;
			if (userPK == null)
			{
				return;
			}
			this.mCurUserPK = userPK;
			GuildRaceBattleProcess.UserBattle userBattle = null;
			List<GuildRaceBattleProcess.UserBattle> battles = this.mCurUserPK.Battles;
			for (int j = 0; j < battles.Count; j++)
			{
				GuildRaceBattleProcess.UserBattle userBattle2 = battles[j];
				if (num2 - userBattle2.TotalTimeSec < 0f)
				{
					userBattle = userBattle2;
					if (num2 - userBattle2.WaitTimeSec < 0f)
					{
						this.IsWaittingTime = true;
						this.mTimeSecMax = userBattle2.WaitTimeSec;
						num3 = userBattle2.WaitTimeSec - num2;
					}
					else
					{
						num2 -= userBattle2.WaitTimeSec;
						this.IsWaittingTime = false;
						this.mTimeSecMax = userBattle2.BattleTimeSec;
						num3 = userBattle2.BattleTimeSec - num2;
					}
					this.mCurBattleIndex = j;
					break;
				}
				num2 -= userBattle2.TotalTimeSec;
			}
			if (this.mTimeSecMax < 1f)
			{
				this.mTimeSecMax = 1f;
			}
			this.mTimeSec = num3;
			this.mCurBattle = userBattle;
			if (this.mTimeSec <= 0f)
			{
				this.mCurUserPKIndex = this.PKList.Count;
				this.AllBattleOver = true;
			}
		}

		private void ReCalcNotArriveScore()
		{
			int curUserPKIndex = this.CurUserPKIndex;
			if (curUserPKIndex < 0)
			{
				this.NotArriveScore = 0;
				return;
			}
			int num = 0;
			List<GuildRaceBattleProcess.UserPK> pklist = this.PKList;
			string guildID = GuildSDKManager.Instance.GuildInfo.GuildID;
			for (int i = curUserPKIndex; i < pklist.Count; i++)
			{
				GuildRaceBattleProcess.UserPK userPK = pklist[i];
				int num2 = this.CalcMyGuildWinScore(userPK, guildID);
				if (num2 > 0)
				{
					num += num2;
				}
			}
			if (num < 0)
			{
				num = 0;
			}
			this.NotArriveScore = num;
		}

		private int CalcMyGuildWinScore(GuildRaceBattleProcess.UserPK userpk, string guildid)
		{
			if (userpk == null)
			{
				return 0;
			}
			if (userpk.WinUser == null)
			{
				return 0;
			}
			if (userpk.WinUser == userpk.User1 && userpk.User1.GuildID == guildid)
			{
				GuildRace_baseRace raceBaseTable = GuildProxy.Table.GetRaceBaseTable(userpk.User1.Position);
				if (raceBaseTable == null)
				{
					return 0;
				}
				return raceBaseTable.TypeIntArray[1];
			}
			else
			{
				if (userpk.WinUser != userpk.User2 || !(userpk.User2.GuildID == guildid))
				{
					return 0;
				}
				GuildRace_baseRace raceBaseTable2 = GuildProxy.Table.GetRaceBaseTable(userpk.User2.Position);
				if (raceBaseTable2 == null)
				{
					return 0;
				}
				return raceBaseTable2.TypeIntArray[1];
			}
		}

		public const int UserPKListCount = 5;

		public const int UserPKBattleCount = 3;

		public const int UserBattleWaitTimeSec = 60;

		public const int UserBattleBattleTimeSec = 60;

		public List<GuildRaceBattleProcess.UserPK> PKList = new List<GuildRaceBattleProcess.UserPK>();

		public List<GuildRaceBattleProcess.UserBattle> BattleList = new List<GuildRaceBattleProcess.UserBattle>();

		public ulong AllBattleStartTimeTick;

		private float mTimeSec;

		private float mTimeSecMax = 1f;

		public bool IsWaittingTime;

		private GuildRaceBattleProcess.UserPK mCurUserPK;

		private int mCurUserPKIndex;

		private GuildRaceBattleProcess.UserBattle mCurBattle;

		private int mCurBattleIndex;

		public int NotArriveScore;

		public bool AllBattleOver;

		public class UserPK
		{
			public void CalcResult()
			{
				this.WinUser = null;
				if (this.Battles == null || this.Battles.Count <= 0)
				{
					this.WinUser = null;
					return;
				}
				long num = 0L;
				long num2 = 0L;
				long num3 = 0L;
				long num4 = 0L;
				if (this.User1 != null && !this.User1.IsEmptyUser)
				{
					num = this.User1.UserData.UserID;
				}
				if (this.User2 != null && !this.User2.IsEmptyUser)
				{
					num2 = this.User2.UserData.UserID;
				}
				for (int i = 0; i < this.Battles.Count; i++)
				{
					GuildRaceBattleProcess.UserBattle userBattle = this.Battles[i];
					if (userBattle != null && userBattle.Result != null)
					{
						if (userBattle.Result.WinUserID != 0L && userBattle.Result.WinUserID == num)
						{
							num3 += 1L;
						}
						if (userBattle.Result.WinUserID != 0L && userBattle.Result.WinUserID == num2)
						{
							num4 += 1L;
						}
					}
				}
				if (num3 > 0L && num3 > num4)
				{
					this.WinUser = this.User1;
				}
				if (num4 > 0L && num4 > num3)
				{
					this.WinUser = this.User2;
				}
			}

			public GuildRaceMember User1;

			public GuildRaceMember User2;

			public GuildRaceMember WinUser;

			public float TotalTimeSec;

			public GuildRaceUserVSRecord Record;

			public List<GuildRaceBattleProcess.UserBattle> Battles = new List<GuildRaceBattleProcess.UserBattle>();

			public int Index;
		}

		public class UserBattle
		{
			public float WaitTimeSec;

			public float BattleTimeSec;

			public float TotalTimeSec;

			public GuildRaceMember User1;

			public GuildRaceMember User2;

			public int Index;

			public GuildRaceUserVSRecordResult Result;
		}
	}
}
