using System;
using System.Collections.Generic;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Server;
using Shop.Arena;
using UnityEngine;

namespace HotFix
{
	public class EquipSUpActivityPackItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.btnDrawOne.m_onClick = new Action(this.OnClickDrawOne);
			this.btnDrawTen.m_onClick = new Action(this.OnClickDrawTen);
			this.redNode1.Value = 0;
			this.redNode10.Value = 0;
		}

		protected override void OnDeInit()
		{
			this.btnDrawOne.m_onClick = null;
			this.btnDrawTen.m_onClick = null;
			this.iapDataModule = null;
			this.propDataModule = null;
		}

		public void SetData(object obj)
		{
			this.iapShopActivityData = this.iapDataModule.GetShopSUpActivityData();
			if (this.iapShopActivityData == null)
			{
				return;
			}
			this.cfgEquipActivity = GameApp.Table.GetManager().GetShop_EquipActivityModelInstance().GetElementById(this.iapShopActivityData.linkId);
			if (this.cfgEquipActivity == null)
			{
				HLog.LogError("Shop_EquipActivities表中找不到id为" + this.iapShopActivityData.activityId.ToString() + "的数据");
				return;
			}
			this.jsonShopSummon = JsonManager.ToObject<JsonShopSummon>(this.iapShopActivityData.shopActDetailDto.SummonJson);
			this.txtTitle.SetText(this.cfgEquipActivity.nameId);
			this.txtDesc.SetText(Singleton<LanguageManager>.Instance.GetInfoByID(this.cfgEquipActivity.descId), true);
			if (this.jsonShopSummon.miniPityCount > 0)
			{
				int shopDrawMiniCount = this.iapDataModule.GetShopDrawMiniCount(this.jsonShopSummon.groupId);
				int num = Mathf.Max(0, this.jsonShopSummon.miniPityCount - shopDrawMiniCount);
				this.txtTip2.SetText(this.cfgEquipActivity.miniPityDesc, string.Format("{0}", num));
			}
			else
			{
				this.txtTip2.text = "";
			}
			if (this.jsonShopSummon.hardPityCount > 0)
			{
				int shopDrawHardCount = this.iapDataModule.GetShopDrawHardCount(this.jsonShopSummon.groupId);
				int num2 = Mathf.Max(0, this.jsonShopSummon.hardPityCount - shopDrawHardCount);
				this.txtTip1.SetText(this.cfgEquipActivity.hardPityDesc, string.Format("{0}", num2));
			}
			else
			{
				this.txtTip1.text = "";
			}
			this.imgIcon.SetImage(this.cfgEquipActivity.atlasId, this.cfgEquipActivity.iconName);
			ItemData itemData = new ItemData();
			itemData.SetID(this.jsonShopSummon.priceId);
			itemData.SetCount((long)this.jsonShopSummon.singlePrice);
			long itemDataCountByid = this.propDataModule.GetItemDataCountByid((ulong)((long)this.jsonShopSummon.priceId));
			if (this.jsonShopSummon.singlePriceOrigin > 0 && itemDataCountByid < (long)this.jsonShopSummon.singlePrice)
			{
				itemData.SetID(2);
				itemData.SetCount((long)this.jsonShopSummon.singlePriceOrigin);
				this.costItemOne.SetData(itemData);
				this.redNode1.Value = 0;
			}
			else
			{
				this.costItemOne.SetData(itemData, itemDataCountByid, itemData.TotalCount);
				this.redNode1.Value = ((itemDataCountByid >= itemData.TotalCount) ? 1 : 0);
			}
			ItemData itemData2 = new ItemData();
			itemData2.SetID(this.jsonShopSummon.priceId);
			itemData2.SetCount((long)this.jsonShopSummon.tenPrice);
			if (this.jsonShopSummon.singlePriceOrigin > 0 && itemDataCountByid < (long)this.jsonShopSummon.tenPrice)
			{
				itemData2.SetID(2);
				itemData2.SetCount((long)this.jsonShopSummon.tenPriceOrigin);
				this.costItemTen.SetData(itemData2);
				this.redNode10.Value = 0;
				return;
			}
			this.costItemTen.SetData(itemData2, itemDataCountByid, itemData2.TotalCount);
			this.redNode10.Value = ((itemDataCountByid >= itemData2.TotalCount) ? 1 : 0);
		}

		private void OnClickDrawOne()
		{
			int num = this.jsonShopSummon.priceId;
			int num2 = this.jsonShopSummon.singlePrice;
			if (this.propDataModule.GetItemDataCountByid((ulong)((long)num)) < (long)num2)
			{
				if (this.jsonShopSummon.singlePriceOrigin <= 0)
				{
					GameApp.View.ShowItemNotEnoughTip(num, true);
					return;
				}
				num = 2;
				num2 = this.jsonShopSummon.singlePriceOrigin;
				if (this.propDataModule.GetItemDataCountByid((ulong)((long)num)) < (long)num2)
				{
					GameApp.View.ShowItemNotEnoughTip(num, true);
					return;
				}
				this.drawCostId = num;
				this.drawCostCount = num2;
			}
			else
			{
				this.drawCostId = num;
				this.drawCostCount = num2;
			}
			this.SendRequest(this.jsonShopSummon.id, 2, 1);
		}

		private void OnClickDrawTen()
		{
			int num = this.jsonShopSummon.priceId;
			int num2 = this.jsonShopSummon.tenPrice;
			if (this.propDataModule.GetItemDataCountByid((ulong)((long)num)) < (long)num2)
			{
				if (this.jsonShopSummon.tenPriceOrigin <= 0)
				{
					GameApp.View.ShowItemNotEnoughTip(num, true);
					return;
				}
				num = 2;
				num2 = this.jsonShopSummon.tenPriceOrigin;
				if (this.propDataModule.GetItemDataCountByid((ulong)((long)num)) < (long)num2)
				{
					GameApp.View.ShowItemNotEnoughTip(num, true);
					return;
				}
				this.drawCostId = num;
				this.drawCostCount = num2;
			}
			else
			{
				this.drawCostId = num;
				this.drawCostCount = num2;
			}
			this.SendRequest(this.jsonShopSummon.id, 2, 2);
		}

		protected virtual void SendRequest(int summonId, int costType, int drawType)
		{
			this.drawCostType = costType;
			NetworkUtils.Shop.ShopDoDrawRequest(summonId, costType, drawType, new Action<bool, ShopDoDrawResponse>(this.SendMessageCallback));
		}

		protected virtual void SendMessageCallback(bool isOk, ShopDoDrawResponse resp)
		{
			if (isOk)
			{
				int count = resp.CommonData.Reward.Count;
				OpenEquipBoxViewModule.OpenData openData = new OpenEquipBoxViewModule.OpenData();
				openData.boxId = this.jsonShopSummon.boxId;
				openData.itemDatas = resp.CommonData.Reward.ToItemDataList();
				openData.ChestType = eEquipChestType.DiamondChest;
				openData.CostType = 2;
				openData.shopSummonId = this.jsonShopSummon.id;
				openData.iapMainActivityType = 2;
				GameApp.View.OpenView(ViewName.OpenEquipBoxViewModule, openData, 1, null, delegate
				{
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
				});
				this.TrackLog(resp);
			}
		}

		private void TrackLog(ShopDoDrawResponse resp)
		{
			if (this.jsonShopSummon == null)
			{
				return;
			}
			int num = 1120899;
			GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(num), resp.CommonData.Reward, null, null, null, null);
			GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName(num), resp.CommonData.CostDto, null);
			if (this.drawCostType == 1)
			{
				GameApp.SDK.Analyze.Track_AD(GameTGATools.ADSourceName(this.jsonShopSummon.adId), "REWARD ", "", resp.CommonData.Reward, null);
			}
			List<TGACommonItemInfo> list = new List<TGACommonItemInfo>();
			if (resp.CommonData != null && resp.CommonData.Reward.Count > 0)
			{
				IAPShopActivityData shopSUpActivityData = this.iapDataModule.GetShopSUpActivityData();
				if (shopSUpActivityData != null && shopSUpActivityData.shopActDetailDto != null && shopSUpActivityData.shopActDetailDto.SummonPoolItems != null)
				{
					for (int i = 0; i < shopSUpActivityData.shopActDetailDto.SummonPoolItems.Count; i++)
					{
						list.Add(new TGACommonItemInfo(shopSUpActivityData.shopActDetailDto.SummonPoolItems[i], 1L));
					}
				}
				string text;
				if (resp.CommonData.Reward.Count > 1)
				{
					text = "up宝箱十连";
				}
				else
				{
					text = "up宝箱单抽";
				}
				GameTGACostCurrency gameTGACostCurrency;
				if (this.drawCostType == 1)
				{
					gameTGACostCurrency = GameTGACostCurrency.Ad;
				}
				else if (this.drawCostId == 0)
				{
					gameTGACostCurrency = GameTGACostCurrency.Free;
				}
				else if (this.drawCostId == 2)
				{
					gameTGACostCurrency = GameTGACostCurrency.Gem;
				}
				else
				{
					gameTGACostCurrency = GameTGACostCurrency.ChestKey;
				}
				GameApp.SDK.Analyze.Track_EquipmentBoxOpen(text, gameTGACostCurrency, this.drawCostCount, resp.CommonData.Reward, list);
			}
		}

		public RectTransform fg;

		public CustomButton btnDrawOne;

		public CustomButton btnDrawTen;

		public CommonCostItem costItemOne;

		public CommonCostItem costItemTen;

		public CustomText txtTitle;

		public CustomTextScrollView txtDesc;

		public CustomText txtTip1;

		public CustomText txtTip2;

		public CustomImage imgIcon;

		public RedNodeOneCtrl redNode1;

		public RedNodeOneCtrl redNode10;

		[NonSerialized]
		public IAPDataModule iapDataModule;

		[NonSerialized]
		public PropDataModule propDataModule;

		[NonSerialized]
		public JsonShopSummon jsonShopSummon;

		[NonSerialized]
		public Shop_EquipActivity cfgEquipActivity;

		[NonSerialized]
		public IAPShopActivityData iapShopActivityData;

		private int drawCostType;

		private int drawCostId;

		private int drawCostCount;
	}
}
