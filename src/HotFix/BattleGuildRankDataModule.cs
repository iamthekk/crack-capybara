using System;
using Dxx.Guild;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.Common;
using UnityEngine;

namespace HotFix
{
	public class BattleGuildRankDataModule : IDataModule
	{
		public PVPRecordDto Record
		{
			get
			{
				return this.m_record;
			}
		}

		public int Duration
		{
			get
			{
				return this.m_duration;
			}
		}

		public int GetName()
		{
			return 114;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_BattleGuildRank_BattleGuildRankEnter, new HandlerEvent(this.OnBattleEnter));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_BattleGuildRank_BattleGuildRankEnter, new HandlerEvent(this.OnBattleEnter));
		}

		public void Reset()
		{
		}

		private void OnBattleEnter(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsBattleGuildRankEnter eventArgsBattleGuildRankEnter = eventargs as EventArgsBattleGuildRankEnter;
			if (eventArgsBattleGuildRankEnter == null)
			{
				return;
			}
			this.m_record = eventArgsBattleGuildRankEnter.m_record;
			this.m_guildRaceUserVSRecord = eventArgsBattleGuildRankEnter.m_guildRaceUserVSRecord;
		}

		public bool IsWin()
		{
			if (this.Record == null)
			{
				return false;
			}
			if (this.m_guildRaceUserVSRecord == null)
			{
				return false;
			}
			bool flag = GuildSDKManager.Instance.GuildInfo != null && GuildSDKManager.Instance.GuildInfo.HasGuild;
			string guildID = this.GetGuildID(this.m_record.OwnerUser);
			string guildID2 = this.GetGuildID(this.m_record.OtherUser);
			string text = (flag ? GuildSDKManager.Instance.GuildInfo.GuildData.GuildID : string.Empty);
			bool flag2 = this.Record.Result != 0;
			if (string.Equals(guildID, text))
			{
				return flag2;
			}
			if (string.Equals(guildID2, text))
			{
				return !flag2;
			}
			return flag2;
		}

		public string GetGuildName(BattleUserDto userDto)
		{
			if (this.Record == null)
			{
				return string.Empty;
			}
			if (this.m_guildRaceUserVSRecord == null)
			{
				return string.Empty;
			}
			if (userDto.UserId == this.m_guildRaceUserVSRecord.User1.UserData.UserID)
			{
				return this.m_guildRaceUserVSRecord.User1.GuildName;
			}
			if (userDto.UserId == this.m_guildRaceUserVSRecord.User2.UserData.UserID)
			{
				return this.m_guildRaceUserVSRecord.User2.GuildName;
			}
			return string.Empty;
		}

		public string GetGuildID(BattleUserDto userDto)
		{
			if (this.Record == null)
			{
				return string.Empty;
			}
			if (this.m_guildRaceUserVSRecord == null)
			{
				return string.Empty;
			}
			if (userDto.UserId == this.m_guildRaceUserVSRecord.User1.UserData.UserID)
			{
				return this.m_guildRaceUserVSRecord.User1.GuildID;
			}
			if (userDto.UserId == this.m_guildRaceUserVSRecord.User2.UserData.UserID)
			{
				return this.m_guildRaceUserVSRecord.User2.GuildID;
			}
			return string.Empty;
		}

		[SerializeField]
		private PVPRecordDto m_record;

		public GuildRaceUserVSRecord m_guildRaceUserVSRecord;

		[SerializeField]
		private int m_duration = 15;
	}
}
