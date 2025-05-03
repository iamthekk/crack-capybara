using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	[GuildInternalModule(10)]
	public class GuildUserDataModule : IGuildModule
	{
		public int ModuleName
		{
			get
			{
				return 10;
			}
		}

		public long UserID { get; private set; }

		public string DeviceID { get; private set; }

		public GuildUserShareData MyUserData
		{
			get
			{
				return this.mMyUserData;
			}
		}

		public bool Init(GuildInitConfig config)
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.RegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.RegisterEvent(18, new GuildHandlerEvent(this.OnGuildSignSetData));
			@event.RegisterEvent(9, new GuildHandlerEvent(this.OnSetSelfUserData));
			@event.RegisterEvent(14, new GuildHandlerEvent(this.OnGuildMemberDataChange));
			@event.RegisterEvent(20, new GuildHandlerEvent(this.OnGuildTaskSetData));
			return true;
		}

		public void UnInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.UnRegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.UnRegisterEvent(18, new GuildHandlerEvent(this.OnGuildSignSetData));
			@event.UnRegisterEvent(9, new GuildHandlerEvent(this.OnSetSelfUserData));
			@event.UnRegisterEvent(14, new GuildHandlerEvent(this.OnGuildMemberDataChange));
			@event.UnRegisterEvent(20, new GuildHandlerEvent(this.OnGuildTaskSetData));
		}

		private void OnLoginSuccess(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_LoginSuccess guildEvent_LoginSuccess = eventArgs as GuildEvent_LoginSuccess;
			if (guildEvent_LoginSuccess != null && guildEvent_LoginSuccess.MyGuildShareDetail != null)
			{
				IList<GuildUserShareData> members = guildEvent_LoginSuccess.MyGuildShareDetail.Members;
				for (int i = 0; i < members.Count; i++)
				{
					if (members[i] != null && members[i].UserID == this.UserID)
					{
						this.mMyUserData = members[i];
						return;
					}
				}
			}
		}

		private void OnGuildSignSetData(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildSignSetData guildEvent_GuildSignSetData = eventArgs as GuildEvent_GuildSignSetData;
			if (guildEvent_GuildSignSetData != null && this.mMyUserData != null)
			{
				this.mMyUserData.DailyActive = guildEvent_GuildSignSetData.UserDailyActive;
				this.mMyUserData.WeeklyActive = guildEvent_GuildSignSetData.UserWeeklyActive;
			}
		}

		private void OnSetSelfUserData(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_SetSelfUserData guildEvent_SetSelfUserData = eventArgs as GuildEvent_SetSelfUserData;
			if (guildEvent_SetSelfUserData != null)
			{
				this.UserID = guildEvent_SetSelfUserData.UserID;
				this.DeviceID = guildEvent_SetSelfUserData.DeviceID;
				this.UserLanguage = guildEvent_SetSelfUserData.LanguageID;
			}
		}

		private void OnGuildMemberDataChange(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildMemberChange guildEvent_GuildMemberChange = eventArgs as GuildEvent_GuildMemberChange;
			if (guildEvent_GuildMemberChange != null)
			{
				string guildID = GuildSDKManager.Instance.GuildInfo.GuildID;
				long userID = GuildSDKManager.Instance.User.UserID;
				if (guildEvent_GuildMemberChange.GuildID != guildID)
				{
					return;
				}
				IList<GuildUserShareData> userList = guildEvent_GuildMemberChange.UserList;
				for (int i = 0; i < userList.Count; i++)
				{
					if (userList[i] != null && userList[i].UserID == userID)
					{
						this.mMyUserData = userList[i];
						return;
					}
				}
			}
		}

		private void OnGuildTaskSetData(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildTaskSetData guildEvent_GuildTaskSetData = eventArgs as GuildEvent_GuildTaskSetData;
			if (guildEvent_GuildTaskSetData != null && this.MyUserData != null)
			{
				if (guildEvent_GuildTaskSetData.UserDailyActive >= 0)
				{
					this.MyUserData.DailyActive = guildEvent_GuildTaskSetData.UserDailyActive;
				}
				if (guildEvent_GuildTaskSetData.UserWeeklyActive >= 0)
				{
					this.MyUserData.WeeklyActive = guildEvent_GuildTaskSetData.UserWeeklyActive;
				}
			}
		}

		public int UserLanguage = 1;

		private GuildUserShareData mMyUserData;
	}
}
