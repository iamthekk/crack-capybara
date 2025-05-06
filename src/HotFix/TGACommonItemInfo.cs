using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class TGACommonItemInfo
	{
		public TGACommonItemInfo(int itemId, long count)
		{
			this.itemId = itemId;
			this.count = count;
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(itemId);
			if (elementById != null)
			{
				this.name = Singleton<LanguageManager>.Instance.GetInfoByID(elementById.nameID);
			}
		}

		public int itemId;

		public long count;

		public string name;
	}
}
