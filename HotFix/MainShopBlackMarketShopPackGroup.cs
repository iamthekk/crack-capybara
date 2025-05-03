using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class MainShopBlackMarketShopPackGroup : MainShopPackGroupBase
	{
		protected override void OnInit()
		{
			this.shopDataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
			this.prefabItem.gameObject.SetActive(false);
			this.refreshShopTimer = new Timer();
		}

		protected override void OnDeInit()
		{
			this.refreshShopTimer.OnDeInit();
			this.refreshShopTimer = null;
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			base.OnUpdate(deltaTime, unscaledDeltaTime);
			this.refreshShopTimer.OnUpdate(deltaTime, unscaledDeltaTime);
		}

		public override void GetPriority(out int priority, out int subPriority)
		{
			priority = 6;
			subPriority = 0;
		}

		public override void UpdateContent()
		{
			this.UpdateAllInfo();
			this.UpdateItems();
			this.refreshShopTimer.OnInit();
			this.refreshShopTimer.m_onFinished = new Action<Timer>(this.UpdateRefreshTime);
			this.UpdateRefreshTime(this.refreshShopTimer);
			GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(this.shopConfig.currencyID);
		}

		private void UpdateRefreshTime(Timer timer)
		{
			long num;
			this.shopDataModule.GetRefreshCountDownTime(ShopType.BlackMarket, GoodsRefreshType.Day, out num);
			string time = Singleton<LanguageManager>.Instance.GetTime(num);
			this.txtCountDown.text = time;
			this.refreshShopTimer.Play(1f);
		}

		private void UpdateAllInfo()
		{
			this.shopDataModule.GetShopConfig(ShopType.BlackMarket, out this.shopConfig);
			this.shopItemConfigList = this.shopDataModule.GetShopItemsConfig(ShopType.BlackMarket, GoodsRefreshType.None);
		}

		public void UpdateItems()
		{
			if (this.items.Count < this.shopItemConfigList.Count)
			{
				for (int i = this.items.Count; i < this.shopItemConfigList.Count; i++)
				{
					MainShopBlackMarketItem mainShopBlackMarketItem = Object.Instantiate<MainShopBlackMarketItem>(this.prefabItem, this.prefabItem.transform.parent, false);
					mainShopBlackMarketItem.gameObject.SetActive(true);
					mainShopBlackMarketItem.Init();
					this.items.Add(mainShopBlackMarketItem);
				}
			}
			else if (this.items.Count > this.shopItemConfigList.Count)
			{
				for (int j = this.items.Count - 1; j >= this.shopItemConfigList.Count; j--)
				{
					this.items[j].gameObject.SetActive(false);
				}
			}
			for (int k = 0; k < this.shopItemConfigList.Count; k++)
			{
				this.items[k].SetData(this.shopItemConfigList[k]);
			}
		}

		public override int PlayAnimation(float startTime, int index)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].gameObject.AddComponent<EnterScaleAnimationCtrl>().PlayShowAnimation(startTime, index + i, 100025);
			}
			return 0;
		}

		public CustomText txtCountDown;

		public MainShopBlackMarketItem prefabItem;

		private List<MainShopBlackMarketItem> items = new List<MainShopBlackMarketItem>();

		private ShopDataModule shopDataModule;

		private IntegralShop_data shopConfig;

		private List<IntegralShop_goods> shopItemConfigList = new List<IntegralShop_goods>();

		private Timer refreshShopTimer;
	}
}
