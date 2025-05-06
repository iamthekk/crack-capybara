using System;
using LocalModels.Bean;

namespace HotFix
{
	public static class TableItemExpand
	{
		public static ItemType GetItemType(this Item_Item table)
		{
			return (ItemType)table.itemType;
		}

		public static PropType GetPropType(this Item_Item item)
		{
			return (PropType)item.propType;
		}

		public static ItemIconSizeType GetIconSizeType(this Item_Item item)
		{
			return (ItemIconSizeType)item.iconSizeType;
		}
	}
}
