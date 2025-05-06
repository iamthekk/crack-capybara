using System;
using System.Collections.Generic;
using System.Linq;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;

namespace HotFix
{
	public class MainShopGuildShopPackGroup : MainShopPackGroupBase
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
			priority = 11;
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
			base.gameObject.SetActive(this.shopItemConfigList.Count > 0);
		}

		private void UpdateRefreshTime(Timer timer)
		{
			long num;
			this.shopDataModule.GetRefreshCountDownTime(ShopType.Guild, this.guildGoodsType, out num);
			string time = Singleton<LanguageManager>.Instance.GetTime(num);
			this.txtCountDown.text = time;
			this.refreshShopTimer.Play(1f);
		}

		private void UpdateAllInfo()
		{
			this.shopDataModule.GetShopConfig(ShopType.Guild, out this.shopConfig);
			IOrderedEnumerable<IntegralShop_goods> orderedEnumerable = from x in this.shopDataModule.GetShopItemsConfig(ShopType.Guild, this.guildGoodsType)
				orderby x.LevelRequirements
				select x;
			this.shopItemConfigList = new List<IntegralShop_goods>();
			if (GuildSDKManager.Instance.GuildInfo.GuildData == null)
			{
				return;
			}
			foreach (IntegralShop_goods integralShop_goods in orderedEnumerable)
			{
				this.shopItemConfigList.Add(integralShop_goods);
			}
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
			if (GuildSDKManager.Instance.GuildInfo.GuildData == null)
			{
				return;
			}
			int guildLevel = GuildSDKManager.Instance.GuildInfo.GuildData.GuildLevel;
			for (int k = 0; k < this.shopItemConfigList.Count; k++)
			{
				IntegralShop_goods integralShop_goods = this.shopItemConfigList[k];
				this.items[k].SetData(this.shopItemConfigList[k]);
				if (integralShop_goods.LevelRequirements > guildLevel)
				{
					this.items[k].SetMask(true);
				}
				else
				{
					this.items[k].SetMask(false);
				}
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

		public GoodsRefreshType guildGoodsType;

		private List<MainShopBlackMarketItem> items = new List<MainShopBlackMarketItem>();

		private ShopDataModule shopDataModule;

		private IntegralShop_data shopConfig;

		private List<IntegralShop_goods> shopItemConfigList = new List<IntegralShop_goods>();

		private Timer refreshShopTimer;
	}
}
