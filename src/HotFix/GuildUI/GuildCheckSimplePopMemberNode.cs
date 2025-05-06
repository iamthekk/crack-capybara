using System;
using Dxx.Guild;
using Framework;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix.GuildUI
{
	public class GuildCheckSimplePopMemberNode : GuildProxy.GuildProxy_BaseBehaviour
	{
		protected override void GuildUI_OnInit()
		{
			if (this.m_headIcon != null)
			{
				this.m_headIcon.Init();
			}
		}

		protected override void GuildUI_OnUnInit()
		{
			if (this.m_headIcon != null)
			{
				this.m_headIcon.DeInit();
			}
			this.m_userShareData = null;
		}

		public void RefreshData(GuildUserShareData data, int index)
		{
			this.m_userShareData = data;
			if (data == null)
			{
				return;
			}
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = data.GetNick();
			}
			bool flag = data.GuildPosition != GuildPositionType.Member;
			if (this.m_positionObj != null)
			{
				this.m_positionObj.SetActive(flag);
			}
			if (flag && this.m_positionTxt != null)
			{
				this.m_positionTxt.text = data.GetPositionLanguage();
			}
			if (this.m_powerTxt != null)
			{
				this.m_powerTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("400274", new object[] { DxxTools.FormatNumber((long)data.Power) });
			}
			if (this.m_headIcon != null)
			{
				this.m_headIcon.Refresh(data.Avatar, data.AvatarFrame, new Action<object>(this.OnClickUser));
			}
		}

		private void OnClickUser(object obj)
		{
			if (this.m_userShareData == null)
			{
				return;
			}
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(this.m_userShareData.UserID);
			if (GameApp.View.IsOpened(ViewName.GuildPlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.GuildPlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.GuildPlayerInformationViewModule, openData, 1, null, null);
		}

		public UIGuildHead m_headIcon;

		public CustomText m_nameTxt;

		public GameObject m_positionObj;

		public CustomText m_positionTxt;

		public CustomText m_powerTxt;

		private GuildUserShareData m_userShareData;
	}
}
