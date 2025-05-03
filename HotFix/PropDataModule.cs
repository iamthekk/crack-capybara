using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;

namespace HotFix
{
	public class PropDataModule : IDataModule
	{
		private LoginDataModule m_loginData
		{
			get
			{
				if (this._loginData == null)
				{
					this._loginData = GameApp.Data.GetDataModule<LoginDataModule>(105);
				}
				return this._loginData;
			}
		}

		public int GetName()
		{
			return 118;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_GameLoginData_ItemData, new HandlerEvent(this.EventItemData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_GameLoginData_ItemData, new HandlerEvent(this.EventItemData));
		}

		public void Reset()
		{
		}

		private void EventItemData(object sender, int type, BaseEventArgs eventArgs)
		{
			EventPropList eventPropList = (EventPropList)eventArgs;
			this.UpdateItemsForLogin(eventPropList.list);
		}

		public List<PropData> GetBagList(PropShowType type)
		{
			if (type == PropShowType.eAll)
			{
				List<PropData> list = new List<PropData>();
				list.AddRange(this.GetBagPropList());
				return list;
			}
			if (type != PropShowType.eProp)
			{
				HLog.LogError(string.Format("PropDataModule.GetBagList type:{0} error.", type));
				return new List<PropData>();
			}
			return this.GetBagPropList();
		}

		private List<PropData> GetBagPropList()
		{
			List<PropData> list = new List<PropData>(this.m_props.Count);
			for (int i = 0; i < this.m_props.Count; i++)
			{
				ItemDto itemDto = this.m_props[i];
				if (itemDto != null)
				{
					Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)itemDto.ItemId);
					if (elementById != null && elementById.inPackage == 1)
					{
						list.Add(itemDto.ToPropData());
					}
				}
			}
			return list;
		}

		public List<ItemData> SortItemDataList(RepeatedField<RewardDto> rewardDtoList)
		{
			List<ItemData> list = new List<ItemData>();
			for (int i = 0; i < rewardDtoList.Count; i++)
			{
				RewardDto rewardDto = rewardDtoList[i];
				list.Add(new ItemData((int)rewardDto.ConfigId, (long)((int)rewardDto.Count)));
			}
			list.ToCombineList();
			List<ItemData> list2 = new List<ItemData>();
			for (int j = 0; j < list.Count; j++)
			{
				if (list[j].ID == 5)
				{
					list2.Add(list[j]);
					list.Remove(list[j]);
					break;
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				if (list[k].ID == 3)
				{
					list2.Add(list[k]);
					list.Remove(list[k]);
					break;
				}
			}
			for (int l = 0; l < list.Count; l++)
			{
				if (list[l].ID == 4)
				{
					list2.Add(list[l]);
					list.Remove(list[l]);
					break;
				}
			}
			for (int m = 0; m < list.Count; m++)
			{
				if (list[m].ID == 1)
				{
					list2.Add(list[m]);
					list.Remove(list[m]);
					break;
				}
			}
			List<ItemData> list3 = new List<ItemData>();
			List<ItemData> list4 = new List<ItemData>();
			for (int n = 0; n < list.Count; n++)
			{
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(list[n].ID);
				if (elementById.itemType != 0 && elementById.itemType != 1)
				{
					list3.Add(list[n]);
				}
				if (elementById.itemType == 1)
				{
					list4.Add(list[n]);
				}
			}
			list2.AddRange(list3);
			list4.Sort(new Comparison<ItemData>(this.SortEquipList));
			list2.AddRange(list4);
			return list2;
		}

		private int SortEquipList(ItemData x, ItemData y)
		{
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(x.ID);
			Item_Item elementById2 = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(y.ID);
			if (elementById.quality > elementById2.quality)
			{
				return -1;
			}
			if (elementById.quality < elementById2.quality)
			{
				return 1;
			}
			if (x.ID > y.ID)
			{
				return -1;
			}
			return 1;
		}

		public void UpdateItemsForLogin(RepeatedField<ItemDto> items)
		{
			this.m_itemsForRowID.Clear();
			this.m_itemsForID.Clear();
			for (int i = 0; i < items.Count; i++)
			{
				this.m_itemsForRowID[items[i].RowId] = items[i];
				this.m_itemsForID[items[i].ItemId] = items[i];
			}
			this.setItemDatas(items.ToItemDtoList());
		}

		public void UpdateItemsForUse(MapField<ulong, ItemDto> mapItems)
		{
			int count = mapItems.Count;
			bool flag = false;
			foreach (ItemDto itemDto in mapItems.Values)
			{
				ulong num = 0UL;
				ItemDto itemDto2;
				this.m_itemsForRowID.TryGetValue(itemDto.RowId, out itemDto2);
				if (itemDto2 != null)
				{
					num = itemDto2.Count;
				}
				if (itemDto.Count == 0UL)
				{
					this.m_itemsForRowID.Remove(itemDto.RowId);
					this.m_itemsForID.Remove(itemDto.ItemId);
				}
				else if (itemDto2 == null)
				{
					itemDto2 = itemDto;
					this.m_itemsForRowID[itemDto.RowId] = itemDto2;
					this.m_itemsForID[itemDto.ItemId] = itemDto2;
				}
				else
				{
					itemDto2.Count = itemDto.Count;
					this.m_itemsForRowID[itemDto.RowId] = itemDto2;
					this.m_itemsForID[itemDto.ItemId] = itemDto2;
				}
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)itemDto.ItemId);
				if (elementById.itemType == 3)
				{
					int propType = elementById.propType;
				}
				if (!flag && num != itemDto.Count && elementById.itemType == 3 && elementById.propType == 5)
				{
					flag = true;
				}
			}
			this.setItemDatas(this.m_itemsForRowID.Values.ToList<ItemDto>());
			for (int i = 0; i < this.m_props.Count; i++)
			{
			}
		}

		public void UpdateItemForUse(ItemDto item)
		{
			ItemDto itemDto;
			this.m_itemsForRowID.TryGetValue(item.RowId, out itemDto);
			if (item.Count == 0UL)
			{
				this.m_itemsForRowID.Remove(item.RowId);
				this.m_itemsForID.Remove(item.ItemId);
			}
			else if (itemDto == null)
			{
				itemDto = item;
				this.m_itemsForRowID[item.RowId] = itemDto;
				this.m_itemsForID[item.ItemId] = itemDto;
			}
			else
			{
				itemDto.Count = item.Count;
				this.m_itemsForRowID[item.RowId] = itemDto;
				this.m_itemsForID[item.ItemId] = itemDto;
			}
			foreach (KeyValuePair<ulong, ItemDto> keyValuePair in this.m_itemsForRowID)
			{
			}
			this.setItemDatas(this.m_itemsForRowID.Values.ToList<ItemDto>());
		}

		public ItemDto GetItemDataByid(ulong id)
		{
			for (int i = 0; i < this.m_props.Count; i++)
			{
				if ((ulong)this.m_props[i].ItemId == id)
				{
					return this.m_props[i];
				}
			}
			return null;
		}

		public long GetItemDataCountByid(ulong id)
		{
			int num = (int)id;
			if (num <= 9)
			{
				switch (num)
				{
				case 1:
				case 4:
					return GameApp.Data.GetDataModule(DataName.LoginDataModule).userCurrency.Coins;
				case 2:
					return (long)GameApp.Data.GetDataModule(DataName.LoginDataModule).userCurrency.Diamonds;
				case 3:
					break;
				default:
					if (num == 9)
					{
						return (long)GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicketCount(UserTicketKind.UserLife);
					}
					break;
				}
			}
			else
			{
				if (num == 10)
				{
					return (long)GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicketCount(UserTicketKind.CrossArena);
				}
				if (num == 19)
				{
					return (long)GameApp.Data.GetDataModule(DataName.TicketDataModule).GetTicketCount(UserTicketKind.Tower);
				}
				if (num == 23)
				{
					return (long)((ulong)GameApp.Data.GetDataModule(DataName.LoginDataModule).userCurrency.ChestScore);
				}
			}
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)id);
			if (elementById != null)
			{
				switch (elementById.itemType)
				{
				case 19:
				{
					PetData petDataByConfigId = GameApp.Data.GetDataModule(DataName.PetDataModule).GetPetDataByConfigId((int)id);
					if (petDataByConfigId != null)
					{
						return (long)petDataByConfigId.petCount;
					}
					return 0L;
				}
				case 20:
				{
					PetData petDataByConfigId2 = GameApp.Data.GetDataModule(DataName.PetDataModule).GetPetDataByConfigId(int.Parse(elementById.itemTypeParam[0]));
					if (petDataByConfigId2 != null)
					{
						return (long)petDataByConfigId2.fragmentCount;
					}
					return 0L;
				}
				case 21:
				case 22:
				case 23:
					return (long)GameApp.Data.GetDataModule(DataName.CollectionDataModule).GetCollectionCount((int)id);
				}
			}
			for (int i = 0; i < this.m_props.Count; i++)
			{
				if ((ulong)this.m_props[i].ItemId == id)
				{
					return (long)this.m_props[i].Count;
				}
			}
			return 0L;
		}

		public bool IsHaveItemDatas(List<ItemData> itemDatas)
		{
			if (itemDatas == null)
			{
				return true;
			}
			for (int i = 0; i < itemDatas.Count; i++)
			{
				ItemData itemData = itemDatas[i];
				if (itemData != null && this.GetItemDataCountByid((ulong)((long)itemData.ID)) < itemData.TotalCount)
				{
					return false;
				}
			}
			return true;
		}

		public ItemDto GetItemDataByServerID(ulong rowId)
		{
			ItemDto itemDto;
			if (this.m_itemsForRowID.TryGetValue(rowId, out itemDto))
			{
				return itemDto;
			}
			return null;
		}

		public ItemDto GetItemData(PropName name)
		{
			ItemDto itemDto;
			if (this.m_itemsForID.TryGetValue((uint)name, out itemDto))
			{
				return itemDto;
			}
			return null;
		}

		public long GetItemCountByServerID(ulong rowId)
		{
			ItemDto itemDto;
			if (this.m_itemsForRowID.TryGetValue(rowId, out itemDto))
			{
				return (long)itemDto.Count;
			}
			return 0L;
		}

		public long GetItemCount(PropName name)
		{
			ItemDto itemData = this.GetItemData(name);
			if (itemData != null)
			{
				return (long)((int)itemData.Count);
			}
			return 0L;
		}

		public void AddItemLocal(PropName name, int count)
		{
			ItemDto itemData = this.GetItemData(name);
			if (itemData != null)
			{
				itemData.Count += (ulong)count;
				return;
			}
			HLog.LogError(string.Format("PropDataModule.AddItemLocal[{0}],count[{1}] error.", name, count));
		}

		private void setItemDatas(List<ItemDto> items)
		{
			this.m_props = items;
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Prop);
		}

		public List<ItemData> GetItemDatas(ItemType itemType, PropType? propType = null)
		{
			int? num;
			if (propType != null)
			{
				PropType? propType2 = propType;
				num = ((propType2 != null) ? new int?((int)propType2.GetValueOrDefault()) : null);
			}
			else
			{
				num = null;
			}
			int? num2 = num;
			List<ItemData> list = new List<ItemData>();
			for (int i = 0; i < this.m_props.Count; i++)
			{
				ItemDto itemDto = this.m_props[i];
				if (itemDto != null)
				{
					Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)itemDto.ItemId);
					if (elementById != null)
					{
						bool flag = false;
						if (elementById.itemType == (int)itemType)
						{
							flag = true;
							if (num2 == null || elementById.propType != num2.Value)
							{
								flag = false;
							}
						}
						if (flag)
						{
							list.Add(itemDto.ToItemData());
						}
					}
				}
			}
			return list;
		}

		public static bool TryGetModelIdByItemId(int itemId, out ItemType itemType, out int memberId)
		{
			memberId = 0;
			itemType = ItemType.eUseItem;
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemId);
			if (elementById != null)
			{
				itemType = (ItemType)elementById.itemType;
				if (itemType == ItemType.ePet)
				{
					Pet_pet elementById2 = GameApp.Table.GetManager().GetPet_petModelInstance().GetElementById(itemId);
					if (elementById2 != null)
					{
						memberId = elementById2.memberId;
					}
				}
				else
				{
					memberId = itemId;
				}
			}
			return memberId > 0;
		}

		public List<ItemDto> m_props = new List<ItemDto>();

		private LoginDataModule _loginData;

		private Dictionary<ulong, ItemDto> m_itemsForRowID = new Dictionary<ulong, ItemDto>();

		private Dictionary<uint, ItemDto> m_itemsForID = new Dictionary<uint, ItemDto>();
	}
}
