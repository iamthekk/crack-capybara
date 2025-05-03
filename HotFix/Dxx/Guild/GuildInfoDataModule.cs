using System;
using System.Collections.Generic;
using Framework;
using Framework.EventSystem;
using HotFix;
using LocalModels.Bean;
using Proto.Guild;

namespace Dxx.Guild
{
	[GuildInternalModule(11)]
	public class GuildInfoDataModule : IGuildModule
	{
		public int DayContributeTimes { get; private set; }

		public int DayAllContributeTimes { get; private set; }

		public List<Guild_guildcontribute> GuildContributeConfigs { get; private set; }

		private void InitContributeConfig()
		{
			IList<Guild_guildcontribute> guild_guildcontributeElements = GameApp.Table.GetManager().GetGuild_guildcontributeElements();
			this.GuildContributeConfigs = new List<Guild_guildcontribute>();
			this.GuildContributeConfigs.AddRange(guild_guildcontributeElements);
		}

		public int ModuleName
		{
			get
			{
				return 11;
			}
		}

		public GuildShareDetailData GuildDetailData
		{
			get
			{
				return this.mMyGuildDetailData;
			}
		}

		public GuildShareData GuildData
		{
			get
			{
				if (this.mMyGuildDetailData == null)
				{
					return null;
				}
				return this.mMyGuildDetailData.ShareData;
			}
		}

		public List<GuildUserShareData> PlayerDataApplyJoin
		{
			get
			{
				return this._playerDataApplyJoin;
			}
		}

		public void SetPlayerDataApplyJoin(List<GuildUserShareData> data)
		{
			this._playerDataApplyJoin = new List<GuildUserShareData>();
			this._playerDataApplyJoin.AddRange(data);
		}

		public GuildUserShareData GetPlayer2JoinData(long userId)
		{
			if (this._playerDataApplyJoin == null)
			{
				return null;
			}
			for (int i = 0; i < this._playerDataApplyJoin.Count; i++)
			{
				GuildUserShareData guildUserShareData = this._playerDataApplyJoin[i];
				if (guildUserShareData.UserID == userId)
				{
					return guildUserShareData;
				}
			}
			return null;
		}

		public string GuildID
		{
			get
			{
				if (this.mMyGuildDetailData == null)
				{
					return string.Empty;
				}
				return this.mMyGuildDetailData.GuildID;
			}
		}

		public string IMGroupId
		{
			get
			{
				if (this.mMyGuildDetailData == null)
				{
					return string.Empty;
				}
				return this.mMyGuildDetailData.IMGroupID;
			}
		}

		public long PresidentUserID
		{
			get
			{
				if (this.mMyGuildDetailData != null && this.mMyGuildDetailData.ShareData != null)
				{
					return this.mMyGuildDetailData.ShareData.PresidentUserID;
				}
				return 0L;
			}
		}

		public string GuildSlogan
		{
			get
			{
				if (this.mMyGuildDetailData == null || this.mMyGuildDetailData.ShareData == null)
				{
					return "";
				}
				return this.mMyGuildDetailData.ShareData.GuildSlogan;
			}
		}

		public string GuildNotice
		{
			get
			{
				if (this.mMyGuildDetailData == null || this.mMyGuildDetailData.ShareData == null)
				{
					return "";
				}
				return this.mMyGuildDetailData.ShareData.GuildNotice;
			}
		}

		public bool HasGuild
		{
			get
			{
				return this.mMyGuildDetailData != null && !string.IsNullOrEmpty(this.mMyGuildDetailData.GuildID);
			}
		}

		public bool Init(GuildInitConfig config)
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.RegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.RegisterEvent(16, new GuildHandlerEvent(this.OnSetMyGuildFeaturesInfo));
			@event.RegisterEvent(17, new GuildHandlerEvent(this.OnGuildLevelUpSetData));
			@event.RegisterEvent(18, new GuildHandlerEvent(this.OnGuildSignSetData));
			@event.RegisterEvent(14, new GuildHandlerEvent(this.OnGuildMemberDataChange));
			@event.RegisterEvent(15, new GuildHandlerEvent(this.OnGetGuildDetailInfo));
			@event.RegisterEvent(12, new GuildHandlerEvent(this.OnGetGuildDetailInfo));
			@event.RegisterEvent(20, new GuildHandlerEvent(this.OnGuildTaskSetData));
			@event.RegisterEvent(21, new GuildHandlerEvent(this.OnGuildClearLevelUpFlag));
			@event.RegisterEvent(22, new GuildHandlerEvent(this.OnGuildClearBeKickOutFlag));
			@event.RegisterEvent(23, new GuildHandlerEvent(this.OnGuildChangeInfo));
			@event.RegisterEvent(24, new GuildHandlerEvent(this.OnApplyJoinCount));
			@event.RegisterEvent(305, new GuildHandlerEvent(this.OnGuildContribute));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_DAY_CHANGE, new HandlerEvent(this.OnDayChange));
			return true;
		}

		public void UnInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.UnRegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.UnRegisterEvent(16, new GuildHandlerEvent(this.OnSetMyGuildFeaturesInfo));
			@event.UnRegisterEvent(17, new GuildHandlerEvent(this.OnGuildLevelUpSetData));
			@event.UnRegisterEvent(18, new GuildHandlerEvent(this.OnGuildSignSetData));
			@event.UnRegisterEvent(14, new GuildHandlerEvent(this.OnGuildMemberDataChange));
			@event.UnRegisterEvent(15, new GuildHandlerEvent(this.OnGetGuildDetailInfo));
			@event.UnRegisterEvent(12, new GuildHandlerEvent(this.OnGetGuildDetailInfo));
			@event.UnRegisterEvent(20, new GuildHandlerEvent(this.OnGuildTaskSetData));
			@event.UnRegisterEvent(21, new GuildHandlerEvent(this.OnGuildClearLevelUpFlag));
			@event.UnRegisterEvent(22, new GuildHandlerEvent(this.OnGuildClearBeKickOutFlag));
			@event.UnRegisterEvent(23, new GuildHandlerEvent(this.OnGuildChangeInfo));
			@event.UnRegisterEvent(24, new GuildHandlerEvent(this.OnApplyJoinCount));
			@event.UnRegisterEvent(305, new GuildHandlerEvent(this.OnGuildContribute));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_DAY_CHANGE, new HandlerEvent(this.OnDayChange));
		}

		private void OnDayChange(object obj, int type, BaseEventArgs args)
		{
			if (Singleton<GameFunctionController>.Instance.IsFunctionOpened(FunctionID.MainUI_Guild, false))
			{
				GuildNetUtil.Guild.DoRequest_GuildLoginRequest(delegate(bool result, GuildGetInfoResponse response)
				{
				});
			}
		}

		private void OnLoginSuccess(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_LoginSuccess guildEvent_LoginSuccess = eventArgs as GuildEvent_LoginSuccess;
			if (guildEvent_LoginSuccess != null)
			{
				if (guildEvent_LoginSuccess.IsJoin)
				{
					this.mMyGuildDetailData = guildEvent_LoginSuccess.MyGuildShareDetail;
					this.mGuildSignData = guildEvent_LoginSuccess.SignData;
					this.IsShowLevelUp = guildEvent_LoginSuccess.IsLevelUp;
					this.ApplyJoinCount = guildEvent_LoginSuccess.ApplyJoinCount;
					if (this.ApplyJoinCount < 0)
					{
						this.ApplyJoinCount = 0;
					}
				}
				else
				{
					this.mMyGuildDetailData = null;
					this.mGuildSignData = null;
					this.BeKickOut = guildEvent_LoginSuccess.BeKickOutInfo;
					this.ApplyJoinCount = 0;
					if (guildEvent_LoginSuccess.QuitGuildTimeStamp >= 0L)
					{
						this.QuitGuildTimeStamp = guildEvent_LoginSuccess.QuitGuildTimeStamp;
					}
				}
				this.HasGetGuildData = true;
				this.DayContributeTimes = guildEvent_LoginSuccess.DayContributeTimes;
				this.DayAllContributeTimes = guildEvent_LoginSuccess.DayAllContributeTimes;
				this.ForceRefreshUI();
				GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.GuildActivity);
			}
			this.InitContributeConfig();
		}

		private void OnSetMyGuildFeaturesInfo(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_SetMyGuildFeaturesInfo guildEvent_SetMyGuildFeaturesInfo = eventArgs as GuildEvent_SetMyGuildFeaturesInfo;
			if (guildEvent_SetMyGuildFeaturesInfo != null && this.HasGuild)
			{
				this.mGuildSignData = guildEvent_SetMyGuildFeaturesInfo.SignData;
				if (guildEvent_SetMyGuildFeaturesInfo.ApplyJoinCount >= 0)
				{
					this.ApplyJoinCount = guildEvent_SetMyGuildFeaturesInfo.ApplyJoinCount;
				}
				this.DayContributeTimes = guildEvent_SetMyGuildFeaturesInfo.DayContributeTimes;
				this.DayAllContributeTimes = guildEvent_SetMyGuildFeaturesInfo.DayAllContributeTimes;
				this.ForceRefreshUI();
			}
		}

		private void OnGuildLevelUpSetData(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildLevelUpSetData guildEvent_GuildLevelUpSetData = eventArgs as GuildEvent_GuildLevelUpSetData;
			if (guildEvent_GuildLevelUpSetData != null)
			{
				this.SetGuildUpdateInfo(guildEvent_GuildLevelUpSetData.GuildUpdateInfo);
			}
		}

		private void OnGuildSignSetData(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildSignSetData guildEvent_GuildSignSetData = eventArgs as GuildEvent_GuildSignSetData;
			if (guildEvent_GuildSignSetData != null)
			{
				this.mGuildSignData = guildEvent_GuildSignSetData.SignData;
				this.SetGuildUpdateInfo(guildEvent_GuildSignSetData.GuildUpdateInfo);
			}
		}

		private void OnGuildMemberDataChange(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildMemberChange guildEvent_GuildMemberChange = eventArgs as GuildEvent_GuildMemberChange;
			if (guildEvent_GuildMemberChange != null)
			{
				if (guildEvent_GuildMemberChange.GuildID != this.GuildID || this.GuildID == string.Empty)
				{
					return;
				}
				List<long> deleteUser = guildEvent_GuildMemberChange.DeleteUser;
				List<GuildUserShareData> members = this.mMyGuildDetailData.Members;
				List<GuildUserShareData> list = new List<GuildUserShareData>(2);
				list.AddRange(guildEvent_GuildMemberChange.UserList);
				for (int i = 0; i < members.Count; i++)
				{
					GuildUserShareData guildUserShareData = members[i];
					for (int j = 0; j < list.Count; j++)
					{
						GuildUserShareData guildUserShareData2 = list[j];
						if (guildUserShareData2 != null && guildUserShareData2.UserID == guildUserShareData.UserID)
						{
							guildUserShareData.CloneFrom(guildUserShareData2);
							list.RemoveAt(j);
							break;
						}
					}
					if (deleteUser != null && deleteUser.Count > 0 && deleteUser.Contains(guildUserShareData.UserID))
					{
						members.RemoveAt(i);
						i--;
					}
				}
				if (list.Count > 0)
				{
					for (int k = 0; k < list.Count; k++)
					{
						if (list[k] != null)
						{
							GuildUserShareData guildUserShareData3 = new GuildUserShareData();
							guildUserShareData3.CloneFrom(list[k]);
							members.Add(guildUserShareData3);
						}
					}
				}
				if (guildEvent_GuildMemberChange.MemberCount > 0)
				{
					this.mMyGuildDetailData.ShareData.GuildMemberCount = guildEvent_GuildMemberChange.MemberCount;
				}
				else
				{
					this.mMyGuildDetailData.ShareData.GuildMemberCount = members.Count;
				}
				this.ForceRefreshUI();
			}
		}

		private void OnGetGuildDetailInfo(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GetGuildDetailInfo guildEvent_GetGuildDetailInfo = eventArgs as GuildEvent_GetGuildDetailInfo;
			if (guildEvent_GetGuildDetailInfo != null)
			{
				if (guildEvent_GetGuildDetailInfo.GuildData == null || guildEvent_GetGuildDetailInfo.GuildData.GuildID != this.GuildID)
				{
					return;
				}
				this.mMyGuildDetailData.ShareData.CloneFrom(guildEvent_GetGuildDetailInfo.GuildData.ShareData);
				if (guildEvent_GetGuildDetailInfo.GuildData.Members.Count > 0)
				{
					this.mMyGuildDetailData.Members.Clear();
					this.mMyGuildDetailData.Members.AddRange(guildEvent_GetGuildDetailInfo.GuildData.Members);
					this.ForceRefreshUI();
				}
			}
		}

		private void OnGuildTaskSetData(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildTaskSetData guildEvent_GuildTaskSetData = eventArgs as GuildEvent_GuildTaskSetData;
			if (guildEvent_GuildTaskSetData != null && this.HasGuild && guildEvent_GuildTaskSetData.UpdateInfo != null)
			{
				this.SetGuildUpdateInfo(guildEvent_GuildTaskSetData.UpdateInfo);
			}
		}

		private void OnGuildClearLevelUpFlag(int type, GuildBaseEvent eventArgs)
		{
			this.IsShowLevelUp = false;
			this.ForceRefreshUI();
		}

		private void OnGuildClearBeKickOutFlag(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_BeKickOutSetData guildEvent_BeKickOutSetData = eventArgs as GuildEvent_BeKickOutSetData;
			if (guildEvent_BeKickOutSetData != null)
			{
				this.BeKickOut = guildEvent_BeKickOutSetData.Info;
				return;
			}
			this.BeKickOut = null;
		}

		private void OnGuildChangeInfo(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildInfoDataChange guildEvent_GuildInfoDataChange = eventArgs as GuildEvent_GuildInfoDataChange;
			if (guildEvent_GuildInfoDataChange != null && guildEvent_GuildInfoDataChange.ChangeInfo != null && this.HasGuild)
			{
				GuildShareData shareData = this.mMyGuildDetailData.ShareData;
				GuildChangeInfo changeInfo = guildEvent_GuildInfoDataChange.ChangeInfo;
				shareData.GuildShowName = changeInfo.guildName;
				shareData.GuildIcon = changeInfo.guildIcon;
				shareData.GuildIconBg = changeInfo.guildIconBg;
				shareData.GuildNotice = changeInfo.guildNotice;
				shareData.GuildSlogan = changeInfo.guildIntro;
				shareData.GuildExp = changeInfo.guildExp;
				if (shareData.GuildLevel != changeInfo.guildLevel)
				{
					this.IsShowLevelUp = true;
				}
				shareData.GuildLevel = changeInfo.guildLevel;
				this.ForceRefreshUI();
			}
		}

		private void OnApplyJoinCount(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_ApplyJoinCount guildEvent_ApplyJoinCount = eventArgs as GuildEvent_ApplyJoinCount;
			if (guildEvent_ApplyJoinCount != null)
			{
				this.ApplyJoinCount = guildEvent_ApplyJoinCount.ApplyJoinCount;
				if (this.ApplyJoinCount < 0)
				{
					this.ApplyJoinCount = 0;
				}
			}
		}

		private void OnGuildContribute(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GuildContribute guildEvent_GuildContribute = eventArgs as GuildEvent_GuildContribute;
			if (guildEvent_GuildContribute != null)
			{
				this.DayContributeTimes = guildEvent_GuildContribute.DayContributeTimes;
				this.DayAllContributeTimes = guildEvent_GuildContribute.DayAllContributeTimes;
			}
		}

		private void SetGuildUpdateInfo(GuildUpdateInfo updateinfo)
		{
			if (this.HasGuild && updateinfo != null)
			{
				GuildShareData shareData = this.mMyGuildDetailData.ShareData;
				if (shareData.GuildLevel != updateinfo.Level)
				{
					this.IsShowLevelUp = true;
				}
				shareData.GuildLevel = updateinfo.Level;
				shareData.GuildExp = updateinfo.Exp;
				shareData.GuildActive = (uint)updateinfo.GuildActive;
				shareData.GuildMemberMaxCount = updateinfo.MaxMemberCount;
				this.ForceRefreshUI();
			}
		}

		public IList<GuildUserShareData> GetMemberList()
		{
			if (this.mMyGuildDetailData == null || this.mMyGuildDetailData.Members == null)
			{
				return this.mEmptyList;
			}
			List<GuildUserShareData> members = this.mMyGuildDetailData.Members;
			members.Sort(delegate(GuildUserShareData a, GuildUserShareData b)
			{
				int num = a.GuildPosition.CompareTo(b.GuildPosition);
				if (num.Equals(0))
				{
					num = b.LastOnlineTime.CompareTo(a.LastOnlineTime);
				}
				return num;
			});
			return members;
		}

		public GuildUserShareData GetGuildPresident()
		{
			if (this.mMyGuildDetailData == null)
			{
				return null;
			}
			return this.mMyGuildDetailData.GetGuildPresident();
		}

		public List<GuildUserShareData> GetMemberByPosition(GuildPositionType position)
		{
			List<GuildUserShareData> list = new List<GuildUserShareData>();
			if (this.mMyGuildDetailData != null && this.mMyGuildDetailData.Members != null)
			{
				List<GuildUserShareData> members = this.mMyGuildDetailData.Members;
				for (int i = 0; i < members.Count; i++)
				{
					GuildUserShareData guildUserShareData = members[i];
					if (guildUserShareData != null && guildUserShareData.GuildPosition == position)
					{
						list.Add(guildUserShareData);
					}
				}
			}
			return list;
		}

		public GuildUserShareData GetMemberShareData(long userID)
		{
			if (this.mMyGuildDetailData != null && this.mMyGuildDetailData.Members != null)
			{
				List<GuildUserShareData> members = this.mMyGuildDetailData.Members;
				for (int i = 0; i < members.Count; i++)
				{
					GuildUserShareData guildUserShareData = members[i];
					if (guildUserShareData != null && guildUserShareData.UserID == userID)
					{
						return guildUserShareData;
					}
				}
			}
			return null;
		}

		public int GetMemberCountByPosition(GuildPositionType position)
		{
			int num = 0;
			if (this.mMyGuildDetailData != null && this.mMyGuildDetailData.Members != null)
			{
				List<GuildUserShareData> members = this.mMyGuildDetailData.Members;
				for (int i = 0; i < members.Count; i++)
				{
					GuildUserShareData guildUserShareData = members[i];
					if (guildUserShareData != null && guildUserShareData.GuildPosition == position)
					{
						num++;
					}
				}
			}
			return num;
		}

		public int GetAllMemberCount()
		{
			int num = 0;
			if (this.mMyGuildDetailData != null && this.mMyGuildDetailData.Members != null)
			{
				return this.mMyGuildDetailData.Members.Count;
			}
			return num;
		}

		private void ForceRefreshUI()
		{
		}

		public GuildShareSimpleData MakeSimpleData()
		{
			GuildShareData guildData = this.GuildData;
			GuildShareSimpleData guildShareSimpleData = new GuildShareSimpleData();
			if (guildData == null)
			{
				return guildShareSimpleData;
			}
			guildShareSimpleData.GuildID = guildData.GuildID;
			guildShareSimpleData.GuildID_ULong = guildData.GuildID_ULong;
			guildShareSimpleData.GuildName = guildData.GuildShowName;
			guildShareSimpleData.GuildIcon = guildData.GuildIcon;
			guildShareSimpleData.GuildIconBg = guildData.GuildIconBg;
			guildShareSimpleData.GuildPower = guildData.GuildPower;
			guildShareSimpleData.GuildLevel = guildData.GuildLevel;
			return guildShareSimpleData;
		}

		public bool HasGetGuildData { get; private set; }

		public bool IsShowLevelUp { get; private set; }

		public GuildBeKickOutInfo BeKickOut { get; private set; }

		public GuildSignData GuildSignData
		{
			get
			{
				return this.mGuildSignData;
			}
		}

		private GuildShareDetailData mMyGuildDetailData;

		private List<GuildUserShareData> _playerDataApplyJoin = new List<GuildUserShareData>();

		private List<GuildUserShareData> mEmptyList = new List<GuildUserShareData>();

		public int ApplyJoinCount;

		public long QuitGuildTimeStamp;

		private GuildSignData mGuildSignData;
	}
}
