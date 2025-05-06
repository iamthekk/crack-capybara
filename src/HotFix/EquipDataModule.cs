using System;
using System.Collections.Generic;
using System.Linq;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.Equip;
using Server;

namespace HotFix
{
	public class EquipDataModule : IDataModule
	{
		public EquipDataModule()
		{
			this.m_equipDressRowIds.Clear();
			for (int i = 0; i < 6; i++)
			{
				this.m_equipDressRowIds.Add(0UL);
			}
		}

		public int GetName()
		{
			return 119;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_EquipDataModule_SetEquipData, new HandlerEvent(this.OnEventSetEquipData));
			manager.RegisterEvent(LocalMessageName.CC_EquipDataModule_RefreshEquipDressRowIds, new HandlerEvent(this.OnEventRefreshEquipDressRowIds));
			manager.RegisterEvent(LocalMessageName.CC_EquipDataModule_UpdateEquipDatas, new HandlerEvent(this.OnEventUpdateEquipDatas));
			manager.RegisterEvent(LocalMessageName.CC_EquipDataModule_RemoveEquipDatas, new HandlerEvent(this.OnEventRemoveEquipDatas));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_EquipDataModule_SetEquipData, new HandlerEvent(this.OnEventSetEquipData));
			manager.UnRegisterEvent(LocalMessageName.CC_EquipDataModule_RefreshEquipDressRowIds, new HandlerEvent(this.OnEventRefreshEquipDressRowIds));
			manager.UnRegisterEvent(LocalMessageName.CC_EquipDataModule_UpdateEquipDatas, new HandlerEvent(this.OnEventUpdateEquipDatas));
			manager.UnRegisterEvent(LocalMessageName.CC_EquipDataModule_RemoveEquipDatas, new HandlerEvent(this.OnEventRemoveEquipDatas));
		}

		public void Reset()
		{
			this.m_equipDatas.Clear();
			this.m_addAttributeData.Clear();
			this.m_equipDressRowIds.Clear();
			for (int i = 0; i < 6; i++)
			{
				this.m_equipDressRowIds.Add(0UL);
			}
		}

		private void UpdateEquipDressRowIds(List<ulong> rowIds)
		{
			for (int i = 0; i < rowIds.Count; i++)
			{
				this.m_equipDressRowIds[i] = 0UL;
			}
			for (int j = 0; j < rowIds.Count; j++)
			{
				if (rowIds[j] == 0UL && j < this.m_equipDressRowIds.Count)
				{
					this.m_equipDressRowIds[j] = ulong.MaxValue;
				}
			}
			for (int k = 0; k < rowIds.Count; k++)
			{
				ulong num = rowIds[k];
				if (num > 0UL)
				{
					EquipData equipDataByRowId = this.GetEquipDataByRowId(num);
					if (equipDataByRowId != null)
					{
						EquipType equipType = equipDataByRowId.equipType;
						int equipTypeStartIndex = EquipTypeHelper.GetEquipTypeStartIndex(equipType);
						List<ulong> equipDressRowIds = this.GetEquipDressRowIds(equipType);
						int num2 = equipDressRowIds.IndexOf(0UL);
						if (num2 >= 0)
						{
							this.m_equipDressRowIds[equipTypeStartIndex + num2] = num;
						}
						else
						{
							num2 = equipDressRowIds.IndexOf(ulong.MaxValue);
							if (num2 >= 0)
							{
								this.m_equipDressRowIds[equipTypeStartIndex + num2] = num;
							}
							else
							{
								this.m_equipDressRowIds[equipTypeStartIndex] = num;
							}
						}
					}
				}
			}
			for (int l = 0; l < this.m_equipDressRowIds.Count; l++)
			{
				if (this.m_equipDressRowIds[l] == 18446744073709551615UL)
				{
					this.m_equipDressRowIds[l] = 0UL;
				}
			}
		}

		private void OnEventSetEquipData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsSetEquipData eventArgsSetEquipData = eventargs as EventArgsSetEquipData;
			if (eventArgsSetEquipData == null)
			{
				return;
			}
			for (int i = 0; i < eventArgsSetEquipData.m_userLoginResponse.Equipments.Count; i++)
			{
				EquipmentDto equipmentDto = eventArgsSetEquipData.m_userLoginResponse.Equipments[i];
				EquipData equipData = equipmentDto.ToEquipData();
				equipData.SetEquipData(equipmentDto);
				this.m_equipDatas[equipData.rowID] = equipData;
			}
			this.UpdateEquipDressRowIds(eventArgsSetEquipData.m_userLoginResponse.WearEquipRowIds.ToList<ulong>());
			this.MathAddAttributeData();
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Equipment);
		}

		private void OnEventRefreshEquipDressRowIds(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsRefreshEquipDressRowIds eventArgsRefreshEquipDressRowIds = eventargs as EventArgsRefreshEquipDressRowIds;
			if (eventArgsRefreshEquipDressRowIds == null)
			{
				return;
			}
			this.UpdateEquipDressRowIds(eventArgsRefreshEquipDressRowIds.m_datas);
			this.MathAddAttributeData();
		}

		private void OnEventUpdateEquipDatas(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsUpdateEquipDatas eventArgsUpdateEquipDatas = eventargs as EventArgsUpdateEquipDatas;
			if (eventArgsUpdateEquipDatas == null)
			{
				return;
			}
			for (int i = 0; i < eventArgsUpdateEquipDatas.m_datas.Count; i++)
			{
				EquipmentDto equipmentDto = eventArgsUpdateEquipDatas.m_datas[i];
				if (equipmentDto != null)
				{
					EquipData equipData;
					this.m_equipDatas.TryGetValue(equipmentDto.RowId, out equipData);
					if (equipData == null)
					{
						equipData = equipmentDto.ToEquipData();
					}
					this.m_equipDressRowIds.Contains(equipmentDto.RowId);
					equipData.SetEquipData(equipmentDto);
					this.m_equipDatas[equipmentDto.RowId] = equipData;
				}
			}
			this.MathAddAttributeData();
			GameApp.SDK.Analyze.UserSet(GameTGAUserSetType.Equipment);
		}

		private void OnEventRemoveEquipDatas(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsRemoveEquipDatas eventArgsRemoveEquipDatas = eventargs as EventArgsRemoveEquipDatas;
			if (eventArgsRemoveEquipDatas == null || eventArgsRemoveEquipDatas.m_datas == null)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < eventArgsRemoveEquipDatas.m_datas.Count; i++)
			{
				long num = eventArgsRemoveEquipDatas.m_datas[i];
				if (this.m_equipDressRowIds.Contains((ulong)num))
				{
					flag = true;
				}
				this.m_equipDatas.Remove((ulong)num);
			}
			this.MathAddAttributeData();
			if (flag)
			{
				GameApp.Event.DispatchNow(null, 145, null);
			}
		}

		public int GetNextEquipTableIDForQuality(int equipID)
		{
			return equipID + 1;
		}

		public List<ulong> ReplaceEquipDressRowIds(List<ulong> rowIds, EquipType equipType, ulong targetRowID, ulong originRowId)
		{
			List<ulong> list = new List<ulong>(rowIds);
			if (targetRowID <= 0UL)
			{
				int num = list.IndexOf(originRowId);
				if (num >= 0)
				{
					list[num] = targetRowID;
				}
			}
			else
			{
				ulong num2 = 0UL;
				int equipTypeStartIndex = EquipTypeHelper.GetEquipTypeStartIndex(equipType);
				List<ulong> equipDressRowIds = this.GetEquipDressRowIds(equipType);
				int num3 = equipDressRowIds.IndexOf(num2);
				if (num3 < 0)
				{
					int num4 = int.MaxValue;
					int num5 = int.MaxValue;
					for (int i = 0; i < equipDressRowIds.Count; i++)
					{
						EquipData equipDataByRowId = this.GetEquipDataByRowId(equipDressRowIds[i]);
						if (equipDataByRowId.qualityColor < num4)
						{
							num3 = i;
							num4 = equipDataByRowId.qualityColor;
							num5 = equipDataByRowId.rank;
						}
						else if (equipDataByRowId.qualityColor == num4 && equipDataByRowId.rank < num5)
						{
							num3 = i;
							num4 = equipDataByRowId.qualityColor;
							num5 = equipDataByRowId.rank;
						}
					}
				}
				if (num3 < 0)
				{
					num3 = 0;
				}
				int num6 = equipTypeStartIndex + num3;
				if (num6 < list.Count)
				{
					list[num6] = targetRowID;
				}
			}
			return list;
		}

		public EquipData GetEquipDataByRowId(ulong rowId)
		{
			EquipData equipData;
			if (this.m_equipDatas.TryGetValue(rowId, out equipData))
			{
				return equipData;
			}
			return null;
		}

		public List<EquipData> GetEquipDatasByRowIds(List<ulong> rowIDs)
		{
			List<EquipData> list = new List<EquipData>(rowIDs.Count);
			for (int i = 0; i < rowIDs.Count; i++)
			{
				ulong num = rowIDs[i];
				EquipData equipData;
				this.m_equipDatas.TryGetValue(num, out equipData);
				if (equipData != null)
				{
					list.Add(equipData);
				}
			}
			return list;
		}

		public bool IsPutOn(ulong rowId)
		{
			return this.m_equipDressRowIds.Contains(rowId);
		}

		public int GetWeaponId()
		{
			ulong equipDressRowId = this.GetEquipDressRowId(EquipType.Weapon, 0);
			if (equipDressRowId == 0UL)
			{
				return 0;
			}
			EquipData equipDataByRowId = this.GetEquipDataByRowId(equipDressRowId);
			if (equipDataByRowId != null)
			{
				return (int)equipDataByRowId.id;
			}
			return 0;
		}

		public ulong GetEquipDressRowId(EquipType equipType, int index = 0)
		{
			ulong num = 0UL;
			int num2 = EquipTypeHelper.GetEquipTypeStartIndex(equipType) + index;
			if (num2 < this.m_equipDressRowIds.Count)
			{
				num = this.m_equipDressRowIds[num2];
			}
			return num;
		}

		public List<ulong> GetEquipDressRowIds(EquipType equipType)
		{
			List<ulong> list = new List<ulong>();
			int equipTypeStartIndex = EquipTypeHelper.GetEquipTypeStartIndex(equipType);
			int equipTypeMaxCount = EquipTypeHelper.GetEquipTypeMaxCount(equipType);
			for (int i = 0; i < equipTypeMaxCount; i++)
			{
				int num = equipTypeStartIndex + i;
				if (num < this.m_equipDressRowIds.Count)
				{
					list.Add(this.m_equipDressRowIds[num]);
				}
			}
			return list;
		}

		public bool NeedEquipRedTip(EquipType equipType, int composeId)
		{
			List<ulong> equipDressRowIds = this.GetEquipDressRowIds(equipType);
			if (equipDressRowIds.IndexOf(0UL) >= 0)
			{
				return true;
			}
			for (int i = 0; i < equipDressRowIds.Count; i++)
			{
				ulong num = equipDressRowIds[i];
				if (num > 0UL)
				{
					EquipData equipDataByRowId = this.GetEquipDataByRowId(num);
					if (equipDataByRowId != null && equipDataByRowId.composeId < composeId)
					{
						return true;
					}
				}
			}
			return false;
		}

		public List<EquipData> GetEquipDatas(bool containsHero = false, bool containsLegend = false)
		{
			List<EquipData> list = new List<EquipData>();
			foreach (KeyValuePair<ulong, EquipData> keyValuePair in this.m_equipDatas)
			{
				if (keyValuePair.Value != null && (containsLegend || keyValuePair.Value.composeId < Singleton<GameConfig>.Instance.Equip_MaxComposeId))
				{
					if (this.m_equipDressRowIds.Contains(keyValuePair.Key))
					{
						if (containsHero)
						{
							list.Add(keyValuePair.Value);
						}
					}
					else
					{
						list.Add(keyValuePair.Value);
					}
				}
			}
			return list;
		}

		public List<EquipData> SortEquipDatas(List<EquipData> equipDatas, EquipSortType sortType)
		{
			IOrderedEnumerable<EquipData> orderedEnumerable = null;
			switch (sortType)
			{
			case EquipSortType.Quality:
				orderedEnumerable = from equip in equipDatas
					orderby equip.composeId descending, equip.equipType, equip.level descending, equip.id
					select equip;
				break;
			case EquipSortType.Pos:
				orderedEnumerable = from equip in equipDatas
					orderby equip.equipType, equip.composeId descending, equip.level descending, equip.id
					select equip;
				break;
			case EquipSortType.Level:
				orderedEnumerable = from equip in equipDatas
					orderby equip.level descending, equip.composeId descending, equip.equipType, equip.id
					select equip;
				break;
			}
			return orderedEnumerable.ToList<EquipData>();
		}

		public List<EquipData> SelectEquipDatas(List<EquipData> equipDatas, int equipType = 0)
		{
			List<EquipData> list = new List<EquipData>(equipDatas.Count);
			EquipType equipType2 = (EquipType)equipType;
			int num;
			if (int.TryParse(equipType2.ToString(), out num))
			{
				return equipDatas;
			}
			for (int i = 0; i < equipDatas.Count; i++)
			{
				EquipData equipData = equipDatas[i];
				if (equipData != null && equipData.equipType == (EquipType)equipType)
				{
					list.Add(equipData);
				}
			}
			return list;
		}

		public List<EquipData> SelectEquipDatasForUnNextQuality(List<EquipData> equipDatas)
		{
			List<EquipData> list = new List<EquipData>(equipDatas.Count);
			for (int i = 0; i < equipDatas.Count; i++)
			{
				EquipData equipData = equipDatas[i];
				if (equipData != null && GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(equipData.composeId) != null)
				{
					list.Add(equipData);
				}
			}
			return list;
		}

		public List<EquipData> GetEquipDatasForMerge(EquipData selectEquipData)
		{
			List<EquipData> list = new List<EquipData>();
			if (selectEquipData == null)
			{
				return list;
			}
			if (GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(selectEquipData.composeId) == null)
			{
				return list;
			}
			list = this.GetEquipDatas(true, false);
			List<EquipData> list2 = new List<EquipData>(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				EquipData equipData = list[i];
				if (equipData != null && this.IsCanToMerge(selectEquipData, equipData, false))
				{
					list2.Add(equipData);
				}
			}
			list = list2;
			return (from equip in list
				orderby equip.rowID == selectEquipData.rowID descending, this.IsCanToMerge(selectEquipData, equip, true) descending, equip.composeId descending, equip.equipType, equip.level descending, equip.id
				select equip).ToList<EquipData>();
		}

		public List<EquipData> GetEquipDatasForMerge(int equipType)
		{
			List<EquipData> list = new List<EquipData>();
			list = this.GetEquipDatas(true, true);
			list = this.SelectEquipDatas(list, equipType);
			list = this.SelectEquipDatasForUnNextQuality(list);
			return this.SortEquipDatas(list, EquipSortType.Quality);
		}

		public List<EquipData> GetEquipDatasFoSelector(int equipType)
		{
			List<EquipData> list = new List<EquipData>();
			list = this.GetEquipDatas(false, true);
			list = this.SelectEquipDatas(list, equipType);
			list = this.SortEquipDatas(list, EquipSortType.Quality);
			for (int i = 0; i < list.Count; i++)
			{
				EquipData equipData = list[i];
			}
			return list;
		}

		public int GetMergeCount(int quality)
		{
			Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(quality);
			if (elementById == null)
			{
				return 0;
			}
			return elementById.composeNeed3;
		}

		public int GetCommonMaterialId(EquipData equipData)
		{
			if (equipData == null)
			{
				return 0;
			}
			Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(equipData.composeId);
			if (elementById == null)
			{
				return 0;
			}
			if (elementById.composeNeed1 == 1)
			{
				switch (equipData.equipType)
				{
				case EquipType.Weapon:
					return elementById.composeItem1;
				case EquipType.Clothes:
					return elementById.composeItem2;
				case EquipType.Ring:
					return elementById.composeItem3;
				case EquipType.Accessory:
					return elementById.composeItem4;
				}
			}
			return 0;
		}

		public bool IsCanMerge(ulong equipRowId)
		{
			EquipData equipDataByRowId = this.GetEquipDataByRowId(equipRowId);
			if (equipDataByRowId == null)
			{
				return false;
			}
			Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(equipDataByRowId.composeId);
			if (elementById == null)
			{
				return false;
			}
			if (elementById.composeTo <= 0)
			{
				return false;
			}
			int composeNeed = elementById.composeNeed1;
			int composeNeed2 = elementById.composeNeed2;
			int composeNeed3 = elementById.composeNeed3;
			long num = 0L;
			if (composeNeed == 1)
			{
				int commonMaterialId = this.GetCommonMaterialId(equipDataByRowId);
				PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
				num += dataModule.GetItemDataCountByid((ulong)((long)commonMaterialId));
			}
			foreach (EquipData equipData in this.m_equipDatas.Values)
			{
				if (equipData.rowID != equipRowId && !this.IsPutOn(equipData.rowID))
				{
					if (composeNeed == 0)
					{
						if (equipData.composeId == composeNeed2)
						{
							num += 1L;
							if (num >= (long)composeNeed3)
							{
								break;
							}
						}
					}
					else if (composeNeed == 1)
					{
						if (equipData.equipType == equipDataByRowId.equipType && equipData.composeId == composeNeed2)
						{
							num += 1L;
							if (num >= (long)composeNeed3)
							{
								break;
							}
						}
					}
					else if (composeNeed == 2)
					{
						if (equipData.id == equipDataByRowId.id)
						{
							num += 1L;
							if (num >= (long)composeNeed3)
							{
								break;
							}
						}
					}
					else if (composeNeed == 3 && equipData.tagId == equipDataByRowId.tagId && equipData.composeId == composeNeed2)
					{
						num += 1L;
						if (num >= (long)composeNeed3)
						{
							break;
						}
					}
				}
			}
			return num >= (long)composeNeed3;
		}

		public bool IsCanToMerge(EquipData current, PropData target)
		{
			if (current == null || target == null)
			{
				return false;
			}
			Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(current.composeId);
			if (elementById == null)
			{
				return false;
			}
			bool flag = false;
			if (elementById.composeNeed1 == 1)
			{
				int commonMaterialId = this.GetCommonMaterialId(current);
				if ((ulong)target.id == (ulong)((long)commonMaterialId))
				{
					flag = true;
				}
			}
			return flag;
		}

		public bool IsCanToMerge(EquipData current, EquipData target, bool isCheckQuality = true)
		{
			if (current == null || target == null)
			{
				return false;
			}
			Equip_equipCompose elementById = GameApp.Table.GetManager().GetEquip_equipComposeModelInstance().GetElementById(current.composeId);
			if (elementById == null)
			{
				return false;
			}
			if (isCheckQuality && target.composeId != elementById.composeNeed2)
			{
				return false;
			}
			bool flag = false;
			switch (elementById.composeNeed1)
			{
			case 0:
				flag = true;
				break;
			case 1:
				flag = target.equipType == current.equipType;
				break;
			case 2:
				flag = target.id == current.id;
				break;
			case 3:
				flag = target.tagId == current.tagId;
				break;
			}
			return flag;
		}

		public List<EquipComposeData> GetOneClickEquipComposeDatas()
		{
			List<EquipComposeData> list = new List<EquipComposeData>();
			Dictionary<ulong, EquipData> dictionary = new Dictionary<ulong, EquipData>(this.m_equipDatas.Count);
			foreach (KeyValuePair<ulong, EquipData> keyValuePair in this.m_equipDatas)
			{
				if (keyValuePair.Value != null && keyValuePair.Value.composeId < Singleton<GameConfig>.Instance.Equip_OneClickMergeMaxComposeId)
				{
					EquipData equipData = new EquipData();
					equipData.CloneFrom(keyValuePair.Value);
					dictionary[keyValuePair.Key] = equipData;
				}
			}
			if (dictionary.Count == 0)
			{
				return list;
			}
			for (;;)
			{
				List<EquipComposeData> list2 = this.CheckOneClickEquipComposeDatas(dictionary);
				if (list2.Count == 0)
				{
					break;
				}
				list.AddRange(list2);
			}
			return list;
		}

		private List<EquipComposeData> CheckOneClickEquipComposeDatas(Dictionary<ulong, EquipData> equipDataDic)
		{
			Dictionary<ulong, EquipData> dictionary = new Dictionary<ulong, EquipData>();
			List<EquipComposeData> list = new List<EquipComposeData>();
			List<EquipData> list2 = equipDataDic.Values.ToList<EquipData>();
			list2 = this.SortEquipDatas(list2, EquipSortType.Quality);
			list2.Reverse();
			for (int i = 0; i < list2.Count; i++)
			{
				EquipData equipData = list2[i];
				if (equipData.composeId < Singleton<GameConfig>.Instance.Equip_OneClickMergeMaxComposeId && !dictionary.ContainsKey(equipData.rowID))
				{
					int mergeCount = this.GetMergeCount(equipData.composeId);
					bool flag = false;
					List<ulong> list3 = new List<ulong>();
					for (int j = 0; j < list2.Count; j++)
					{
						EquipData equipData2 = list2[j];
						if (equipData.rowID != equipData2.rowID && !this.IsPutOn(equipData2.rowID) && equipData2.composeId < Singleton<GameConfig>.Instance.Equip_OneClickMergeMaxComposeId && !dictionary.ContainsKey(equipData2.rowID) && this.IsCanToMerge(equipData, equipData2, true))
						{
							list3.Add(equipData2.rowID);
							if (list3.Count >= mergeCount)
							{
								flag = true;
								dictionary.TryAdd(equipData.rowID, equipData);
								if (equipDataDic.ContainsKey(equipData.rowID))
								{
									equipDataDic.Remove(equipData.rowID);
								}
								for (int k = 0; k < list3.Count; k++)
								{
									dictionary.TryAdd(list3[k], equipDataDic[list3[k]]);
									if (equipDataDic.ContainsKey(list3[k]))
									{
										equipDataDic.Remove(list3[k]);
									}
								}
								break;
							}
						}
					}
					if (flag)
					{
						equipData.composeId++;
						EquipComposeData equipComposeData = new EquipComposeData();
						equipComposeData.MainRowId = equipData.rowID;
						equipComposeData.RowIds.AddRange(list3);
						list.Add(equipComposeData);
					}
				}
			}
			return list;
		}

		public bool IsCanOneClickEquipCompose()
		{
			Dictionary<ulong, EquipData> dictionary = new Dictionary<ulong, EquipData>(this.m_equipDatas.Count);
			foreach (KeyValuePair<ulong, EquipData> keyValuePair in this.m_equipDatas)
			{
				if (keyValuePair.Value != null && keyValuePair.Value.composeId < Singleton<GameConfig>.Instance.Equip_OneClickMergeMaxComposeId)
				{
					dictionary[keyValuePair.Key] = keyValuePair.Value;
				}
			}
			return dictionary.Count != 0 && this.CheckOneClickEquipComposeData(dictionary) != null;
		}

		private EquipComposeData CheckOneClickEquipComposeData(Dictionary<ulong, EquipData> equipDataDic)
		{
			EquipComposeData equipComposeData = null;
			Dictionary<ulong, EquipData> dictionary = new Dictionary<ulong, EquipData>();
			new Dictionary<ulong, long>();
			List<EquipData> list = equipDataDic.Values.ToList<EquipData>();
			list = this.SortEquipDatas(list, EquipSortType.Quality);
			list.Reverse();
			for (int i = 0; i < list.Count; i++)
			{
				EquipData equipData = list[i];
				if (equipData.composeId < Singleton<GameConfig>.Instance.Equip_OneClickMergeMaxComposeId && !dictionary.ContainsKey(equipData.rowID))
				{
					int mergeCount = this.GetMergeCount(equipData.composeId);
					bool flag = false;
					List<ulong> list2 = new List<ulong>();
					for (int j = 0; j < list.Count; j++)
					{
						EquipData equipData2 = list[j];
						if (equipData.rowID != equipData2.rowID && !this.IsPutOn(equipData2.rowID) && equipData2.composeId < Singleton<GameConfig>.Instance.Equip_OneClickMergeMaxComposeId && !dictionary.ContainsKey(equipData2.rowID) && this.IsCanToMerge(equipData, equipData2, true))
						{
							list2.Add(equipData2.rowID);
							if (list2.Count >= mergeCount)
							{
								flag = true;
								dictionary.TryAdd(equipData.rowID, equipData);
								if (equipDataDic.ContainsKey(equipData.rowID))
								{
									equipDataDic.Remove(equipData.rowID);
								}
								for (int k = 0; k < list2.Count; k++)
								{
									dictionary.TryAdd(list2[k], equipDataDic[list2[k]]);
									if (equipDataDic.ContainsKey(list2[k]))
									{
										equipDataDic.Remove(list2[k]);
									}
								}
								break;
							}
						}
					}
					if (flag)
					{
						equipComposeData = new EquipComposeData();
						equipComposeData.MainRowId = equipData.rowID;
						equipComposeData.RowIds.AddRange(list2);
						break;
					}
				}
			}
			return equipComposeData;
		}

		public bool IsCanLevelUp(EquipData equipData)
		{
			bool flag = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)equipData.id)
				.HasNextLevelUpID((int)equipData.level, equipData.evolution);
			bool flag2 = equipData.IsFullLevel();
			return flag && !flag2;
		}

		public bool IsHaveLevelUpCost(EquipData equipData, out int costDataId)
		{
			costDataId = 0;
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			bool flag = true;
			List<ItemData> levelUpCosts = this.GetLevelUpCosts((int)equipData.id, (int)equipData.level);
			for (int i = 0; i < levelUpCosts.Count; i++)
			{
				ItemData itemData = levelUpCosts[i];
				if (itemData != null)
				{
					long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)((long)itemData.ID));
					if (itemData.TotalCount > itemDataCountByid)
					{
						costDataId = itemData.ID;
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		public bool IsMatchTalentLimit(EquipData equipData)
		{
			int evolutionId = equipData.GetEvolutionId();
			Equip_equipEvolution elementById = GameApp.Table.GetManager().GetEquip_equipEvolutionModelInstance().GetElementById(evolutionId);
			return GameApp.Data.GetDataModule(DataName.TalentDataModule).TalentStage >= elementById.talentLimit;
		}

		public bool IsHaveEvolutionCost(EquipData equipData)
		{
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			bool flag = true;
			List<ItemData> evolutionCosts = this.GetEvolutionCosts((int)equipData.id, equipData.evolution);
			for (int i = 0; i < evolutionCosts.Count; i++)
			{
				ItemData itemData = evolutionCosts[i];
				if (itemData != null)
				{
					long itemDataCountByid = dataModule.GetItemDataCountByid((ulong)((long)itemData.ID));
					if (itemData.TotalCount > itemDataCountByid)
					{
						flag = false;
						break;
					}
				}
			}
			return flag;
		}

		public int GetCanUpgradeEquipCount(EquipData data)
		{
			int num = 0;
			int evolutionId = data.GetEvolutionId();
			Equip_equipEvolution elementById = GameApp.Table.GetManager().GetEquip_equipEvolutionModelInstance().GetElementById(evolutionId);
			int level = (int)data.level;
			int maxLevel = elementById.maxLevel;
			if (level == maxLevel)
			{
				return 0;
			}
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			Dictionary<int, long> dictionary = new Dictionary<int, long>();
			Dictionary<int, long> dictionary2 = new Dictionary<int, long>();
			for (int i = level + 1; i <= maxLevel; i++)
			{
				List<ItemData> levelUpCosts = this.GetLevelUpCosts((int)data.id, i);
				for (int j = 0; j < levelUpCosts.Count; j++)
				{
					ItemData itemData = levelUpCosts[j];
					if (itemData != null)
					{
						long num2;
						dictionary.TryGetValue(itemData.ID, out num2);
						num2 += itemData.TotalCount;
						dictionary[itemData.ID] = num2;
						if (!dictionary2.ContainsKey(itemData.ID))
						{
							dictionary2[itemData.ID] = dataModule.GetItemDataCountByid((ulong)((long)itemData.ID));
						}
					}
				}
				bool flag = true;
				foreach (KeyValuePair<int, long> keyValuePair in dictionary)
				{
					if (keyValuePair.Value > dictionary2[keyValuePair.Key])
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				num++;
			}
			return num;
		}

		public List<ItemData> GetLevelUpCosts(int equipID, int level)
		{
			new List<ItemData>();
			return GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById(equipID)
				.GetLevelUpCosts(level);
		}

		public List<ItemData> GetEvolutionCosts(int equipID, int evolution)
		{
			new List<ItemData>();
			return GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById(equipID)
				.GetEvolutionCosts(evolution);
		}

		public List<EquipSkillInfo> GetAllSkillInfoList(int equipID)
		{
			List<EquipSkillInfo> list = new List<EquipSkillInfo>();
			int num = equipID / 100;
			Equip_equipSkill elementById = GameApp.Table.GetManager().GetEquip_equipSkillModelInstance().GetElementById(num);
			int[] qualitySkill = elementById.qualitySkill;
			int[] qualityUnlock = elementById.qualityUnlock;
			for (int i = 0; i < qualitySkill.Length; i++)
			{
				int num2 = qualitySkill[i];
				int num3 = qualityUnlock[i];
				if (num3 != 1)
				{
					list.Add(new EquipSkillInfo(num3, num2, i + 1));
				}
			}
			return list;
		}

		public List<EquipSkillInfo> GetSkillInfoList(int equipID, int quality)
		{
			List<EquipSkillInfo> list = new List<EquipSkillInfo>();
			int num = equipID / 100;
			Equip_equipSkill elementById = GameApp.Table.GetManager().GetEquip_equipSkillModelInstance().GetElementById(num);
			for (int i = 0; i < elementById.qualityUnlock.Length; i++)
			{
				int num2 = elementById.qualitySkill[i];
				int num3 = elementById.qualityUnlock[i];
				if (num3 != 1 && quality.Equals(num3))
				{
					list.Add(new EquipSkillInfo(quality, num2, i + 1));
				}
			}
			return list;
		}

		public List<EquipSkillInfo> GetUnLockSkillInfoList(int equipID, int quality)
		{
			List<EquipSkillInfo> list = new List<EquipSkillInfo>();
			int num = equipID / 100;
			Equip_equipSkill elementById = GameApp.Table.GetManager().GetEquip_equipSkillModelInstance().GetElementById(num);
			int[] qualitySkill = elementById.qualitySkill;
			int[] qualityUnlock = elementById.qualityUnlock;
			for (int i = 0; i < qualitySkill.Length; i++)
			{
				int num2 = qualitySkill[i];
				int num3 = qualityUnlock[i];
				if (quality >= num3)
				{
					list.Add(new EquipSkillInfo(num3, num2, i + 1));
				}
			}
			return list;
		}

		public EquipAction GetCurEquipAction(int equipID, int quality)
		{
			int num = equipID / 100;
			Equip_equipSkill elementById = GameApp.Table.GetManager().GetEquip_equipSkillModelInstance().GetElementById(num);
			int[] qualitySkill = elementById.qualitySkill;
			int[] qualityUnlock = elementById.qualityUnlock;
			for (int i = qualitySkill.Length - 1; i >= 0; i--)
			{
				int num2 = qualitySkill[i];
				int num3 = qualityUnlock[i];
				if (quality >= num3)
				{
					Equip_skill elementById2 = GameApp.Table.GetManager().GetEquip_skillModelInstance().GetElementById(num2);
					if (elementById2 == null)
					{
						HLog.LogError(string.Format("skillId:{0} is not exist, please check Equip.xls skill sheet", num2));
					}
					else
					{
						string action = elementById2.action;
						if (string.IsNullOrEmpty(action))
						{
							return new EquipAction();
						}
						return JsonManager.ToObject<EquipAction>(action);
					}
				}
			}
			return new EquipAction();
		}

		public bool IsHaveCanLevelUp()
		{
			bool flag = false;
			for (int i = 0; i < this.m_equipDressRowIds.Count; i++)
			{
				ulong num = this.m_equipDressRowIds[i];
				if (num != 0UL)
				{
					EquipData equipDataByRowId = this.GetEquipDataByRowId(num);
					int num2;
					if (equipDataByRowId != null && (this.IsCanLevelUp(equipDataByRowId) && this.IsHaveLevelUpCost(equipDataByRowId, out num2)))
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		public bool IsHaveCanEvolution()
		{
			bool flag = false;
			for (int i = 0; i < this.m_equipDressRowIds.Count; i++)
			{
				ulong num = this.m_equipDressRowIds[i];
				if (num != 0UL)
				{
					EquipData equipDataByRowId = this.GetEquipDataByRowId(num);
					if (equipDataByRowId != null && (equipDataByRowId.IsCanEvolution() && this.IsMatchTalentLimit(equipDataByRowId) && this.IsHaveEvolutionCost(equipDataByRowId)))
					{
						flag = true;
						break;
					}
				}
			}
			return flag;
		}

		private void MathAddAttributeData()
		{
			this.m_addAttributeData.Clear();
			RepeatedField<EquipmentDto> repeatedField = new RepeatedField<EquipmentDto>();
			for (int i = 0; i < this.m_equipDressRowIds.Count; i++)
			{
				ulong num = this.m_equipDressRowIds[i];
				if (num != 0UL)
				{
					EquipData equipData;
					this.m_equipDatas.TryGetValue(num, out equipData);
					if (equipData != null)
					{
						repeatedField.Add(equipData.ToEquipmentDto());
					}
				}
			}
			AddAttributeEquip addAttributeEquip = new AddAttributeEquip(GameApp.Table.GetManager());
			addAttributeEquip.SetData(repeatedField);
			this.m_addAttributeData.Merge(addAttributeEquip.MathAll());
		}

		public AddAttributeData MathAttributeData(EquipData equipData)
		{
			AddAttributeData addAttributeData = new AddAttributeData();
			if (equipData == null)
			{
				return addAttributeData;
			}
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			List<int> list2 = new List<int>();
			int level = (int)equipData.level;
			int evolution = equipData.evolution;
			int composeId = equipData.composeId;
			list = GameApp.Table.GetManager().GetEquip_equipModelInstance().GetElementById((int)equipData.id)
				.GetMergeAttributeData(level, evolution);
			EquipAction curEquipAction = this.GetCurEquipAction((int)equipData.id, composeId);
			List<MergeAttributeData> mergeAttributeData = curEquipAction.GetMergeAttributeData();
			if (mergeAttributeData != null && mergeAttributeData.Count > 0)
			{
				list.AddRange(mergeAttributeData);
			}
			if (curEquipAction.rageSkill > 0)
			{
				list2.Add(curEquipAction.rageSkill);
			}
			if (curEquipAction.skillIds != null && curEquipAction.skillIds.Length != 0)
			{
				list2.AddRange(curEquipAction.skillIds);
			}
			addAttributeData.m_attributeDatas = list.Merge();
			addAttributeData.m_skillIDs = list2;
			return addAttributeData;
		}

		public double MathCombatData(EquipData equipData)
		{
			CombatData combatData = new CombatData();
			AddAttributeData addAttributeData = this.MathAttributeData(equipData);
			List<int> skillIDs = addAttributeData.m_skillIDs;
			MemberAttributeData memberAttributeData = new MemberAttributeData();
			memberAttributeData.MergeAttributes(addAttributeData.m_attributeDatas, false);
			combatData.MathCombat(GameApp.Table.GetManager(), memberAttributeData, skillIDs);
			return combatData.CurComba;
		}

		public bool IsHaveIdleEquip()
		{
			foreach (ulong num in this.m_equipDatas.Keys)
			{
				if (!this.m_equipDressRowIds.Contains(num))
				{
					return true;
				}
			}
			return false;
		}

		public List<ulong> m_equipDressRowIds = new List<ulong>();

		public Dictionary<ulong, EquipData> m_equipDatas = new Dictionary<ulong, EquipData>();

		public AddAttributeData m_addAttributeData = new AddAttributeData();
	}
}
