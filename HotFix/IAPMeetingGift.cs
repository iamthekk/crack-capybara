using System;
using System.Collections.Generic;
using Framework;
using Google.Protobuf.Collections;
using LocalModels.Bean;

namespace HotFix
{
	public class IAPMeetingGift
	{
		public long EndTime { get; private set; }

		public IAPMeetingGift(PurchaseCommonData commonDataVal)
		{
			this.commonData = commonDataVal;
		}

		public void SetData(MapField<int, ulong> buyList, MapField<int, uint> rewardSignList, long endTime, bool isInitData)
		{
			if (isInitData)
			{
				this.giftList.Clear();
				IList<IAP_PushPacks> allElements = GameApp.Table.GetManager().GetIAP_PushPacksModelInstance().GetAllElements();
				for (int i = 0; i < allElements.Count; i++)
				{
					if (allElements[i].packType == 7)
					{
						this.giftList.Add(allElements[i]);
					}
				}
				this.giftList.Sort((IAP_PushPacks a, IAP_PushPacks b) => a.id.CompareTo(b.id));
			}
			this.boughtMap = buyList;
			this.rewardSignMap = rewardSignList;
			this.EndTime = endTime;
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			if (this.EndTime > 0L && this.EndTime > serverTimestamp)
			{
				string text = "MeetingGift";
				DxxTools.UI.RemoveServerTimeClockCallback(text);
				DxxTools.UI.AddServerTimeCallback(text, new Action(this.OnServerClockTimeOut), this.EndTime, 0);
			}
		}

		private void OnServerClockTimeOut()
		{
			this.EndTime = -1L;
			if (GameApp.View.IsOpened(ViewName.MeetingGiftViewModule))
			{
				GameApp.View.CloseView(ViewName.MeetingGiftViewModule, null);
			}
			GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshLimitTimeLeftButton, null);
		}

		public void UpdateRewardSign(MapField<int, uint> rewardSignList)
		{
			this.rewardSignMap = rewardSignList;
			RedPointController.Instance.ReCalc("IAPGift.Meeting", true);
			this.IsUnlock = false;
		}

		public List<IAP_PushPacks> GetGifts()
		{
			return this.giftList;
		}

		public bool IsBoughtAny()
		{
			return this.boughtMap.Count > 0;
		}

		public bool IsBought(int id)
		{
			return this.boughtMap.ContainsKey(id);
		}

		public bool IsCanGet(int id)
		{
			for (int i = 0; i < 3; i++)
			{
				int num = i + 1;
				if (this.IsCanGet(id, num))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsCanGet(int id, int day)
		{
			ulong num;
			return !this.IsGet(id, day) && this.boughtMap.TryGetValue(id, ref num) && DxxTools.Time.ServerTimestamp - (long)num >= (long)(86400 * (day - 1));
		}

		public bool IsGet(int id, int day)
		{
			int num = day - 1;
			uint num2;
			return this.rewardSignMap.TryGetValue(id, ref num2) && ((num2 >> num) & 1U) > 0U;
		}

		public bool IsAllGet()
		{
			if (this.GetUnBuyGift() != null)
			{
				return false;
			}
			for (int i = 0; i < this.giftList.Count; i++)
			{
				int id = this.giftList[i].id;
				for (int j = 0; j < 3; j++)
				{
					int num = j + 1;
					if (!this.IsGet(id, num))
					{
						return false;
					}
				}
			}
			return true;
		}

		public bool IsAllEnd()
		{
			return this.IsAllGet() || this.EndTime == -1L;
		}

		public bool IsNoEndTime()
		{
			return this.boughtMap.Count > 0;
		}

		public IAP_PushPacks GetUnBuyGift()
		{
			for (int i = 0; i < this.giftList.Count; i++)
			{
				int id = this.giftList[i].id;
				if (!this.IsBought(id))
				{
					return this.giftList[i];
				}
			}
			return null;
		}

		public long GetLastTime()
		{
			if (this.IsNoEndTime() || this.EndTime <= 0L)
			{
				return -1L;
			}
			return this.EndTime - DxxTools.Time.ServerTimestamp;
		}

		public bool IsClaimPreviousDay(int id, int day)
		{
			return this.IsGet(id, day - 1);
		}

		public long GetClaimTime(int id, int day)
		{
			if (this.IsGet(id, day))
			{
				return 0L;
			}
			if (this.IsCanGet(id, day))
			{
				return 0L;
			}
			ulong num;
			if (this.boughtMap.TryGetValue(id, ref num))
			{
				int num2 = day - 1;
				return (long)(num + (ulong)((long)(num2 * 24 * 3600)) - (ulong)DxxTools.Time.ServerTimestamp);
			}
			return 0L;
		}

		public bool IsRedPoint()
		{
			foreach (int num in this.boughtMap.Keys)
			{
				for (int i = 0; i < 3; i++)
				{
					int num2 = i + 1;
					if (!this.IsGet(num, num2) && DxxTools.Time.ServerTimestamp - (long)this.boughtMap[num] >= (long)(86400 * i))
					{
						return true;
					}
				}
			}
			return false;
		}

		public void SetUnlock()
		{
			this.IsUnlock = true;
			this.EndTime = 0L;
			NetworkUtils.PlayerData.SendUserGetIapInfoRequest(null);
		}

		private readonly PurchaseCommonData commonData;

		private List<IAP_PushPacks> giftList = new List<IAP_PushPacks>();

		private MapField<int, ulong> boughtMap = new MapField<int, ulong>();

		private MapField<int, uint> rewardSignMap;

		private const int MAX_GET_DAY = 3;

		public bool IsUnlock;
	}
}
