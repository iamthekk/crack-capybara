using System;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildRaceRewardsShowItem_RewardItemUI : UIGuildItem
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.Obj_Win.SetActive(false);
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnUnInit();
		}

		public void SetShowWinObj(bool show)
		{
			this.Obj_Win.SetActive(show);
		}

		public GameObject Obj_Win;
	}
}
