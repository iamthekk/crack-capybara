using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Pay;
using UnityEngine;

namespace HotFix
{
	public class UIMeetingDayItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.rewardItem.gameObject.SetActive(false);
		}

		protected override void OnDeInit()
		{
			for (int i = 0; i < this.rewardItems.Count; i++)
			{
				this.rewardItems[i].DeInit();
			}
			this.rewardItems.Clear();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			if (this.giftPack == null)
			{
				return;
			}
			long claimTime = this.iapDataModule.MeetingGift.GetClaimTime(this.giftPack.id, this.mDay);
			if (claimTime > 0L)
			{
				this.textTime.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimeeting_time", new object[] { Singleton<LanguageManager>.Instance.GetTime(claimTime) });
				return;
			}
			this.textTime.text = "";
		}

		public void SetData(IAP_PushPacks gift, int day)
		{
			if (gift == null)
			{
				return;
			}
			this.giftPack = gift;
			this.mDay = day;
			this.isBought = this.iapDataModule.MeetingGift.IsBought(gift.id);
			this.isGet = this.iapDataModule.MeetingGift.IsGet(gift.id, day);
			this.textDay.text = Singleton<LanguageManager>.Instance.GetInfoByID("uimeeting_day", new object[] { day });
			List<ItemData> rewardItemData = gift.GetRewardItemData(day);
			for (int i = 0; i < this.rewardItems.Count; i++)
			{
				this.rewardItems[i].gameObject.SetActiveSafe(false);
			}
			for (int j = 0; j < rewardItemData.Count; j++)
			{
				UIMeetingRewardItem uimeetingRewardItem;
				if (j < this.rewardItems.Count)
				{
					uimeetingRewardItem = this.rewardItems[j];
				}
				else
				{
					GameObject gameObject = Object.Instantiate<GameObject>(this.rewardItem);
					gameObject.SetParentNormal(this.parent, false);
					uimeetingRewardItem = gameObject.GetComponent<UIMeetingRewardItem>();
					uimeetingRewardItem.Init();
					this.rewardItems.Add(uimeetingRewardItem);
				}
				uimeetingRewardItem.SetData(rewardItemData[j].ToPropData(), gift, day, new Action(this.OnClickItem));
				uimeetingRewardItem.gameObject.SetActiveSafe(true);
			}
		}

		private void OnClickItem()
		{
			if (this.isBought && !this.isGet)
			{
				NetworkUtils.Purchase.DoFirstRechargeRewardV1Request(this.giftPack.id, this.mDay, delegate(bool result, FirstRechargeRewardV1Response resp)
				{
					if (result)
					{
						if (resp.CommonData.Reward != null && resp.CommonData.Reward.Count > 0)
						{
							DxxTools.UI.OpenRewardCommon(resp.CommonData.Reward, null, true);
						}
						GameApp.Event.DispatchNow(this, LocalMessageName.CC_UI_MeetingGift_RefreshUI, null);
					}
				});
			}
		}

		public CustomText textDay;

		public CustomText textTime;

		public Transform parent;

		public GameObject rewardItem;

		private List<UIMeetingRewardItem> rewardItems = new List<UIMeetingRewardItem>();

		private IAP_PushPacks giftPack;

		private int mDay;

		private bool isBought;

		private bool isGet;

		private IAPDataModule iapDataModule;
	}
}
