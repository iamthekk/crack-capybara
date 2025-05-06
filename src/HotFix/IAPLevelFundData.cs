using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class IAPLevelFundData
	{
		public IAPLevelFund.RewardState freeRewardState
		{
			get
			{
				bool flag = this.CheckArrive();
				if (this.freeState >= 1)
				{
					return IAPLevelFund.RewardState.Collected;
				}
				if (this.freeState == 0 && flag)
				{
					return IAPLevelFund.RewardState.CanCollect;
				}
				return IAPLevelFund.RewardState.Lock;
			}
		}

		public IAPLevelFund.RewardState payRewardState
		{
			get
			{
				bool flag = this.CheckArrive();
				if (this.payState >= 1)
				{
					return IAPLevelFund.RewardState.Collected;
				}
				bool flag2 = false;
				IAPLevelFundGroup levelFundGroup = GameApp.Data.GetDataModule(DataName.IAPDataModule).LevelFund.GetLevelFundGroup(this.FundId);
				if (levelFundGroup != null)
				{
					flag2 = levelFundGroup.HasBuy;
				}
				if (flag2 && this.payState == 0 && flag)
				{
					return IAPLevelFund.RewardState.CanCollect;
				}
				return IAPLevelFund.RewardState.Lock;
			}
		}

		public void Init(IAP_LevelFundReward tab, int fundId, IAPLevelFundGroupKind kind)
		{
			this.ID = tab.id;
			this.FundId = fundId;
			this.StringParam = tab.param;
			int num;
			if (int.TryParse(tab.param, out num))
			{
				this.IntParam = num;
			}
			else
			{
				HLog.LogError(string.Format("LevelFundReward parse param error id = {0} param = {1}", tab.id, tab.param));
				this.IntParam = 0;
			}
			this.freeRewards = tab.freeReward.ToPropDataList();
			this.payRewards = tab.fundReward.ToPropDataList();
			this.payState = 0;
			this.freeState = 0;
			this.GroupKind = kind;
		}

		public bool CheckArrive()
		{
			switch (this.GroupKind)
			{
			case IAPLevelFundGroupKind.TalentLevel:
				return GameApp.Data.GetDataModule(DataName.TalentDataModule).TalentExp >= this.IntParam;
			case IAPLevelFundGroupKind.TowerLevel:
				return GameApp.Data.GetDataModule(DataName.TowerDataModule).CompleteTowerLevelId >= this.IntParam;
			case IAPLevelFundGroupKind.RogueDungeonFloor:
				return (ulong)GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).CurrentFloorID > (ulong)((long)this.IntParam);
			default:
				return false;
			}
		}

		public int ID;

		public int FundId;

		public string StringParam;

		public int IntParam;

		public int freeState;

		public int payState;

		public IAPLevelFundGroupKind GroupKind;

		public List<PropData> freeRewards;

		public List<PropData> payRewards;
	}
}
