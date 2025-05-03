using System;
using Dxx.Guild;
using Framework.Logic.UI;
using LocalModels.Bean;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HotFix.GuildUI
{
	public class UIGuildHall_BaseInfo : UIGuildHall_Base
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
			this.buttonMore.onClick.AddListener(new UnityAction(this.OnShowGuildMore));
			this.buttonLevelUp.onClick.AddListener(new UnityAction(this.OnGuildLevelUp));
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.buttonMore != null)
			{
				this.buttonMore.onClick.RemoveListener(new UnityAction(this.OnShowGuildMore));
			}
			if (this.buttonLevelUp != null)
			{
				this.buttonLevelUp.onClick.RemoveListener(new UnityAction(this.OnGuildLevelUp));
			}
			base.GuildUI_OnUnInit();
		}

		public override void OnRefreshUI()
		{
			GuildShareData guildData = GuildSDKManager.Instance.GuildInfo.GuildData;
			if (guildData == null)
			{
				return;
			}
			this.iconCtrl.SetIcon(guildData.GuildIcon);
			Guild_guildLevel guildLevelTable = GuildProxy.Table.GetGuildLevelTable(guildData.GuildLevel);
			int num = guildData.GuildExp;
			if (guildLevelTable != null)
			{
				num = guildLevelTable.Exp;
			}
			this.textName.text = guildData.GuildShowName;
			this.textMember.text = GuildProxy.Language.GetInfoByID2("400029", guildData.GuildMemberCount, guildData.GuildMemberMaxCount);
			this.textLevel.text = guildData.GuildLevel.ToString();
			bool flag = guildData.GuildLevel == this.MaxLevel;
			this.textExp.text = (flag ? GuildProxy.Language.GetInfoByID("400257") : string.Format("{0}/{1}", guildData.GuildExp, num));
			this.sliderExp.value = (flag ? 1f : ((float)guildData.GuildExp / (float)num));
		}

		private void OnShowGuildMore()
		{
			GuildProxy.UI.OpenUIGuildInfoPop(null);
		}

		private void OnGuildLevelUp()
		{
			GuildProxy.UI.OpenUIGuildLevelInfo(null);
		}

		[Header("基础信息")]
		public UIGuildIcon iconCtrl;

		public CustomText textName;

		public CustomText textMember;

		public CustomButton buttonMore;

		public CustomButton buttonLevelUp;

		public CustomText textLevel;

		public CustomText textExp;

		public Slider sliderExp;
	}
}
