using System;
using System.Collections.Generic;
using Framework;
using Google.Protobuf.Collections;
using LocalModels.Bean;

namespace HotFix
{
	public class IAPOpenServerGift
	{
		public IAPOpenServerGift(PurchaseCommonData commonDataVal)
		{
			this.commonData = commonDataVal;
			this.loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
		}

		public void SetData(RepeatedField<uint> buyPurchaseIDsVal, bool isClear)
		{
			if (isClear)
			{
				this.buyPurchaseIDs.Clear();
			}
			foreach (uint num in buyPurchaseIDsVal)
			{
				this.buyPurchaseIDs.Add((int)num);
			}
		}

		public bool IsEnable()
		{
			return GameApp.Purchase.IsEnable && this.GetPurchaseData() != null && !this.IsBuy() && !this.IsTimeOut();
		}

		public bool IsBuy()
		{
			return this.buyPurchaseIDs.Contains(602);
		}

		public bool IsTimeOut()
		{
			return this.GetLastTime() < 0L;
		}

		public long GetLastTime()
		{
			long num;
			if (this.TryFinishedTime(out num))
			{
				return num - this.loginDataModule.ServerUTC;
			}
			return -1L;
		}

		public string GetLastTimeString()
		{
			long lastTime = this.GetLastTime();
			if (lastTime < 0L)
			{
				return string.Empty;
			}
			return Singleton<LanguageManager>.Instance.GetTime(lastTime);
		}

		public bool TryFinishedTime(out long time)
		{
			time = (long)this.loginDataModule.registerTimestamp;
			IAP_PushPacks elementById = GameApp.Table.GetManager().GetIAP_PushPacksModelInstance().GetElementById(602);
			long num;
			if (elementById != null && long.TryParse(elementById.parameters, out num))
			{
				time += num;
				return true;
			}
			return false;
		}

		public PurchaseCommonData.PurchaseData GetPurchaseData()
		{
			if (this.commonData == null)
			{
				return null;
			}
			return this.commonData.GetPurchaseDataByID(602);
		}

		public int GetPriority()
		{
			IAP_PushPacks elementById = GameApp.Table.GetManager().GetIAP_PushPacksModelInstance().GetElementById(602);
			if (elementById == null)
			{
				return -1;
			}
			return elementById.priority;
		}

		private const int ID = 602;

		private readonly List<int> buyPurchaseIDs = new List<int>();

		private readonly PurchaseCommonData commonData;

		private readonly LoginDataModule loginDataModule;
	}
}
