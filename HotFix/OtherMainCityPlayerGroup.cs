using System;
using Framework;
using Framework.Logic.Component;
using Framework.Logic.UI;
using Proto.User;

namespace HotFix
{
	public class OtherMainCityPlayerGroup : CustomBehaviour
	{
		protected override void OnInit()
		{
			this.m_avatarCtrl.Init();
			this.m_avatarCtrl.OnClick = new Action<UIAvatarCtrl>(this.OnClickAvatar);
		}

		protected override void OnDeInit()
		{
			this.m_avatarCtrl.DeInit();
			this.m_data = null;
		}

		public void RefreshUI(UserGetCityInfoResponse data)
		{
			this.m_data = data;
			if (this.m_nickTxt != null)
			{
				this.m_nickTxt.text = (string.IsNullOrEmpty(data.NickName) ? DxxTools.GetDefaultNick(data.UserId) : data.NickName);
			}
			if (this.m_avatarCtrl != null)
			{
				this.m_avatarCtrl.RefreshData((int)data.Avatar, (int)data.AvatarFrame);
			}
			if (this.m_powerTxt != null)
			{
				this.m_powerTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID("9205", new object[] { DxxTools.FormatNumber(data.Power) });
			}
			bool flag = !string.IsNullOrEmpty(data.GuildName);
			if (this.m_guildTxt != null)
			{
				string text = (flag ? Singleton<LanguageManager>.Instance.GetInfoByID("9304", new object[] { data.GuildName }) : Singleton<LanguageManager>.Instance.GetInfoByID("9304", new object[] { Singleton<LanguageManager>.Instance.GetInfoByID("9307") }));
				this.m_guildTxt.text = text;
			}
		}

		private void OnClickAvatar(UIAvatarCtrl obj)
		{
			if (this.m_data == null)
			{
				return;
			}
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(this.m_data.UserId);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		public CustomText m_nickTxt;

		public UIAvatarCtrl m_avatarCtrl;

		public CustomText m_powerTxt;

		public CustomText m_guildTxt;

		public UserGetCityInfoResponse m_data;
	}
}
