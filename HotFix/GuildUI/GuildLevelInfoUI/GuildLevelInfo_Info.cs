using System;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine.UI;

namespace HotFix.GuildUI.GuildLevelInfoUI
{
	public class GuildLevelInfo_Info : GuildLevelInfo_Base
	{
		private int MaxLevel
		{
			get
			{
				return GuildProxy.Table.GetGuildLevelTableAll().Count;
			}
		}

		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.IconCtrl.Init();
		}

		protected override void GuildUI_OnUnInit()
		{
			UIGuildIcon iconCtrl = this.IconCtrl;
			if (iconCtrl != null)
			{
				iconCtrl.DeInit();
			}
			this.IconCtrl = null;
			base.GuildUI_OnUnInit();
		}

		public override void RefreshUI()
		{
			GuildShareData guildData = base.SDK.GuildInfo.GuildData;
			Guild_guildLevel guildLevelTable = GuildProxy.Table.GetGuildLevelTable(guildData.GuildLevel);
			int num = guildData.GuildExp;
			if (guildLevelTable != null)
			{
				num = guildLevelTable.Exp;
			}
			this.TextName.text = guildData.GuildShowName;
			this.TextLevel.text = guildData.GuildLevel.ToString();
			this.IconCtrl.SetIcon(guildData.GuildIcon);
			bool flag = guildData.GuildLevel == this.MaxLevel;
			this.TextExp.text = (flag ? GuildProxy.Language.GetInfoByID("400257") : string.Format("{0}/{1}", guildData.GuildExp, num));
			this.SliderExp.value = (flag ? 1f : ((float)guildData.GuildExp / (float)num));
		}

		public UIGuildIcon IconCtrl;

		public Slider SliderExp;

		public CustomText TextExp;

		public CustomText TextLevel;

		public CustomText TextName;
	}
}
