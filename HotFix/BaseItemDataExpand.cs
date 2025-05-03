using System;

namespace HotFix
{
	public static class BaseItemDataExpand
	{
		public static ItemData ToItemData(this BaseItemData data)
		{
			return new ItemData((int)data.id, (long)((int)data.count));
		}

		public static PropData ToPropData(this BaseItemData data)
		{
			return new PropData
			{
				id = data.id,
				count = data.count
			};
		}
	}
}
