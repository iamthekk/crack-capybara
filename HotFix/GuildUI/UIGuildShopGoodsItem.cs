using System;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using Proto.Guild;
using UnityEngine;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildShopGoodsItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.goldTextCtrl.Init();
			this.itemCtrl.Init();
			this.itemCtrl.OnClick = new Action<UIGuildItem>(this.OnClickItem);
			this.buttonBuy.onClick.AddListener(new UnityAction(this.OnClickBuy));
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonBuy != null)
			{
				this.buttonBuy.onClick.RemoveListener(new UnityAction(this.OnClickBuy));
			}
			if (this.itemCtrl != null)
			{
				this.itemCtrl.DeInit();
			}
			this.itemCtrl = null;
			if (this.goldTextCtrl != null)
			{
				this.goldTextCtrl.DeInit();
			}
			this.goldTextCtrl = null;
		}

		public void Refresh(GuildShopData data)
		{
			this.shopData = data;
			GuildItemData showItem = data.GetShowItem();
			this.textName.text = GuildProxy.Table.GetItemShowName(showItem.id);
			Guild_guildShop guildShopTable = GuildProxy.Table.GetGuildShopTable(data.ShopID);
			if (guildShopTable != null)
			{
				GuildProxy.Resources.SetDxxImage(this.imageBg, GuildProxy.Resources.GetGuildAtlasID(), this.GetGoodsBg(guildShopTable.bgColor));
			}
			float num = data.Discount * 10f;
			string discountBg = this.GetDiscountBg(num);
			if (string.IsNullOrEmpty(discountBg))
			{
				this.imageDiscount.gameObject.SetActiveSafe(false);
			}
			else
			{
				this.imageDiscount.gameObject.SetActiveSafe(true);
				GuildProxy.Resources.SetDxxImage(this.imageDiscount, GuildProxy.Resources.GetGuildAtlasID(), discountBg);
			}
			this.textDiscount.text = ((num < 10f) ? GuildProxy.Language.GetInfoByID1("400187", num) : "");
			this.goldTextCtrl.SetCurrencyType(data.Cost.id);
			this.goldTextCtrl.SetValue(data.Cost.count);
			this.itemCtrl.SetItem(showItem);
			this.itemCtrl.RefreshUI();
			this.textFreeCount.text = GuildProxy.Language.GetInfoByID("400186");
			if (this.shopData.Count == this.shopData.MaxBuyCount)
			{
				this.costObj.SetActiveSafe(false);
				this.realFreeObj.SetActiveSafe(false);
				this.adFreeObj.SetActiveSafe(false);
				this.finishObj.SetActiveSafe(true);
				this.textLimit.text = "";
				return;
			}
			this.finishObj.SetActiveSafe(false);
			this.costObj.SetActiveSafe(!this.shopData.IsRealFree && !this.shopData.IsAdFree);
			this.realFreeObj.SetActiveSafe(this.shopData.IsRealFree);
			this.adFreeObj.SetActiveSafe(this.shopData.IsAdFree);
			this.textLimit.text = GuildProxy.Language.GetInfoByID2("400215", data.MaxBuyCount - data.Count, data.MaxBuyCount);
		}

		private string GetGoodsBg(int colorId)
		{
			switch (colorId)
			{
			case 0:
				return "UICommon_CubeBG_0";
			case 1:
				return "UICommon_CubeBG_1";
			case 2:
				return "UICommon_CubeBG_2";
			case 3:
				return "UICommon_CubeBG_3";
			case 4:
				return "UICommon_CubeBG_4";
			default:
				return "UICommon_CubeBG_5";
			}
		}

		private string GetDiscountBg(float discount)
		{
			if (discount == 0f)
			{
				return "";
			}
			if (discount < 3f)
			{
				return "DailyStore_Discount_3";
			}
			if (discount < 5f)
			{
				return "DailyStore_Discount_2";
			}
			if (discount < 10f)
			{
				return "DailyStore_Discount_1";
			}
			return "";
		}

		private void OnBuy()
		{
			GuildNetUtil.Guild.DoRequest_GuildShopBuy(this.shopData.GetShopType(), this.shopData.ShopID, delegate(bool result, GuildShopBuyResponse response)
			{
				if (result && response.Code == 0 && response.CommonData.Reward != null)
				{
					GuildProxy.UI.OpenUICommonReward(response.CommonData.Reward, null);
				}
			});
		}

		private void OnClickItem(UIGuildItem item)
		{
			GuildItemData showItem = this.shopData.GetShowItem();
			if (showItem.id == 1)
			{
				GuildProxy.UI.OpenUIBuyPop(this.shopData.GetShowItem(), this.shopData.Cost, new Action(this.OnBuy));
				return;
			}
			GuildProxy.UI.OpenUIItemPop(item.gameObject, showItem);
		}

		private void OnClickBuy()
		{
			if (this.shopData == null || this.shopData.Count == this.shopData.MaxBuyCount)
			{
				return;
			}
			if (this.shopData.IsRealFree)
			{
				this.OnBuy();
				return;
			}
			if (this.shopData.IsAdFree)
			{
				this.OnBuy();
				return;
			}
			GuildProxy.UI.OpenUIBuyPop(this.shopData.GetShowItem(), this.shopData.Cost, new Action(this.OnBuy));
		}

		public CustomImage imageBg;

		public CustomText textName;

		public CustomImage imageDiscount;

		public CustomText textDiscount;

		public UIGuildGoldText goldTextCtrl;

		public UIGuildItem itemCtrl;

		public CustomButton buttonBuy;

		public CustomText textLimit;

		public GameObject costObj;

		public GameObject realFreeObj;

		public GameObject adFreeObj;

		public CustomText textFreeCount;

		public GameObject finishObj;

		private GuildShopData shopData;
	}
}
