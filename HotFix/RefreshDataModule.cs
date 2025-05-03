using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Proto.Common;
using Proto.User;
using Shop.Arena;
using UnityEngine;

namespace HotFix
{
	public class RefreshDataModule : IDataModule
	{
		public int GetName()
		{
			return 107;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.RegisterEvent(LocalMessageName.CC_DAY_CHANGE, new HandlerEvent(this.OnPullDayChangeData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_DAY_CHANGE, new HandlerEvent(this.OnPullDayChangeData));
		}

		public void Reset()
		{
		}

		public void StartRefreshCheck()
		{
			this.refreshKeys.Clear();
			this.refreshKeys.Add(this.ShopFreeDrawResetRefreshKey);
			this.refreshDelayTime.Add((long)Random.Range(1, 10));
			this.petDataModule = GameApp.Data.GetDataModule(DataName.PetDataModule);
			this.shopDataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			GlobalUpdater.Instance.RegisterUpdater(new Action(this.NoneZeroTimeChangeRefreshChecker));
		}

		private void NoneZeroTimeChangeRefreshChecker()
		{
			this.timer += Time.unscaledTime;
			if (this.timer >= 2f)
			{
				this.timer = 0f;
				long serverTimestamp = DxxTools.Time.ServerTimestamp;
				float realtimeSinceStartup = Time.realtimeSinceStartup;
				for (int i = 0; i < this.refreshKeys.Count; i++)
				{
					string text = this.refreshKeys[i];
					long num = this.refreshDelayTime[i];
					long num2 = this.GetResetTimestamp(text) + num;
					float num3;
					if ((!this.latestRefreshTimeDic.TryGetValue(text, out num3) || realtimeSinceStartup >= num3 + 10f) && !this.netWaitingKeys.Contains(text) && num2 > 0L && serverTimestamp >= num2 + num)
					{
						this.RequestRefreshData(text);
					}
				}
			}
		}

		private long GetResetTimestamp(string key)
		{
			long num = 0L;
			if (key.Equals(this.ShopFreeDrawResetRefreshKey))
			{
				num = this.iapDataModule.ShopFreeDrawResetTimestamp;
			}
			else if (key.Equals(this.PetFreeDrawResetRefreshKey))
			{
				num = this.petDataModule.FreeDrawResetTimestamp;
			}
			return num;
		}

		private void RequestRefreshData(string key)
		{
			if (this.netWaitingKeys.Contains(key))
			{
				return;
			}
			this.netWaitingKeys.Add(key);
			this.latestRefreshTimeDic[key] = Time.realtimeSinceStartup;
			if (this.refreshKeys.IndexOf(key) >= 0)
			{
				this.refreshDelayTime[this.refreshKeys.IndexOf(key)] = (long)Random.Range(1, 10);
			}
			if (key == this.PetFreeDrawResetRefreshKey)
			{
				NetworkUtils.Pet.PetInfoRefreshRequest(delegate(bool isOk, UserRefDataResponse resp)
				{
					this.netWaitingKeys.Remove(key);
				});
				return;
			}
			if (key == this.ShopFreeDrawResetRefreshKey)
			{
				NetworkUtils.Purchase.ShopGetInfoRequest(delegate(bool isOk, ShopGetInfoResponse resp)
				{
					this.netWaitingKeys.Remove(key);
					if (isOk && resp != null)
					{
						IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
						if (resp.ShopAllDataDto.IapInfo == null)
						{
							resp.ShopAllDataDto.IapInfo = new IAPDto();
						}
						dataModule.InitForServer(resp.ShopAllDataDto);
						GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(resp.ShopAllDataDto.AdData);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_UI_Shop_Refresh, null);
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_UI_AD_REFRESH, null);
					}
				});
			}
		}

		private void OnPullDayChangeData(object sender, int type, BaseEventArgs eventargs)
		{
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChange_InternalShop_DataPull, null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChange_ShopInfo_DataPull, null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChange_SevenDayCarnival_DataPull, null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChange_Task_DataPull, null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChange_Arena_DataPull, null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChange_Pet_DataPull, null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChange_Ticket_DataPull, null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChange_HangUp_DataPull, null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChangeRefreshPushGiftData, null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_DayChange_ActivitySlotTrain_DataPull, null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_SignIn_Day_Update, null);
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_UI_MeetingGift_RefreshUI, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_MainCity_Refresh, null);
			GameApp.Event.DispatchNow(this, LocalMessageName.CC_DayChange_ChainPacks_DataPull, null);
			NetworkUtils.ActivityCommon.ActivityGetListRequest(true, null);
		}

		private string PetFreeDrawResetRefreshKey = "PetFreeDrawResetRefreshKey";

		private string ShopFreeDrawResetRefreshKey = "ShopFreeDrawResetRefreshKey";

		private PetDataModule petDataModule;

		private ShopDataModule shopDataModule;

		private IAPDataModule iapDataModule;

		private List<string> refreshKeys = new List<string>();

		private List<long> refreshDelayTime = new List<long>();

		private Dictionary<string, float> latestRefreshTimeDic = new Dictionary<string, float>();

		private List<string> netWaitingKeys = new List<string>();

		private float timer;
	}
}
