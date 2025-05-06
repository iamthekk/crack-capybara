using System;
using System.Collections.Generic;
using Framework;
using LocalModels.Bean;
using Server;

namespace HotFix
{
	public static class MergeAttributeDataHelper
	{
		public static List<MergeAttributeData> MinusMergeList(List<MergeAttributeData> mergeData1, List<MergeAttributeData> mergeData2)
		{
			List<MergeAttributeData> list = new List<MergeAttributeData>();
			if (mergeData1 != null)
			{
				for (int i = 0; i < mergeData1.Count; i++)
				{
					MergeAttributeData mergeAttributeData = mergeData1[i].Clone();
					list.Add(mergeAttributeData);
				}
			}
			if (mergeData2 != null)
			{
				for (int j = 0; j < mergeData2.Count; j++)
				{
					MergeAttributeData mergeAttributeData2 = mergeData2[j].Clone();
					mergeAttributeData2.Multiply(-1);
					list.Add(mergeAttributeData2);
				}
			}
			list = list.Merge();
			for (int k = list.Count - 1; k >= 0; k--)
			{
				if (list[k].Value == 0)
				{
					list.RemoveAt(k);
				}
			}
			return list;
		}

		public static string GetMergeAttributeLangId(MergeAttributeData mergeData)
		{
			string text = "";
			if (mergeData != null)
			{
				Attribute_AttrText elementById = GameApp.Table.GetManager().GetAttribute_AttrTextModelInstance().GetElementById(mergeData.Header);
				if (elementById != null)
				{
					return elementById.LanguageId;
				}
			}
			return text;
		}
	}
}
