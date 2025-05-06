using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

namespace HotFix
{
	public class MainShopGiftPackGroup : MainShopPackGroupBase
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.prefabItem.gameObject.SetActive(false);
			this.timeCtrl.OnRefreshText += this.OnRefreshTimeText;
			this.timeCtrl.OnChangeState += this.OnChangeState;
			this.timeCtrl.Init();
		}

		protected override void OnDeInit()
		{
			this.timeCtrl.DeInit();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.timeCtrl.OnUpdate(deltaTime, unscaledDeltaTime);
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].OnUpdate(deltaTime, unscaledDeltaTime);
			}
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			switch (this.packType)
			{
			case IAPTimePackData.PackType.Daily:
				priority = 8;
				break;
			case IAPTimePackData.PackType.Weekly:
				priority = 9;
				break;
			case IAPTimePackData.PackType.Month:
				priority = 10;
				break;
			default:
				priority = 0;
				break;
			}
			subPriority = 0;
		}

		public override void UpdateContent()
		{
			List<PurchaseCommonData.PurchaseData> purchaseData = this.iapDataModule.TimePackData.GetPurchaseData(this.packType);
			if (purchaseData == null || purchaseData.Count <= 0)
			{
				base.gameObject.SetActive(false);
				return;
			}
			base.gameObject.SetActive(true);
			if (purchaseData == null)
			{
				return;
			}
			if (this.items.Count < purchaseData.Count)
			{
				for (int i = this.items.Count; i < purchaseData.Count; i++)
				{
					MainShopGiftPackItem mainShopGiftPackItem = Object.Instantiate<MainShopGiftPackItem>(this.prefabItem, this.prefabItem.transform.parent, false);
					mainShopGiftPackItem.gameObject.SetActive(true);
					mainShopGiftPackItem.Init();
					this.items.Add(mainShopGiftPackItem);
				}
			}
			else if (this.items.Count > purchaseData.Count)
			{
				for (int j = this.items.Count - 1; j >= purchaseData.Count; j--)
				{
					this.items[j].gameObject.SetActive(false);
				}
			}
			for (int k = 0; k < purchaseData.Count; k++)
			{
				this.items[k].SetData(purchaseData[k]);
			}
			this.timeCtrl.Play();
		}

		private IAPShopTimeCtrl.State OnChangeState(IAPShopTimeCtrl arg)
		{
			if (this.GetRefreshTime() <= 0L)
			{
				NetworkUtils.PlayerData.TipSendUserGetInfoRequest("shop_data_refresh", null);
				return IAPShopTimeCtrl.State.Load;
			}
			return IAPShopTimeCtrl.State.Show;
		}

		private string OnRefreshTimeText(IAPShopTimeCtrl arg)
		{
			return this.GetRefreshTimeString();
		}

		private long GetRefreshTime()
		{
			return this.iapDataModule.TimePackData.GetRefreshTime(this.packType);
		}

		private string GetRefreshTimeString()
		{
			long refreshTime = this.GetRefreshTime();
			return Singleton<LanguageManager>.Instance.GetInfoByID("2923", new object[] { this.iapDataModule.TimePackData.GetFormatTimeStr(refreshTime) });
		}

		public override int PlayAnimation(float startTime, int index)
		{
			this.titleFg.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index, 10024);
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].fg.gameObject.AddComponent<EnterMoveXAnimationCtrl>().PlayShowAnimation(startTime, index + 1 + i, 10024);
			}
			return 0;
		}

		public IAPShopTimeCtrl timeCtrl;

		public MainShopGiftPackItem prefabItem;

		public IAPTimePackData.PackType packType;

		private IAPDataModule iapDataModule;

		private List<MainShopGiftPackItem> items = new List<MainShopGiftPackItem>();
	}
}
