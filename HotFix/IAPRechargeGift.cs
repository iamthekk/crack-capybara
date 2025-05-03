using System;
using System.Collections.Generic;
using Framework;
using Google.Protobuf.Collections;
using LocalModels.Bean;

namespace HotFix
{
	public class IAPRechargeGift
	{
		public IAPRechargeGift(PurchaseCommonData commonDataVal)
		{
			this.commonData = commonDataVal;
		}

		public void SetData(RepeatedField<uint> buyList, MapField<uint, ulong> passTime, bool isInitData)
		{
			if (isInitData)
			{
				this.giftList.Clear();
				IList<IAP_PushPacks> allElements = GameApp.Table.GetManager().GetIAP_PushPacksModelInstance().GetAllElements();
				for (int i = 0; i < allElements.Count; i++)
				{
					if (allElements[i].packType == 6)
					{
						this.giftList.Add(allElements[i]);
					}
				}
				this.giftList.Sort((IAP_PushPacks a, IAP_PushPacks b) => a.group.CompareTo(b.group));
			}
			this.passTimeMap = passTime;
			this.boughtList.Clear();
			this.boughtList.AddRange(buyList);
		}

		public bool IsEnable()
		{
			return GameApp.Purchase.IsEnable && this.GetCurrentPack() != null;
		}

		public IAP_PushPacks GetCurrentPack()
		{
			for (int i = 0; i < this.giftList.Count; i++)
			{
				if (!this.HasBuy(this.giftList[i].id) && this.GetPassTime(this.giftList[i].id) - (ulong)DxxTools.Time.ServerTimestamp > 0UL)
				{
					return this.giftList[i];
				}
			}
			return null;
		}

		public bool HasBuy(int id)
		{
			for (int i = 0; i < this.boughtList.Count; i++)
			{
				if ((ulong)this.boughtList[i] == (ulong)((long)id))
				{
					return true;
				}
			}
			return false;
		}

		public ulong GetPassTime(int id)
		{
			ulong num;
			if (this.passTimeMap.TryGetValue((uint)id, ref num))
			{
				return num;
			}
			return 0UL;
		}

		private readonly PurchaseCommonData commonData;

		private List<uint> boughtList = new List<uint>();

		private MapField<uint, ulong> passTimeMap;

		private List<IAP_PushPacks> giftList = new List<IAP_PushPacks>();
	}
}
