using System;
using LocalModels.Bean;

namespace HotFix
{
	public static class TableItemGiftExpand
	{
		public static ItemGiftType GetItemGiftType(this ItemGift_ItemGift table)
		{
			return (ItemGiftType)table.Type;
		}
	}
}
