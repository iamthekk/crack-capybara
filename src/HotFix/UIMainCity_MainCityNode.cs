using System;
using Framework;
using Framework.EventSystem;
using Framework.Logic.UI;
using UnityEngine;

namespace HotFix
{
	public class UIMainCity_MainCityNode : UIBaseMainCityNode
	{
		public override int FunctionOpenID
		{
			get
			{
				return 0;
			}
		}

		public override MainCityName Name
		{
			get
			{
				return MainCityName.MainCity;
			}
		}

		public override string RedName
		{
			get
			{
				return string.Empty;
			}
		}

		public override int NameLanguageID
		{
			get
			{
				return 6401;
			}
		}

		public override string NameLanguageIDStr
		{
			get
			{
				return "";
			}
		}

		protected override void OnInit()
		{
			this.m_loginDataModule = GameApp.Data.GetDataModule(DataName.LoginDataModule);
			base.OnInit();
			this.m_lordAvatar.OnClick = new Action<UIAvatarCtrl>(this.OnClickLordAvatar);
			this.m_lordAvatar.Init();
		}

		public override void OnShow()
		{
			base.OnShow();
			this.OnRefreshUI();
			GameApp.Event.RegisterEvent(LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, new HandlerEvent(this.OnEventRefreshLordAddSlaveData));
		}

		public override void OnHide()
		{
			GameApp.Event.UnRegisterEvent(LocalMessageName.CC_GameLoginData_RefreshLordAddSlaveData, new HandlerEvent(this.OnEventRefreshLordAddSlaveData));
			base.OnHide();
		}

		protected override void OnDeInit()
		{
			base.OnDeInit();
			if (this.m_lordAvatar != null)
			{
				this.m_lordAvatar.DeInit();
			}
			this.m_loginDataModule = null;
		}

		public override void OnLanguageChange()
		{
			base.OnLanguageChange();
			if (this.m_conquerTxt != null)
			{
				this.m_conquerTxt.text = Singleton<LanguageManager>.Instance.GetInfoByID_LogError(6451);
			}
		}

		protected override void OnClickLockBt()
		{
			base.OnClickLockBt();
		}

		protected override void OnClickUnlockBt()
		{
			base.OnClickUnlockBt();
			ConquerViewModule.OpenData openData = new ConquerViewModule.OpenData();
			openData.m_targetUserID = this.m_loginDataModule.userId;
			openData.m_targetNick = this.m_loginDataModule.NickName;
			if (GameApp.View.IsOpened(ViewName.ConquerViewModule))
			{
				GameApp.View.CloseView(ViewName.ConquerViewModule, null);
			}
			GameApp.View.OpenView(ViewName.ConquerViewModule, openData, 1, null, null);
		}

		private void OnClickLordAvatar(UIAvatarCtrl obj)
		{
			if (this.m_loginDataModule.LordData == null)
			{
				return;
			}
			PlayerInformationViewModule.OpenData openData = new PlayerInformationViewModule.OpenData(this.m_loginDataModule.LordData.LordUid);
			if (GameApp.View.IsOpened(ViewName.PlayerInformationViewModule))
			{
				GameApp.View.CloseView(ViewName.PlayerInformationViewModule, null);
			}
			GameApp.View.OpenView(ViewName.PlayerInformationViewModule, openData, 1, null, null);
		}

		private void OnRefreshUI()
		{
			if (this.m_loginDataModule.LordData != null && this.m_loginDataModule.LordData.LordUid > 0L)
			{
				this.m_conquerGroup.gameObject.SetActive(true);
				this.m_lordAvatar.RefreshData(this.m_loginDataModule.LordData.LordAvatar, this.m_loginDataModule.LordData.LordAvatarFrame);
				return;
			}
			this.m_conquerGroup.gameObject.SetActive(false);
		}

		private void OnEventRefreshLordAddSlaveData(object sender, int type, BaseEventArgs eventargs)
		{
			this.OnRefreshUI();
		}

		public RectTransform m_conquerGroup;

		public UIAvatarCtrl m_lordAvatar;

		public CustomText m_conquerTxt;

		private LoginDataModule m_loginDataModule;
	}
}
