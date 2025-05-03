using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using Shop.Arena;
using UnityEngine;

namespace HotFix
{
	public class ShopActivitySUpBigRewardSelectViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.prefabItem.gameObject.SetActive(false);
		}

		public override void OnOpen(object data)
		{
			IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
			this._iapShopActivityData = dataModule.GetShopSUpActivityData();
			this._canRewardCount = dataModule.GetEquipSUpPoolLeftSelectTime();
			this.listData = this._iapShopActivityData.shopActDetailDto.SummonPoolItems.ToList<int>();
			if (this.listData == null)
			{
				this.listData = new List<int>();
			}
			this.UpdateItems();
			this.UpdateSelect(this.selectItem);
			this.UpdateBtnSelect();
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
			if (this._iapShopActivityData == null)
			{
				GameApp.View.CloseView(ViewName.ShopActivitySUpBigRewardSelectViewModule, null);
				return;
			}
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			if (this._iapShopActivityData.endTimestamp <= serverTimestamp)
			{
				GameApp.View.CloseView(ViewName.ShopActivitySUpBigRewardSelectViewModule, null);
				return;
			}
		}

		public override void OnClose()
		{
			this.selectItem = null;
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].DeInit();
				Object.Destroy(this.items[i].gameObject);
			}
			this.items.Clear();
		}

		public override void OnDelete()
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
			this.popCommon.OnClick = new Action<UIPopCommon.UIPopCommonClickType>(this.OnUIPopCommonClick);
			this.btnSelect.m_onClick = new Action(this.OnBtnSelectConfirmClick);
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
			this.popCommon.OnClick = null;
			this.btnSelect.m_onClick = null;
		}

		private void UpdateItems()
		{
			for (int i = 0; i < this.listData.Count; i++)
			{
				ShopActivitySUpBigRewardSelectViewModule_Item shopActivitySUpBigRewardSelectViewModule_Item;
				if (i < this.items.Count)
				{
					shopActivitySUpBigRewardSelectViewModule_Item = this.items[i];
				}
				else
				{
					shopActivitySUpBigRewardSelectViewModule_Item = Object.Instantiate<ShopActivitySUpBigRewardSelectViewModule_Item>(this.prefabItem, this.prefabItem.transform.parent, false);
					shopActivitySUpBigRewardSelectViewModule_Item.Init();
					shopActivitySUpBigRewardSelectViewModule_Item.onClickCallback = new Action<ShopActivitySUpBigRewardSelectViewModule_Item>(this.OnBenSelectClick);
					this.items.Add(shopActivitySUpBigRewardSelectViewModule_Item);
				}
				shopActivitySUpBigRewardSelectViewModule_Item.gameObject.SetActive(true);
				shopActivitySUpBigRewardSelectViewModule_Item.UpdateData(i, this.listData[i]);
			}
			for (int j = this.listData.Count; j < this.items.Count; j++)
			{
				this.items[j].gameObject.SetActive(false);
			}
		}

		private void UpdateSelect(ShopActivitySUpBigRewardSelectViewModule_Item item)
		{
			for (int i = 0; i < this.items.Count; i++)
			{
				this.items[i].SetSelect(this.items[i] == item);
			}
			this.UpdateBtnSelect();
		}

		private void UpdateBtnSelect()
		{
			if (this.selectItem != null)
			{
				this.btnSelect.GetComponent<UIGrays>().Recovery();
				return;
			}
			this.btnSelect.GetComponent<UIGrays>().SetUIGray();
		}

		private void OnUIPopCommonClick(UIPopCommon.UIPopCommonClickType clickType)
		{
			if (clickType <= UIPopCommon.UIPopCommonClickType.ButtonClose)
			{
				this.OnBtnCloseClick();
			}
		}

		private void OnBtnCloseClick()
		{
			GameApp.View.CloseView(ViewName.ShopActivitySUpBigRewardSelectViewModule, null);
		}

		private void OnBenSelectClick(ShopActivitySUpBigRewardSelectViewModule_Item item)
		{
			if (this._canRewardCount <= 0)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("suppool_progress_not_full"));
				return;
			}
			this.selectItem = item;
			this.UpdateSelect(item);
		}

		private void OnBtnSelectConfirmClick()
		{
			if (this._canRewardCount <= 0)
			{
				GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("suppool_progress_not_full"));
				return;
			}
			if (this.selectItem != null)
			{
				int selectEquipId = this.selectItem.equipId;
				NetworkUtils.Shop.DoBuyItemRewardRequest(this._iapShopActivityData.activityId, this.selectItem.equipId, delegate(bool result, BuyItemRewardResponse resp)
				{
					GameApp.View.CloseView(ViewName.ShopActivitySUpBigRewardSelectViewModule, null);
					IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
					int equipSUpPoolLeftSelectTime = dataModule.GetEquipSUpPoolLeftSelectTime();
					List<TGACommonItemInfo> list = new List<TGACommonItemInfo>();
					IAPShopActivityData shopSUpActivityData = dataModule.GetShopSUpActivityData();
					if (shopSUpActivityData != null && shopSUpActivityData.shopActDetailDto != null && shopSUpActivityData.shopActDetailDto.SummonPoolItems != null)
					{
						for (int i = 0; i < shopSUpActivityData.shopActDetailDto.SummonPoolItems.Count; i++)
						{
							list.Add(new TGACommonItemInfo(shopSUpActivityData.shopActDetailDto.SummonPoolItems[i], 1L));
						}
					}
					TGACommonItemInfo tgacommonItemInfo = new TGACommonItemInfo(selectEquipId, 1L);
					GameApp.SDK.Analyze.Track_UpEquipmentOptional((long)equipSUpPoolLeftSelectTime, list, tgacommonItemInfo);
				});
				return;
			}
			GameApp.View.ShowStringTip(Singleton<LanguageManager>.Instance.GetInfoByID("suppool_select_tip"));
		}

		public UIPopCommon popCommon;

		public ShopActivitySUpBigRewardSelectViewModule_Item prefabItem;

		public CustomButton btnSelect;

		private List<ShopActivitySUpBigRewardSelectViewModule_Item> items = new List<ShopActivitySUpBigRewardSelectViewModule_Item>();

		private List<int> listData = new List<int>();

		private ShopActivitySUpBigRewardSelectViewModule_Item selectItem;

		private IAPShopActivityData _iapShopActivityData;

		private int _canRewardCount;
	}
}
