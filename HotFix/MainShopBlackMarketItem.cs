using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.IntegralShop;
using UnityEngine;

namespace HotFix
{
	public class MainShopBlackMarketItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.shopDataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
			this.propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.redNode.Value = 0;
			this.imgAd.gameObject.SetActive(false);
			this.uiItem.SetCountShowType(UIItem.CountShowType.ShowAll);
			this.uiItem.Init();
			this.btnItem.m_onClick = new Action(this.OnBtnBuyClick);
		}

		protected override void OnDeInit()
		{
			this.uiItem.DeInit();
			this.btnItem.m_onClick = null;
		}

		public void SetMask(bool isMask)
		{
			this._isMask = isMask;
			this.Mask.SetActiveSafe(isMask);
			this.MaskText.text = Singleton<LanguageManager>.Instance.GetInfoByID(this._maskText, new object[] { this.model.LevelRequirements });
		}

		public void SetData(IntegralShop_goods itemModel)
		{
			this.model = itemModel;
			if (itemModel == null)
			{
				return;
			}
			int num;
			int num2;
			this.shopDataModule.GetShopItemData(this.model, out num, out num2);
			ItemData itemData = new ItemData(num, (long)num2);
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(num);
			this.txtTitle.text = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			int boughtTimes = this.shopDataModule.GetBoughtTimes(itemModel);
			int num3 = Mathf.Max(itemModel.BuyTimes - boughtTimes, 0);
			this.txtBuyTimes.text = Singleton<LanguageManager>.Instance.GetInfoByID("blackmarket_buy_times", new object[] { num3, itemModel.BuyTimes });
			if (num3 <= 0)
			{
				this.goCheckMark.SetActive(true);
				this.txtPrice.transform.parent.gameObject.SetActive(false);
			}
			else
			{
				this.goCheckMark.SetActive(false);
				this.txtPrice.transform.parent.gameObject.SetActive(true);
				Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(itemModel.currencyID);
				if (item_Item != null)
				{
					this.imgCostIcon.SetImage(item_Item.atlasID, item_Item.icon);
				}
				int num4;
				int num5;
				this.shopDataModule.GetPrice(itemModel, out num4, out num5);
				num4.ToString();
				string text = DxxTools.FormatNumber((long)num5);
				if (!this.shopDataModule.IsEnoughCurrency(itemModel))
				{
					text = "<color=#F63D39>" + text + "</color>";
				}
				this.txtPrice.text = text ?? "";
			}
			int num6;
			bool offInfo = this.shopDataModule.GetOffInfo(itemModel, out num6);
			this.goOff.SetActive(offInfo);
			if (offInfo)
			{
				this.txtOffValue.text = string.Format("{0}%", num6);
			}
			this.uiItem.SetData(itemData.ToPropData());
			this.uiItem.OnRefresh();
		}

		private void OnBtnBuyClick()
		{
			if (this.model == null)
			{
				return;
			}
			if (this._isMask)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID(this._maskText, new object[] { this.model.LevelRequirements }));
				return;
			}
			int boughtTimes = this.shopDataModule.GetBoughtTimes(this.model);
			if (Mathf.Max(this.model.BuyTimes - boughtTimes, 0) <= 0)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID_LogError(2954));
				return;
			}
			IntegralShop_data integralShop_data;
			this.shopDataModule.GetShopConfig((ShopType)this.model.TypeId, out integralShop_data);
			int num;
			int num2;
			this.shopDataModule.GetPrice(this.model, out num, out num2);
			if (this.propDataModule.GetItemDataCountByid((ulong)((long)this.model.currencyID)) < (long)num2)
			{
				GameApp.View.ShowItemNotEnoughTip(this.model.currencyID, true);
				return;
			}
			NetworkUtils.Shop.IntegralShopBuyRequest((ShopType)this.model.TypeId, this.model.ID, integralShop_data.currencyID, num2, delegate(bool isOk, IntegralShopBuyResponse response)
			{
				if (isOk)
				{
					DxxTools.UI.OpenRewardCommon(response.CommonData.Reward, delegate
					{
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_IAPData_RefreshIAPInfoData, null);
					}, true);
					if (this.model.TypeId == 2)
					{
						GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(12902002), response.CommonData.Reward, null, null, null, null);
						GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName(12902002), response.CommonData.CostDto, null);
					}
					else if (this.model.TypeId == 1)
					{
						GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(12902001), response.CommonData.Reward, null, null, null, null);
						GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName(12902001), response.CommonData.CostDto, null);
					}
					else if (this.model.TypeId == 4)
					{
						GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(12902003), response.CommonData.Reward, null, null, null, null);
						GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName(12902003), response.CommonData.CostDto, null);
					}
					GameApp.SDK.Analyze.Track_Shop((ShopType)this.model.TypeId, response.CommonData.CostDto, response.CommonData.Reward);
				}
			});
		}

		public CustomButton btnItem;

		public CustomText txtTitle;

		public UIItem uiItem;

		public GameObject goOff;

		public CustomText txtOffValue;

		public CustomText txtBuyTimes;

		public GameObject goCheckMark;

		public CustomText txtPrice;

		public CustomImage imgAd;

		public CustomImage imgCostIcon;

		public RedNodeOneCtrl redNode;

		public GameObject Mask;

		public CustomText MaskText;

		private IntegralShop_goods model;

		private ShopDataModule shopDataModule;

		private PropDataModule propDataModule;

		private bool _isMask;

		private readonly string _maskText = "magic_shop_unlock_level";
	}
}
