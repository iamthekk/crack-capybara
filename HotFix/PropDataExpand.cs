using System;
using System.Collections.Generic;
using System.Text;
using Framework;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Server;

namespace HotFix
{
	public static class PropDataExpand
	{
		public static Item_Item GetTableItem(this PropData propData)
		{
			return GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)propData.id);
		}

		public static bool IsHaveGift(this Item_Item item)
		{
			return item.itemGiftId != 0;
		}

		public static ItemGift_ItemGift GetItemGift(this Item_Item item)
		{
			int itemGiftId = item.itemGiftId;
			if (!item.IsHaveGift())
			{
				return null;
			}
			return GameApp.Table.GetManager().GetItemGift_ItemGiftModelInstance().GetElementById(itemGiftId);
		}

		public static ItemGift_ItemGift GetItemGift(this PropData propData)
		{
			return propData.GetTableItem().GetItemGift();
		}

		public static List<PropData> ToPropDatas(this RepeatedField<ItemDto> items)
		{
			List<PropData> list = new List<PropData>(items.Count);
			for (int i = 0; i < items.Count; i++)
			{
				PropData propData = items[i].ToPropData();
				list.Add(propData);
			}
			return list;
		}

		public static List<ItemData> ToItemDatas(this RepeatedField<ItemDto> items)
		{
			List<ItemData> list = new List<ItemData>(items.Count);
			for (int i = 0; i < items.Count; i++)
			{
				ItemData itemData = items[i].ToItemData();
				list.Add(itemData);
			}
			return list;
		}

		public static List<ItemDto> ToItemDtoList(this RepeatedField<ItemDto> items)
		{
			List<ItemDto> list = new List<ItemDto>(items.Count);
			for (int i = 0; i < items.Count; i++)
			{
				ItemDto itemDto = items[i];
				list.Add(itemDto);
			}
			return list;
		}

		public static BaseItemData ToBaseItemData(this PropData item)
		{
			return new BaseItemData
			{
				rowId = item.rowId,
				id = item.id,
				count = item.count
			};
		}

		public static RepeatedField<ItemDto> ToItemDtos(this List<PropData> propDatas)
		{
			RepeatedField<ItemDto> repeatedField = new RepeatedField<ItemDto>();
			for (int i = 0; i < propDatas.Count; i++)
			{
				ItemDto itemDto = propDatas[i].ToItemDto();
				repeatedField.Add(itemDto);
			}
			return repeatedField;
		}

		public static PropData ToPropData(this ItemDto item)
		{
			return new PropData
			{
				rowId = item.RowId,
				id = item.ItemId,
				count = item.Count
			};
		}

		public static ItemData ToItemData(this ItemDto item)
		{
			return new ItemData((int)item.ItemId, (long)item.Count);
		}

		public static PropData ToPropData(this ItemData item)
		{
			return new PropData
			{
				id = (uint)item.ID,
				count = (ulong)((uint)item.TotalCount)
			};
		}

		public static PropData ToPropData(this ItemData item, bool calcDynamic)
		{
			if (calcDynamic)
			{
				item.SetReCalc();
			}
			return new PropData
			{
				id = (uint)item.ID,
				count = (ulong)((uint)item.TotalCount)
			};
		}

		public static ItemDto ToItemDto(this PropData propData)
		{
			return new ItemDto
			{
				RowId = propData.rowId,
				ItemId = propData.id,
				Count = propData.count
			};
		}

		public static ItemData ToItemData(this PropData propData)
		{
			return new ItemData((int)propData.id, (long)((int)propData.count));
		}

		public static List<ItemData> ToItemDatas(this List<PropData> propDatas)
		{
			List<ItemData> list = new List<ItemData>();
			for (int i = 0; i < propDatas.Count; i++)
			{
				list.Add(propDatas[i].ToItemData());
			}
			return list;
		}

		public static ItemData ToItemData(this EquipData equipData)
		{
			return new ItemData((int)equipData.id, (long)equipData.count);
		}

		public static EquipmentDto ToEquipmentDto(this EquipData equipData)
		{
			return new EquipmentDto
			{
				RowId = equipData.rowID,
				EquipId = equipData.id,
				Level = equipData.level,
				Exp = equipData.exp,
				Evolution = (uint)equipData.evolution
			};
		}

		public static CollectionDto ToCollectionDto(this CollectionData collectionData)
		{
			return new CollectionDto
			{
				RowId = collectionData.rowId,
				CollecType = collectionData.collectionType,
				ConfigId = (uint)collectionData.itemId,
				CollecCount = (uint)collectionData.fragMentCount,
				CollecStar = (uint)collectionData.collectionStar
			};
		}

		public static RelicDto ToRelicDto(this RelicData relicData)
		{
			return new RelicDto
			{
				RelicId = (uint)relicData.m_id,
				Level = (uint)relicData.m_level,
				Star = (uint)relicData.m_quality
			};
		}

		public static List<ItemData> GetRewards(this ItemGift_ItemGift itemGift)
		{
			List<string> listString = itemGift.Rewards.GetListString('|');
			List<ItemData> list = new List<ItemData>(listString.Count);
			for (int i = 0; i < listString.Count; i++)
			{
				string text = listString[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					if (array.Length == 2)
					{
						int num;
						int.TryParse(array[0], out num);
						int num2;
						int.TryParse(array[1], out num2);
						ItemData itemData = new ItemData(num, (long)num2);
						list.Add(itemData);
					}
				}
			}
			return list;
		}

		public static PropData ToPropData(this EquipData equip)
		{
			return new PropData
			{
				rowId = equip.rowID,
				id = equip.id,
				count = (ulong)equip.count,
				level = equip.level,
				exp = equip.exp
			};
		}

		public static PropData ToPropData(this EquipmentDto equip)
		{
			return new PropData
			{
				rowId = equip.RowId,
				id = equip.EquipId,
				count = 1UL,
				level = equip.Level,
				exp = equip.Exp
			};
		}

		public static EquipData ToEquipData(this EquipmentDto equip)
		{
			if (equip == null || equip.RowId <= 0UL)
			{
				return null;
			}
			EquipData equipData = new EquipData();
			equipData.SetEquipData(equip);
			equipData.count = 1;
			return equipData;
		}

		public static List<EquipmentDto> ToEquipDtoList(this RepeatedField<EquipmentDto> equips)
		{
			List<EquipmentDto> list = new List<EquipmentDto>(equips.Count);
			for (int i = 0; i < equips.Count; i++)
			{
				EquipmentDto equipmentDto = equips[i];
				list.Add(equipmentDto);
			}
			return list;
		}

		public static List<EquipData> ToEquipList(this RepeatedField<EquipmentDto> equips)
		{
			List<EquipData> list = new List<EquipData>(equips.Count);
			for (int i = 0; i < equips.Count; i++)
			{
				EquipmentDto equipmentDto = equips[i];
				EquipData equipData = new EquipData();
				equipData.SetEquipData(equips[i]);
				list.Add(equipData);
			}
			return list;
		}

		public static List<PropData> ToPropList(this List<ItemData> items)
		{
			List<PropData> list = new List<PropData>();
			for (int i = 0; i < items.Count; i++)
			{
				list.Add(new PropData
				{
					id = (uint)items[i].ID,
					count = (ulong)items[i].Count
				});
			}
			return list;
		}

		public static List<PropData> ToPropList(this List<ItemDto> items)
		{
			List<PropData> list = new List<PropData>();
			for (int i = 0; i < items.Count; i++)
			{
				ItemDto itemDto = items[i];
				list.Add(new PropData
				{
					rowId = itemDto.RowId,
					id = itemDto.ItemId,
					count = itemDto.Count
				});
			}
			return list;
		}

		public static List<PropData> ToPropList(this List<EquipmentDto> equips)
		{
			List<PropData> list = new List<PropData>();
			for (int i = 0; i < equips.Count; i++)
			{
				EquipmentDto equipmentDto = equips[i];
				list.Add(new PropData
				{
					rowId = equipmentDto.RowId,
					id = equipmentDto.EquipId,
					count = 1UL,
					level = equipmentDto.Level,
					exp = equipmentDto.Exp
				});
			}
			return list;
		}

		public static EquipmentDto ToEquipmentDto(this PropData propData)
		{
			return new EquipmentDto
			{
				RowId = propData.rowId,
				EquipId = propData.id,
				Level = propData.level,
				Exp = propData.exp
			};
		}

		public static EquipData ToEquipData(this PropData propData)
		{
			EquipmentDto equipmentDto = propData.ToEquipmentDto();
			EquipData equipData = new EquipData();
			equipData.SetEquipData(equipmentDto);
			return equipData;
		}

		public static RelicData ToRelicData(this PropData propData)
		{
			return new RelicData
			{
				m_id = (int)propData.id,
				m_level = 1
			};
		}

		public static PetDto ToPetDto(this PropData propData)
		{
			return new PetDto
			{
				RowId = propData.rowId,
				ConfigId = propData.id,
				PetLv = 1U
			};
		}

		public static List<PropData> ToCombinePropData(this List<PropData> list)
		{
			Dictionary<long, PropData> dictionary = new Dictionary<long, PropData>();
			List<PropData> list2 = new List<PropData>();
			for (int i = 0; i < list.Count; i++)
			{
				long num = (long)((ulong)list[i].id * 100UL + (ulong)list[i].level);
				if (dictionary.ContainsKey(num))
				{
					dictionary[num].count += list[i].count;
				}
				else
				{
					PropData propData = list[i];
					PropData propData2 = new PropData();
					propData2.count = propData.count;
					propData2.id = propData.id;
					propData2.rowId = propData.rowId;
					propData2.level = propData.level;
					propData2.exp = propData.exp;
					dictionary.Add(num, propData2);
					list2.Add(propData2);
				}
			}
			return list2;
		}

		public static List<ItemData> ToCombineList(this List<ItemData> list)
		{
			Dictionary<long, ItemData> dictionary = new Dictionary<long, ItemData>();
			List<ItemData> list2 = new List<ItemData>();
			for (int i = 0; i < list.Count; i++)
			{
				ItemData itemData = list[i];
				long num = (long)itemData.ID;
				ItemData itemData2;
				if (dictionary.TryGetValue(num, out itemData2))
				{
					itemData2.Combine(itemData);
				}
				else
				{
					ItemData itemData3 = itemData.Copy();
					list2.Add(itemData3);
					dictionary.Add(num, itemData3);
				}
			}
			return list2;
		}

		public static PropData ToPropData(this RewardDto reward)
		{
			return new PropData
			{
				id = reward.ConfigId,
				count = reward.Count
			};
		}

		public static ItemData ToItemData(this RewardDto reward)
		{
			return new ItemData((int)reward.ConfigId, (long)((int)reward.Count));
		}

		public static List<ItemData> ToItemDatas(this List<RewardDto> rewards)
		{
			List<ItemData> list = new List<ItemData>(rewards.Count);
			for (int i = 0; i < rewards.Count; i++)
			{
				RewardDto rewardDto = rewards[i];
				list.Add(rewardDto.ToItemData());
			}
			return list;
		}

		public static List<ItemData> ToItemDatas(this RepeatedField<RewardDto> rewards)
		{
			List<ItemData> list = new List<ItemData>(rewards.Count);
			for (int i = 0; i < rewards.Count; i++)
			{
				RewardDto rewardDto = rewards[i];
				list.Add(rewardDto.ToItemData());
			}
			return list;
		}

		public static StringBuilder ToStringBuilder(this List<ItemData> itemDatas)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < itemDatas.Count; i++)
			{
				ItemData itemData = itemDatas[i];
				stringBuilder.Append(itemData.ToStringItem());
				if (i < itemDatas.Count - 1)
				{
					stringBuilder.Append("|");
				}
			}
			return stringBuilder;
		}

		public static StringBuilder ToStringBuilder(this List<EquipData> equipDatas)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < equipDatas.Count; i++)
			{
				EquipData equipData = equipDatas[i];
				stringBuilder.Append(string.Format("{0},{1}", equipData.id, equipData.count));
				if (i < equipDatas.Count - 1)
				{
					stringBuilder.Append("|");
				}
			}
			return stringBuilder;
		}

		public static StringBuilder ToStringBuilder(this Dictionary<uint, uint> itemData)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			foreach (KeyValuePair<uint, uint> keyValuePair in itemData)
			{
				stringBuilder.Append(string.Format("{0},{1}", keyValuePair.Key, keyValuePair.Value));
				if (num != itemData.Count - 1)
				{
					stringBuilder.Append("|");
				}
				num++;
			}
			return stringBuilder;
		}

		public static StringBuilder ToStringBuilder(this List<ulong> list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.Append(list[i]);
				if (i != list.Count - 1)
				{
					stringBuilder.Append(",");
				}
			}
			return stringBuilder;
		}

		public static StringBuilder ToStringBuilder(this List<int> list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.Append(list[i]);
				if (i != list.Count - 1)
				{
					stringBuilder.Append(",");
				}
			}
			return stringBuilder;
		}

		public static StringBuilder ToStringBuilder(this List<string> list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < list.Count; i++)
			{
				stringBuilder.Append(list[i]);
				if (i != list.Count - 1)
				{
					stringBuilder.Append(",");
				}
			}
			return stringBuilder;
		}

		public static StringBuilder ToStringBuilder(this string[] list)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < list.Length; i++)
			{
				stringBuilder.Append(list[i]);
				if (i != list.Length - 1)
				{
					stringBuilder.Append(",");
				}
			}
			return stringBuilder;
		}

		public static StringBuilder ToStringBuilder(this RepeatedField<RewardDto> Reward)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < Reward.Count; i++)
			{
				stringBuilder.Append(string.Format("{0},{1}", Reward[i].ConfigId, Reward[i].Count));
				if (i != Reward.Count - 1)
				{
					stringBuilder.Append("|");
				}
			}
			return stringBuilder;
		}

		public static StringBuilder ToStringBuilder(this RepeatedField<HeroDto> Heros)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < Heros.Count; i++)
			{
				stringBuilder.Append(Heros[i].HeroId);
				if (i != Heros.Count - 1)
				{
					stringBuilder.Append(",");
				}
			}
			return stringBuilder;
		}

		public static StringBuilder ToStringBuilder(this RepeatedField<PetDto> pets)
		{
			List<PetDto> list = new List<PetDto>();
			for (int i = 0; i < pets.Count; i++)
			{
				if (pets[i].PetType != 2U)
				{
					list.Add(pets[i]);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int j = 0; j < list.Count; j++)
			{
				PetDto petDto = list[j];
				if (petDto.PetType != 2U)
				{
					stringBuilder.Append(string.Format("{0},{1},{2}", petDto.RowId, petDto.ConfigId, petDto.PetLv));
					if (j != list.Count - 1)
					{
						stringBuilder.Append("|");
					}
				}
			}
			return stringBuilder;
		}
	}
}
