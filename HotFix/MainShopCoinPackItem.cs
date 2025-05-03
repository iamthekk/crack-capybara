using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Shop.Arena;

namespace HotFix
{
	public class MainShopCoinPackItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.adDataModule = GameApp.Data.GetDataModule(DataName.AdDataModule);
			this.redNode.Value = 0;
			this.btnItem.m_onClick = new Action(this.OnBtnItemClick);
		}

		protected override void OnDeInit()
		{
			this.btnItem.m_onClick = null;
		}

		public void SetData(Shop_ShopSell cfg)
		{
			this.cfgCoinPack = cfg;
			this.cfgShop = GameApp.Table.GetManager().GetShop_ShopModelInstance().GetElementById(cfg.id);
			this.imgIcon.SetImage(this.cfgShop.iconAtlasId, this.cfgShop.iconName);
			this.txtName.SetText(this.cfgShop.descId);
			List<ItemData> list = this.cfgShop.products.ToItemDataList();
			ItemData itemData = ((list != null && list.Count > 0) ? list[0] : null);
			if (itemData != null)
			{
				this.txtTitle.text = DxxTools.FormatNumber(itemData.TotalCount);
			}
			else
			{
				this.txtTitle.text = "";
			}
			int watchTimes = this.adDataModule.GetWatchTimes(this.cfgShop.adId);
			int maxTimes = this.adDataModule.GetMaxTimes(this.cfgShop.adId);
			bool flag = watchTimes < maxTimes;
			bool flag2 = this.cfgShop.cost != null && this.cfgShop.cost.Length != 0;
			this.redNode.Value = (flag ? 1 : 0);
			if (flag)
			{
				this.imgAd.gameObject.SetActive(true);
				this.imgCostIcon.gameObject.SetActive(false);
				this.txtPrice.gameObject.SetActive(true);
				this.txtPrice.text = Singleton<LanguageManager>.Instance.GetInfoByID("ui_shop_free");
				this.imgCheckMark.gameObject.SetActive(false);
				return;
			}
			if (flag2)
			{
				this.imgAd.gameObject.SetActive(false);
				this.txtPrice.gameObject.SetActive(true);
				List<ItemData> list2 = this.cfgShop.cost.ToItemDataList();
				if (list2 != null && list2.Count > 0)
				{
					this.imgCostIcon.gameObject.SetActive(true);
					if (GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)list2[0].ID)) < list2[0].TotalCount)
					{
						this.txtPrice.text = string.Format("<color=#F63D39>{0}</color>", list2[0].TotalCount);
					}
					else
					{
						this.txtPrice.text = list2[0].TotalCount.ToString();
					}
				}
				else
				{
					this.imgCostIcon.gameObject.SetActive(false);
					this.txtPrice.text = "";
				}
				this.imgCheckMark.gameObject.SetActive(false);
				return;
			}
			this.imgAd.gameObject.SetActive(false);
			this.imgCostIcon.gameObject.SetActive(false);
			this.txtPrice.gameObject.SetActive(false);
			this.imgCheckMark.gameObject.SetActive(true);
		}

		private void OnBtnItemClick()
		{
			if (this.cfgCoinPack == null)
			{
				return;
			}
			int watchTimes = this.adDataModule.GetWatchTimes(this.cfgShop.adId);
			int maxTimes = this.adDataModule.GetMaxTimes(this.cfgShop.adId);
			bool flag = watchTimes < maxTimes;
			bool flag2 = this.cfgShop.cost != null && this.cfgShop.cost.Length != 0;
			if (flag)
			{
				AdBridge.PlayRewardVideo(this.cfgShop.adId, delegate(bool isSuccess)
				{
					if (isSuccess)
					{
						NetworkUtils.Shop.ShopBuyItemRequest(this.cfgCoinPack.id, 1, delegate(bool isOk, ShopBuyItemResponse resp)
						{
							if (isOk && resp != null)
							{
								GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(11204), resp.CommonData.Reward, null, null, null, null);
								GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName(11204), resp.CommonData.CostDto, null);
								GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(this.cfgShop.adId), "REWARD ", "", resp.CommonData.Reward, null);
							}
						});
					}
				});
				return;
			}
			if (flag2)
			{
				NetworkUtils.Shop.ShopBuyItemRequest(this.cfgCoinPack.id, 2, delegate(bool isOk, ShopBuyItemResponse resp)
				{
					if (isOk && resp != null)
					{
						GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(11204), resp.CommonData.Reward, null, null, null, null);
						GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName(11204), resp.CommonData.CostDto, null);
						GameApp.SDK.Analyze.Track_Shop(ShopType.BlackMarket, resp.CommonData.CostDto, resp.CommonData.Reward);
					}
				});
				return;
			}
			string infoByID = Singleton<LanguageManager>.Instance.GetInfoByID("ui_shop_buy_max");
			GameApp.View.ShowStringTip(infoByID);
		}

		public CustomButton btnItem;

		public CustomText txtTitle;

		public CustomImage imgIcon;

		public CustomText txtName;

		public CustomText txtPrice;

		public CustomImage imgAd;

		public CustomImage imgCostIcon;

		public CustomImage imgCheckMark;

		public RedNodeOneCtrl redNode;

		private IAPDataModule iapDataModule;

		private IAP_Purchase iapPurchaseTable;

		private AdDataModule adDataModule;

		private Shop_ShopSell cfgCoinPack;

		private Shop_Shop cfgShop;
	}
}
