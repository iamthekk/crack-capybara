using System;
using System.Collections.Generic;
using Framework;
using Framework.DataModule;
using Framework.EventSystem;
using Google.Protobuf.Collections;
using LocalModels.Bean;
using Proto.Common;
using Proto.User;
using Server;

namespace HotFix
{
	public class RelicDataModule : IDataModule
	{
		public int GetName()
		{
			return 121;
		}

		public void RegisterEvents(EventSystemManager manager)
		{
			manager.RegisterEvent(LocalMessageName.CC_GameLoginData_RelicsData, new HandlerEvent(this.OnEventSetRelicsData));
			manager.RegisterEvent(LocalMessageName.CC_GameLoginData_UpdateRelicsData, new HandlerEvent(this.OnEventUpdateRelicsData));
		}

		public void UnRegisterEvents(EventSystemManager manager)
		{
			manager.UnRegisterEvent(LocalMessageName.CC_GameLoginData_RelicsData, new HandlerEvent(this.OnEventSetRelicsData));
			manager.UnRegisterEvent(LocalMessageName.CC_GameLoginData_UpdateRelicsData, new HandlerEvent(this.OnEventUpdateRelicsData));
		}

		public void Reset()
		{
		}

		private void InitData(UserLoginResponse response)
		{
			Dictionary<int, RelicDto> dictionary = new Dictionary<int, RelicDto>();
			for (int i = 0; i < response.Relics.Count; i++)
			{
				RelicDto relicDto = response.Relics[i];
				if (relicDto != null)
				{
					dictionary[(int)relicDto.RelicId] = relicDto;
				}
			}
			this.m_relicDataDic.Clear();
			IList<Relic_group> allElements = GameApp.Table.GetManager().GetRelic_groupModelInstance().GetAllElements();
			for (int j = 0; j < allElements.Count; j++)
			{
				Relic_group relic_group = allElements[j];
				if (relic_group != null)
				{
					for (int k = 0; k < relic_group.Content.Length; k++)
					{
						int num = relic_group.Content[k];
						if (num > 0 && GameApp.Table.GetManager().GetRelic_relicModelInstance().GetElementById(num) != null)
						{
							RelicData relicData = new RelicData(num, 0, 0);
							RelicDto relicDto2;
							dictionary.TryGetValue(num, out relicDto2);
							if (relicDto2 != null)
							{
								relicData.m_level = (int)relicDto2.Level;
								relicData.m_quality = (int)relicDto2.Star;
								relicData.m_active = true;
							}
							this.m_relicDataDic[num] = relicData;
						}
					}
				}
			}
			this.m_permissionDic.Clear();
			Dictionary<int, RelicDataModule.PermissionsData> dictionary2 = this.MathAllPermissionsDatas();
			IList<Relic_data> allElements2 = GameApp.Table.GetManager().GetRelic_dataModelInstance().GetAllElements();
			for (int l = 0; l < allElements2.Count; l++)
			{
				Relic_data relic_data = allElements2[l];
				if (relic_data != null)
				{
					RelicDataModule.PermissionsData permissionsData;
					dictionary2.TryGetValue(relic_data.id, out permissionsData);
					RelicDataModule.PermissionsData permissionsData2 = new RelicDataModule.PermissionsData();
					permissionsData2.SetTable(relic_data, "0");
					if (permissionsData != null)
					{
						permissionsData2.Copy(permissionsData);
					}
					this.m_permissionDic[relic_data.id] = permissionsData2;
				}
			}
			this.MathAddAttributeData();
		}

		private void UpdateData(RepeatedField<RelicDto> updateDatas)
		{
			for (int i = 0; i < updateDatas.Count; i++)
			{
				RelicDto relicDto = updateDatas[i];
				if (relicDto != null)
				{
					int relicId = (int)relicDto.RelicId;
					RelicData relicData;
					this.m_relicDataDic.TryGetValue(relicId, out relicData);
					if (relicData == null)
					{
						relicData = new RelicData(relicId, 0, 0);
					}
					relicData.m_level = (int)relicDto.Level;
					relicData.m_quality = (int)relicDto.Star;
					relicData.m_active = true;
					this.m_relicDataDic[relicId] = relicData;
				}
			}
			Dictionary<int, RelicDataModule.PermissionsData> dictionary = this.MathAllPermissionsDatas();
			foreach (KeyValuePair<int, RelicDataModule.PermissionsData> keyValuePair in this.m_permissionDic)
			{
				if (keyValuePair.Value != null)
				{
					RelicDataModule.PermissionsData permissionsData;
					dictionary.TryGetValue(keyValuePair.Key, out permissionsData);
					if (permissionsData != null)
					{
						keyValuePair.Value.Copy(permissionsData);
					}
				}
			}
			this.MathAddAttributeData();
		}

		public bool IsHaveAllRedPoint()
		{
			bool flag = false;
			foreach (KeyValuePair<int, RelicData> keyValuePair in this.m_relicDataDic)
			{
				if (this.IsShowRedPointObj(keyValuePair.Value))
				{
					flag = true;
					break;
				}
			}
			return flag;
		}

		public bool IsShowRedPointObj(RelicData relicData)
		{
			bool flag = false;
			if (relicData == null)
			{
				return flag;
			}
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			Relic_relic elementById = GameApp.Table.GetManager().GetRelic_relicModelInstance().GetElementById(relicData.m_id);
			if (elementById == null)
			{
				return flag;
			}
			if (elementById.type == 1)
			{
				if (!relicData.m_active)
				{
					if (dataModule.GetItemDataCountByid((ulong)((long)elementById.id)) > 0L)
					{
						return true;
					}
					if (dataModule.GetItemDataCountByid((ulong)((long)elementById.unlockCostID)) >= (long)elementById.unlockCostNumber)
					{
						return true;
					}
				}
				else
				{
					int num;
					if (this.IsCanUpdateLevelForLevel(relicData.m_id, relicData.m_level, out num) && this.IsHaveUpdateLevelCost(relicData))
					{
						return true;
					}
					int num2;
					int num3;
					if (this.IsCanUpdateStarForQuality(relicData.m_id, relicData.m_quality, out num2) && this.IsHaveUpdateStarCost(relicData) && this.IsCanUpdateStarForLevelClamp(relicData.m_id, relicData.m_level, relicData.m_quality, out num3))
					{
						return true;
					}
				}
			}
			else if (elementById.type == 2 && !relicData.m_active && dataModule.GetItemDataCountByid((ulong)((long)elementById.id)) > 0L)
			{
				return true;
			}
			return flag;
		}

		public RelicData GetRelicDataByID(int id)
		{
			RelicData relicData;
			this.m_relicDataDic.TryGetValue(id, out relicData);
			return relicData;
		}

		public List<RelicData> GetRelicDatasByGroup(int groupID)
		{
			Relic_group elementById = GameApp.Table.GetManager().GetRelic_groupModelInstance().GetElementById(groupID);
			List<RelicData> list = new List<RelicData>();
			for (int i = 0; i < elementById.Content.Length; i++)
			{
				int num = elementById.Content[i];
				if (num > 0)
				{
					RelicData relicDataByID = this.GetRelicDataByID(num);
					if (relicDataByID != null)
					{
						list.Add(relicDataByID);
					}
				}
			}
			return list;
		}

		public int GetActiveRelicCountByGroup(int groupID)
		{
			Relic_group elementById = GameApp.Table.GetManager().GetRelic_groupModelInstance().GetElementById(groupID);
			int num = 0;
			for (int i = 0; i < elementById.Content.Length; i++)
			{
				int num2 = elementById.Content[i];
				if (num2 > 0)
				{
					RelicData relicDataByID = this.GetRelicDataByID(num2);
					if (relicDataByID != null && relicDataByID.m_active)
					{
						num++;
					}
				}
			}
			return num;
		}

		public int GetGroupMinQuality(int groupID, out bool isAllActive)
		{
			Relic_group elementById = GameApp.Table.GetManager().GetRelic_groupModelInstance().GetElementById(groupID);
			int num = 100;
			isAllActive = true;
			for (int i = 0; i < elementById.Content.Length; i++)
			{
				int num2 = elementById.Content[i];
				if (num2 > 0)
				{
					RelicData relicDataByID = this.GetRelicDataByID(num2);
					if (relicDataByID != null)
					{
						if (!relicDataByID.m_active)
						{
							num = 0;
							isAllActive = false;
							break;
						}
						if (relicDataByID.m_quality < num)
						{
							num = relicDataByID.m_quality;
						}
					}
				}
			}
			return num;
		}

		public Dictionary<int, List<MergeAttributeData>> GetGroupAttributesByGroup(int groupID)
		{
			Dictionary<int, List<MergeAttributeData>> dictionary = new Dictionary<int, List<MergeAttributeData>>();
			Relic_group elementById = GameApp.Table.GetManager().GetRelic_groupModelInstance().GetElementById(groupID);
			if (elementById == null)
			{
				return dictionary;
			}
			for (int i = 0; i < elementById.GroupAttributes.Length; i++)
			{
				string text = elementById.GroupAttributes[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					if (array.Length >= 2)
					{
						int num = int.Parse(array[0]);
						List<MergeAttributeData> list = new List<MergeAttributeData>();
						for (int j = 1; j < array.Length; j++)
						{
							string text2 = array[j];
							if (!string.IsNullOrEmpty(text2))
							{
								MergeAttributeData mergeAttributeData = new MergeAttributeData(text2, null, null);
								if (!string.IsNullOrEmpty(mergeAttributeData.Header))
								{
									list.Add(mergeAttributeData);
								}
							}
						}
						dictionary[num] = list;
					}
				}
			}
			return dictionary;
		}

		public List<MergeAttributeData> GetGroupAttributesByGroup(int groupID, int quality)
		{
			Dictionary<int, List<MergeAttributeData>> groupAttributesByGroup = this.GetGroupAttributesByGroup(groupID);
			if (groupAttributesByGroup == null)
			{
				return null;
			}
			List<MergeAttributeData> list;
			groupAttributesByGroup.TryGetValue(quality, out list);
			return list;
		}

		public int GetUpdataLevelID(int relicID, int level)
		{
			return GameApp.Table.GetManager().GetRelic_relicModelInstance().GetElementById(relicID)
				.AttributesType * 1000 + level;
		}

		public List<ItemData> GetUpdateLevelCost(int relicID, int level)
		{
			int updataLevelID = this.GetUpdataLevelID(relicID, level);
			Relic_updateLevel elementById = GameApp.Table.GetManager().GetRelic_updateLevelModelInstance().GetElementById(updataLevelID);
			if (elementById == null)
			{
				return null;
			}
			List<ItemData> list = new List<ItemData>();
			for (int i = 0; i < elementById.levelupCost.Length; i++)
			{
				string text = elementById.levelupCost[i];
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

		public bool IsHaveUpdateLevelCost(RelicData data)
		{
			if (data == null)
			{
				return false;
			}
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			bool flag = true;
			List<ItemData> updateLevelCost = this.GetUpdateLevelCost(data.m_id, data.m_level);
			for (int i = 0; i < updateLevelCost.Count; i++)
			{
				ItemData itemData = updateLevelCost[i];
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

		public bool IsCanUpdateLevelForLevel(int relicID, int level, out int nextLevel)
		{
			nextLevel = 0;
			if (level <= 0)
			{
				return false;
			}
			int updataLevelID = this.GetUpdataLevelID(relicID, level);
			Relic_updateLevel elementById = GameApp.Table.GetManager().GetRelic_updateLevelModelInstance().GetElementById(updataLevelID);
			if (elementById == null)
			{
				return false;
			}
			nextLevel = elementById.nextID;
			return elementById.nextID > 0;
		}

		public List<MergeAttributeData> GetUpdateLevelAttributes(int relicID, int level)
		{
			int updataLevelID = this.GetUpdataLevelID(relicID, level);
			return this.GetUpdateLevelAttributes(updataLevelID);
		}

		public List<MergeAttributeData> GetUpdateLevelAttributes(int updateLevelID)
		{
			Relic_updateLevel elementById = GameApp.Table.GetManager().GetRelic_updateLevelModelInstance().GetElementById(updateLevelID);
			if (elementById == null)
			{
				return null;
			}
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			for (int i = 0; i < elementById.Attributes.Length; i++)
			{
				string text = elementById.Attributes[i];
				if (!string.IsNullOrEmpty(text))
				{
					MergeAttributeData mergeAttributeData = new MergeAttributeData(text, null, null);
					if (!string.IsNullOrEmpty(mergeAttributeData.Header))
					{
						list.Add(mergeAttributeData);
					}
				}
			}
			return list;
		}

		public int GetUpdateStarID(int relicID, int quality)
		{
			return GameApp.Table.GetManager().GetRelic_relicModelInstance().GetElementById(relicID)
				.starUpType * 1000 + quality;
		}

		public List<ItemData> GetUpdateStarCost(int relicID, int quality)
		{
			Relic_relic elementById = GameApp.Table.GetManager().GetRelic_relicModelInstance().GetElementById(relicID);
			if (elementById == null)
			{
				return null;
			}
			if (elementById.type != 1)
			{
				return null;
			}
			int updateStarID = this.GetUpdateStarID(relicID, quality);
			Relic_starUp elementById2 = GameApp.Table.GetManager().GetRelic_starUpModelInstance().GetElementById(updateStarID);
			if (elementById2 == null)
			{
				return null;
			}
			ItemData itemData = new ItemData(elementById.unlockCostID, (long)elementById2.starUpCost);
			return new List<ItemData> { itemData };
		}

		public bool IsHaveUpdateStarCost(RelicData data)
		{
			if (data == null)
			{
				return false;
			}
			PropDataModule dataModule = GameApp.Data.GetDataModule(DataName.PropDataModule);
			bool flag = true;
			List<ItemData> updateStarCost = this.GetUpdateStarCost(data.m_id, data.m_quality);
			for (int i = 0; i < updateStarCost.Count; i++)
			{
				ItemData itemData = updateStarCost[i];
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

		public bool IsCanUpdateStarForQuality(int relicID, int quality, out int nextQuality)
		{
			nextQuality = 0;
			Relic_relic elementById = GameApp.Table.GetManager().GetRelic_relicModelInstance().GetElementById(relicID);
			if (elementById == null)
			{
				return false;
			}
			if (elementById.type != 1)
			{
				return false;
			}
			int updateStarID = this.GetUpdateStarID(relicID, quality);
			Relic_starUp elementById2 = GameApp.Table.GetManager().GetRelic_starUpModelInstance().GetElementById(updateStarID);
			if (elementById2 == null)
			{
				return false;
			}
			nextQuality = elementById2.nextID;
			return elementById2.nextID > 0;
		}

		public bool IsCanUpdateStarForLevelClamp(int relicID, int level, int quality, out int levelClamp)
		{
			levelClamp = 0;
			Relic_relic elementById = GameApp.Table.GetManager().GetRelic_relicModelInstance().GetElementById(relicID);
			if (elementById == null)
			{
				return false;
			}
			if (elementById.type != 1)
			{
				return false;
			}
			int updateStarID = this.GetUpdateStarID(relicID, quality);
			Relic_starUp elementById2 = GameApp.Table.GetManager().GetRelic_starUpModelInstance().GetElementById(updateStarID);
			if (elementById2 == null)
			{
				return false;
			}
			levelClamp = elementById2.requireLevel;
			return level >= elementById2.requireLevel;
		}

		public List<MergeAttributeData> GetUpdateStarAttributes(int relicID, int quality)
		{
			Relic_relic elementById = GameApp.Table.GetManager().GetRelic_relicModelInstance().GetElementById(relicID);
			if (elementById == null)
			{
				return null;
			}
			if (elementById.type != 1)
			{
				return null;
			}
			int updateStarID = this.GetUpdateStarID(relicID, quality);
			return this.GetUpdateStarAttributes(updateStarID);
		}

		public List<MergeAttributeData> GetUpdateStarAttributes(int starupID)
		{
			Relic_starUp elementById = GameApp.Table.GetManager().GetRelic_starUpModelInstance().GetElementById(starupID);
			if (elementById == null)
			{
				return null;
			}
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			for (int i = 0; i < elementById.starAttributes.Length; i++)
			{
				string text = elementById.starAttributes[i];
				if (!string.IsNullOrEmpty(text))
				{
					MergeAttributeData mergeAttributeData = new MergeAttributeData(text, null, null);
					if (!string.IsNullOrEmpty(mergeAttributeData.Header))
					{
						list.Add(mergeAttributeData);
					}
				}
			}
			return list;
		}

		public List<RelicDataModule.PermissionsData> GetUpdateStarPermissionsDatas(int relicID, int quality)
		{
			Relic_relic elementById = GameApp.Table.GetManager().GetRelic_relicModelInstance().GetElementById(relicID);
			if (elementById == null)
			{
				return null;
			}
			if (elementById.type != 2)
			{
				return null;
			}
			int updateStarID = this.GetUpdateStarID(relicID, quality);
			return this.GetUpdateStarPermissionsDatas(updateStarID);
		}

		public List<RelicDataModule.PermissionsData> GetUpdateStarPermissionsDatas(int starupID)
		{
			Relic_starUp elementById = GameApp.Table.GetManager().GetRelic_starUpModelInstance().GetElementById(starupID);
			if (elementById == null)
			{
				return null;
			}
			List<RelicDataModule.PermissionsData> list = new List<RelicDataModule.PermissionsData>();
			for (int i = 0; i < elementById.starAttributes.Length; i++)
			{
				string text = elementById.starAttributes[i];
				if (!string.IsNullOrEmpty(text))
				{
					string[] array = text.Split(',', StringSplitOptions.None);
					if (array.Length == 2)
					{
						int num = int.Parse(array[0]);
						string text2 = array[1];
						Relic_data elementById2 = GameApp.Table.GetManager().GetRelic_dataModelInstance().GetElementById(num);
						if (elementById2 != null)
						{
							RelicDataModule.PermissionsData permissionsData = new RelicDataModule.PermissionsData();
							permissionsData.SetTable(elementById2, text2);
							list.Add(permissionsData);
						}
					}
				}
			}
			return list;
		}

		public RelicDataModule.PermissionsData GetPermissionsDataByID(int id)
		{
			if (this.m_permissionDic == null)
			{
				return null;
			}
			RelicDataModule.PermissionsData permissionsData;
			this.m_permissionDic.TryGetValue(id, out permissionsData);
			return permissionsData;
		}

		private void MathAddAttributeData()
		{
			this.m_addAttributeData.Clear();
			AddAttributeRelic addAttributeRelic = new AddAttributeRelic(GameApp.Table.GetManager());
			RepeatedField<RelicDto> repeatedField = new RepeatedField<RelicDto>();
			foreach (KeyValuePair<int, RelicData> keyValuePair in this.m_relicDataDic)
			{
				if (keyValuePair.Value != null && keyValuePair.Value.m_active)
				{
					repeatedField.Add(keyValuePair.Value.ToRelicDto());
				}
			}
			addAttributeRelic.SetData(repeatedField);
			this.m_addAttributeData.Merge(addAttributeRelic.MathAll());
		}

		public Dictionary<int, RelicDataModule.PermissionsData> MathAllPermissionsDatas()
		{
			List<RelicDataModule.PermissionsData> list = new List<RelicDataModule.PermissionsData>();
			foreach (KeyValuePair<int, RelicData> keyValuePair in this.m_relicDataDic)
			{
				if (keyValuePair.Value != null && keyValuePair.Value.m_active)
				{
					List<RelicDataModule.PermissionsData> updateStarPermissionsDatas = this.GetUpdateStarPermissionsDatas(keyValuePair.Value.m_id, keyValuePair.Value.m_quality);
					if (updateStarPermissionsDatas != null)
					{
						list.AddRange(updateStarPermissionsDatas);
					}
				}
			}
			Dictionary<int, RelicDataModule.PermissionsData> dictionary = new Dictionary<int, RelicDataModule.PermissionsData>();
			for (int i = 0; i < list.Count; i++)
			{
				RelicDataModule.PermissionsData permissionsData = list[i];
				if (permissionsData != null)
				{
					RelicDataModule.PermissionsData permissionsData2;
					dictionary.TryGetValue(permissionsData.m_id, out permissionsData2);
					if (permissionsData2 == null)
					{
						permissionsData2 = permissionsData.Clone();
					}
					else
					{
						permissionsData2.Merge(permissionsData);
					}
					dictionary[permissionsData.m_id] = permissionsData2;
				}
			}
			return dictionary;
		}

		private void OnEventSetRelicsData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsSetRelicData eventArgsSetRelicData = eventargs as EventArgsSetRelicData;
			if (eventArgsSetRelicData == null)
			{
				return;
			}
			this.InitData(eventArgsSetRelicData.m_userLoginResponse);
		}

		private void OnEventUpdateRelicsData(object sender, int type, BaseEventArgs eventargs)
		{
			EventArgsUpdateRelicData eventArgsUpdateRelicData = eventargs as EventArgsUpdateRelicData;
			if (eventArgsUpdateRelicData == null)
			{
				return;
			}
			this.UpdateData(eventArgsUpdateRelicData.m_datas);
		}

		public Dictionary<int, RelicData> m_relicDataDic = new Dictionary<int, RelicData>();

		public Dictionary<int, RelicDataModule.PermissionsData> m_permissionDic = new Dictionary<int, RelicDataModule.PermissionsData>();

		public AddAttributeData m_addAttributeData = new AddAttributeData();

		public enum ParamType
		{
			Int = 1,
			Float,
			Bool
		}

		[RuntimeDefaultSerializedProperty]
		public class PermissionsData
		{
			public void SetTable(Relic_data table, string value)
			{
				this.m_id = table.id;
				this.m_value = value;
				this.m_languageID = table.LangugaeID.ToString();
				this.m_paramType = (RelicDataModule.ParamType)table.ParamType;
			}

			public int GetInt()
			{
				int num;
				int.TryParse(this.m_value, out num);
				return num;
			}

			public float GetFloat()
			{
				float num;
				float.TryParse(this.m_value, out num);
				return num;
			}

			public bool GetBool()
			{
				return string.Equals(this.m_value, "1");
			}

			public object GetObject()
			{
				object obj = null;
				switch (this.m_paramType)
				{
				case RelicDataModule.ParamType.Int:
					obj = this.GetInt();
					break;
				case RelicDataModule.ParamType.Float:
					obj = this.GetFloat();
					break;
				case RelicDataModule.ParamType.Bool:
					obj = this.GetBool();
					break;
				}
				return obj;
			}

			public string GetValueString()
			{
				string text = string.Empty;
				object @object = this.GetObject();
				switch (this.m_paramType)
				{
				case RelicDataModule.ParamType.Int:
					text = @object.ToString();
					break;
				case RelicDataModule.ParamType.Float:
					text = ((float)@object * 100f).ToString() + "%";
					break;
				case RelicDataModule.ParamType.Bool:
					text = string.Empty;
					break;
				}
				return text;
			}

			public RelicDataModule.PermissionsData Clone()
			{
				return new RelicDataModule.PermissionsData
				{
					m_id = this.m_id,
					m_value = this.m_value,
					m_paramType = this.m_paramType,
					m_languageID = this.m_languageID
				};
			}

			public void Copy(RelicDataModule.PermissionsData data)
			{
				this.m_id = data.m_id;
				this.m_value = data.m_value;
				this.m_paramType = data.m_paramType;
				this.m_languageID = data.m_languageID;
			}

			public RelicDataModule.PermissionsData Merge(RelicDataModule.PermissionsData data)
			{
				if (data == null)
				{
					return this;
				}
				if (data.m_id != this.m_id)
				{
					return this;
				}
				switch (this.m_paramType)
				{
				case RelicDataModule.ParamType.Int:
					this.m_value = (this.GetInt() + data.GetInt()).ToString();
					break;
				case RelicDataModule.ParamType.Float:
					this.m_value = (this.GetFloat() + data.GetFloat()).ToString();
					break;
				case RelicDataModule.ParamType.Bool:
					this.m_value = ((this.GetBool() || data.GetBool()) ? "1" : "0");
					break;
				}
				return this;
			}

			public RelicDataModule.PermissionsData CopyTo()
			{
				return new RelicDataModule.PermissionsData
				{
					m_id = this.m_id,
					m_value = this.m_value,
					m_paramType = this.m_paramType,
					m_languageID = this.m_languageID
				};
			}

			public int m_id;

			public string m_value = string.Empty;

			public RelicDataModule.ParamType m_paramType = RelicDataModule.ParamType.Int;

			public string m_languageID;
		}
	}
}
