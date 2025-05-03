using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Framework;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class IAPBattlePass
	{
		public IAPBattlePassDto BattlePassDto
		{
			get
			{
				return this.mBattlePass;
			}
		}

		public List<IAPBattlePassData> DataList
		{
			get
			{
				return this.mBattlePassList;
			}
		}

		public int BattlePassID
		{
			get
			{
				if (this.mBattlePass == null)
				{
					return 0;
				}
				return (int)this.mBattlePass.BattlePassId;
			}
		}

		public int CurrentScore
		{
			get
			{
				if (this.mBattlePass == null)
				{
					return 0;
				}
				return (int)this.mBattlePass.Score;
			}
		}

		public bool HasBuy
		{
			get
			{
				return this.mBattlePass != null && this.mBattlePass.Buy == 1U;
			}
		}

		public long EndTime { get; private set; }

		public int BattlePassPurchaseID
		{
			get
			{
				return 401;
			}
		}

		public int FinalRewardGetCount
		{
			get
			{
				if (this.mBattlePass == null)
				{
					return 0;
				}
				return (int)this.mBattlePass.RewardFinalCount;
			}
		}

		public int FinalRewardCanGetCount
		{
			get
			{
				if (this.mBattlePass == null)
				{
					return 0;
				}
				return (int)this.mBattlePass.CanRewardFinalCount;
			}
		}

		public int FinalRewardMaxCount { get; private set; }

		public int Level { get; private set; }

		public IAPBattlePass(PurchaseCommonData commonData)
		{
			this.m_commonData = commonData;
		}

		internal void SetDatas(IAPBattlePassDto battlepass, bool clear)
		{
			int battlePassID = this.BattlePassID;
			this.mBattlePass = battlepass;
			IAP_BattlePass elementById = GameApp.Table.GetManager().GetIAP_BattlePassModelInstance().GetElementById(this.BattlePassID);
			if (elementById == null)
			{
				this.mBattlePassList.Clear();
				if (GameApp.Purchase.IsEnable)
				{
					HLog.LogError(string.Format("[IAP]未定义通行证：{0}", this.BattlePassID));
				}
				return;
			}
			LoginDataModule dataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.EndTime = dataModule.ServerOpenMidNightTimestamp + (long)((elementById.endTime + 1) * 24 * 3600);
			this.FinalRewardMaxCount = elementById.finalRewardTimes;
			if (battlePassID != this.BattlePassID || clear)
			{
				this.mBattlePassList.Clear();
			}
			if (this.mBattlePassList.Count == 0)
			{
				this.OnReBuildList();
			}
			this.RefreshLevel();
			RedPointController.Instance.ReCalc("IAPRechargeGift", true);
		}

		private void OnReBuildList()
		{
			this.mBattlePassList.Clear();
			int battlePassID = this.BattlePassID;
			IAP_BattlePass elementById = GameApp.Table.GetManager().GetIAP_BattlePassModelInstance().GetElementById(battlePassID);
			if (elementById == null)
			{
				HLog.LogError(string.Format("[IAP]未知通行证:{0}", battlePassID));
				return;
			}
			int groupId = elementById.groupId;
			IList<IAP_BattlePassReward> allElements = GameApp.Table.GetManager().GetIAP_BattlePassRewardModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				IAP_BattlePassReward iap_BattlePassReward = allElements[i];
				if (iap_BattlePassReward.groupId == groupId)
				{
					IAPBattlePassData iapbattlePassData = new IAPBattlePassData();
					iapbattlePassData.ID = iap_BattlePassReward.id;
					iapbattlePassData.Level = iap_BattlePassReward.level;
					iapbattlePassData.Index = iap_BattlePassReward.level - 1;
					iapbattlePassData.Type = (BattlePassType)iap_BattlePassReward.type;
					iapbattlePassData.FreeRewards = iap_BattlePassReward.freeReward.ToPropDataList();
					iapbattlePassData.PayRewards = iap_BattlePassReward.battlePassReward.ToPropDataList();
					iapbattlePassData.Score = iap_BattlePassReward.score;
					iapbattlePassData.IsFinal = iapbattlePassData.Type == BattlePassType.FinalLoop;
					this.mBattlePassList.Add(iapbattlePassData);
				}
			}
			this.mBattlePassList.Sort(new Comparison<IAPBattlePassData>(IAPBattlePassData.Sort));
			for (int j = 0; j < this.mBattlePassList.Count; j++)
			{
				IAPBattlePassData iapbattlePassData2 = this.mBattlePassList[j];
				if (j > 0)
				{
					iapbattlePassData2.LastScore = this.mBattlePassList[j - 1].Score;
				}
				else
				{
					iapbattlePassData2.LastScore = iapbattlePassData2.Score;
				}
				if (j + 1 >= this.mBattlePassList.Count)
				{
					iapbattlePassData2.NextScore = iapbattlePassData2.Score;
				}
				else
				{
					iapbattlePassData2.NextScore = this.mBattlePassList[j + 1].Score;
				}
			}
		}

		public bool IsHaveRedPoint()
		{
			if (this.mBattlePassList == null || this.mBattlePassList.Count == 0)
			{
				return false;
			}
			if (this.mBattlePass == null || this.mBattlePass.BattlePassId == 0U)
			{
				return false;
			}
			if (this.GetNextTime() <= 0L)
			{
				return true;
			}
			if (this.mBattlePass.CanRewardFinalCount > 0U)
			{
				return true;
			}
			bool hasBuy = this.HasBuy;
			for (int i = 0; i < this.mBattlePassList.Count; i++)
			{
				IAPBattlePassData iapbattlePassData = this.mBattlePassList[i];
				if (iapbattlePassData != null && !iapbattlePassData.IsFinal && iapbattlePassData.Score <= this.CurrentScore)
				{
					bool flag;
					bool flag2;
					this.GetBattlePassRewardGet(iapbattlePassData.ID, out flag, out flag2);
					if (!flag || (hasBuy && !flag2))
					{
						return true;
					}
				}
			}
			return false;
		}

		public long GetNextTime()
		{
			long num = this.EndTime - DxxTools.Time.ServerTimestamp;
			return Math.Max(0L, num);
		}

		public void ChangeScore(int score)
		{
			if (this.mBattlePass != null)
			{
				this.mBattlePass.Score = (uint)score;
			}
			this.RefreshLevel();
			RedPointController.Instance.ReCalc("IAPRechargeGift", true);
		}

		private void RefreshLevel()
		{
			if (this.BattlePassDto == null)
			{
				return;
			}
			int currentScore = this.CurrentScore;
			for (int i = 0; i < this.mBattlePassList.Count; i++)
			{
				if (currentScore < this.mBattlePassList[i].Score)
				{
					this.Level = i;
					return;
				}
				if (this.mBattlePassList[i].Type == BattlePassType.FinalLoop)
				{
					this.Level = i;
					return;
				}
			}
		}

		public void GetBattlePassRewardGet(int id, out bool freereward, out bool payreward)
		{
			if (this.mBattlePass == null)
			{
				freereward = false;
				payreward = false;
				return;
			}
			freereward = this.mBattlePass.FreeRewardIdList.Contains((uint)id);
			payreward = this.mBattlePass.BattlePassRewardIdList.Contains((uint)id);
		}

		public IAPBattlePassData GetLastData(int id)
		{
			int i = 0;
			while (i < this.mBattlePassList.Count)
			{
				if (this.mBattlePassList[i].ID == id)
				{
					if (i > 0)
					{
						return this.mBattlePassList[i - 1];
					}
					return null;
				}
				else
				{
					i++;
				}
			}
			return null;
		}

		public IAPBattlePassData GetCurrentData()
		{
			if (this.mBattlePassList == null)
			{
				return null;
			}
			int currentScore = this.CurrentScore;
			for (int i = 0; i < this.mBattlePassList.Count; i++)
			{
				if (this.mBattlePassList[i].Score > currentScore)
				{
					return this.mBattlePassList[i];
				}
			}
			return this.mBattlePassList[this.mBattlePassList.Count - 1];
		}

		public bool IsOpenBuyScore()
		{
			IAP_BattlePass elementById = GameApp.Table.GetManager().GetIAP_BattlePassModelInstance().GetElementById(this.BattlePassID);
			if (elementById == null)
			{
				return false;
			}
			long num = GameApp.Data.GetDataModule(DataName.LoginDataModule).ServerOpenMidNightTimestamp + (long)(elementById.buyTime * 24 * 3600);
			return num <= 0L || DxxTools.Time.ServerTimestamp >= num;
		}

		[return: TupleElementNames(new string[] { "min", "max", "current" })]
		public ValueTuple<int, int, int> GetShowProgress()
		{
			IAPBattlePassData currentData = this.GetCurrentData();
			int num4;
			int num5;
			int num6;
			if (currentData.IsFinal)
			{
				int score = currentData.Score;
				int num = currentData.LastScore + currentData.Score;
				int currentScore = this.CurrentScore;
				int num2 = this.FinalRewardMaxCount;
				num2--;
				while (currentScore >= num && num2 > 0)
				{
					num2--;
					num += currentData.Score;
				}
				int num3 = num;
				num4 = num3 - currentData.Score;
				num5 = num3;
				num6 = currentScore;
			}
			else
			{
				int lastScore = currentData.LastScore;
				int nextScore = currentData.NextScore;
				num4 = lastScore;
				num5 = nextScore;
				num6 = this.CurrentScore;
			}
			return new ValueTuple<int, int, int>(num4, num5, num6);
		}

		public bool IsAllEnd()
		{
			return this.BattlePassID == 0;
		}

		private IAPBattlePassDto mBattlePass;

		private List<IAPBattlePassData> mBattlePassList = new List<IAPBattlePassData>();

		private PurchaseCommonData m_commonData;
	}
}
