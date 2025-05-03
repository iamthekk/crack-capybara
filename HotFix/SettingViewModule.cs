using System;
using Dxx.Guild;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using Framework.ViewModule;
using UnityEngine.Events;

namespace HotFix
{
	public class SettingViewModule : BaseViewModule
	{
		public override void OnCreate(object data)
		{
			this.m_loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			this.m_addAttributeDataModule = GameApp.Data.GetDataModule(DataName.AddAttributeDataModule);
			this.m_chapterDataModule = GameApp.Data.GetDataModule(DataName.ChapterDataModule);
			this.m_avatarCtrl.Init();
			this.m_userIDCtrl.Init();
			this.m_qualityCtrl.Init();
			this.m_languageCtrl.Init();
			this.m_musicCtrl.Init();
			this.m_soundCtrl.Init();
			this.m_vibrationCtrl.Init();
		}

		public override void OnOpen(object data)
		{
			this.m_openData = data as SettingViewModule.OpenData;
			this.OnRefresh();
			this.m_maskBt.onClick.AddListener(new UnityAction(this.OnClickCloseView));
			this.m_closeBt.onClick.AddListener(new UnityAction(this.OnClickCloseView));
			this.m_nickBt.onClick.AddListener(new UnityAction(this.OnClickNickBt));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventRefreshLanguage));
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameLoginData_UserInfoChange, new HandlerEvent(this.OnEventUserInfoChange));
		}

		public override void OnClose()
		{
			if (this.m_openData != null && this.m_openData.onCloseCallback != null)
			{
				this.m_openData.onCloseCallback();
			}
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_REFRESH_LANGUAGE_FINISH, new HandlerEvent(this.OnEventRefreshLanguage));
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameLoginData_UserInfoChange, new HandlerEvent(this.OnEventUserInfoChange));
			this.m_maskBt.onClick.RemoveAllListeners();
			this.m_closeBt.onClick.RemoveAllListeners();
			this.m_nickBt.onClick.RemoveAllListeners();
		}

		public override void OnDelete()
		{
			if (this.m_avatarCtrl != null)
			{
				this.m_avatarCtrl.DeInit();
			}
			if (this.m_userIDCtrl != null)
			{
				this.m_userIDCtrl.DeInit();
			}
			if (this.m_qualityCtrl != null)
			{
				this.m_qualityCtrl.DeInit();
			}
			if (this.m_languageCtrl != null)
			{
				this.m_languageCtrl.DeInit();
			}
			if (this.m_musicCtrl != null)
			{
				this.m_musicCtrl.DeInit();
			}
			if (this.m_soundCtrl != null)
			{
				this.m_soundCtrl.DeInit();
			}
			if (this.m_vibrationCtrl != null)
			{
				this.m_vibrationCtrl.DeInit();
			}
		}

		public override void OnUpdate(float deltaTime, float unscaledDeltaTime)
		{
		}

		public override void RegisterEvents(EventSystemManager manager)
		{
		}

		public override void UnRegisterEvents(EventSystemManager manager)
		{
		}

		private void OnRefresh()
		{
			this.FreshAvatar();
			if (this.m_nickTxt != null)
			{
				this.m_nickTxt.text = this.m_loginDataModule.NickName;
			}
			if (this.m_guildTxt != null)
			{
				string text = ((GuildSDKManager.Instance.GuildInfo != null && GuildSDKManager.Instance.GuildInfo.HasGuild) ? Singleton<LanguageManager>.Instance.GetInfoByID("UISetting_GuildName", new object[] { GuildSDKManager.Instance.GuildInfo.GuildData.GuildShowName }) : Singleton<LanguageManager>.Instance.GetInfoByID("UISetting_GuildName", new object[] { Singleton<LanguageManager>.Instance.GetInfoByID("UISetting_NoGuild") }));
				this.m_guildTxt.text = text;
			}
			if (this.m_powerTxt != null)
			{
				this.m_powerTxt.text = DxxTools.FormatNumber((long)this.m_addAttributeDataModule.Combat);
			}
			if (this.m_chapterTxt != null)
			{
				this.m_chapterTxt.text = DxxTools.GetChapterLevel(this.m_chapterDataModule.ChapterID, this.m_chapterDataModule.MaxStage);
			}
			if (this.redNodeAvatar != null)
			{
				bool flag = this.m_loginDataModule.IsAvatarOrFrameRedNode();
				this.redNodeAvatar.gameObject.SetActiveSafe(flag);
			}
		}

		private void FreshAvatar()
		{
			if (this.m_avatarCtrl != null)
			{
				this.m_avatarCtrl.RefreshData(this.m_loginDataModule.Avatar, this.m_loginDataModule.AvatarFrame);
				this.m_avatarCtrl.OnClick = new Action<UIAvatarCtrl>(this.OnClickAvatar);
			}
		}

		private void OnEventRefreshLanguage(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefresh();
		}

		private void OnEventUserInfoChange(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefresh();
		}

		private void OnClickCloseView()
		{
			if (this.m_openData != null)
			{
				MoreExtensionViewModule.TryBackOpenView(this.m_openData.srcViewName);
			}
			GameApp.View.CloseView(ViewName.SettingViewModule, null);
		}

		private void OnClickNickBt()
		{
			GameApp.View.OpenView(ViewName.PlayerNameViewModule, null, 1, null, null);
		}

		private void OnClickAvatar(UIAvatarCtrl obj)
		{
			GameApp.View.OpenView(ViewName.PlayerAvatarClothesViewModule, 1, 1, null, null);
		}

		public CustomButton m_maskBt;

		public CustomButton m_closeBt;

		public UIAvatarCtrl m_avatarCtrl;

		public CustomText m_nickTxt;

		public CustomButton m_nickBt;

		public CustomText m_guildTxt;

		public CustomText m_powerTxt;

		public CustomText m_chapterTxt;

		public UISettingUserIDCtrl m_userIDCtrl;

		public UISettingQualityCtrl m_qualityCtrl;

		public UISettingLanguageCtrl m_languageCtrl;

		public UISettingMusicCtrl m_musicCtrl;

		public UISettingSoundCtrl m_soundCtrl;

		public UISettingVibrationCtrl m_vibrationCtrl;

		public RedNodeOneCtrl redNodeAvatar;

		private LoginDataModule m_loginDataModule;

		private AddAttributeDataModule m_addAttributeDataModule;

		private ChapterDataModule m_chapterDataModule;

		private SettingViewModule.OpenData m_openData;

		public class OpenData
		{
			public ViewName srcViewName;

			public Action onCloseCallback;
		}
	}
}
