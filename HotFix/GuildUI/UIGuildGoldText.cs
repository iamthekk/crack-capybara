using System;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix.GuildUI
{
	public class UIGuildGoldText : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public void SetCurrencyType(int id)
		{
			Item_Item itemTable = GuildProxy.Table.GetItemTable(id);
			if (itemTable != null)
			{
				GuildProxy.Resources.SetDxxImage(this.ImageIcon, itemTable.atlasID, itemTable.icon);
			}
		}

		public void SetValue(int count)
		{
			this.TextCount.text = string.Format("X {0}", count);
		}

		public CustomImage ImageIcon;

		public CustomText TextCount;
	}
}
