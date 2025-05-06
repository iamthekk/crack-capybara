using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;
using Shop.Arena;
using UnityEngine;

namespace HotFix
{
	public class MainShopEquipChestDiamond : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this.shopDataModule = GameApp.Data.GetDataModule(DataName.ShopDataModule);
			this.propDataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			this.btnDrawOne.m_onClick = new Action(this.OnClickDrawOne);
			this.btnDrawTen.m_onClick = new Action(this.OnClickDrawTen);
			this.redNode1.Value = 0;
			this.redNode10.Value = 0;
			this.btnProbability.m_onClick = new Action(this.OnClickProbability);
		}

		protected override void OnDeInit()
		{
			this.btnDrawOne.m_onClick = null;
			this.btnDrawTen.m_onClick = null;
			this.btnProbability.m_onClick = null;
		}

		public void SetData()
		{
			this.cfgEquipActivity = this.shopDataModule.GetEquipChestTableByType(eEquipChestType.DiamondChest);
			this.cfgShopSummon = GameApp.Table.GetManager().GetShop_SummonModelInstance().GetElementById(this.cfgEquipActivity.id);
			this.txtTitle.SetText(this.cfgEquipActivity.nameId);
			this.txtDesc.SetText(Singleton<LanguageManager>.Instance.GetInfoByID(this.cfgEquipActivity.descId), true);
			if (this.cfgShopSummon.miniPityCount > 0)
			{
				int shopDrawMiniCount = this.iapDataModule.GetShopDrawMiniCount(this.cfgShopSummon.id);
				int num = Mathf.Max(0, this.cfgShopSummon.miniPityCount - shopDrawMiniCount);
				this.txtTip2.SetText(this.cfgEquipActivity.miniPityDesc, string.Format("{0}", num));
			}
			else
			{
				this.txtTip2.text = "";
			}
			if (this.cfgShopSummon.hardPityCount > 0)
			{
				int shopDrawHardCount = this.iapDataModule.GetShopDrawHardCount(this.cfgShopSummon.id);
				int num2 = Mathf.Max(0, this.cfgShopSummon.hardPityCount - shopDrawHardCount);
				this.txtTip1.SetText(this.cfgEquipActivity.hardPityDesc, string.Format("{0}", num2));
			}
			else
			{
				this.txtTip1.text = "";
			}
			this.imgIcon.SetImage(this.cfgEquipActivity.atlasId, this.cfgEquipActivity.iconName);
			ItemData itemData = new ItemData();
			itemData.SetID(this.cfgShopSummon.priceId);
			itemData.SetCount((long)this.cfgShopSummon.singlePrice);
			long itemDataCountByid = this.propDataModule.GetItemDataCountByid((ulong)((long)this.cfgShopSummon.priceId));
			if (this.cfgShopSummon.singlePriceOrigin > 0 && itemDataCountByid < (long)this.cfgShopSummon.singlePrice)
			{
				itemData.SetID(2);
				itemData.SetCount((long)this.cfgShopSummon.singlePriceOrigin);
				this.costItemOne.SetData(itemData);
				this.redNode1.Value = 0;
			}
			else
			{
				this.costItemOne.SetData(itemData, itemDataCountByid, itemData.TotalCount);
				this.redNode1.Value = ((itemDataCountByid >= itemData.TotalCount) ? 1 : 0);
			}
			ItemData itemData2 = new ItemData();
			itemData2.SetID(this.cfgShopSummon.priceId);
			itemData2.SetCount((long)this.cfgShopSummon.tenPrice);
			if (this.cfgShopSummon.tenPriceOrigin > 0 && itemDataCountByid < (long)this.cfgShopSummon.tenPrice)
			{
				itemData2.SetID(2);
				itemData2.SetCount((long)this.cfgShopSummon.tenPriceOrigin);
				this.costItemTen.SetData(itemData2);
				this.redNode10.Value = 0;
				return;
			}
			this.costItemTen.SetData(itemData2, itemDataCountByid, itemData2.TotalCount);
			this.redNode10.Value = ((itemDataCountByid >= itemData2.TotalCount) ? 1 : 0);
		}

		private void OnClickDrawOne()
		{
			int num = this.cfgShopSummon.priceId;
			int num2 = this.cfgShopSummon.singlePrice;
			if (this.propDataModule.GetItemDataCountByid((ulong)((long)num)) < (long)num2)
			{
				if (this.cfgShopSummon.singlePriceOrigin <= 0)
				{
					GameApp.View.ShowItemNotEnoughTip(num, true);
					return;
				}
				num = 2;
				num2 = this.cfgShopSummon.singlePriceOrigin;
				if (this.propDataModule.GetItemDataCountByid((ulong)((long)num)) < (long)num2)
				{
					GameApp.View.ShowItemNotEnoughTip(num, true);
					return;
				}
			}
			this.drawCostId = num;
			this.drawCostCount = num2;
			this.drawType = 1;
			this.SendRequest(this.cfgShopSummon.id, 2, 1);
		}

		private void OnClickDrawTen()
		{
			int num = this.cfgShopSummon.priceId;
			int num2 = this.cfgShopSummon.tenPrice;
			if (this.propDataModule.GetItemDataCountByid((ulong)((long)num)) < (long)num2)
			{
				if (this.cfgShopSummon.tenPriceOrigin <= 0)
				{
					GameApp.View.ShowItemNotEnoughTip(num, true);
					return;
				}
				num = 2;
				num2 = this.cfgShopSummon.tenPriceOrigin;
				if (this.propDataModule.GetItemDataCountByid((ulong)((long)num)) < (long)num2)
				{
					GameApp.View.ShowItemNotEnoughTip(num, true);
					return;
				}
			}
			this.drawCostId = num;
			this.drawCostCount = num2;
			this.drawType = 2;
			this.SendRequest(this.cfgShopSummon.id, 2, 2);
		}

		private void OnClickProbability()
		{
			GameApp.View.OpenView(ViewName.EquipShopProbabilityViewModule, this.cfgShopSummon.id, 1, null, null);
		}

		protected virtual void SendRequest(int summonId, int costType, int drawType)
		{
			NetworkUtils.Shop.ShopDoDrawRequest(summonId, costType, drawType, new Action<bool, ShopDoDrawResponse>(this.SendMessageCallback));
		}

		protected virtual void SendMessageCallback(bool isOk, ShopDoDrawResponse resp)
		{
			if (isOk)
			{
				int count = resp.CommonData.Reward.Count;
				OpenEquipBoxViewModule.OpenData openData = new OpenEquipBoxViewModule.OpenData();
				openData.boxId = this.cfgShopSummon.boxId;
				openData.itemDatas = resp.CommonData.Reward.ToItemDataList();
				openData.ChestType = eEquipChestType.DiamondChest;
				openData.CostType = 2;
				openData.shopSummonId = this.cfgShopSummon.id;
				GameApp.View.OpenView(ViewName.OpenEquipBoxViewModule, openData, 1, null, delegate
				{
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
				});
				int num = 1120800 + this.cfgShopSummon.boxId;
				GameApp.SDK.Analyze.Track_Get(GameTGATools.GetSourceName(num), resp.CommonData.Reward, null, null, null, null);
				GameApp.SDK.Analyze.Track_Cost(GameTGATools.CostSourceName(num), resp.CommonData.CostDto, null);
				if (this.cfgShopSummon.boxId == 4)
				{
					string text = ((this.drawType == 1) ? "传说秘宝箱单抽" : "传说秘宝箱十连");
					GameTGACostCurrency gameTGACostCurrency = ((this.drawCostId == 2) ? GameTGACostCurrency.Gem : GameTGACostCurrency.ChestKey);
					GameApp.SDK.Analyze.Track_EquipmentBoxOpen(text, gameTGACostCurrency, this.drawCostCount, resp.CommonData.Reward, null);
				}
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

		public CustomImage imgSPlus;

		public RedNodeOneCtrl redNode1;

		public RedNodeOneCtrl redNode10;

		public IAPDataModule iapDataModule;

		public ShopDataModule shopDataModule;

		public PropDataModule propDataModule;

		public Shop_Summon cfgShopSummon;

		public Shop_EquipActivity cfgEquipActivity;

		public CustomButton btnProbability;

		private int drawCostId;

		private int drawCostCount;

		private int drawType;
	}
}
