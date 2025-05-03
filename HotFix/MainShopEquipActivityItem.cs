using System;
using Framework;
using LocalModels.Bean;
using Shop.Arena;
using UnityEngine;

namespace HotFix
{
	public class MainShopEquipActivityItem : IAPShopMainEquipActivityBase
	{
		protected override void OnInit()
		{
			this.iapDataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
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
			this.iapDataModule = null;
			this.propDataModule = null;
		}

		public void SetData(IAPShopActivityData data)
		{
			this.cfgEquipActivity = GameApp.Table.GetManager().GetShop_EquipActivityModelInstance().GetElementById(data.linkId);
			if (this.cfgEquipActivity == null)
			{
				HLog.LogError("Shop_EquipActivities表中找不到id为" + data.activityId.ToString() + "的数据");
				return;
			}
			this.cfgShopSummon = GameApp.Table.GetManager().GetShop_SummonModelInstance().GetElementById(data.linkId);
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
			if (this.cfgShopSummon.singlePriceOrigin > 0 && itemDataCountByid < (long)this.cfgShopSummon.tenPrice)
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
				openData.ChestType = eEquipChestType.ActivityChest;
				openData.CostType = 2;
				openData.shopSummonId = this.cfgShopSummon.id;
				GameApp.View.OpenView(ViewName.OpenEquipBoxViewModule, openData, 1, null, delegate
				{
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_ShopDataMoudule_RefreshShopData, null);
				});
			}
		}

		public IAPDataModule iapDataModule;

		public PropDataModule propDataModule;

		public Shop_Summon cfgShopSummon;

		public Shop_EquipActivity cfgEquipActivity;
	}
}
