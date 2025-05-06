using System;
using System.Collections.Generic;

namespace HotFix
{
	public static class ConfigTool
	{
		public static List<ItemData> FormatItemsString(string str)
		{
			List<ItemData> list = new List<ItemData>();
			if (!string.IsNullOrEmpty(str))
			{
				foreach (string text in str.Split('|', StringSplitOptions.None))
				{
					if (!string.IsNullOrEmpty(text))
					{
						string[] array2 = text.Split(',', StringSplitOptions.None);
						int num;
						int num2;
						if (array2.Length == 2 && int.TryParse(array2[0], out num) && int.TryParse(array2[1], out num2))
						{
							ItemData itemData = new ItemData(num, (long)num2);
							list.Add(itemData);
						}
					}
				}
			}
			return list;
		}
	}
}
