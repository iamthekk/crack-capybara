using System;
using System.Collections.Generic;

namespace Dxx.Guild
{
	[GuildInternalModule(18)]
	public class GuildDonationDataModule : IGuildModule
	{
		public int ModuleName
		{
			get
			{
				return 18;
			}
		}

		public GuildDonationInfo DonationInfo { get; private set; }

		public IList<GuildItemData> RecvDonationItems
		{
			get
			{
				return this.mRecvDonationItems;
			}
		}

		public bool HasRecvDonationItems
		{
			get
			{
				return this.mRecvDonationItems != null && this.mRecvDonationItems.Count > 0;
			}
		}

		public bool Init(GuildInitConfig config)
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.RegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.RegisterEvent(301, new GuildHandlerEvent(this.OnRecvDonationItem));
			@event.RegisterEvent(302, new GuildHandlerEvent(this.OnRecvDonationItemClear));
			@event.RegisterEvent(303, new GuildHandlerEvent(this.OnSetDonationInfo));
			@event.RegisterEvent(304, new GuildHandlerEvent(this.OnDonationAddRecords));
			return true;
		}

		public void UnInit()
		{
			GuildEventModule @event = GuildSDKManager.Instance.Event;
			@event.UnRegisterEvent(10, new GuildHandlerEvent(this.OnLoginSuccess));
			@event.UnRegisterEvent(301, new GuildHandlerEvent(this.OnRecvDonationItem));
			@event.UnRegisterEvent(302, new GuildHandlerEvent(this.OnRecvDonationItemClear));
			@event.UnRegisterEvent(303, new GuildHandlerEvent(this.OnSetDonationInfo));
			@event.UnRegisterEvent(304, new GuildHandlerEvent(this.OnDonationAddRecords));
		}

		private void OnLoginSuccess(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_LoginSuccess guildEvent_LoginSuccess = eventArgs as GuildEvent_LoginSuccess;
			if (guildEvent_LoginSuccess != null)
			{
				if (guildEvent_LoginSuccess.IsJoin)
				{
					this.DonationInfo = guildEvent_LoginSuccess.DonationInfo;
				}
				if (this.DonationInfo == null)
				{
					this.DonationInfo = new GuildDonationInfo();
				}
			}
		}

		private void OnRecvDonationItem(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_RecvDonationItem guildEvent_RecvDonationItem = eventArgs as GuildEvent_RecvDonationItem;
			if (guildEvent_RecvDonationItem != null && guildEvent_RecvDonationItem.Rewards != null && guildEvent_RecvDonationItem.Rewards.Count > 0)
			{
				this.mRecvDonationItems.AddRange(guildEvent_RecvDonationItem.Rewards);
			}
		}

		private void OnRecvDonationItemClear(int type, GuildBaseEvent eventArgs)
		{
			this.mRecvDonationItems.Clear();
		}

		private void OnSetDonationInfo(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_SetDonationInfo guildEvent_SetDonationInfo = eventArgs as GuildEvent_SetDonationInfo;
			if (guildEvent_SetDonationInfo != null)
			{
				this.DonationInfo = guildEvent_SetDonationInfo.DonationInfo;
			}
			if (this.DonationInfo == null)
			{
				this.DonationInfo = new GuildDonationInfo();
			}
		}

		private void OnDonationAddRecords(int type, GuildBaseEvent eventArgs)
		{
			GuildEvent_DonationAddRecords guildEvent_DonationAddRecords = eventArgs as GuildEvent_DonationAddRecords;
			if (guildEvent_DonationAddRecords != null)
			{
				GuildDonationRecordList guildDonationRecordList = null;
				int type2 = guildEvent_DonationAddRecords.type;
				if (type2 != 1)
				{
					if (type2 == 2)
					{
						guildDonationRecordList = this.RecvRecords;
					}
				}
				else
				{
					guildDonationRecordList = this.SendRecords;
				}
				if (guildDonationRecordList != null)
				{
					guildDonationRecordList.AddRecords(guildEvent_DonationAddRecords.Records);
				}
			}
		}

		private List<GuildItemData> mRecvDonationItems = new List<GuildItemData>();

		public GuildDonationRecordList RecvRecords = new GuildDonationRecordList();

		public GuildDonationRecordList SendRecords = new GuildDonationRecordList();
	}
}
