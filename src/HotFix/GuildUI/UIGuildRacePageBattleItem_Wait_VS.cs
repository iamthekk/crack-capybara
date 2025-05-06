using System;
using Dxx.Guild;
using Framework.Logic.UI;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageBattleItem_Wait_VS : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public void SetData(GuildRaceUserVSRecord data)
		{
			this.Data = data;
		}

		public void RefreshUI()
		{
			if (this.Data == null)
			{
				return;
			}
			this.Text_Index.text = GuildProxy.Language.GetInfoByID1_LogError(400430, this.Data.Index);
		}

		public CustomText Text_Index;

		public GuildRaceUserVSRecord Data;
	}
}
