using System;
using System.Collections.Generic;
using Server;

namespace HotFix
{
	public static class TGACommonItemInfoExtend
	{
		public static string ToJson(this TGACommonItemInfo info)
		{
			if (info == null)
			{
				return "{}";
			}
			return JsonManager.SerializeObject(info);
		}

		public static string ToJson(this List<TGACommonItemInfo> list)
		{
			if (list == null)
			{
				return "{}";
			}
			return JsonManager.SerializeObject(list);
		}
	}
}
