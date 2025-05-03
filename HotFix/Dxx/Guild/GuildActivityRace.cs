using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	public class GuildActivityRace
	{
		public int SeasonID
		{
			get
			{
				return this.mSeasonID;
			}
		}

		public ulong SeasonStartTime
		{
			get
			{
				return this.mSeasonStartTime;
			}
		}

		public ulong SeasonEndTime
		{
			get
			{
				return this.mSeasonEndTime;
			}
		}

		public int CurrentState
		{
			get
			{
				return this.mCurrentState;
			}
		}

		public bool IsGuildReg
		{
			get
			{
				return this.mGuildRaceInfo != null && this.mGuildRaceInfo.IsGuildReg;
			}
		}

		public bool IsMemberReg { get; private set; }

		public GuildRaceGuild GuildRaceInfo
		{
			get
			{
				return this.mGuildRaceInfo;
			}
		}

		public IList<GuildRaceGuild> AllGuildOfGroup
		{
			get
			{
				return this.mAllGuildOfGroup;
			}
		}

		public int LastRaceDan
		{
			get
			{
				return this.RaceDan;
			}
		}

		public int RaceDan
		{
			get
			{
				if (this.mGuildRaceInfo == null)
				{
					return 0;
				}
				return this.mGuildRaceInfo.RaceDan;
			}
		}

		public bool HasRaceGroup { get; private set; }

		public bool HasGetRaceDataFromServer { get; private set; }

		public void InitBaseInfo(GuildEvent_RaceSetBaseInfo e)
		{
			this.mSeasonID = e.SeasonID;
			this.mSeasonStartTime = e.SeasonStartTime;
			this.mSeasonEndTime = e.SeasonEndTime;
			this.mCurrentState = e.RaceStage;
			this.IsMemberReg = e.IsUserApply;
			this.mGuildRaceInfo = e.MyGuildInfo;
			this.mAllGuildOfGroup.Clear();
			if (e.AllGuild != null)
			{
				this.mAllGuildOfGroup.AddRange(e.AllGuild);
			}
			this.mAllGuildOfGroup.Sort(new Comparison<GuildRaceGuild>(GuildRaceGuild.SortRank));
			this.HasRaceGroup = true;
			if (this.mGuildRaceInfo != null && this.mGuildRaceInfo.IsGuildReg && this.mCurrentState >= 3 && this.mAllGuildOfGroup.Count <= 0)
			{
				this.HasRaceGroup = false;
			}
			this.HasGetRaceDataFromServer = true;
		}

		public void ChangeApplyState(GuildEvent_RaceApply e)
		{
			if (this.mGuildRaceInfo != null && e.IsGuildApply != -1)
			{
				this.mGuildRaceInfo.IsGuildReg = e.IsGuildApply == 1;
			}
			if (e.IsUserApply != -1)
			{
				this.IsMemberReg = e.IsUserApply == 1;
			}
		}

		private int mSeasonID;

		private ulong mSeasonStartTime;

		private ulong mSeasonEndTime;

		private int mCurrentState;

		private GuildRaceGuild mGuildRaceInfo;

		private List<GuildRaceGuild> mAllGuildOfGroup = new List<GuildRaceGuild>();
	}
}
