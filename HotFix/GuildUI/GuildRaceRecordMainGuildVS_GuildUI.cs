using System;
using Dxx.Guild;
using Framework.Logic.UI;

namespace HotFix.GuildUI
{
	public class GuildRaceRecordMainGuildVS_GuildUI : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.Icon.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			UIGuildIcon icon = this.Icon;
			if (icon == null)
			{
				return;
			}
			icon.DeInit();
		}

		public void SetData(GuildRaceGuild data)
		{
			this.Data = data;
		}

		public void RefreshUI()
		{
			if (this.Data == null || base.gameObject == null)
			{
				return;
			}
			this.Icon.SetIcon(this.Data.ShareData.GuildIcon);
			this.Text_Name.text = this.Data.ShareData.GuildShowName;
			this.Text_Power.text = GuildProxy.Language.GetInfoByID1("400416", GuildProxy.Language.FormatNumber((long)this.Data.TotalPower));
		}

		public UIGuildIcon Icon;

		public CustomText Text_Name;

		public CustomText Text_Power;

		public GuildRaceGuild Data;
	}
}
