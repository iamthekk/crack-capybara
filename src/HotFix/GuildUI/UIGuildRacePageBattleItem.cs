using System;
using Dxx.Guild;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class UIGuildRacePageBattleItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
		}

		protected override void GuildUI_OnUnInit()
		{
		}

		public virtual void OnUpdate()
		{
		}

		public virtual void SetData(GuildRaceUserVSRecord data)
		{
			this.Data = data;
		}

		public virtual void RefreshUI()
		{
		}

		public virtual float Size_Y()
		{
			return (base.gameObject.transform as RectTransform).sizeDelta.y;
		}

		public virtual void SetGuildName(string guildName1, string guildName2)
		{
		}

		public GuildRaceUserVSRecord Data;
	}
}
