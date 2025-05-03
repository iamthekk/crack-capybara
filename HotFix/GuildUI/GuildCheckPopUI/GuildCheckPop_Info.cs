using System;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.UI;

namespace HotFix.GuildUI.GuildCheckPopUI
{
	public class GuildCheckPop_Info : GuildCheckPop_Base
	{
		protected override void GuildUI_OnInit()
		{
			base.GuildUI_OnInit();
			this.GuildIcon.Init();
			this.Empty_SloganTip.SetActive(false);
		}

		protected override void GuildUI_OnUnInit()
		{
			base.GuildUI_OnInit();
			this.GuildIcon.DeInit();
		}

		public override void RefreshUI(GuildShareData sharedata)
		{
			this.GuildIcon.SetIcon(sharedata.GuildIcon);
			this.Text_Name.text = sharedata.GuildShowName;
			this.Text_Slogan.text = sharedata.GuildNotice;
			this.Empty_SloganTip.SetActive(string.IsNullOrEmpty(sharedata.GuildNotice));
			this.Text_Level.text = sharedata.GuildLevel.ToString();
			this.Text_ExpLevel.text = "Lv." + sharedata.GuildLevel.ToString();
			this.Text_Power.text = DxxTools.FormatNumber(sharedata.GuildPower);
			this.Text_MemberCount.SetText(400029, sharedata.GuildMemberCount.ToString(), sharedata.GuildMemberMaxCount.ToString());
			this.Text_GuildId.text = sharedata.GuildID;
			Guild_guildLevel guildLevelTable = GuildProxy.Table.GetGuildLevelTable(sharedata.GuildLevel);
			if (guildLevelTable != null)
			{
				this.Text_Exp.text = string.Format("{0}/{1}", sharedata.GuildExp, guildLevelTable.Exp);
				this.Slider_Exp.value = (float)sharedata.GuildExp / (float)guildLevelTable.Exp;
			}
		}

		[SerializeField]
		private UIGuildIcon GuildIcon;

		[SerializeField]
		private CustomText Text_Name;

		[SerializeField]
		private CustomText Text_Slogan;

		[SerializeField]
		private GameObject Empty_SloganTip;

		[SerializeField]
		private CustomText Text_Power;

		[SerializeField]
		private CustomText Text_Level;

		[SerializeField]
		private CustomText Text_MemberCount;

		[SerializeField]
		private CustomText Text_Exp;

		[SerializeField]
		private Slider Slider_Exp;

		[SerializeField]
		private CustomText Text_ExpLevel;

		[SerializeField]
		private CustomText Text_GuildId;
	}
}
