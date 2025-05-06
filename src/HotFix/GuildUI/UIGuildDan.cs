using System;
using Framework.Logic.UI;
using LocalModels.Bean;

namespace HotFix.GuildUI
{
	public class UIGuildDan : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public void SetData(int danId)
		{
			GuildBOSS_guildSubection guildBossDanTable = GuildProxy.Table.GetGuildBossDanTable(danId);
			if (guildBossDanTable == null)
			{
				return;
			}
			GuildProxy.Resources.SetDxxImage(this.Image_Dan, guildBossDanTable.atlasId, guildBossDanTable.atlasName);
			if (!string.IsNullOrEmpty(guildBossDanTable.StarIcon))
			{
				GuildProxy.Resources.SetDxxImage(this.Image_StarOne, guildBossDanTable.atlasId, guildBossDanTable.StarIcon);
				GuildProxy.Resources.SetDxxImage(this.Image_StarTwo, guildBossDanTable.atlasId, guildBossDanTable.StarIcon);
				GuildProxy.Resources.SetDxxImage(this.Image_StarThree, guildBossDanTable.atlasId, guildBossDanTable.StarIcon);
			}
			if (!string.IsNullOrEmpty(guildBossDanTable.StarIconBg))
			{
				this.Image_DanBg.gameObject.SetActiveSafe(true);
				GuildProxy.Resources.SetDxxImage(this.Image_DanBg, guildBossDanTable.atlasId, guildBossDanTable.StarIconBg);
			}
			else
			{
				this.Image_DanBg.gameObject.SetActiveSafe(false);
			}
			this.Image_StarOne.gameObject.SetActiveSafe(guildBossDanTable.RankStar >= 1);
			this.Image_StarTwo.gameObject.SetActiveSafe(guildBossDanTable.RankStar >= 2);
			this.Image_StarThree.gameObject.SetActiveSafe(guildBossDanTable.RankStar >= 3);
		}

		public CustomImage Image_Dan;

		public CustomImage Image_DanBg;

		public CustomImage Image_StarOne;

		public CustomImage Image_StarTwo;

		public CustomImage Image_StarThree;
	}
}
