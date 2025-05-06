using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic.Modules;
using Proto.Common;
using Proto.CrossArena;

namespace HotFix
{
	public class CrossArenaDataModule : IDataModule
	{
		public void UpdateMyRankInfo(CrossArenaRankDto rankDto)
		{
			if (rankDto != null)
			{
				this.Score = rankDto.Score;
				this.Rank = rankDto.RankIndex + 1;
				this.MyRankInfo.SetData(rankDto);
			}
			else
			{
				LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
				this.MyRankInfo.UserID = dataModule.userId;
				this.MyRankInfo.Nick = dataModule.NickName;
				this.MyRankInfo.Avatar = dataModule.Avatar;
				this.MyRankInfo.AvatarFrame = dataModule.AvatarFrame;
				this.MyRankInfo.Score = this.Score;
				AddAttributeDataModule dataModule2 = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
				this.MyRankInfo.Power = (long)dataModule2.Combat;
				this.MyRankInfo.Rank = this.Rank;
			}
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_CrossArena_MyRankInfoUpdate, null);
		}

		public bool HasGroup
		{
			get
			{
				return this.Rank > 0;
			}
		}

		public int GetName()
		{
			return 132;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_DayChange_Arena_DataPull, new HandlerEvent(this.OnDayChangeArenaDataPull));
			manager.RegisterEvent(LocalMessageName.CC_CrossArena_SetInfo, new HandlerEvent(this.OnSetInfo));
			manager.RegisterEvent(LocalMessageName.CC_CrossArena_RefreshChallengeList, new HandlerEvent(this.OnRefreshChallengeList));
			manager.RegisterEvent(LocalMessageName.CC_CrossArena_SaveCurrentDanToLocalCache, new HandlerEvent(this.OnSaveCurrentDanToLocalCache));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_DayChange_Arena_DataPull, new HandlerEvent(this.OnDayChangeArenaDataPull));
			manager.UnRegisterEvent(LocalMessageName.CC_CrossArena_SetInfo, new HandlerEvent(this.OnSetInfo));
			manager.UnRegisterEvent(LocalMessageName.CC_CrossArena_RefreshChallengeList, new HandlerEvent(this.OnRefreshChallengeList));
			manager.UnRegisterEvent(LocalMessageName.CC_CrossArena_SaveCurrentDanToLocalCache, new HandlerEvent(this.OnSaveCurrentDanToLocalCache));
		}

		public void Reset()
		{
			this.Dan = 0;
			this.Rank = 0;
			this.Score = 0;
			this.TotalMemberCount = 0;
			this.RefreshOppListCount = -1;
			this.OppList.Clear();
			this.LocalCache = new CrossArenaLocalCache();
		}

		private void OnSaveLastDanCache(int seasonId)
		{
			CrossArenaProgress crossArenaProgress = new CrossArenaProgress();
			crossArenaProgress.BuildProgress();
			this.LocalCache.Save(this.Dan, crossArenaProgress.SeasonCloseTime, seasonId);
		}

		private void OnDayChangeArenaDataPull(object sender, int type, BaseEventArgs eventArgs)
		{
			NetworkUtils.CrossArena.DoCrossArenaGetInfoRequest(delegate(bool result, CrossArenaGetInfoResponse resp)
			{
			});
		}

		private void OnSetInfo(object sender, int type, BaseEventArgs eventArgs)
		{
			this.LocalCache.ReadLocalData();
			EventArgsSetCrossArenaInfo eventArgsSetCrossArenaInfo = eventArgs as EventArgsSetCrossArenaInfo;
			if (eventArgsSetCrossArenaInfo != null)
			{
				this.Dan = (int)eventArgsSetCrossArenaInfo.Dan;
				this.Rank = (int)eventArgsSetCrossArenaInfo.Rank;
				this.Score = (int)eventArgsSetCrossArenaInfo.Score;
				this.CurSeasonId = (int)eventArgsSetCrossArenaInfo.CurSeasonId;
				this.TotalMemberCount = (int)eventArgsSetCrossArenaInfo.TotalMemberCount;
				this.GroupId = (int)eventArgsSetCrossArenaInfo.GroupId;
				if (this.TotalMemberCount < 1)
				{
					this.TotalMemberCount = 1;
				}
			}
			if (!this.LocalCache.HasLastDanCache && this.CurSeasonId > 0)
			{
				this.OnSaveLastDanCache(this.CurSeasonId);
			}
			this.UpdateMyRankInfo(null);
		}

		private void OnRefreshChallengeList(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsCrossArenaRefreshOppList eventArgsCrossArenaRefreshOppList = eventArgs as EventArgsCrossArenaRefreshOppList;
			if (eventArgsCrossArenaRefreshOppList != null)
			{
				this.OppList = eventArgsCrossArenaRefreshOppList.Members;
				if (this.OppList == null)
				{
					this.OppList = new List<CrossArenaRankMember>();
				}
				this.RefreshOppListCount = eventArgsCrossArenaRefreshOppList.RefreshCount;
			}
		}

		private void OnSaveCurrentDanToLocalCache(object sender, int type, BaseEventArgs eventArgs)
		{
			CrossArenaDataModule dataModule = GameApp.Data.GetDataModule(DataName.CrossArenaDataModule);
			this.OnSaveLastDanCache(dataModule.CurSeasonId);
		}

		public static string GetCrossArenaDanName(int dan)
		{
			if (dan <= 0)
			{
				dan = 1;
			}
			return Singleton<LanguageManager>.Instance.GetInfoByID((15021 + dan - 1).ToString());
		}

		public static string GetCrossArenaDanName(int dan, LanguageType languageType)
		{
			if (dan <= 0)
			{
				dan = 1;
			}
			return Singleton<LanguageManager>.Instance.GetInfoByID((15021 + dan - 1).ToString(), new object[] { languageType });
		}

		public bool IsInDayCloseTime(out int openHour, out int closeHour)
		{
			closeHour = 0;
			openHour = 6;
			CrossArenaProgress crossArenaProgress = new CrossArenaProgress();
			crossArenaProgress.BuildProgress();
			if (DxxTools.Time.ServerTimestamp < crossArenaProgress.DailyOpenTime)
			{
				int hours = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
				DateTime dateTime = DxxTools.Time.UnixTimestampToDateTime((double)crossArenaProgress.DailyOpenTime);
				DateTime dateTime2 = DxxTools.Time.UnixTimestampToDateTime((double)crossArenaProgress.DailyCloseTime);
				openHour = dateTime.Hour + hours;
				if (openHour > 24)
				{
					openHour -= 24;
				}
				closeHour = dateTime2.Hour + hours;
				if (closeHour > 24)
				{
					closeHour -= 24;
				}
				return true;
			}
			return false;
		}

		public int Dan = 1;

		public int Rank;

		public int Score;

		public int TotalMemberCount;

		public int CurSeasonId;

		public int GroupId;

		public CrossArenaRankMember MyRankInfo = new CrossArenaRankMember();

		public int RefreshOppListCount = -1;

		public List<CrossArenaRankMember> OppList = new List<CrossArenaRankMember>();

		public CrossArenaLocalCache LocalCache = new CrossArenaLocalCache();
	}
}
