using System;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildHall_Base : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public virtual void OnRefreshUI()
		{
		}

		public void RefreshParent()
		{
			Action onRefreshParent = this.OnRefreshParent;
			if (onRefreshParent == null)
			{
				return;
			}
			onRefreshParent();
		}

		public Animator Ani;

		public Action OnRefreshParent;
	}
}
