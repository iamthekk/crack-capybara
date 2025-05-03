using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using Proto.Common;

namespace HotFix
{
	public class IAPDataModule : IDataModule
	{
		public PurchaseCommonData Common { get; private set; }

		public IAPDiamondsPackData DiamondsPackData { get; private set; }

		public IAPTimePackData TimePackData { get; private set; }

		public IAPBattlePass BattlePass { get; private set; }

		public IAPLevelFund LevelFund { get; private set; }

		public IAPMonthCardData MonthCard { get; private set; }

		public IAPChapterGift ChapterGift { get; private set; }

		public IAPFirstGift FirstGift { get; private set; }

		public IAPOpenServerGift OpenServerGift { get; private set; }

		public IAPLimitedTimeGift LimitedTimeGift { get; private set; }

		public IAPSuperValuePack SuperValuePack { get; private set; }

		public IAPRechargeGift RechargeGift { get; private set; }

		public IAPMeetingGift MeetingGift { get; private set; }

		public long ShopFreeDrawResetTimestamp { get; private set; }

		public Dictionary<int, int> ShopDrawMiniCount { get; private set; } = new Dictionary<int, int>();

		public Dictionary<int, int> ShopDrawHardCount { get; private set; } = new Dictionary<int, int>();

		public Dictionary<int, int> FreeCostTimes { get; private set; } = new Dictionary<int, int>();

		public Dictionary<int, IAPShopActivityData> ActivityDict { get; private set; } = new Dictionary<int, IAPShopActivityData>();

		public int AccountTotalPayTimes { get; private set; }

		public decimal AccountTotalCharge { get; private set; }

		public long AccountFirstPayTime { get; private set; }

		public bool IsShowFirstRecharge { get; private set; }

		public IAPDataModule()
		{
			this.Common = new PurchaseCommonData();
			this.DiamondsPackData = new IAPDiamondsPackData(this.Common);
			this.TimePackData = new IAPTimePackData(this.Common);
			this.BattlePass = new IAPBattlePass(this.Common);
			this.LevelFund = new IAPLevelFund(this.Common);
			this.MonthCard = new IAPMonthCardData(this.Common);
			this.ChapterGift = new IAPChapterGift(this.Common);
			this.FirstGift = new IAPFirstGift(this.Common);
			this.OpenServerGift = new IAPOpenServerGift(this.Common);
			this.LimitedTimeGift = new IAPLimitedTimeGift(this.Common);
			this.SuperValuePack = new IAPSuperValuePack(this.Common);
			this.RechargeGift = new IAPRechargeGift(this.Common);
			this.MeetingGift = new IAPMeetingGift(this.Common);
		}

		public int GetName()
		{
			return 134;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_IAPData_RefreshCommonData, new HandlerEvent(this.OnEventRefreshCommonData));
			manager.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventRefreshIAPInfoData));
			manager.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPBattlePassData, new HandlerEvent(this.OnEventRefreshIAPBattlePassData));
			manager.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPBattlePassScore, new HandlerEvent(this.OnEventRefreshIAPBattlePassScore));
			manager.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPLevelFundRewards, new HandlerEvent(this.OnEventRefreshIAPLevelFundRewards));
			manager.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPChapterGiftData, new HandlerEvent(this.OnEventRefreshIAPChapterGiftData));
			manager.RegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPFirstGiftData, new HandlerEvent(this.OnEventRefreshIAPFirstGiftData));
			manager.RegisterEvent(LocalMessageName.CC_IAP_BattleFail_ShowFirstRechargeUI, new HandlerEvent(this.OnEventRefreshShowRechargeUI));
			manager.RegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshCommonData, new HandlerEvent(this.OnEventRefreshCommonData));
			manager.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPInfoData, new HandlerEvent(this.OnEventRefreshIAPInfoData));
			manager.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPBattlePassData, new HandlerEvent(this.OnEventRefreshIAPBattlePassData));
			manager.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPBattlePassScore, new HandlerEvent(this.OnEventRefreshIAPBattlePassScore));
			manager.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPLevelFundRewards, new HandlerEvent(this.OnEventRefreshIAPLevelFundRewards));
			manager.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPChapterGiftData, new HandlerEvent(this.OnEventRefreshIAPChapterGiftData));
			manager.UnRegisterEvent(LocalMessageName.CC_IAPData_RefreshIAPFirstGiftData, new HandlerEvent(this.OnEventRefreshIAPFirstGiftData));
			manager.UnRegisterEvent(LocalMessageName.CC_IAP_BattleFail_ShowFirstRechargeUI, new HandlerEvent(this.OnEventRefreshShowRechargeUI));
			manager.UnRegisterEvent(LocalMessageName.CC_Function_Open, new HandlerEvent(this.OnFunctionOpen));
		}

		public void Reset()
		{
			this.Common = new PurchaseCommonData();
			this.DiamondsPackData = new IAPDiamondsPackData(this.Common);
			this.TimePackData = new IAPTimePackData(this.Common);
			this.BattlePass = new IAPBattlePass(this.Common);
			this.LevelFund = new IAPLevelFund(this.Common);
			this.MonthCard = new IAPMonthCardData(this.Common);
			this.ChapterGift = new IAPChapterGift(this.Common);
			this.FirstGift = new IAPFirstGift(this.Common);
			this.OpenServerGift = new IAPOpenServerGift(this.Common);
			this.LimitedTimeGift = new IAPLimitedTimeGift(this.Common);
			this.SuperValuePack = new IAPSuperValuePack(this.Common);
			this.RechargeGift = new IAPRechargeGift(this.Common);
			this.MeetingGift = new IAPMeetingGift(this.Common);
			this.ShopDrawMiniCount.Clear();
			this.ShopDrawHardCount.Clear();
			this.FreeCostTimes.Clear();
			this.ActivityDict.Clear();
			this.ShowSUpPoolPreviewCount = 0;
		}

		public int GetShopDrawMiniCount(int configId)
		{
			int num;
			this.ShopDrawMiniCount.TryGetValue(configId, out num);
			return num;
		}

		public int GetShopDrawHardCount(int configId)
		{
			int num;
			this.ShopDrawHardCount.TryGetValue(configId, out num);
			return num;
		}

		public int GetFreeCostTimes(int configId)
		{
			int num;
			this.FreeCostTimes.TryGetValue(configId, out num);
			return num;
		}

		public void InitForServer(ShopAllDataDto dataDto)
		{
			if (dataDto == null)
			{
				return;
			}
			this.RefreshCommonData(dataDto.RechargeIds, true);
			this.RefreshIAPInfo(dataDto.IapInfo, true, true);
			this.UpdateShopDrawCount(dataDto.ShopDrawDto);
			this.UpdateShopActivity(dataDto.ShopAct);
		}

		public List<IAPShopActivityData> GetShopActivities(int iapShopActivityType)
		{
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			List<IAPShopActivityData> list = new List<IAPShopActivityData>();
			foreach (IAPShopActivityData iapshopActivityData in this.ActivityDict.Values)
			{
				if (iapshopActivityData.activityType == iapShopActivityType && iapshopActivityData.startTimestamp <= serverTimestamp && serverTimestamp <= iapshopActivityData.endTimestamp)
				{
					list.Add(iapshopActivityData);
				}
			}
			return list;
		}

		public void UpdateShopActivity(ShopActDto dto)
		{
			if (dto == null || dto.ShopDetails == null)
			{
				return;
			}
			for (int i = 0; i < dto.ShopDetails.Count; i++)
			{
				ShopActDetailDto shopActDetailDto = dto.ShopDetails[i];
				if (shopActDetailDto != null)
				{
					int shopActivity = (int)shopActDetailDto.ShopActivity;
					if (this.ActivityDict.ContainsKey(shopActivity))
					{
						this.ActivityDict[shopActivity].UpdateData(shopActDetailDto);
					}
					else
					{
						this.ActivityDict[shopActivity] = new IAPShopActivityData(shopActDetailDto);
					}
				}
			}
		}

		public void UpdateShopDrawCount(ShopDrawDto dto)
		{
			if (dto == null)
			{
				return;
			}
			this.ShopFreeDrawResetTimestamp = (long)dto.DayResetTimeStamp;
			this.ShopDrawMiniCount.Clear();
			this.ShopDrawHardCount.Clear();
			this.FreeCostTimes.Clear();
			foreach (KeyValuePair<uint, uint> keyValuePair in dto.MiniDrawCount)
			{
				this.ShopDrawMiniCount[(int)keyValuePair.Key] = (int)keyValuePair.Value;
			}
			foreach (KeyValuePair<uint, uint> keyValuePair2 in dto.HardDrawCount)
			{
				this.ShopDrawHardCount[(int)keyValuePair2.Key] = (int)keyValuePair2.Value;
			}
			foreach (KeyValuePair<uint, uint> keyValuePair3 in dto.FreeCostTimes)
			{
				this.FreeCostTimes[(int)keyValuePair3.Key] = (int)keyValuePair3.Value;
			}
		}

		public void UpdateTGAInfo(TGAInfoDto dto)
		{
			if (dto == null)
			{
				return;
			}
			this.AccountTotalPayTimes = dto.TotalPayTimes;
			if (!string.IsNullOrEmpty(dto.TotalChargeStr))
			{
				decimal num;
				if (decimal.TryParse(dto.TotalChargeStr, out num))
				{
					this.AccountTotalCharge = num;
				}
			}
			else
			{
				this.AccountTotalCharge = (decimal)dto.TotalCharge;
			}
			this.AccountFirstPayTime = dto.FirstPayTime;
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.IAP);
		}

		private void RefreshCommonData(MapField<uint, uint> buyIds, bool isClear)
		{
			this.Common.SetPurchaseData(buyIds, isClear);
			RedPointController.Instance.ReCalc("MainShop", true);
			RedPointController.Instance.ReCalc("IAPRechargeGift", true);
			RedPointController.Instance.ReCalc("IAPPrivileggeCard", true);
		}

		private void RefreshIAPInfo(IAPDto iapInfo, bool isClear, bool isInitData)
		{
			if (iapInfo == null)
			{
				return;
			}
			this.TimePackData.SetData(iapInfo.PacksBuyCount, iapInfo.PacksResetTimeDay, iapInfo.PacksResetTimeWeek, iapInfo.PacksResetTimeMonth, isClear);
			this.BattlePass.SetDatas(iapInfo.BattlePassInfo, isClear);
			this.LevelFund.SetDatas(iapInfo.BuyLevelFundGroupId, iapInfo.LevelFundReward, iapInfo.FreeLevelFundReward, isClear);
			this.MonthCard.SetData(iapInfo.MonthCardMap, isClear, iapInfo.IsHeadFrameActive);
			this.ChapterGift.SetData(iapInfo.ChapterGiftTime, isClear, !isInitData);
			this.FirstGift.SetData(iapInfo.FirstRechargeReward, (int)iapInfo.TotalRecharge, isClear);
			this.OpenServerGift.SetData(iapInfo.BuyOpenServerGiftId, isClear);
			this.LimitedTimeGift.SetData(isClear);
			this.RechargeGift.SetData(iapInfo.BuyFirstChargeGiftId, iapInfo.FirstChargeGiftPassTime, isInitData);
			this.MeetingGift.SetData(iapInfo.BuyFirstChargeGiftInfo, iapInfo.FirstChargeGiftReward, iapInfo.FirstChargeGiftEndTime, isInitData);
			GameApp.Data.GetDataModule(DataName.ActivitySlotTrainDataModule).CommonUpdateTurntablePayCount(iapInfo.TurntablePayCount);
			GameApp.Data.GetDataModule(DataName.ChapterBattlePassDataModule).UpdateData(iapInfo.ChapterBattlePassDto);
			RedPointController.Instance.ReCalc("MainShop", true);
			RedPointController.Instance.ReCalc("IAPGift", true);
			RedPointController.Instance.ReCalc("IAPRechargeGift", true);
			RedPointController.Instance.ReCalc("IAPGift.Meeting", true);
			RedPointController.Instance.ReCalc("IAPPrivileggeCard", true);
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.IAP);
		}

		private void OnEventRefreshCommonData(object sender, int type, BaseEventArgs args)
		{
			EventArgsRefreshIAPCommonData eventArgsRefreshIAPCommonData = args as EventArgsRefreshIAPCommonData;
			if (eventArgsRefreshIAPCommonData == null)
			{
				return;
			}
			this.RefreshCommonData(eventArgsRefreshIAPCommonData.BuyId, eventArgsRefreshIAPCommonData.IsClear);
		}

		private void OnEventRefreshIAPInfoData(object sender, int type, BaseEventArgs args)
		{
			EventArgsRefreshIAPInfoData eventArgsRefreshIAPInfoData = args as EventArgsRefreshIAPInfoData;
			if (eventArgsRefreshIAPInfoData == null)
			{
				return;
			}
			this.RefreshIAPInfo(eventArgsRefreshIAPInfoData.IapInfo, false, false);
		}

		private void OnEventRefreshIAPBattlePassData(object sender, int type, BaseEventArgs args)
		{
			EventArgsRefreshIAPBattlePassData eventArgsRefreshIAPBattlePassData = args as EventArgsRefreshIAPBattlePassData;
			if (eventArgsRefreshIAPBattlePassData == null)
			{
				return;
			}
			this.RefreshIAPBattlePass(eventArgsRefreshIAPBattlePassData.BattlePassDto, false);
		}

		private void OnEventRefreshIAPBattlePassScore(object sender, int type, BaseEventArgs args)
		{
			EventArgsRefreshIAPBattlePassScore eventArgsRefreshIAPBattlePassScore = args as EventArgsRefreshIAPBattlePassScore;
			if (eventArgsRefreshIAPBattlePassScore == null)
			{
				return;
			}
			this.RefreshIAPBattlePassScore(eventArgsRefreshIAPBattlePassScore.Score);
		}

		private void OnEventRefreshIAPLevelFundRewards(object sender, int type, BaseEventArgs args)
		{
			EventArgsRefreshIAPLevelFundRewards eventArgsRefreshIAPLevelFundRewards = args as EventArgsRefreshIAPLevelFundRewards;
			if (eventArgsRefreshIAPLevelFundRewards == null)
			{
				return;
			}
			if (this.LevelFund != null)
			{
				this.LevelFund.SetDatas(null, eventArgsRefreshIAPLevelFundRewards.PayRewards, eventArgsRefreshIAPLevelFundRewards.FreeRewards, false);
			}
		}

		private void OnEventRefreshIAPChapterGiftData(object sender, int type, BaseEventArgs args)
		{
			EventArgsRefreshIAPChapterGiftData eventArgsRefreshIAPChapterGiftData = args as EventArgsRefreshIAPChapterGiftData;
			if (eventArgsRefreshIAPChapterGiftData == null)
			{
				return;
			}
			this.ChapterGift.SetData(eventArgsRefreshIAPChapterGiftData.ChapterGiftTime, false, true);
		}

		private void OnEventRefreshIAPFirstGiftData(object sender, int type, BaseEventArgs args)
		{
			EventArgsRefreshIAPFirstGiftData eventArgsRefreshIAPFirstGiftData = args as EventArgsRefreshIAPFirstGiftData;
			if (eventArgsRefreshIAPFirstGiftData == null)
			{
				return;
			}
			this.FirstGift.SetData(eventArgsRefreshIAPFirstGiftData.m_firstRechargeReward, eventArgsRefreshIAPFirstGiftData.m_totalRecharge, false);
		}

		private void OnEventRefreshShowRechargeUI(object sender, int type, BaseEventArgs args)
		{
			EventArgsBool eventArgsBool = args as EventArgsBool;
			if (eventArgsBool != null)
			{
				this.SetShowFirstRecharge(eventArgsBool.Value);
			}
		}

		private void OnFunctionOpen(object sender, int type, BaseEventArgs eventArgs)
		{
			EventArgsFunctionOpen eventArgsFunctionOpen = eventArgs as EventArgsFunctionOpen;
			if (eventArgsFunctionOpen == null)
			{
				return;
			}
			int functionID = eventArgsFunctionOpen.FunctionID;
			if (functionID == 0)
			{
				return;
			}
			if (functionID == 201)
			{
				GameApp.Data.GetDataModule(DataName.IAPDataModule).MeetingGift.SetUnlock();
			}
		}

		private void RefreshIAPBattlePass(IAPBattlePassDto battlepass, bool isClear)
		{
			if (this.BattlePass != null)
			{
				this.BattlePass.SetDatas(battlepass, isClear);
			}
		}

		private void RefreshIAPBattlePassScore(int score)
		{
			if (this.BattlePass != null)
			{
				this.BattlePass.ChangeScore(score);
			}
		}

		public void SetShowFirstRecharge(bool isShow)
		{
			this.IsShowFirstRecharge = isShow;
		}

		public int ShowSUpPoolPreviewCount { get; private set; }

		public int ShopSUpPoolDrawCount { get; private set; }

		public IAPShopActivityData GetShopSUpActivityData()
		{
			List<IAPShopActivityData> shopActivities = this.GetShopActivities(2);
			if (shopActivities.Count > 0)
			{
				return shopActivities[0];
			}
			return null;
		}

		public int GetEquipSUpPoolDrawCount()
		{
			return this.ShopSUpPoolDrawCount;
		}

		public int GetEquipSUpPoolLeftSelectTime()
		{
			int equipSUpPoolDrawCount = this.GetEquipSUpPoolDrawCount();
			int shopEquipSUpExchangeCount = Singleton<GameConfig>.Instance.ShopEquipSUpExchangeCount;
			return equipSUpPoolDrawCount / shopEquipSUpExchangeCount;
		}

		public bool CanShowSUpPoolPreview(out int shopActivityId)
		{
			shopActivityId = 0;
			IAPShopActivityData shopSUpActivityData = this.GetShopSUpActivityData();
			if (shopSUpActivityData != null && this.ShowSUpPoolPreviewCount <= 0)
			{
				shopActivityId = shopSUpActivityData.activityId;
				return true;
			}
			return false;
		}

		public void SetShowSUpPoolPreviewCount()
		{
			int showSUpPoolPreviewCount = this.ShowSUpPoolPreviewCount;
			this.ShowSUpPoolPreviewCount = showSUpPoolPreviewCount + 1;
		}

		public void UpdateShopSUpPoolDrawCount(int count)
		{
			this.ShopSUpPoolDrawCount = count;
		}
	}
}
