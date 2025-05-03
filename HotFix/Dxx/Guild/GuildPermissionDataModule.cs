using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	[GuildInternalModule(12)]
	public class GuildPermissionDataModule : IGuildModule
	{
		public int ModuleName
		{
			get
			{
				return 12;
			}
		}

		public bool Init(GuildInitConfig config)
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.RegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.RegisterEvent(14, new GuildHandlerEvent(this.OnGuildMemberDataChange));
			@event.RegisterEvent(13, new GuildHandlerEvent(this.OnUserGuildPositionChg));
			return true;
		}

		public void UnInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.UnRegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.UnRegisterEvent(14, new GuildHandlerEvent(this.OnGuildMemberDataChange));
			@event.UnRegisterEvent(13, new GuildHandlerEvent(this.OnUserGuildPositionChg));
		}

		public void SetPermissionRule(GuildPermissionBase rule)
		{
			this.PermissionRule = rule;
			if (this.PermissionRule == null)
			{
				this.PermissionRule = new GuildPermissionBase();
			}
		}

		public bool HasPermission(GuildPermissionKind permission, GuildUserShareData otheruser)
		{
			if (this.PermissionRule == null)
			{
				this.PermissionRule = new GuildPermissionBase();
			}
			GuildUserShareData myUserData = GuildSDKManager.Instance.User.MyUserData;
			return this.PermissionRule.HasPermission(permission, myUserData, otheruser);
		}

		private void OnLoginSuccess(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_LoginSuccess guildEvent_LoginSuccess = eventArgs as GuildEvent_LoginSuccess;
			if (guildEvent_LoginSuccess != null && guildEvent_LoginSuccess.MyGuildShareDetail != null)
			{
				IList<GuildUserShareData> members = guildEvent_LoginSuccess.MyGuildShareDetail.Members;
				long userID = GuildSDKManager.Instance.User.UserID;
				for (int i = 0; i < members.Count; i++)
				{
					if (members[i] != null && members[i].UserID == userID)
					{
						this.MyGuildPosition = members[i].GuildPosition;
						return;
					}
				}
			}
		}

		private void OnGuildMemberDataChange(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildMemberChange guildEvent_GuildMemberChange = eventArgs as GuildEvent_GuildMemberChange;
			if (guildEvent_GuildMemberChange != null)
			{
				GuildPositionType myGuildPosition = this.MyGuildPosition;
				string guildID = GuildSDKManager.Instance.GuildInfo.GuildID;
				long userID = GuildSDKManager.Instance.User.UserID;
				if (guildEvent_GuildMemberChange.GuildID != guildID)
				{
					return;
				}
				if (guildEvent_GuildMemberChange.DeleteUser.Contains(userID))
				{
					this.MyGuildPosition = (GuildPositionType)0;
					return;
				}
				IList<GuildUserShareData> userList = guildEvent_GuildMemberChange.UserList;
				for (int i = 0; i < userList.Count; i++)
				{
					if (userList[i] != null && userList[i].UserID == userID)
					{
						this.MyGuildPosition = userList[i].GuildPosition;
						break;
					}
				}
				if (myGuildPosition != this.MyGuildPosition)
				{
					GuildEvent_MyPositionChange guildEvent_MyPositionChange = new GuildEvent_MyPositionChange();
					guildEvent_MyPositionChange.SetData(myGuildPosition, this.MyGuildPosition);
					GuildSDKManager.Instance.Event.DispatchNow(25, guildEvent_MyPositionChange);
				}
			}
		}

		private void OnUserGuildPositionChg(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_UserGuildPositionChg guildEvent_UserGuildPositionChg = eventArgs as GuildEvent_UserGuildPositionChg;
			if (guildEvent_UserGuildPositionChg != null)
			{
				long userID = guildEvent_UserGuildPositionChg.UserID;
				long userID2 = GuildSDKManager.Instance.User.UserID;
			}
		}

		public GuildPositionType MyGuildPosition;

		public GuildPermissionBase PermissionRule;
	}
}
