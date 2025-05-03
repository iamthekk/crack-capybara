using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class IAPLimitedTimeGift
	{
		public IAPLimitedTimeGift(PurchaseCommonData commonDataVal)
		{
			this.commonData = commonDataVal;
			this.loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
		}

		public void SetData(bool isClear)
		{
			this.TryTime(out this.startTime, out this.endTime);
		}

		public bool IsEnable()
		{
			return GameApp.Purchase.IsEnable && this.GetPurchaseData() != null && this.IsOnTime();
		}

		public bool IsOnTime()
		{
			return this.loginDataModule.ServerUTC >= this.startTime && this.loginDataModule.ServerUTC < this.endTime;
		}

		public long GetLastTime()
		{
			long num = -1L;
			if (!this.IsOnTime())
			{
				return num;
			}
			return this.endTime - this.loginDataModule.ServerUTC;
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

		public bool TryTime(out long startTimeVal, out long endTimeVal)
		{
			startTimeVal = (long)this.loginDataModule.registerTimestamp;
			endTimeVal = (long)this.loginDataModule.registerTimestamp;
			IAP_PushPacks elementById = GameApp.Table.GetManager().GetIAP_PushPacksModelInstance().GetElementById(603);
			if (elementById == null)
			{
				return false;
			}
			List<long> listLong = elementById.parameters.GetListLong('|');
			if (listLong.Count != 2)
			{
				return false;
			}
			startTimeVal = listLong[0];
			endTimeVal = listLong[1];
			return true;
		}

		public PurchaseCommonData.PurchaseData GetPurchaseData()
		{
			if (this.commonData == null)
			{
				return null;
			}
			return this.commonData.GetPurchaseDataByID(603);
		}

		public int GetPriority()
		{
			IAP_PushPacks elementById = GameApp.Table.GetManager().GetIAP_PushPacksModelInstance().GetElementById(603);
			if (elementById == null)
			{
				return -1;
			}
			return elementById.priority;
		}

		private const int ID = 603;

		private long startTime;

		private long endTime;

		private readonly PurchaseCommonData commonData;

		private readonly LoginDataModule loginDataModule;
	}
}
