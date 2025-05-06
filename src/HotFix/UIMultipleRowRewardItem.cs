using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class UIMultipleRowRewardItem : CustomBehaviour
	{
		protected override void OnInit()
		{
		}

		protected override void OnDeInit()
		{
		}

		public void SetData(int itemId, int rate)
		{
			Item_Item item_Item = GameApp.Table.GetManager().GetItem_Item(itemId);
			if (item_Item == null)
			{
				return;
			}
			this.textName.text = Singleton<LanguageManager>.Instance.GetInfoByID(item_Item.nameID);
			this.textRate.text = string.Format("{0}%", rate);
			string atlasPath = GameApp.Table.GetAtlasPath(item_Item.atlasID);
			this.imageIcon.SetImage(atlasPath, item_Item.icon);
		}

		public CustomText textName;

		public CustomText textRate;

		public CustomImage imageIcon;
	}
}
