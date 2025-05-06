using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class IAPLevelFundGroup
	{
		public List<IAPLevelFundData> LevelFundList
		{
			get
			{
				return this.mLevelFundList;
			}
		}

		public bool HasBuy { get; set; }

		public int PurchaseID
		{
			get
			{
				return this.ID;
			}
		}

		public int Level
		{
			get
			{
				if (this.mLevelFundList == null)
				{
					return 0;
				}
				int num = this.mLevelFundList.Count;
				for (int i = 0; i < this.mLevelFundList.Count; i++)
				{
					if (this.mLevelFundList[i].freeRewardState == IAPLevelFund.RewardState.Lock)
					{
						num = i;
						break;
					}
				}
				return num;
			}
		}

		public List<int> FreeCollectedRewardedList
		{
			get
			{
				List<int> list = new List<int>();
				if (this.mLevelFundList.Count > 0)
				{
					for (int i = 0; i < this.mLevelFundList.Count; i++)
					{
						if (this.mLevelFundList[i].freeRewardState == IAPLevelFund.RewardState.Collected)
						{
							list.Add(i + 1);
						}
					}
				}
				return list;
			}
		}

		public List<int> PayCollectedRewardedList
		{
			get
			{
				List<int> list = new List<int>();
				if (this.mLevelFundList.Count > 0)
				{
					for (int i = 0; i < this.mLevelFundList.Count; i++)
					{
						if (this.mLevelFundList[i].payRewardState == IAPLevelFund.RewardState.Collected)
						{
							list.Add(i + 1);
						}
					}
				}
				return list;
			}
		}

		public void BuildData(IAP_LevelFund lftab)
		{
			this.ID = lftab.id;
			this.GroupID = lftab.groupId;
			this.Index = lftab.index;
			this.GroupKind = (IAPLevelFundGroupKind)lftab.paramType;
			this.ProductsRewards = new List<PropData>();
			this.ProductsRewards.AddRange(lftab.products.ToPropDataList());
			this.mLevelFundList = new List<IAPLevelFundData>();
			IList<IAP_LevelFundReward> allElements = GameApp.Table.GetManager().GetIAP_LevelFundRewardModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				IAP_LevelFundReward iap_LevelFundReward = allElements[i];
				if (iap_LevelFundReward.groupId == this.GroupID)
				{
					IAPLevelFundData iaplevelFundData = new IAPLevelFundData();
					iaplevelFundData.Init(iap_LevelFundReward, this.ID, this.GroupKind);
					this.mLevelFundList.Add(iaplevelFundData);
				}
			}
		}

		public List<IAPLevelFundData> GetNotGetList()
		{
			List<IAPLevelFundData> list = new List<IAPLevelFundData>();
			for (int i = 0; i < this.LevelFundList.Count; i++)
			{
				if (this.LevelFundList[i].payRewardState == IAPLevelFund.RewardState.Lock || this.LevelFundList[i].freeRewardState == IAPLevelFund.RewardState.Lock)
				{
					list.Add(this.LevelFundList[i]);
				}
			}
			return list;
		}

		public List<IAPLevelFundData> GetList()
		{
			return this.LevelFundList;
		}

		public List<PropData> GetTotalRewards()
		{
			List<PropData> list = new List<PropData>();
			if (this.ProductsRewards != null)
			{
				list.AddRange(this.ProductsRewards);
			}
			for (int i = 0; i < this.mLevelFundList.Count; i++)
			{
				IAPLevelFundData iaplevelFundData = this.mLevelFundList[i];
				list.AddRange(iaplevelFundData.payRewards);
				list.AddRange(iaplevelFundData.freeRewards);
			}
			return list;
		}

		public int GetTotalDiamondsCount()
		{
			int num = 0;
			if (this.ProductsRewards != null)
			{
				foreach (PropData propData in this.ProductsRewards)
				{
					if (propData.id == 2U)
					{
						num += (int)propData.count;
					}
				}
			}
			for (int i = 0; i < this.mLevelFundList.Count; i++)
			{
				IAPLevelFundData iaplevelFundData = this.mLevelFundList[i];
				if (iaplevelFundData.payRewards != null)
				{
					foreach (PropData propData2 in iaplevelFundData.payRewards)
					{
						if (propData2.id == 2U)
						{
							num += (int)propData2.count;
						}
					}
				}
				if (iaplevelFundData.freeRewards != null)
				{
					foreach (PropData propData3 in iaplevelFundData.freeRewards)
					{
						if (propData3.id == 2U)
						{
							num += (int)propData3.count;
						}
					}
				}
			}
			return num;
		}

		public int GetTotalKeyCount(int itemId)
		{
			int num = 0;
			if (this.ProductsRewards != null)
			{
				foreach (PropData propData in this.ProductsRewards)
				{
					if ((ulong)propData.id == (ulong)((long)itemId))
					{
						num += (int)propData.count;
					}
				}
			}
			for (int i = 0; i < this.mLevelFundList.Count; i++)
			{
				IAPLevelFundData iaplevelFundData = this.mLevelFundList[i];
				if (iaplevelFundData.payRewards != null)
				{
					foreach (PropData propData2 in iaplevelFundData.payRewards)
					{
						if ((ulong)propData2.id == (ulong)((long)itemId))
						{
							num += (int)propData2.count;
						}
					}
				}
				if (iaplevelFundData.freeRewards != null)
				{
					foreach (PropData propData3 in iaplevelFundData.freeRewards)
					{
						if ((ulong)propData3.id == (ulong)((long)itemId))
						{
							num += (int)propData3.count;
						}
					}
				}
			}
			return num;
		}

		public void SetPayRewardCollected(IntegerArray array)
		{
			if (array == null)
			{
				return;
			}
			IList<int> integerArray_ = array.IntegerArray_;
			for (int i = 0; i < this.mLevelFundList.Count; i++)
			{
				if (integerArray_.Contains(this.mLevelFundList[i].ID))
				{
					this.mLevelFundList[i].payState = 1;
				}
			}
		}

		public void SetFreeRewardCollected(IntegerArray array)
		{
			if (array == null)
			{
				return;
			}
			IList<int> integerArray_ = array.IntegerArray_;
			for (int i = 0; i < this.mLevelFundList.Count; i++)
			{
				if (integerArray_.Contains(this.mLevelFundList[i].ID))
				{
					this.mLevelFundList[i].freeState = 1;
				}
			}
		}

		public bool IsAllRewardCollected()
		{
			for (int i = 0; i < this.mLevelFundList.Count; i++)
			{
				if (this.mLevelFundList[i].freeRewardState != IAPLevelFund.RewardState.Collected)
				{
					return false;
				}
				if (this.mLevelFundList[i].payRewardState != IAPLevelFund.RewardState.Collected)
				{
					return false;
				}
			}
			return true;
		}

		public List<int> GetCanCollectList()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.mLevelFundList.Count; i++)
			{
				if (this.mLevelFundList[i].freeRewardState == IAPLevelFund.RewardState.CanCollect || this.mLevelFundList[i].payRewardState == IAPLevelFund.RewardState.CanCollect)
				{
					list.Add(this.mLevelFundList[i].ID);
				}
			}
			return list;
		}

		public string GetNextShowProgress()
		{
			switch (this.GroupKind)
			{
			case IAPLevelFundGroupKind.TalentLevel:
			{
				int talentExp = GameApp.Data.GetDataModule(DataName.TalentDataModule).TalentExp;
				for (int i = 0; i < this.mLevelFundList.Count; i++)
				{
					if (this.LevelFundList[i].IntParam > talentExp)
					{
						return this.LevelFundList[i].IntParam.ToString();
					}
				}
				List<IAPLevelFundData> levelFundList = this.LevelFundList;
				return levelFundList[levelFundList.Count - 1].IntParam.ToString();
			}
			case IAPLevelFundGroupKind.TowerLevel:
			{
				TowerDataModule dataModule = GameApp.Data.GetDataModule(DataName.TowerDataModule);
				int curTowerLevelId = dataModule.CurTowerLevelId;
				for (int j = 0; j < this.mLevelFundList.Count; j++)
				{
					if (this.LevelFundList[j].IntParam > curTowerLevelId)
					{
						int intParam = this.LevelFundList[j].IntParam;
						TowerChallenge_Tower towerConfigByLevelId = dataModule.GetTowerConfigByLevelId(intParam);
						int towerConfigNum = dataModule.GetTowerConfigNum(towerConfigByLevelId);
						int levelNumByLevelId = dataModule.GetLevelNumByLevelId(intParam);
						return string.Format("{0}-{1}", towerConfigNum, levelNumByLevelId);
					}
				}
				List<IAPLevelFundData> levelFundList2 = this.LevelFundList;
				int intParam2 = levelFundList2[levelFundList2.Count - 1].IntParam;
				TowerChallenge_Tower towerConfigByLevelId2 = dataModule.GetTowerConfigByLevelId(intParam2);
				int towerConfigNum2 = dataModule.GetTowerConfigNum(towerConfigByLevelId2);
				int levelNumByLevelId2 = dataModule.GetLevelNumByLevelId(intParam2);
				return string.Format("{0}-{1}", towerConfigNum2, levelNumByLevelId2);
			}
			case IAPLevelFundGroupKind.RogueDungeonFloor:
			{
				uint currentFloorID = GameApp.Data.GetDataModule(DataName.RogueDungeonDataModule).CurrentFloorID;
				for (int k = 0; k < this.mLevelFundList.Count; k++)
				{
					if ((long)this.LevelFundList[k].IntParam > (long)((ulong)currentFloorID))
					{
						int intParam3 = this.LevelFundList[k].IntParam;
						return intParam3.ToString();
					}
				}
				List<IAPLevelFundData> levelFundList3 = this.LevelFundList;
				int intParam4 = levelFundList3[levelFundList3.Count - 1].IntParam;
				return intParam4.ToString();
			}
			default:
				return "--";
			}
		}

		public int ID;

		public int GroupID;

		public int Index;

		public IAPLevelFundGroupKind GroupKind;

		public List<PropData> ProductsRewards;

		private List<IAPLevelFundData> mLevelFundList;
	}
}
