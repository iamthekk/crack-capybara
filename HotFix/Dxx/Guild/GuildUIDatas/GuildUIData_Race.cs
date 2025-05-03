using System;

namespace Dxx.Guild.GuildUIDatas
{
	public class GuildUIData_Race
	{
		public void Init()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			if (@event != null)
			{
				@event.RegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
				@event.RegisterEvent(401, new GuildHandlerEvent(this.OnRaceSetBaseInfo));
				@event.RegisterEvent(402, new GuildHandlerEvent(this.OnRaceApplyChange));
			}
		}

		public void DeInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			if (@event != null)
			{
				@event.UnRegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
				@event.RegisterEvent(401, new GuildHandlerEvent(this.OnRaceSetBaseInfo));
				@event.UnRegisterEvent(402, new GuildHandlerEvent(this.OnRaceApplyChange));
			}
		}

		private void OnLoginSuccess(int type, GuildBaseEvent eventArgs)
		{
			if (!GuildSDKManager.Instance.GuildInfo.HasGuild)
			{
				this.mCachedRaceSeasonEndTime = 0UL;
				return;
			}
			this.CheckReGetRaceInfo();
		}

		private void OnRaceSetBaseInfo(int type, GuildBaseEvent eventArgs)
		{
			if (!GuildSDKManager.Instance.GuildInfo.HasGuild)
			{
				this.mCachedRaceSeasonEndTime = 0UL;
				return;
			}
			GuildActivityRace guildRace = GuildSDKManager.Instance.GuildActivity.GuildRace;
			if (guildRace != null)
			{
				this.mCachedRaceSeasonEndTime = guildRace.SeasonEndTime;
				return;
			}
			this.mCachedRaceSeasonEndTime = 0UL;
			this.CheckReGetRaceInfo();
		}

		private void OnRaceApplyChange(int type, GuildBaseEvent eventArgs)
		{
			GuildProxy.RedPoint.CalcRedPoint("Guild.Activity.Race", true);
		}

		private void CheckReGetRaceInfo()
		{
			if (this.mCachedRaceSeasonEndTime == 0UL)
			{
				this.mCachedRaceSeasonEndTime = 1UL;
				GuildNetUtil.Guild.DoRequest_GuildRaceInfoRequest(null);
				return;
			}
		}

		private ulong mCachedRaceSeasonEndTime;
	}
}
