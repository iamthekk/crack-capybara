using System;
using Dxx.Guild;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.Guild;
using UnityEngine.Events;

namespace HotFix
{
	public class SocialityGuildNode : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_avatarRelation.Init();
			this.m_avatarRelation.OnClick = new Action<UIAvatarRelation>(this.OnClickAvatarRelation);
			if (this.m_visitBt != null)
			{
				this.m_visitBt.onClick.AddListener(new UnityAction(this.OnClickVisitBt));
			}
		}

		protected override void OnDeInit()
		{
			if (this.m_avatarRelation != null)
			{
				this.m_avatarRelation.OnClick = null;
			}
			if (this.m_visitBt != null)
			{
				this.m_visitBt.onClick.RemoveAllListeners();
			}
			if (this.m_avatarRelation != null)
			{
				this.m_avatarRelation.DeInit();
			}
			this.m_data = null;
		}

		private void OnClickAvatarRelation(UIAvatarRelation obj)
		{
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(this.m_data.UserId);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		private void OnClickVisitBt()
		{
			OtherMainCityViewModule.OpenData openData = new OtherMainCityViewModule.OpenData();
			openData.m_userID = this.m_data.UserId;
			if (GameApp.View.IsOpened(ViewName.OtherMainCityViewModule))
			{
				GameApp.View.CloseView(ViewName.OtherMainCityViewModule, null);
			}
			GameApp.View.OpenView(ViewName.OtherMainCityViewModule, openData, 1, null, null);
		}

		public void RefreshData(GuildMemberInfoDto data, int index)
		{
			this.m_data = data;
			if (data == null)
			{
				return;
			}
			int num = -1;
			int num2 = -1;
			if (this.m_data.Extra != null && this.m_data.Extra.LordUid > 0L)
			{
				num = this.m_data.Extra.LordAvatar;
				num2 = this.m_data.Extra.LordAvatarFrame;
			}
			if (this.m_avatarRelation != null)
			{
				this.m_avatarRelation.RefreshData((int)this.m_data.Avatar, (int)this.m_data.AvatarFrame, (int)this.m_data.Level, num, num2);
			}
			if (this.m_nameTxt != null)
			{
				this.m_nameTxt.text = (string.IsNullOrEmpty(this.m_data.NickName) ? DxxTools.GetDefaultNick(this.m_data.UserId) : this.m_data.NickName);
			}
			if (this.m_powerTxt != null)
			{
				this.m_powerTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("9205", new object[] { DxxTools.FormatNumber((long)this.m_data.BattlePower) });
			}
			if (this.m_officeTxt != null)
			{
				this.m_officeTxt.text = GuildUserShareDataEx.GetPositionLanguageByPos((int)this.m_data.Position);
			}
		}

		public UIAvatarRelation m_avatarRelation;

		public CustomText m_nameTxt;

		public CustomText m_powerTxt;

		public CustomText m_officeTxt;

		public CustomButton m_visitBt;

		public GuildMemberInfoDto m_data;
	}
}
