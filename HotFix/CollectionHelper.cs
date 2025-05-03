using System;
using System.Collections.Generic;

namespace HotFix
{
	public static class CollectionHelper
	{
		public static void SortCollectionList(List<CollectionData> list)
		{
			if (list == null)
			{
				return;
			}
			list.Sort(delegate(CollectionData a, CollectionData b)
			{
				int num = b.quality.CompareTo(a.quality);
				if (num.Equals(0))
				{
					num = a.itemId.CompareTo(b.itemId);
				}
				return num;
			});
		}

		public static string GetRarityName(int rarity)
		{
			string text = "";
			switch (rarity)
			{
			case 1:
				text = "C";
				break;
			case 2:
				text = "B";
				break;
			case 3:
				text = "A";
				break;
			case 4:
				text = "AA";
				break;
			case 5:
				text = "AAA";
				break;
			case 6:
				text = "S";
				break;
			case 7:
				text = "SS";
				break;
			case 8:
				text = "SSS";
				break;
			}
			return text;
		}
	}
}
