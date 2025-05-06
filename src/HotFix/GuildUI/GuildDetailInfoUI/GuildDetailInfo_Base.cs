using System;
using Dxx.Guild;

namespace HotFix.GuildUI.GuildDetailInfoUI
{
	public abstract class GuildDetailInfo_Base : GuildProxy.GuildProxy_BaseBehaviour
	{
		public GuildCreateData CreateData { get; set; }

		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public abstract void RefreshUI(GuildShareData sharedata, GuildShareDetailData detaildata);
	}
}
