using System;

namespace HotFix.GuildUI
{
	public abstract class UIGuildBossBaseCtrl : GuildProxy.GuildProxy_BaseBehaviour
	{
		public abstract UIGuildBossTag BossTag { get; }

		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public bool isShowAni;
	}
}
