using System;
using Dxx.Guild;
using Framework.Logic.UI;
using UnityEngine.Events;

namespace HotFix.GuildUI
{
	public class UIGuildListItem : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			this.iconCtrl.Init();
			this.Button_Scan.onClick.AddListener(new UnityAction(this.OnScan));
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.Button_Scan != null)
			{
				this.Button_Scan.onClick.RemoveListener(new UnityAction(this.OnScan));
			}
			UIGuildIcon uiguildIcon = this.iconCtrl;
			if (uiguildIcon != null)
			{
				uiguildIcon.DeInit();
			}
			this.iconCtrl = null;
		}

		public void SetData(GuildShareData data)
		{
			this.m_data = data;
			this.iconCtrl.SetIcon(this.m_data.GuildIcon);
			this.Text_Name.text = this.m_data.GuildShowName;
			this.Text_Language.text = this.m_data.GetLanguageString();
			this.Text_LevelNeed.text = this.m_data.LevelNeed.ToString();
			this.Text_JoinType.text = this.m_data.GetJoinTypeString();
			this.Text_Members.text = GuildProxy.Language.GetInfoByID2("400029", this.m_data.GuildMemberCount, this.m_data.GuildMemberMaxCount);
		}

		private void OnScan()
		{
			GuildProxy.UI.OpenUIGuildCheckPop(this.m_data.GuildID, 0UL);
		}

		public UIGuildIcon iconCtrl;

		public CustomText Text_Name;

		public CustomText Text_Language;

		public CustomText Text_LevelNeed;

		public CustomText Text_JoinType;

		public CustomText Text_Members;

		public CustomButton Button_Scan;

		private GuildShareData m_data;
	}
}
