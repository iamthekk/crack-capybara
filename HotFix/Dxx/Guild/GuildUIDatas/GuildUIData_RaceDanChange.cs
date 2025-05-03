using System;
using Framework.Logic;

namespace Dxx.Guild.GuildUIDatas
{
	public class GuildUIData_RaceDanChange
	{
		public ulong SeasonEndTime { get; private set; }

		public void Init()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			this.ReadHasShowDanChange();
		}

		public void DeInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
		}

		public void ReadHasShowDanChange()
		{
			string userString = Utility.PlayerPrefs.GetUserString("Guild_ShowRaceDanChange", "");
			if (!string.IsNullOrEmpty(userString))
			{
				ulong num;
				if (ulong.TryParse(userString, out num))
				{
					this.SeasonEndTime = num;
					return;
				}
				this.SeasonEndTime = 0UL;
			}
		}

		public bool HasShowRaceDan()
		{
			this.ReadHasShowDanChange();
			GuildSDKManager instance = GuildSDKManager.Instance;
			if (!instance.GuildInfo.HasGuild)
			{
				return false;
			}
			GuildActivityRace guildRace = instance.GuildActivity.GuildRace;
			return guildRace != null && guildRace.SeasonEndTime == this.SeasonEndTime;
		}

		public void SaveCurrentSeason()
		{
			ulong num = 0UL;
			GuildSDKManager instance = GuildSDKManager.Instance;
			if (instance.GuildInfo.HasGuild)
			{
				GuildActivityRace guildRace = instance.GuildActivity.GuildRace;
				if (guildRace != null)
				{
					num = guildRace.SeasonEndTime;
				}
			}
			this.SeasonEndTime = num;
			Utility.PlayerPrefs.SetUserString("Guild_ShowRaceDanChange", this.SeasonEndTime.ToString());
		}
	}
}
