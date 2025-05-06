using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Framework.Logic;
using LocalModels.Bean;
using Proto.Common;
using Proto.IntegralShop;
using Shop.Arena;

namespace HotFix
{
	public class ShopDataModule : IDataModule
	{
		public int GetName()
		{
			return 128;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_ShopDataMoudule_RefreshShopData, new HandlerEvent(this.OnEventRefreshShopData));
			manager.RegisterEvent(LocalMessageName.CC_ShopDataMoudule_BuyShopItem, new HandlerEvent(this.OnEventBuyShopItem));
			manager.RegisterEvent(LocalMessageName.CC_DayChange_InternalShop_DataPull, new HandlerEvent(this.OnEventDayChangeInternalShopDataPull));
			manager.RegisterEvent(LocalMessageName.CC_DayChange_ShopInfo_DataPull, new HandlerEvent(this.OnEventDayChangeShopInfoDataPull));
			manager.RegisterEvent(LocalMessageName.CC_GameLogin_LoginData, new HandlerEvent(this.OnLoginFinish));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_ShopDataMoudule_RefreshShopData, new HandlerEvent(this.OnEventRefreshShopData));
			manager.UnRegisterEvent(LocalMessageName.CC_ShopDataMoudule_BuyShopItem, new HandlerEvent(this.OnEventBuyShopItem));
			manager.UnRegisterEvent(LocalMessageName.CC_DayChange_InternalShop_DataPull, new HandlerEvent(this.OnEventDayChangeInternalShopDataPull));
			manager.UnRegisterEvent(LocalMessageName.CC_DayChange_ShopInfo_DataPull, new HandlerEvent(this.OnEventDayChangeShopInfoDataPull));
			manager.UnRegisterEvent(LocalMessageName.CC_GameLogin_LoginData, new HandlerEvent(this.OnLoginFinish));
		}

		public void Reset()
		{
		}

		private void OnLoginFinish(object sender, int type, BaseEventArgs eventargs)
		{
			NetworkUtils.Shop.IntegralShopGetInfoRequest(ShopType.Guild, delegate(bool isOk, IntegralShopGetInfoResponse resp)
			{
			});
			NetworkUtils.Shop.IntegralShopGetInfoRequest(ShopType.ManaCrystal, delegate(bool isOk, IntegralShopGetInfoResponse resp)
			{
			});
		}

		private void OnEventDayChangeInternalShopDataPull(object sender, int type, BaseEventArgs eventargs)
		{
			this._dayChangeGetResponseCount = 0;
			NetworkUtils.Shop.IntegralShopGetInfoRequest(ShopType.BlackMarket, delegate(bool isOk, IntegralShopGetInfoResponse resp)
			{
				if (isOk)
				{
					this._dayChangeGetResponseCount++;
					if (this._dayChangeGetResponseCount == 3)
					{
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_UI_Shop_Refresh, null);
					}
				}
			});
			NetworkUtils.Shop.IntegralShopGetInfoRequest(ShopType.Guild, delegate(bool isOk, IntegralShopGetInfoResponse resp)
			{
				if (isOk)
				{
					this._dayChangeGetResponseCount++;
					if (this._dayChangeGetResponseCount == 3)
					{
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_UI_Shop_Refresh, null);
					}
				}
			});
			NetworkUtils.Shop.IntegralShopGetInfoRequest(ShopType.ManaCrystal, delegate(bool isOk, IntegralShopGetInfoResponse resp)
			{
				if (isOk)
				{
					this._dayChangeGetResponseCount++;
					if (this._dayChangeGetResponseCount == 3)
					{
						GameApp.Event.DispatchNow(null, LocalMessageName.CC_UI_Shop_Refresh, null);
					}
				}
			});
		}

		private void OnEventDayChangeShopInfoDataPull(object sender, int type, BaseEventArgs eventargs)
		{
			NetworkUtils.Purchase.ShopGetInfoRequest(delegate(bool isOk, ShopGetInfoResponse resp)
			{
				if (isOk && resp != null)
				{
					IAPDataModule dataModule = GameApp.Data.GetDataModule(DataName.IAPDataModule);
					if (resp.ShopAllDataDto.IapInfo == null)
					{
						resp.ShopAllDataDto.IapInfo = new IAPDto();
					}
					dataModule.InitForServer(resp.ShopAllDataDto);
					GameApp.Data.GetDataModule(DataName.AdDataModule).UpdateAdData(resp.ShopAllDataDto.AdData);
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_UI_Shop_Refresh, null);
					GameApp.Event.DispatchNow(null, LocalMessageName.CC_UI_AD_REFRESH, null);
				}
			});
		}

		private void OnEventRefreshShopData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsRefreshShopData eventArgsRefreshShopData = eventargs as EventArgsRefreshShopData;
			if (eventArgsRefreshShopData == null || eventArgsRefreshShopData.ShopType == ShopType.Null)
			{
				return;
			}
			List<IntegralShop_goods> itemConfigList;
			if (!this.shopItemsConfigDic.TryGetValue(eventArgsRefreshShopData.ShopType, out itemConfigList))
			{
				itemConfigList = new List<IntegralShop_goods>();
				this.shopItemsConfigDic.Add(eventArgsRefreshShopData.ShopType, itemConfigList);
			}
			itemConfigList.Clear();
			if (eventArgsRefreshShopData.ShopInfo == null)
			{
				this.shopInfoDic.Remove(eventArgsRefreshShopData.ShopType);
				this.shopRefreshTimestampDic.Remove(eventArgsRefreshShopData.ShopType);
				return;
			}
			this.shopInfoDic[eventArgsRefreshShopData.ShopType] = eventArgsRefreshShopData.ShopInfo;
			this.shopRefreshTimestampDic[eventArgsRefreshShopData.ShopType] = new Dictionary<GoodsRefreshType, long>();
			this.shopRefreshTimestampDic[eventArgsRefreshShopData.ShopType][GoodsRefreshType.Day] = eventArgsRefreshShopData.ShopInfo.RefTimeDay;
			this.shopRefreshTimestampDic[eventArgsRefreshShopData.ShopType][GoodsRefreshType.Week] = eventArgsRefreshShopData.ShopInfo.RefTimeWeek;
			this.shopRefreshTimestampDic[eventArgsRefreshShopData.ShopType][GoodsRefreshType.Month] = eventArgsRefreshShopData.ShopInfo.RefTimeMonth;
			if (eventArgsRefreshShopData.ShopInfo.GoodsConfigId == null || eventArgsRefreshShopData.ShopInfo.GoodsConfigId.Count <= 0)
			{
				return;
			}
			eventArgsRefreshShopData.ShopInfo.GoodsConfigId.ToList<uint>().ForEach(delegate(uint id)
			{
				IntegralShop_goods elementById = GameApp.Table.GetManager().GetIntegralShop_goodsModelInstance().GetElementById((int)id);
				if (elementById != null)
				{
					itemConfigList.Add(elementById);
				}
			});
			itemConfigList.Sort((IntegralShop_goods a, IntegralShop_goods b) => a.Sort.CompareTo(b.Sort));
		}

		private void OnEventBuyShopItem(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsBuyShopItem eventArgsBuyShopItem = eventargs as EventArgsBuyShopItem;
			IntegralShop_goods integralShop_goods;
			IntegralShopDto integralShopDto;
			if (eventArgsBuyShopItem == null || !this.GetItemConfig(eventArgsBuyShopItem.ShopItemId, out integralShop_goods) || !this.shopInfoDic.TryGetValue((ShopType)integralShop_goods.TypeId, out integralShopDto))
			{
				return;
			}
			integralShopDto.BuyConfigId.Add((uint)eventArgsBuyShopItem.ShopItemId);
		}

		public bool HasShopInfo(ShopType shopType)
		{
			return this.shopInfoDic.ContainsKey(shopType);
		}

		public List<IntegralShop_goods> GetShopItemsConfig(ShopType shopType, GoodsRefreshType refreshType = GoodsRefreshType.None)
		{
			List<IntegralShop_goods> list;
			if (!this.shopItemsConfigDic.TryGetValue(shopType, out list))
			{
				list = new List<IntegralShop_goods>();
			}
			if (list.Count > 0 && refreshType != GoodsRefreshType.None)
			{
				this._tempList.Clear();
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].RefreshType == (int)refreshType)
					{
						this._tempList.Add(list[i]);
					}
				}
				return this._tempList;
			}
			return list;
		}

		public bool GetShopConfig(ShopType shopType, out IntegralShop_data shopConfig)
		{
			shopConfig = GameApp.Table.GetManager().GetIntegralShop_dataModelInstance().GetElementById((int)shopType);
			return shopConfig != null;
		}

		public bool GetItemConfig(int itemId, out IntegralShop_goods itemConfig)
		{
			itemConfig = GameApp.Table.GetManager().GetIntegralShop_goodsModelInstance().GetElementById(itemId);
			return itemConfig != null;
		}

		public bool GetShopRefreshInfo(ShopType shopType, out int refreshCost)
		{
			IntegralShopDto integralShopDto;
			IntegralShop_data integralShop_data;
			if (!this.shopInfoDic.TryGetValue(shopType, out integralShopDto) || !this.GetShopConfig(shopType, out integralShop_data))
			{
				refreshCost = 0;
				return false;
			}
			int round = (int)integralShopDto.Round;
			int[] refreshCost2 = integralShop_data.RefreshCost;
			if (round >= refreshCost2.Length)
			{
				refreshCost = 0;
				return false;
			}
			refreshCost = refreshCost2[round];
			return true;
		}

		public void GetRefreshCount(ShopType shopType, out int remainCount, out int totalCount)
		{
			IntegralShopDto integralShopDto;
			IntegralShop_data integralShop_data;
			if (!this.shopInfoDic.TryGetValue(shopType, out integralShopDto) || !this.GetShopConfig(shopType, out integralShop_data))
			{
				remainCount = 0;
				totalCount = 0;
				return;
			}
			int[] refreshCost = integralShop_data.RefreshCost;
			totalCount = refreshCost.Length;
			remainCount = Utility.Math.Max(totalCount - (int)integralShopDto.Round, 0);
		}

		public void GetShopItemData(IntegralShop_goods data, out int id, out int count)
		{
			id = data.Items[0];
			count = data.Items[1];
		}

		public bool GetOffInfo(IntegralShop_goods data, out int off)
		{
			int discount = data.Discount;
			if (discount >= 100)
			{
				off = 0;
				return false;
			}
			off = 100 - discount;
			return true;
		}

		public void GetPrice(IntegralShop_goods data, out int originalPrice, out int curPrice)
		{
			originalPrice = data.Price;
			curPrice = data.Price * data.Discount / 100;
		}

		public bool IsEnoughCurrency(IntegralShop_goods itemData)
		{
			IntegralShop_data integralShop_data;
			if (!this.GetShopConfig((ShopType)itemData.TypeId, out integralShop_data))
			{
				return false;
			}
			long itemDataCountByid = GameApp.Data.GetDataModule(DataName.PropDataModule).GetItemDataCountByid((ulong)((long)itemData.currencyID));
			int num;
			int num2;
			this.GetPrice(itemData, out num, out num2);
			return (long)num2 <= itemDataCountByid;
		}

		public bool GetIsSellOut(IntegralShop_goods itemModel)
		{
			IntegralShopDto integralShopDto;
			return !this.shopInfoDic.TryGetValue((ShopType)itemModel.TypeId, out integralShopDto) || integralShopDto.BuyConfigId.Count((uint id) => (ulong)id == (ulong)((long)itemModel.ID)) >= itemModel.BuyTimes;
		}

		public int GetBoughtTimes(IntegralShop_goods itemModel)
		{
			IntegralShopDto integralShopDto;
			if (this.shopInfoDic.TryGetValue((ShopType)itemModel.TypeId, out integralShopDto))
			{
				return integralShopDto.BuyConfigId.Count((uint id) => (ulong)id == (ulong)((long)itemModel.ID));
			}
			return 0;
		}

		public bool GetRefreshCountDownTime(ShopType shopType, GoodsRefreshType type, out long countDown)
		{
			Dictionary<GoodsRefreshType, long> dictionary;
			if (!this.shopRefreshTimestampDic.TryGetValue(shopType, out dictionary))
			{
				countDown = 0L;
				return false;
			}
			long num = dictionary[type];
			long serverTimestamp = DxxTools.Time.ServerTimestamp;
			countDown = Math.Max(num - serverTimestamp, 0L);
			return true;
		}

		public Shop_EquipActivity GetEquipChestTableByType(eEquipChestType type)
		{
			IList<Shop_EquipActivity> allElements = GameApp.Table.GetManager().GetShop_EquipActivityModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				if (allElements[i].type == (int)type)
				{
					return allElements[i];
				}
			}
			return null;
		}

		private readonly Dictionary<ShopType, List<IntegralShop_goods>> shopItemsConfigDic = new Dictionary<ShopType, List<IntegralShop_goods>>();

		private readonly Dictionary<ShopType, IntegralShopDto> shopInfoDic = new Dictionary<ShopType, IntegralShopDto>();

		private readonly Dictionary<ShopType, Dictionary<GoodsRefreshType, long>> shopRefreshTimestampDic = new Dictionary<ShopType, Dictionary<GoodsRefreshType, long>>();

		private int _dayChangeGetResponseCount;

		private List<IntegralShop_goods> _tempList = new List<IntegralShop_goods>();
	}
}
