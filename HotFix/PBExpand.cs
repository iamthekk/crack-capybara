using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using Proto.Common;

namespace HotFix
{
	public static class PBExpand
	{
		public static List<ItemData> ToItemDataList(this RepeatedField<RewardDto> dtos)
		{
			List<ItemData> list = new List<ItemData>();
			if (dtos == null)
			{
				return list;
			}
			for (int i = 0; i < dtos.Count; i++)
			{
				ItemData itemData = new ItemData();
				itemData.SetID(dtos[i].ConfigId);
				itemData.SetCount((long)dtos[i].Count);
				list.Add(itemData);
			}
			return list;
		}
	}
}
