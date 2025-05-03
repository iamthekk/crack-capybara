using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using LocalModels;
using LocalModels.Bean;
using Proto.Common;

namespace Server
{
	public class AddAttributeRelic : BaseAddAttribute
	{
		public AddAttributeRelic(LocalModelManager tableManager)
			: base(tableManager)
		{
		}

		public void SetData(RepeatedField<RelicDto> relicDtos)
		{
			this.m_datas = relicDtos;
		}

		public override AddAttributeData MathAll()
		{
			this.OnRefreshDic();
			AddAttributeData addAttributeData = new AddAttributeData();
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			List<MergeAttributeData> list2 = this.MathAllRelicAttributes();
			if (list2 != null)
			{
				list.AddRange(list2);
			}
			List<MergeAttributeData> list3 = this.MathAllGroupAttributes();
			if (list3 != null)
			{
				list.AddRange(list3);
			}
			addAttributeData.m_attributeDatas.AddRange(list.Merge());
			return addAttributeData;
		}

		private void OnRefreshDic()
		{
			Dictionary<int, RelicDto> dictionary = new Dictionary<int, RelicDto>();
			for (int i = 0; i < this.m_datas.Count; i++)
			{
				RelicDto relicDto = this.m_datas[i];
				if (relicDto != null)
				{
					dictionary[(int)relicDto.RelicId] = relicDto;
				}
			}
			this.m_relicIDs.Clear();
			IList<Relic_group> allElements = this.m_tableManager.GetRelic_groupModelInstance().GetAllElements();
			for (int j = 0; j < allElements.Count; j++)
			{
				Relic_group relic_group = allElements[j];
				if (relic_group != null)
				{
					for (int k = 0; k < relic_group.Content.Length; k++)
					{
						int num = relic_group.Content[k];
						if (num > 0 && this.m_tableManager.GetRelic_relicModelInstance().GetElementById(num) != null)
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
							this.m_relicIDs[num] = relicData;
						}
					}
				}
			}
		}

		public RelicData GetRelicDataByID(int id)
		{
			RelicData relicData;
			this.m_relicIDs.TryGetValue(id, out relicData);
			return relicData;
		}

		public int GetGroupMinQuality(int groupID, out bool isAllActive)
		{
			Relic_group elementById = this.m_tableManager.GetRelic_groupModelInstance().GetElementById(groupID);
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
			Relic_group elementById = this.m_tableManager.GetRelic_groupModelInstance().GetElementById(groupID);
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
			return this.m_tableManager.GetRelic_relicModelInstance().GetElementById(relicID).AttributesType * 1000 + level;
		}

		public List<MergeAttributeData> GetUpdateLevelAttributes(int relicID, int level)
		{
			int updataLevelID = this.GetUpdataLevelID(relicID, level);
			return this.GetUpdateLevelAttributes(updataLevelID);
		}

		public List<MergeAttributeData> GetUpdateLevelAttributes(int updateLevelID)
		{
			Relic_updateLevel elementById = this.m_tableManager.GetRelic_updateLevelModelInstance().GetElementById(updateLevelID);
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
			return this.m_tableManager.GetRelic_relicModelInstance().GetElementById(relicID).starUpType * 1000 + quality;
		}

		public List<MergeAttributeData> GetUpdateStarAttributes(int relicID, int quality)
		{
			Relic_relic elementById = this.m_tableManager.GetRelic_relicModelInstance().GetElementById(relicID);
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
			Relic_starUp elementById = this.m_tableManager.GetRelic_starUpModelInstance().GetElementById(starupID);
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

		private List<MergeAttributeData> MathAllRelicAttributes()
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			foreach (KeyValuePair<int, RelicData> keyValuePair in this.m_relicIDs)
			{
				if (keyValuePair.Value != null && keyValuePair.Value.m_active)
				{
					List<MergeAttributeData> updateLevelAttributes = this.GetUpdateLevelAttributes(keyValuePair.Value.m_id, keyValuePair.Value.m_level);
					if (updateLevelAttributes != null)
					{
						list.AddRange(updateLevelAttributes);
					}
					List<MergeAttributeData> updateStarAttributes = this.GetUpdateStarAttributes(keyValuePair.Value.m_id, keyValuePair.Value.m_quality);
					if (updateStarAttributes != null)
					{
						list.AddRange(updateStarAttributes);
					}
				}
			}
			return list;
		}

		private List<MergeAttributeData> MathAllGroupAttributes()
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			IList<Relic_group> allElements = this.m_tableManager.GetRelic_groupModelInstance().GetAllElements();
			for (int i = 0; i < allElements.Count; i++)
			{
				Relic_group relic_group = allElements[i];
				if (relic_group != null)
				{
					bool flag;
					int groupMinQuality = this.GetGroupMinQuality(relic_group.id, out flag);
					if (flag)
					{
						List<MergeAttributeData> groupAttributesByGroup = this.GetGroupAttributesByGroup(relic_group.id, groupMinQuality);
						if (groupAttributesByGroup != null)
						{
							list.AddRange(groupAttributesByGroup);
						}
					}
				}
			}
			return list;
		}

		public RepeatedField<RelicDto> m_datas;

		public Dictionary<int, RelicData> m_relicIDs = new Dictionary<int, RelicData>();
	}
}
