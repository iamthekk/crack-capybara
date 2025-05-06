using System;
using Framework;
using LocalModels.Bean;

namespace HotFix
{
	public class FlyItemBagItem : BaseFlyItem
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public override void SetData(object param)
		{
			if (param == null)
			{
				return;
			}
			int num = (int)param;
			Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(num);
			if (item_Item != null)
			{
				string atlasPath = GameApp.Table.GetAtlasPath(item_Item.atlasID);
				this.m_icon.SetImage(atlasPath, item_Item.icon);
			}
		}
	}
}
