using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix
{
	public class UIMainCityGoldNodeItem : CustomBehaviour
	{
		protected override void OnInit()
		{
			if (this.m_itemID > 0)
			{
				Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(this.m_itemID);
				if (this.m_icon != null)
				{
					this.m_icon.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
				}
			}
		}

		protected override void OnDeInit()
		{
		}

		public void SetItemID(int itemID)
		{
			if (this.m_itemID == itemID)
			{
				return;
			}
			this.m_itemID = itemID;
			Item_Item elementById = GameApp.Table.GetManager().GetItem_ItemModelInstance().GetElementById(this.m_itemID);
			if (this.m_icon != null)
			{
				this.m_icon.SetImage(GameApp.Table.GetAtlasPath(elementById.atlasID), elementById.icon);
			}
		}

		public void SetText(string info)
		{
			if (this.m_text != null)
			{
				this.m_text.text = info;
			}
		}

		public CustomImage m_icon;

		public CustomText m_text;

		private int m_itemID;
	}
}
