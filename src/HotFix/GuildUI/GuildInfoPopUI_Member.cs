using System;
using Dxx.Guild;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildInfoPopUI_Member : GuildInfoPopUI_Base
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.MemberUI.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
			this.MemberUI.DeInit();
		}

		public void RefreshMember(GuildUserShareData data)
		{
			this.MemberUI.Refresh(data);
		}

		[SerializeField]
		private UIGuildHallMemberItem MemberUI;
	}
}
