using System;
using Framework.Logic;

namespace Dxx.Guild.GuildUIDatas
{
	public class GuildUIData_RaceBattleMatch
	{
		public ulong SeasonEndTime { get; private set; }

		public void Init()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			this.ReadHasShowMatch();
		}

		public void DeInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
		}

		public void ReadHasShowMatch()
		{
			string userString = Utility.PlayerPrefs.GetUserString("Guild_ShowRaceMatch", "");
			if (!string.IsNullOrEmpty(userString))
			{
				string[] array = userString.Split(',', StringSplitOptions.None);
				if (array.Length >= 2)
				{
					ulong num;
					if (ulong.TryParse(array[0], out num))
					{
						this.SeasonEndTime = num;
					}
					else
					{
						this.SeasonEndTime = 0UL;
					}
					int num2;
					if (int.TryParse(array[1], out num2))
					{
						this.ShownDay = num2;
						return;
					}
					this.ShownDay = 0;
				}
			}
		}

		public bool CanShowMatch(int day)
		{
			this.ReadHasShowMatch();
			GuildSDKManager instance = GuildSDKManager.Instance;
			if (!instance.GuildInfo.HasGuild)
			{
				return false;
			}
			GuildActivityRace guildRace = instance.GuildActivity.GuildRace;
			return guildRace != null && (guildRace.SeasonEndTime > this.SeasonEndTime || guildRace.SeasonEndTime != this.SeasonEndTime || day > this.ShownDay || (guildRace.SeasonEndTime == this.SeasonEndTime && day > this.ShownDay));
		}

		public void SaveMatchDay(int day)
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
			this.ShownDay = day;
			string text = string.Format("{0},{1}", this.SeasonEndTime, this.ShownDay);
			Utility.PlayerPrefs.SetUserString("Guild_ShowRaceMatch", text.ToString());
		}

		public int ShownDay;
	}
}
