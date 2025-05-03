using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.User;
using UnityEngine;

namespace HotFix
{
	public class TicketDailyExchangeDataModule : IDataModule
	{
		public int GetName()
		{
			return 146;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
		}

		public void Reset()
		{
		}

		public long GetFreeAdLifeReFreshTime(UserTicketKind ticketType)
		{
			if (this.freeAdLifeRefreshTimeDic.ContainsKey(ticketType))
			{
				return this.freeAdLifeRefreshTimeDic[ticketType];
			}
			return 0L;
		}

		public void SetFreeAdLifeReFreshTime(UserTicketKind ticketType, long time)
		{
			this.freeAdLifeRefreshTimeDic[ticketType] = time;
			RedPointController.Instance.ReCalc("Currency.Energy", true);
		}

		public void ClearFreeAdLifeReFreshTime(UserTicketKind ticketType)
		{
			this.freeAdLifeRefreshTimeDic.Remove(ticketType);
		}

		public void SetLoginData(UserLoginResponse loginResponse)
		{
			this.freeAdLifeRefreshTimeDic.Clear();
			this.SetFreeAdLifeReFreshTime(UserTicketKind.UserLife, loginResponse.FreeAdLifeRefreshtime);
		}

		public int GetAdID(UserTicketKind ticketType)
		{
			if (ticketType == UserTicketKind.UserLife)
			{
				return 15;
			}
			HLog.LogError("TicketDailyExchange : Unknown ticket type = " + ticketType.ToString());
			return 0;
		}

		public int GetAdWatchedTimes(UserTicketKind ticketType)
		{
			int adID = this.GetAdID(ticketType);
			if (adID == 0)
			{
				return 0;
			}
			return GameApp.Data.GetDataModule(DataName.AdDataModule).GetWatchTimes(adID);
		}

		public int GetAdWatchedMaxTimes(UserTicketKind ticketType)
		{
			int adID = this.GetAdID(ticketType);
			if (adID == 0)
			{
				return 0;
			}
			return GameApp.Data.GetDataModule(DataName.AdDataModule).GetMaxTimes(adID);
		}

		public int GetLeftAdWatchTimes(UserTicketKind ticketType)
		{
			return Mathf.Max(0, this.GetAdWatchedMaxTimes(ticketType) - this.GetAdWatchedTimes(ticketType));
		}

		public int GetNextAdWatchGetNum(UserTicketKind ticketType)
		{
			if (ticketType == UserTicketKind.UserLife)
			{
				int adWatchedMaxTimes = this.GetAdWatchedMaxTimes(ticketType);
				string[] array = GameApp.Table.GetManager().GetGameConfig_Config(2008).Value.Split('|', StringSplitOptions.None);
				int num = Mathf.Clamp(adWatchedMaxTimes, 0, array.Length - 1);
				return int.Parse(array[num].Split(',', StringSplitOptions.None)[1]);
			}
			return 0;
		}

		public void OnDayChangeUpdated()
		{
			GameApp.Event.DispatchNow(null, 124, null);
			RedPointController.Instance.ReCalc("Currency.Energy", true);
		}

		public bool ShowAnyRed(UserTicketKind ticketType)
		{
			return this.GetLeftAdWatchTimes(ticketType) > 0 || this.GetFreeAdLifeReFreshTime(ticketType) <= DxxTools.Time.ServerTimestamp;
		}

		private Dictionary<UserTicketKind, long> freeAdLifeRefreshTimeDic = new Dictionary<UserTicketKind, long>();
	}
}
