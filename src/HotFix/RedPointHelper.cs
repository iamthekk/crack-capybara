using System;
using System.Collections.Generic;
using Framework;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public static class RedPointHelper
	{
		private static bool IsNeedCalcMainChestTab(MapField<ulong, ItemDto> mapItems)
		{
			foreach (ItemDto itemDto in mapItems.Values)
			{
				if (itemDto.ItemId == 71U || itemDto.ItemId == 72U || itemDto.ItemId == 73U || itemDto.ItemId == 74U || itemDto.ItemId == 75U)
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsNeedCalcMainShopTab(MapField<ulong, ItemDto> mapItems)
		{
			using (IEnumerator<ItemDto> enumerator = mapItems.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.ItemId == 61U)
					{
						return true;
					}
				}
			}
			return false;
		}

		private static bool IsNeedCalcMainEquipTab(MapField<ulong, ItemDto> mapItems)
		{
			foreach (ItemDto itemDto in mapItems.Values)
			{
				if (itemDto.ItemId == 24U || itemDto.ItemId == 25U || itemDto.ItemId == 11U || itemDto.ItemId == 26U || itemDto.ItemId == 28U || itemDto.ItemId == 29U)
				{
					return true;
				}
				Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item((int)itemDto.ItemId);
				if (item_Item != null && (item_Item.itemType == 30 || item_Item.itemType == 31))
				{
					return true;
				}
			}
			return false;
		}

		public static void CurrencyChangeCheck(UserCurrency newCurrency, UserCurrency oldCurrency)
		{
			if (newCurrency == null || oldCurrency == null)
			{
				return;
			}
			if (newCurrency.Coins != oldCurrency.Coins)
			{
				RedPointController.Instance.ReCalcAsync("Talent");
			}
		}

		public static void ItemChangeCheck(MapField<ulong, ItemDto> mapItems)
		{
			if (mapItems == null)
			{
				return;
			}
			if (RedPointHelper.IsNeedCalcMainChestTab(mapItems))
			{
				RedPointController.Instance.ReCalcAsync("Chest");
			}
			if (RedPointHelper.IsNeedCalcMainEquipTab(mapItems))
			{
				RedPointController.Instance.ReCalcAsync("Equip");
			}
			if (RedPointHelper.IsNeedCalcMainShopTab(mapItems))
			{
				RedPointController.Instance.ReCalcAsync("MainShop");
			}
			foreach (ItemDto itemDto in mapItems.Values)
			{
				RedPointHelper.ItemChangeEventDispatchAsync((int)itemDto.ItemId);
			}
		}

		private static void ItemChangeEventDispatchAsync(int itemId)
		{
			if (RedPointHelper.cacheChangeList.Contains(itemId))
			{
				return;
			}
			DelayCall.Instance.CallOnce(1000, delegate
			{
				RedPointHelper.cacheChangeList.Remove(itemId);
				GameApp.Event.DispatchNow(null, LocalMessageName.CC_Item_Update, new EventArgsItemUpdate().SetData(itemId));
			});
		}

		public static void EquipChangeCheck(RepeatedField<EquipmentDto> equips)
		{
			RedPointController.Instance.ReCalcAsync("Equip");
		}

		public static void PetChangeCheck(RepeatedField<PetDto> pets)
		{
			RedPointController.Instance.ReCalcAsync("Equip");
		}

		public static void CollectionChangeCheck(RepeatedField<CollectionDto> dtos)
		{
			RedPointController.Instance.ReCalcAsync("Equip");
		}

		public static List<int> cacheChangeList = new List<int>();
	}
}
