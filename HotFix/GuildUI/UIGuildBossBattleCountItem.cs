using System;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildBossBattleCountItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public void Refresh(bool isActive)
		{
			if (isActive)
			{
				this.image.color = Color.white;
				return;
			}
			this.image.color = Color.black;
		}

		public CustomImage image;
	}
}
