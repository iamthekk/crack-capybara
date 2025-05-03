using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class IAPFirstGift
	{
		public IAPFirstGift(PurchaseCommonData commonDataVal)
		{
			this.commonData = commonDataVal;
		}

		public void SetData(bool firstRechargeRewardVal, int totalRechargeVal, bool isClear)
		{
			this.firstRechargeReward = firstRechargeRewardVal;
			this.totalRecharge = totalRechargeVal;
		}

		public bool IsEnable()
		{
			return GameApp.Purchase.IsEnable && !this.IsReceive();
		}

		public bool IsConsume()
		{
			return this.totalRecharge > 0;
		}

		public bool IsReceive()
		{
			return this.firstRechargeReward;
		}

		public int GetDataID()
		{
			return 601;
		}

		public int GetPriority()
		{
			IAP_PushPacks elementById = GameApp.Table.GetManager().GetIAP_PushPacksModelInstance().GetElementById(this.GetDataID());
			if (elementById == null)
			{
				return -1;
			}
			return elementById.priority;
		}

		private const int ID = 601;

		private PurchaseCommonData commonData;

		private bool firstRechargeReward;

		private int totalRecharge;
	}
}
