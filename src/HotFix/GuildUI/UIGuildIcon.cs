using System;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix.GuildUI
{
	public class UIGuildIcon : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public void SetIcon(int icon)
		{
			if (icon == 0)
			{
				icon = 1001;
			}
			Guild_guildStyle guildStyleTable = GuildProxy.Table.GetGuildStyleTable(icon);
			if (guildStyleTable != null)
			{
				GuildProxy.Resources.SetDxxImage(this.Image_Icon, guildStyleTable.AtlasID, guildStyleTable.Icon);
			}
		}

		public const int DefaultIcon = 1001;

		public CustomImage Image_Icon;
	}
}
