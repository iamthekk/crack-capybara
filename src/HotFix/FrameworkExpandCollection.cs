using System;
using Framework;
using Proto.Common;

namespace HotFix
{
	public static class FrameworkExpandCollection
	{
		public static int GetCollectionId(this CollectionDto dto)
		{
			int num;
			if (dto.CollecType == 2U)
			{
				num = int.Parse(GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)dto.ConfigId)
					.itemTypeParam[0]);
			}
			else
			{
				num = int.Parse(GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById((int)dto.ConfigId)
					.itemTypeParam[0]);
			}
			return num;
		}
	}
}
