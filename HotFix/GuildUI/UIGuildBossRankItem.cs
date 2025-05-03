using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildBossRankItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.buttonBorder.onClick.AddListener(new UnityAction(this.OnClickGuild));
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonBorder != null)
			{
				this.buttonBorder.onClick.AddListener(new UnityAction(this.OnClickGuild));
			}
			if (this.guildIcon != null)
			{
				this.guildIcon.DeInit();
			}
		}

		public void SetData(GuildBossGuildRankData data)
		{
			this.rankData = data;
		}

		public void RefreshUI()
		{
			if (this.rankData == null || this.rankData.GuildData == null)
			{
				this.RefreshAsNull();
				return;
			}
			this.guildIcon.SetIcon(this.rankData.GuildData.GuildIcon);
			this.textName.text = this.rankData.GuildData.GuildName;
			if (this.rankData.Rank <= 0)
			{
				this.textRank.text = "-";
			}
			else
			{
				this.textRank.text = this.rankData.Rank.ToString();
			}
			this.textHurt.text = GuildProxy.Language.FormatNumber(this.rankData.GuildData.GuildDamage);
			this.textPower.text = GuildProxy.Language.FormatNumber(this.rankData.GuildData.GuildPower);
		}

		private void RefreshAsNull()
		{
		}

		private void OnClickGuild()
		{
		}

		public CustomText textName;

		public CustomText textRank;

		public CustomText textHurt;

		public CustomText textPower;

		public CustomButton buttonBorder;

		public UIGuildIcon guildIcon;

		private GuildBossGuildRankData rankData;
	}
}
