using System;
using Framework.Logic.UI;

namespace HotFix.GuildUI
{
	public class UIGuildBossHurtItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public void Refresh(string name, long hurt)
		{
			this.textInfo.text = GuildProxy.Language.GetInfoByID2("400231", name, hurt);
		}

		public CustomText textInfo;
	}
}
