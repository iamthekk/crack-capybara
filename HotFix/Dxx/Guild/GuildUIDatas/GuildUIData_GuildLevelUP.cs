using System;
using Framework.Logic;

namespace Dxx.Guild.GuildUIDatas
{
	public class GuildUIData_GuildLevelUP
	{
		public int HasShowLevelUpLevel { get; private set; }

		public string HasShowLevelUpGuildID { get; private set; } = "";

		public void Init()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			if (@event != null)
			{
				@event.RegisterEvent(21, new GuildHandlerEvent(this.OnGuildClearLevelUpFlag));
			}
			this.ReadHasShowLevelUp();
		}

		public void DeInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			if (@event != null)
			{
				@event.UnRegisterEvent(21, new GuildHandlerEvent(this.OnGuildClearLevelUpFlag));
			}
		}

		private void OnGuildClearLevelUpFlag(int type, GuildBaseEvent eventArgs)
		{
			GuildInfoDataModule guildInfo = GuildSDKManager.Instance.GuildInfo;
			if (guildInfo.HasGuild)
			{
				this.SetHasShowLevelUp(guildInfo.GuildData.GuildLevel);
			}
		}

		public void SetHasShowLevelUp(int level)
		{
			if (GuildSDKManager.Instance.GuildInfo.HasGuild)
			{
				this.HasShowLevelUpGuildID = GuildSDKManager.Instance.GuildInfo.GuildID;
				this.HasShowLevelUpLevel = level;
				string text = string.Format("{0},{1}", this.HasShowLevelUpGuildID, this.HasShowLevelUpLevel);
				Utility.PlayerPrefs.SetUserString("Guild_ShowLevelUp", text);
			}
		}

		public void ReadHasShowLevelUp()
		{
			string userString = Utility.PlayerPrefs.GetUserString("Guild_ShowLevelUp", "");
			if (!string.IsNullOrEmpty(userString))
			{
				string[] array = userString.Split(',', StringSplitOptions.None);
				if (array.Length >= 2)
				{
					this.HasShowLevelUpGuildID = array[0];
					int num;
					if (int.TryParse(array[1], out num))
					{
						this.HasShowLevelUpLevel = num;
					}
					else
					{
						this.HasShowLevelUpLevel = 0;
					}
				}
			}
			if (string.IsNullOrEmpty(this.HasShowLevelUpGuildID) && GuildSDKManager.Instance.GuildInfo.HasGuild)
			{
				this.HasShowLevelUpGuildID = GuildSDKManager.Instance.GuildInfo.GuildID;
			}
		}

		public bool HasShowLevelUp(int level)
		{
			this.ReadHasShowLevelUp();
			string text = "";
			if (GuildSDKManager.Instance.GuildInfo.HasGuild)
			{
				text = GuildSDKManager.Instance.GuildInfo.GuildID;
			}
			return !string.IsNullOrEmpty(text) && text == this.HasShowLevelUpGuildID && level <= this.HasShowLevelUpLevel;
		}

		public void SetCanShowLevelUpFlag(bool canshow)
		{
			this.CanShowGuildLevelUp = canshow;
		}

		public bool CanShowGuildLevelUp = true;
	}
}
