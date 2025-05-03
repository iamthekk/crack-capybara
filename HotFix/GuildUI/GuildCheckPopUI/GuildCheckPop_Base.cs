using System;
using Dxx.Guild;

namespace HotFix.GuildUI.GuildCheckPopUI
{
	public abstract class GuildCheckPop_Base : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public abstract void RefreshUI(GuildShareData sharedata);
	}
}
