using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	[GuildInternalModule(14)]
	public class GuildListModule : IGuildModule
	{
		public int ModuleName
		{
			get
			{
				return 14;
			}
		}

		public bool Init(GuildInitConfig config)
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.RegisterEvent(11, new GuildHandlerEvent(this.OnGetHallList));
			@event.RegisterEvent(12, new GuildHandlerEvent(this.OnGetGuildDetailInfo));
			return true;
		}

		public void UnInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.UnRegisterEvent(11, new GuildHandlerEvent(this.OnGetHallList));
			@event.UnRegisterEvent(12, new GuildHandlerEvent(this.OnGetGuildDetailInfo));
			this.mCustomListDic.Clear();
		}

		private void OnGetHallList(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GetGuildList guildEvent_GetGuildList = eventArgs as GuildEvent_GetGuildList;
			if (guildEvent_GetGuildList != null)
			{
				this.GetListGroup(guildEvent_GetGuildList.GroupID).AddRange(guildEvent_GetGuildList.GuildList);
			}
		}

		private void OnGetGuildDetailInfo(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_GetGuildDetailInfo guildEvent_GetGuildDetailInfo = eventArgs as GuildEvent_GetGuildDetailInfo;
			if (guildEvent_GetGuildDetailInfo != null)
			{
				this.SetGuildDetailData(guildEvent_GetGuildDetailInfo.GuildData);
			}
		}

		public List<GuildShareData> GetGuildList()
		{
			return this.GetListGroup(0).GetGuildList();
		}

		public GuildShareDetailData GetGuildDetailData(string guildid)
		{
			GuildShareDetailData guildShareDetailData;
			if (this.mGuildDetailDataDic.TryGetValue(guildid, out guildShareDetailData))
			{
				return guildShareDetailData;
			}
			return null;
		}

		public GuildUserShareData GetGuildMemberData(long userId, out GuildShareDetailData detailData)
		{
			detailData = null;
			foreach (string text in this.mGuildDetailDataDic.Keys)
			{
				GuildShareDetailData guildShareDetailData = this.mGuildDetailDataDic[text];
				if (guildShareDetailData != null)
				{
					GuildUserShareData memberData = guildShareDetailData.GetMemberData(userId);
					if (memberData != null)
					{
						detailData = guildShareDetailData;
						return memberData;
					}
				}
			}
			return null;
		}

		public void SetGuildDetailData(GuildShareDetailData data)
		{
			if (data != null)
			{
				this.mGuildDetailDataDic[data.GuildID] = data;
			}
		}

		public void SetGuildDetailData(List<GuildShareDetailData> datas)
		{
			if (datas != null)
			{
				foreach (GuildShareDetailData guildShareDetailData in datas)
				{
					this.SetGuildDetailData(guildShareDetailData);
				}
			}
		}

		public GuildListGroup GetListGroup(int groupid)
		{
			GuildListGroup guildListGroup;
			if (this.mCustomListDic.TryGetValue(groupid, out guildListGroup))
			{
				return guildListGroup;
			}
			guildListGroup = new GuildListGroup();
			this.mCustomListDic[groupid] = guildListGroup;
			return guildListGroup;
		}

		public void ReleaseListGorup(int groupid)
		{
			GuildListGroup guildListGroup;
			if (this.mCustomListDic.TryGetValue(groupid, out guildListGroup))
			{
				guildListGroup.Clear();
			}
		}

		public const int DefaultGuildListGroupID = 0;

		private Dictionary<int, GuildListGroup> mCustomListDic = new Dictionary<int, GuildListGroup>();

		private Dictionary<string, GuildShareDetailData> mGuildDetailDataDic = new Dictionary<string, GuildShareDetailData>();
	}
}
