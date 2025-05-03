using System;
using System.Collections.Generic;
using System.Linq;
using LocalModels;
using LocalModels.Bean;

namespace Server
{
	public static class Tools
	{
		public static List<string> GetListString(this string info, char separator = '|')
		{
			List<string> list = new List<string>();
			if (string.IsNullOrEmpty(info))
			{
				return list;
			}
			foreach (string text in info.Split(separator, StringSplitOptions.None))
			{
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(text);
				}
			}
			return list;
		}

		public static List<MergeAttributeData> Merge(this List<MergeAttributeData> datas)
		{
			Dictionary<string, MergeAttributeData> dictionary = new Dictionary<string, MergeAttributeData>();
			for (int i = 0; i < datas.Count; i++)
			{
				MergeAttributeData mergeAttributeData = datas[i];
				if (mergeAttributeData != null)
				{
					MergeAttributeData mergeAttributeData2;
					dictionary.TryGetValue(mergeAttributeData.Header, out mergeAttributeData2);
					if (mergeAttributeData2 == null)
					{
						mergeAttributeData2 = mergeAttributeData.Clone();
					}
					else
					{
						mergeAttributeData2.Merge(mergeAttributeData);
					}
					dictionary[mergeAttributeData.Header] = mergeAttributeData2;
				}
			}
			return dictionary.Values.ToList<MergeAttributeData>();
		}

		public static FP GetAttributeValue(this List<MergeAttributeData> datas, string attributeKey)
		{
			for (int i = 0; i < datas.Count; i++)
			{
				MergeAttributeData mergeAttributeData = datas[i];
				if (mergeAttributeData.Header == attributeKey)
				{
					return mergeAttributeData.Value;
				}
			}
			return FP._0;
		}

		public static List<MergeAttributeData> GetMergeAttributeData(this Equip_equip equip_Equip)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			foreach (string text in equip_Equip.baseAttributes.Replace("\n", "").Replace(" ", "").Replace("\t", "")
				.Replace("\r", "")
				.Split('|', StringSplitOptions.None))
			{
				if (!string.IsNullOrEmpty(text))
				{
					MergeAttributeData mergeAttributeData = new MergeAttributeData(text, null, null);
					list.Add(mergeAttributeData);
				}
			}
			return list;
		}

		public static int GetEvolutionTableID(this Equip_equip equip_Equip, int evolution)
		{
			return equip_Equip.tagID * 100 + evolution;
		}

		public static List<MergeAttributeData> GetMergeAttributeData(this Equip_equip equip_Equip, LocalModelManager tableManager, int level, int evolution)
		{
			if (evolution == 0)
			{
				HLog.LogError(string.Format("equipId:{0}, evolution can not be 0", equip_Equip.id));
				return new List<MergeAttributeData>();
			}
			List<MergeAttributeData> mergeAttributeData = equip_Equip.GetMergeAttributeData();
			int evolutionTableID = equip_Equip.GetEvolutionTableID(evolution);
			int evolutionTableID2 = equip_Equip.GetEvolutionTableID(evolution - 1);
			Equip_equipEvolution equip_equipEvolution = ((evolution - 1 > 0) ? tableManager.GetEquip_equipEvolutionModelInstance().GetElementById(evolutionTableID2) : null);
			Equip_equipEvolution elementById = tableManager.GetEquip_equipEvolutionModelInstance().GetElementById(evolutionTableID);
			long[] upgradeAttributes = elementById.upgradeAttributes;
			long[] evolutionAttributes = elementById.evolutionAttributes;
			int num;
			if (equip_equipEvolution != null)
			{
				num = Math.Max(level - equip_equipEvolution.maxLevel, 0);
			}
			else
			{
				num = Math.Max(level - 1, 0);
			}
			int composeId = equip_Equip.composeId;
			int num2 = 0;
			for (int i = 1; i <= composeId; i++)
			{
				Equip_equipCompose elementById2 = tableManager.GetEquip_equipComposeModelInstance().GetElementById(i);
				if (elementById2 != null)
				{
					num2 += elementById2.qualityAttributes;
				}
			}
			float num3 = 1f + (float)num2 / 10000f;
			for (int j = 0; j < mergeAttributeData.Count; j++)
			{
				MergeAttributeData mergeAttributeData2 = mergeAttributeData[j];
				mergeAttributeData2.Value = evolutionAttributes[j];
				if (upgradeAttributes.Length > j)
				{
					long num4 = upgradeAttributes[j];
					if (num4 > 0L && num > 0)
					{
						mergeAttributeData2.Plus((long)num * num4);
					}
				}
				mergeAttributeData2.Multiply(num3);
			}
			return mergeAttributeData;
		}

		public static int GetUpdataLevelTableID(this Equip_equip equip_Equip, int level)
		{
			if (level <= 0)
			{
				level = 1;
			}
			return equip_Equip.Type * 10000 + level;
		}

		public static string GetSymbolString(bool value)
		{
			if (value)
			{
				return "+";
			}
			return "-";
		}
	}
}
